using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Xml;
using System.Web;
using System.Diagnostics;
using System.IO;

namespace PortResourceProxy
{
    public class PortResourceProxy : IDisposable
    {

        private int _Port;
        private HttpListener _MyListener;
        private Dictionary<String, String> _Map;
        private Thread _WorkerThread;
        private CancellationTokenSource _CancelToken;
        private PortResourceProxyMonitor _Monitor;
        /// <summary>
        /// Filename of ResourceMap XML
        /// </summary>
        private String PATH = "\\ResourceMap.xml";

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="myPort">Port on wich the proxy listen</param>
        /// <param name="myMonitor">The form, which contains the pseudo console</param>
        public PortResourceProxy(int myPort, PortResourceProxyMonitor myMonitor)
        {
            _Monitor = myMonitor;
            _Port = myPort;
            PATH = System.Windows.Forms.Application.StartupPath + PATH;
            LoadMap();
            _CancelToken = new CancellationTokenSource();
            _WorkerThread = new Thread(new ThreadStart(WorkingThread));
            _MyListener = new HttpListener();
            _MyListener.Prefixes.Add("http://*:" + myPort.ToString() + "/");
            _MyListener.Start();
            
            _WorkerThread.Start();
        }
        /// <summary>
        /// Loads and parses the ResourceMap configuration
        /// </summary>
        private void LoadMap()
        {
            _Map = new Dictionary<string, string>();
            XmlDocument xml = new XmlDocument();
            _Monitor.AddConsoleLine("-> Status: Try to load ResourceMap from: " + PATH);
            try
            {
                xml.Load(PATH);
            }
            catch(Exception exc)
            {
                _Monitor.AddConsoleLine("-> Error:" + exc.Message);
                _Monitor.AddConsoleLine("-> Please locate a correct ResourceMap file at:" + System.Windows.Forms.Application.StartupPath);
            }
            
            var child = xml.FirstChild.NextSibling.FirstChild.FirstChild;
            while (null != child)
            {
                _Map.Add(child.FirstChild.InnerText, child.ChildNodes[1].InnerText);
                _Monitor.AddConsoleLine(String.Format("-> Status: Now Mapped Source \"{0}\" to destination \"{1}\"", child.FirstChild.InnerText, child.ChildNodes[1].InnerText));
                child = child.NextSibling;
            }

        }
        
        /// <summary>
        /// Worker thread, waits for incoming http requests to handle
        /// </summary>
        private void WorkingThread()
        {
            try
            {
                while (!_CancelToken.IsCancellationRequested)
                {
                    if (_MyListener.IsListening)
                    {
                        var result = _MyListener.BeginGetContext(new AsyncCallback(Request), _MyListener);
                    }
                    Thread.Sleep(10);
                }
            }
            catch
            {  
            }
          
            
        }

