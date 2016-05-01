#Bingo Calculator 1.001

###Data Collection
Data is split up into 4 different types.
* Areas
* Nodes
* Objects
* Paths

###Areas
Areas are the overarching data, some examples of areas are Kokiri Forest or Link's House.

###Nodes
Nodes are locations in an area. This can be quite complicated because most of the time a node is right next to or inside of a loading zone.

####Entry Nodes
An entry node is a node that comes from a loading zone. Generally it is two different locations. There is the standard location which comes from running into a loading zone. There is also the location after watching a cutscene which puts you slightly closer to the loading zone you came from.

####Loading Zone Nodes
These nodes occur on the first frame that the screen is entirely black.

###Objects
Objects are a general term for anything you can collect inside of the game. For example, a deku shield, watching the deku tree cutscene, or getting the kokiri emerald. These all objects. Objects are used inside of paths to sequence events.

###Paths
Paths are the meat of this app. A path is travelling from one node to another. For example, travelling from Link's House to the Kokiri Sword Chest would be a path. In this path we would obtain the Kokiri Sword and Saria's intro cut scene. We could then make more paths which require Saria's cut scene in order to be used. This is how we deal with cutscenes.

There are many different variables in paths.
####Name
This name must not be shared with any other path of the same age, console, and area.
####Area Destination
This is the area that is travelled to in this path.
####Start Node
The location that this path starts at.
####End Node
The location that this path ends at.
####Rupees
This indicates the change in rupees of this path. For example, if you gain 12 rupees this value would be 12. If you paid 40 rupees you would make this -40.
####Bombs
This is the change in bombs. Works just like rupees.
####Bombchus
This is the change in bombchus. Works just like rupees.
####Time to Add, Time of Day
Time is the amount of time it takes for the path to complete. Time of day is the amount of time that the day-night cycle has changed in game.
You can add several different times, or add a time to an already existing node using the plus button. You can also delete a time/time of day pair with the minus button.
####Objects Obtained
This is the objects that you will obtain inside the path.
####Objects Required
These are the objects that are required in order to take this path.
####Objects Prohibited
These are the objects that you must not have in order to take this path.
####Time of Day Event
This is an event in the path that is dependent on the day-night cycle in game. For example, at 27 second into a path you might require that it be night time to gather a skulltula.

###Timing a Path
To time the paths here are the software that I use.
Amarec TV records the video file. I use the codev x264 to compress it so it's not enormous. I use virtual dub for timing the video file. You just simple find the frame that is all white/black and subtract the first frame of movement from that. If the ending node is not a loading zone, you just end it on first frame of input.

###Time of Day
* Work in Progress

######Amarec TV:
######http://www.amarectv.com/english/amarectv_e.htm
######x264 codec:
######https://sourceforge.net/projects/x264vfw/files/latest/download
######Virtual Dub:
######http://virtualdub.sourceforge.net/