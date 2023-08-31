> this readme is still a work in progress ;)
# My Micro Management
A small application for the time-tracking lovers.

## Summary
My Micro management revolves around a simple objective: track events and time throughout the day.

Its core model is a *TimeSession*, a unit of work related to a *Project*.

Projects are user-management entities, representing a 'context' where they might spend time.

The main use case of the application is the choosing of a *Project*, which triggers a timer for the length of the *TimeSession*

![plot](/docs/In%20progress.png)

Switching projects restarts the timer and saves the TimeSession.

![plot](/docs/switched%20to.png)

## Getting Started
### Prerequisites
To run the project you would need:


- **[Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/)**, configured with *ASP.NET* and *Cross-Platform UI Application* Modules
- **[NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)** installed

> You can verify your .NET version with ```dotnet --version```

### Installation and Getting started
The Repository contains three main applications
- The MAUI Desktop app, the client
- A Web API project dealing the authentication aspect of the application (users registering / login / roles management policies etc...)
- Another Web API project for the TimeSessions and Projects management.

#### Running the MAUI App:
No additional steps needed here, running it from Visual Studio directly.

#### Running the Authentication project
To get started with the authentication project you will need to: 
##### 1. Get the Database started: 
- Create an empty ```C:\Repos\Temp\MyAuthDB-dev.db``` file, this will be our DB
> Note this static file path will be removed in a later step and configured accordingly.
- Open the solution in Visual Studio, and select the ```MicroManagement.Auth.Migrations.SQLite``` project as startup project.
- Open the Nuget Package Manager console in Visual Studio and Select  ```MicroManagement.Auth.Migrations.SQLite``` as Default Project.
- Run ```Update-Database```.
The migrations should get applied and your database will get created successfully.

##### 2. Set the application's JWT secrets
- Right click the ```MicroManagement.Auth.WebAPI``` project and select *Manage User Secrets*.
- You will need to specify two secrets related to the JWT signing and validation: ```Jwt:AccessKey``` and the ```Jwt:RefreshKey``` for, respectively, the access tokens and refresh tokens. They need to be at least 128 bits secret keys. ([Key Generator](https://generate-random.org/encryption-key-generator?count=1&bytes=128&cipher=aes-256-cbc&string=&password=)) May help you for local use.

##### 3. Set the Google Client Secrets
- Right click the ```MicroManagement.Auth.WebAPI``` project and select *Manage User Secrets*.
- You will need to specify two secrets related to the google social auth: ```google:client-id``` and the ```google:client-secret```. You will need to create a OAuth 2.0 Client with Type "Web Application".

##### 4. Run the application
You should be able to run the application at this step, using Visual Studio or the command line ```dotnet run```.

#### Running the Projects and TimeSession project
##### 1. Create the DB
- Create an empty ```C:\Repos\Temp\MyDb-dev-dev.db``` file, this will be our DB
> Note this static file path will be removed in a later step and configured accordingly.
- Open the solution in Visual Studio, and select the ```MicroManagement.Persistence.Migrations.SQLite``` project as startup project.
- Open the Nuget Package Manager console in Visual Studio and Select  ```MicroManagement.Persistence.Migrations.SQLite``` as Default Project.
- Run ```Update-Database```.
The migrations should get applied and your database will get created successfully.

##### 2. Set the application's JWT secrets
- Right click the ```MicroManagement.Service``` project and select *Manage User Secrets*.
- You will need to specify the secret related to the JWT signing and validation: ```Jwt:AccessKey```. It needs to be **the same** as it was set in the ```MicroManagement.Auth.WebAPI``` project.

##### 4. Run the application
You should be able to run the application at this step, using Visual Studio or the command line ```dotnet run```.
