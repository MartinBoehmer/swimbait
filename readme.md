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

## Usage
Swimbait requires some environment variables. 
        
	 [Environment]::SetEnvironmentVariable("Swimbait:RelayHost", "192.168.1.213", "User") # IP Address of real MusicCast speaker. Used for man in the middle replays. 
	 [Environment]::SetEnvironmentVariable("Swimbait:ReplayLogFolder", "D:\Downloads\Swimbait", "User") # Folder for activity log 

## Blog 
There is some more reading on this [blog entry](http://blog.turbine51.net/2016/04/04/Yamaha-Musiccast-Protocol/)

