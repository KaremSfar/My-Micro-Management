# Use the official .NET 9 SDK image from Microsoft as the build environment
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory inside the container
WORKDIR /app

COPY Auth/MicroManagement.Auth.DataContracts/MicroManagement.Auth.DataContracts.csproj ./Auth/MicroManagement.Auth.DataContracts/
COPY Auth/MicroManagement.Auth.Migrations.AzureSQL/MicroManagement.Auth.Migrations.AzureSQL.csproj ./Auth/MicroManagement.Auth.Migrations.AzureSQL/
COPY Auth/MicroManagement.Auth.Migrations.SQLite/MicroManagement.Auth.Migrations.SQLite.csproj ./Auth/MicroManagement.Auth.Migrations.SQLite/
COPY Auth/MicroManagement.Auth.Migrations.SQLite/MicroManagement.Auth.Migrations.SQLite.csproj ./Auth/MicroManagement.Auth.Migrations.SQLite/
COPY Auth/MicroManagement.Auth.Migrations.Postgres/MicroManagement.Auth.Migrations.Postgres.csproj ./Auth/MicroManagement.Auth.Migrations.Postgres/
COPY Auth/MicroManagement.Auth.EFDbContext/MicroManagement.Auth.EFDbContext.csproj ./Auth/MicroManagement.Auth.EFDbContext/

COPY Core/MicroManagement.Shared.Startup/MicroManagement.Shared.Startup.csproj ./Core/MicroManagement.Shared.Startup/
COPY Auth/MicroManagement.Auth.WebAPI/MicroManagement.Auth.WebAPI.csproj ./Auth/MicroManagement.Auth.WebAPI/

# Copy the solution file and project files first to restore dependencies
COPY ./Auth/MicroManagement.Authentication.Service.sln ./Auth/

# Restore dependencies only
RUN dotnet restore ./Auth/MicroManagement.Authentication.Service.sln

# Copy the remaining source files
COPY . ./

# Publish the specific Web API project
RUN dotnet publish ./Auth/MicroManagement.Auth.WebAPI/MicroManagement.Auth.WebAPI.csproj -c Release -o /out

# Use the runtime image for a smaller final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the build output from the previous stage
COPY --from=build /out .

# Expose the port the application will run on
EXPOSE 81

# Define the entry point for the container
ENTRYPOINT ["dotnet", "MicroManagement.Auth.WebAPI.dll", "--urls", "http://0.0.0.0:81"]
