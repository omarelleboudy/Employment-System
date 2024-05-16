# Employment System

## Prerequisites:
- .NET 7
- MS SQL Server

## How To Run:

### Setting Up the Environment:
1. **Database Setup**:
   - Make sure SQL Server is set up and ready for new database instances.
   - Set up your connection string in `appsettings.json`.
   - For local testing, ensure that the connection strings include `"Encrypted=false;"`. Otherwise, Entity Framework Core may have issues creating the database automatically.

2. **Logging Database**:
   - Create a logging database with a name that matches the one under the connection string `Serilog:LoggingDBConnection` in `appsettings.json`.
   - The database can be empty, as the project will create the required table.

3. **Hangfire Database**:
   - Create a Hangfire database with a name that matches the one under the connection string `HangFireDB` in `appsettings.json`.
   - The database can be empty, as the project will create the required tables.

### Running the Application:
1. **Apply Migrations**:
   - Migrations needed to build up the database are already included. Check the `Migrations` folder under the Data Project.
   - Launch the Package Manager Console from `Tools > NuGet Package Manager > Package Manager Console`.
   - Run the command `Update-Database`.

2. **Verify Database Creation**:
   - Check SQL Server to ensure that a database was added with all the required tables.

3. **Build and Run**:
   - Clean and rebuild the solution.
   - Run the application.
