PortResourceProxy
.....................................................................................................

This is a small local Proxy. It was born from the idea to bypass the "Same-Origin-Policy" by proxying 
all services that runs on different ports (may for AJAX request purposes).

Features
	. able to proxy several port-services under on port
	. free configurable source - destination "ResourceMap.xml" to set the http/resource pattern
	. manual start and stop of the proxy
	. console-like output of the status and errors
	

Status
	. there are some unreported bugs
	. the keyfeatures are almost completly operative
	. there still have to add some error messages an feedbacks
	. there are some Exceptions because of the HttpListener - but they don't interrupt the operation
	. may the proxy should get a better way of configuration 
	. just tested on Windows 7 (run as administrator)

How to use
	. just click on download 
	. navigate to .\PortResourceProxy\PortResourceProxy\bin\Release
	. run "PortResourceProxy.exe" as administrator (because you'll open a new port)
	. chooe a port, you want to use
	. you have to edit the ResourceMap.xml:
		. click "Edit resource Map"
		. notepad.exe will be launched (you can also use your favorite xml editor, of course) 
		. just choose a source (/pattern/) and destination (an example is already in the ResourceMap.xml)
	. click on "Start" - if every went fine, you get "-> Status: Successful started!"
	. now you can may browse to your location (http://localhost:"your port"/"you source"

	. Addition
		. if the browser automaticaly loads content from other locations you need to configure that in ResourceMap.xml
		. every redirection will be shown in the console
		. if there is a request for a non-configured resource, you'll see it in the console, too
		. there are a lot of messages and status reports on the console
		. you can allow and deny parameters and paths by setting the checkboxes - but there is not yet a report for


It was developed with Visual Studio 2010 on .net 4.0

Michael Schilonka (Schille)
