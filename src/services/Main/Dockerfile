# Use the official .NET 9 SDK image from Microsoft as the build environment
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy csproj files and restore as distinct layers
COPY MicroManagement.DataContracts/MicroManagement.DataContracts.csproj ./MicroManagement.DataContracts/
COPY MicroManagement.Service/MicroManagement.Service.WebAPI.csproj ./MicroManagement.Service/
COPY MicroManagement.Service.Abstractions/MicroManagement.Service.Abstractions.csproj ./MicroManagement.Service.Abstractions/
COPY Persistence/MicroManagement.Persistence.Abstraction/MicroManagement.Persistence.Abstraction.csproj ./Persistence/MicroManagement.Persistence.Abstraction/
COPY Persistence/MicroManagement.Persistence.EF/MicroManagement.Persistence.EF.csproj ./Persistence/MicroManagement.Persistence.EF/
COPY Persistence/MicroManagement.Persistence.Migrations.AzureSQL/MicroManagement.Persistence.Migrations.AzureSQL.csproj ./Persistence/MicroManagement.Persistence.Migrations.AzureSQL/
COPY Persistence/MicroManagement.Persistence.SQLite.MigrationsApplier/MicroManagement.Persistence.Migrations.SQLite.csproj ./Persistence/MicroManagement.Persistence.SQLite.MigrationsApplier/

RUN dotnet restore ./MicroManagement.Service.sln

# Copy the entire solution and restore dependencies
COPY . ./

# Publish the specific Web API project
RUN dotnet publish ./Service/MicroManagement.Service/MicroManagement.Service.WebAPI.csproj -c Release -o /out

# Use the runtime image for a smaller final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the build output from the previous stage
COPY --from=build /out .

# Expose the port the application will run on
EXPOSE 81

# Define the entry point for the container
ENTRYPOINT ["dotnet", "MicroManagement.Service.WebAPI.dll", "--urls", "http://0.0.0.0:81"]