# ShippingPortGOF

ShippingPortGOF is a C# .NET 7.0 Console application made as part of the faculty graduate studies course "Design Patterns". This application showcases the use of multiple GOF Design Patterns (Gamma, E.; Helm, R.; Johnson, R.; Vlissides, J. Design Patterns: Elements of Reusable of Object-Oriented Design. Addison-Wesley, Reading, MA, 1995.). Also, this application is designed following the MVC (Model-View-Controller) software architectural pattern. 

## Used GOF Design Patterns:

- Singleton
- Factory Method
- Visitor
- Observer
- Chain of Responsibility
- Iterator
- Memento
- Composite

## Running from command line

Application has multiple mandatory arguments and one optional needed to initialize the app with the data stored in csv files whose names are passed as arguments. "data" folder contains examples of those files that are ready for use. Options only accept the filename and not the filepath so you need to position your terminal in the same folder where csv files are stored. Example of a Powershell command with all the correct options and arguments:

PS C:\Users\User\source\repos\ShippingPortGOF\data> & "C:\Users\User\source\repos\ShippingPortGOF\ShippingPortGOF\bin\Debug\net7.0\ShippingPortGOF.exe" -p SP_port.csv -b SP_ship.csv -s SP_schedule.csv -m SP_mooring.csv -d SP_pier.csv -dm SP_pier_mooring.csv -c SP_channel.csv

## Options

-p - Mandatory option that accepts the file needed for port initialization.

-b - Mandatory option that accepts the file needed for ships initialization.

-m - Mandatory option that accepts the file needed for moorings initialization.

-d - Mandatory option that accepts the file needed for piers initalization.

-dm - Mandatory option that accepts the file needed for connecting the moorings to its pier.

-c - Mandatory option that accepts the file needed for communication channels initalization.

-s - Option that is optional and accepts the file needed for loading the schedules.

## Ships and moorings types
Mooring type | Mooring type abbreviation | Ship type | Ship type abbreviation
------------ | ------------------- | ----- | ---------------------
Passenger | PA | Ferry | FE
||| Catamaran | CA
||| Passenger ship | PS
||| Cruise ship | CR
Business | BU | Fishing boat | FI
||| Freighter | FR
Other | OT | Yacht | YA
||| Boat | BO
||| Diving boat | DI

## Commands
STATUS
- Description: Overview of the status (available or taken) of ship mooring at the moment of application virtual time.


VIRTUAL TIME dd.mm.yyyy. hh:mm:ss
- Description: Changes virtual time with entered date and time.

MOORINGS [mooring_type] [status] [datetime_from] [datetime_until]
- Statuses: T - taken, A - available
- Datetime format: dd.mm.yyyy. hh:mm:ss
- Description: Overview of taken (T) or available (A) ship moorings of a certain type in a given time range
where the time from and to when the mooring is taken (T) or available (A) is also printed.

LOAD RESERVATIONS [filename].csv
- Description: Loads the reservation request file.

RESERVED MOORING [ship ID]
- Description: Creates a request for enabling/permitting mooring to a ship with a specified ID
to his reserved mooring.

NON RESERVED MOORING [ship ID] [duration in hours]
- Description: Creates a request for enabling/permitting mooring to a ship with a specified ID
to a non reserved mooring, where the duration of the mooring in hours is specified.

CHANNEL [ship ID] [channel frequency] [Q]
- Description: Connects a ship with a specified ID to a specified communication channel where that ship then has available
all information sent over that channel. If argument "Q" is specified, command disconnects the ship from a specified channel.

PRINT SETTINGS [H | F | SN]
- Description: Sets table print settings. The command can receive several different options ( H -
header, F - footer, SN - sequence number). The order of abbreviations is arbitrary.

TAKEN MOORINGS dd.mm.yyyy. hh:mm
- Description: Prints the total number of taken ship moorings by type at a given time.

BACKUP "[name]"
- Description: Performs a backup of the current state of occupancy of all ship moorings and the current application virtual time.

RESTORE "[name]"
- Description: Returns the state of ship moorings to one of the specified saved mooring occupancy states and associated
virtual time.

SHIP [ship ID]
- Description: Checks the status of the ship with the specified ID in the current
virtual time.

Q
- Description: Terminates the program.
