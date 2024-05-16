# Prerequisites:
-	.NET 7
-	MS SQL Server

# How To Run:

## This guide assumes that all required NuGET packages are already installed. If you are not sure, run a Clean and Rebuild before starting.

1-	Make sure SQL Server is set up and ready for new databases instances, and set up your connection string in appsettings.


2-	For Local Testing, keep in mind that the connection strings require “Encrypted=false;” or Entity Framework Core may not be able to automatically create a database.


3-	Create a Logging Database with a name that matches the one under the connection string “Serilog:LoggingDBConnection” in the appsettings.json. The database can be empty, as 
the project will take care of creating the required Table.


4-	Create a Hangfire Database with a name that matches the one under the connection string “HangFireDB” in the appsettings.json. The database can be empty, as 
the project will take care of creating the required Tables.


5-	Migrations needed to build up the database are already there (check the folder Migrations under Data Project), launch the Package Manager Console from Tools > NuGET Package Manager > Package Manager Console


6-	Run the command Update-Database


7-	Check SQL Server to see if a database was added with all the required Tables.


8-	Clean, Rebuild, then Run.

