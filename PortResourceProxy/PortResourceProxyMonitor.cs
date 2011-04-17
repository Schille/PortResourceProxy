using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Xml;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;



namespace PortResourceProxy
{
    public partial class PortResourceProxyMonitor : Form
    {
        PortResourceProxy _Proxy;
        private String PATH = "\\ResourceMap.xml";

        public PortResourceProxyMonitor()
        {
            InitializeComponent();
            
            
        }
        
        /// <summary>
        /// Button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            //if proxy has to start
            if (btnStartStop.Text.Equals("Start"))
            {
                tbPort.Enabled = false;
                try
                {
                    AddConsoleLine(string.Format("\n====Starting PortResourceProxy on Port: {0}===\n", tbPort.Text));
                    _Proxy = new PortResourceProxy(int.Parse(tbPort.Text),this);
                    AddConsoleLine(string.Format("-> Status: Successful started!\n"));
                }
                catch (Exception ex)
                {
                    AddConsoleLine(string.Format("-> Error: {0}", ex.Message));
                    AddConsoleLine("-> Status: PortResourceProxy not started!\n");
                    tbPort.Enabled = true;
                    return;
                }
                btnStartStop.Text = "Stop";
            }
            //if proxy has to stop
            else
            {
                try
                {
                    _Proxy.Dispose();
                    btnStartStop.Text = "Start";
                    AddConsoleLine("-> Status: PortResourceProxy stoped!");
                    _Proxy = null;
                    tbPort.Enabled = true;
                }
                catch (Exception exc)
                {
                  AddConsoleLine("-> Error: " + exc.Message);  
                }
                
            }
                 
        }


        void PortResourceProxyMonitor_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
           if(_Proxy != null)
            _Proxy.Dispose(); 
        }

        private void btnEditRMap_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\windows\system32\notepad.exe", System.Windows.Forms.Application.StartupPath+PATH);
        }

        
        

    }

            
}
