#Swimbait#

Definition: 
> A class of fishing lures that imitate fish

An ASP.NET Core application that emulates the Yamaha MusicCast protocol. 

[![swimbait MyGet Build Status](https://www.myget.org/BuildSource/Badge/swimbait?identifier=dfa0a82e-57fe-4328-a97a-932c467510d3)](https://www.myget.org/)

## Projects
### Swimbait.Server
Emulates a Yamaha MusicCast speaker by serving and responding REST HTTP on multiple ports and UDP.

Current status is

* `[almost there]` ios MusicCast app can discover the fake Swimbait server 
* `[todo]` swimbait server can respond to and receive requests to stream audio 

### Swimbait.Console
See its [readme file](https://github.com/neutmute/swimbait/tree/master/src/Swimbait.Console)

### MusicCast.Console
Client for controlling a MusicCast speaker

## Setup
Swimbait requires some environment variables:

###### Windows Powershell
Restart Visual Studio after setting these in a powershell prompt 

	 # IP Address of real MusicCast speaker. Used for man in the middle replays. 
	 [Environment]::SetEnvironmentVariable("Swimbait:RelayHost", "192.168.1.213", "User") 

	 # Folder for activity log 
	 [Environment]::SetEnvironmentVariable("Swimbait:ReplayLogFolder", "D:\Downloads\Swimbait", "User")

## Swimbait.Server Execution
1. Run the `Swimbait.Server` project
1. Test the HTTP server by browsing to `http://<your-machines-IP>/MediaRenderer/desc.xml`
1. Open the MusicCast app on your phone
2. Connect to a new device
3. Press `C` a few times into the server console while the app is waiting for a connection
4. The MusicCast app should think it has found a MusicCast speaker
5. The app will respond with an error. (More work required)

## Documentation
The [doc](https://github.com/neutmute/swimbait/tree/master/doc) folder contains various captures and collected information, including the Yamaha authored [Yamaha Extended Control API Specification (Basic) PDF](https://github.com/neutmute/swimbait/raw/master/doc/yamaha/YXC_API_Spec_Basic.pdf) which describes much of the MusicCast REST API.  

### Blog 
There is some more reading at this [blog entry](http://blog.turbine51.net/2016/04/04/Yamaha-Musiccast-Protocol/)