        /// <summary>
        /// Callback function to handle incoming http requests
        /// Gets the EndGetContext of the HttpListener
        /// Assigns the source - to the destinaton adresse
        /// Sends the "proxied" response to the client
        /// </summary>
        /// <param name="myResult"></param>
        private void Request(IAsyncResult myResult)
        {
            HttpListenerContext Client = null;

            lock (_Map)
            {
                try
                {
                    if (_MyListener == null || !_MyListener.IsListening)
                        return;
                    Client = _MyListener.EndGetContext(myResult);
                }
                catch
                {
                    if (Client == null)
                    {
                        return;
                    }
                }

                String RequestedResource = String.Empty;
                String Url = String.Empty;
                if (Client.Request.Url.Segments.Length >= 2)
                {
                    RequestedResource = Client.Request.Url.Segments[1];
                    Url = Client.Request.Url.AbsolutePath.Replace(RequestedResource, "");
                }

                String Parameter = Client.Request.Url.Query;
                try
                {

                    //Checking the availability of the pattern
                    var Origin = checkMap(RequestedResource);
                    if (Origin != null)
                    {
                        //if "Allow Paths", resource path will be added
                        if (_Monitor.cbAllowPaths.Checked)
                        {
                            //show message if a resource in a deeper path was requested
                            if (Origin.EndsWith("/"))
                                Origin = Origin.Remove(Origin.Length - 1);
                            if (Url.Length > 1)
                                Origin += Url;
                        }
                       
                        //if "Allow Parameter" add Parameter
                        if (_Monitor.cbAllowQuery.Checked)
                        {
                            //show message if resource with parameter was requested
                            Origin += Parameter;
                        }


                        var request = (HttpWebRequest)WebRequest.Create(Origin);


                        try
                        {
                            foreach (var item in Client.Request.Headers.AllKeys)
                            {
                                switch (item)
                                {
                                    case "Accept":
                                        request.Accept = Client.Request.Headers[item];
                                        break;

                                    case "Connection":
                                        break;

                                    case "Host":
                                        break;

                                    case "User-Agent":
                                        request.UserAgent = Client.Request.UserAgent;
                                        break;

                                    case "Referer":
                                        request.Referer = Client.Request.Headers[item];
                                        break;

                                    default:
                                        request.Headers.Add(item, Client.Request.Headers[item]);
                                        break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                          //there are even some exceptions 'bout building the http header  
                        }

                        request.ContentType = Client.Request.ContentType;
                        request.Method = Client.Request.HttpMethod;

                        HttpWebResponse response = null;

                        try
                        {
                            //gets the response of the proxied resource
                            response = (HttpWebResponse)request.GetResponse();
                        }
                        catch (WebException e)
                        {
                            //may there is a web exception (e.g. 404)
                            //lets return the error to the requesting client
                            response = e.Response as HttpWebResponse;
                        }

                        if (response == null)
                        {
                            Client.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                        }
                        else
                        {
                            SendResponse(response, Client);
                            return;
                        }

                    }

                    //If there were no ResourceMap for the requested resource
                    _Monitor.AddConsoleLine(String.Format("-> Error: \"No map\" at:{0} for pattern:{1}", Client.Request.LocalEndPoint.ToString(), RequestedResource));
                    Stream output = Client.Response.OutputStream;

                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes("<HTML><BODY> " + "<h1>No map for the request <i>\"" + RequestedResource + "\"</i> was created!</h1>" + "</BODY></HTML>");
                    Client.Response.ContentLength64 = buffer.Length;
                    output.Write(buffer, 0, buffer.Length);


                }
                catch (Exception exc)
                {

                }
                finally
                {   
                    //nevertheless close the opened tcp coonnection
                    Client.Response.Close();
                }
            }

        }


        /// <summary>
        /// Checks the availability of the requested resource in the _Map
        /// Does a little fault tolerance
        /// </summary>
        /// <param name="RequestedResource"></param>
        /// <returns></returns>
        private String checkMap(String RequestedResource)
        {
            if (_Map.ContainsKey(RequestedResource))
            {
                return _Map[RequestedResource];
            }
            if (_Map.ContainsKey(RequestedResource + "/"))
            {
                return _Map[RequestedResource + "/"];
            }
            if (_Map.ContainsKey(RequestedResource.Replace("/", "")))
            {
                return _Map[RequestedResource.Replace("/", "")];
            }
            return null;
        }

        /// <summary>
        /// Generates the resulting response
        /// Sets the header to the resulting client request
        /// </summary>
        /// <param name="myServerResponse"></param>
        /// <param name="myRequestContext"></param>
        private void SendResponse(HttpWebResponse myServerResponse, HttpListenerContext myRequestContext)
        {
            myRequestContext.Response.StatusCode = (int)myServerResponse.StatusCode;
            myRequestContext.Response.ContentType = myServerResponse.ContentType;
            myRequestContext.Response.ProtocolVersion = myServerResponse.ProtocolVersion;
            if (myServerResponse.ContentLength != -1)
            myRequestContext.Response.ContentLength64 = myServerResponse.ContentLength;

            //header transpose
            foreach (var item in myServerResponse.Headers.AllKeys)
            {
                switch (item)
                {
                    case "Content-Length":
                        break;
                    default:
                        myRequestContext.Response.AddHeader(item, myServerResponse.Headers[item]);
                        break;
                }
            }
           
            try
            {
                //stream transpose
                myServerResponse.GetResponseStream().CopyTo(myRequestContext.Response.OutputStream);
                myRequestContext.Response.OutputStream.Flush();

                
            }
            catch
            {
            }
            finally
            {
                lock (_Monitor)
                {
                    _Monitor.AddConsoleLine(String.Format("-> Status: Redirected request from: [{0}]: {1} to:  {2}", myRequestContext.Request.LocalEndPoint.Address.ToString(), myRequestContext.Request.Url.Port, myServerResponse.ResponseUri.AbsoluteUri.ToString()));
                }
                
                //close the tcp socket 
                myServerResponse.GetResponseStream().Close();
                myServerResponse.Close();

                myRequestContext.Response.OutputStream.Close();
                myRequestContext.Response.Close();
            }

        }


        /// <summary>
        /// Dispose the currently used _Proxy Object
        /// </summary>
        public void Dispose()
        {
            Stop();
            
        }


        /// <summary>
        /// There are correctly some problems 'bout disposing the HttpListener
        /// Stopping the worker thread
        /// </summary>
        private void Stop()
        {
            _Monitor = null;

            if (_MyListener != null)
            {
                _MyListener.Abort();
                ((IDisposable)_MyListener).Dispose();
             
            }
            _CancelToken.Cancel();
            if (_WorkerThread != null)
            {
                _WorkerThread.Join();

                if (_WorkerThread.IsAlive)
                {
                    _WorkerThread.Abort();
                }
            }
            
        }
        
    }
}
