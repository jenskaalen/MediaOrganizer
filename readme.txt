HOW TO USE

QUICKSTART:
Edit MediaDirectories.xml and Modules.xml.
Run MediaOrganizer.exe


Configuring MediaDirectories.xml:
This file can have up to several "Handler" elements defined with a required "type" attribute.
Atm the only valid handlers are "ShowMediaHandler" and "StandardMediaHandler".

Edit the example values in the file.

Modules.xml:
Can have several modules. 

Unzipper module will unzip any .rar or .zip files found (unzipped files will be marked so they won't be re-unzipped).

RssDownloader will automatically download torrents mathcing given patterns.

Edit the example values in the file.


Scheduling:
After the initial configuration has been made a task can be set up to run the application at intervals.
Make the application be executed with the same working directory as the MediaOrganizer.exe.

Errors:
Any errors will be dumped to log.log.