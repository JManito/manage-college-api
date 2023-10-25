# manage-college-api
# Project Setup
This document provides step-by-step instructions on how to set up and run the Manage College API. Before you begin, ensure that you have Git, Visual Studio, and SQL Server installed on your system.

### Prerequisites
- Git
- Visual Studio
- SQL Server Express (LocalDB)
- .NET Framework

### Clone the Repository
1. git clone https://github.com/JManito/manage-college-api.git
1. cd manage-college-api
### Configure the Database
1. Open SQL Server Management Studio (SSMS).
1. Connect to your LocalDB instance.
1. Create a new database or use an existing one.
1. Update the connection string in *appsettings.json* to point to your LocalDB
```
{
  "ConnectionStrings": {
    "ManageCollegeConnectionSting": "Server={your-server};Database=College;TrustServerCertificate=True;Trusted_Connection=True"
  }
}
```

### Database Migrations
*We are using Entity Framework Code First, you need to create or update the database schema. Use the following commands in the Package Manager Console:*
```
> Enable-Migrations
> Update-Database -TargetMigration 20231024184553_CreateDbAndSeedData
```

### Build and Run the Application
1. Open the project in Visual Studio.
1. Build the project to restore packages and compile the application.
1. Set the API project as the startup project.
1. Press F5 or click "Start" to run the application.

### Testing
You can use tools like Postman or Swagger to test API endpoints.

