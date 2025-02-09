# Use the official .NET 9 SDK image from Microsoft as the build environment
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy csproj files and restore as distinct layers
COPY Main/MicroManagement.DataContracts/MicroManagement.DataContracts.csproj ./Main/MicroManagement.DataContracts/
COPY Main/MicroManagement.Service/MicroManagement.Service.WebAPI.csproj ./Main/MicroManagement.Service/
COPY Main/MicroManagement.Service.Abstractions/MicroManagement.Service.Abstractions.csproj ./Main/MicroManagement.Service.Abstractions/
COPY Main/Persistence/MicroManagement.Persistence.Abstraction/MicroManagement.Persistence.Abstraction.csproj ./Main/Persistence/MicroManagement.Persistence.Abstraction/
COPY Main/Persistence/MicroManagement.Persistence.EF/MicroManagement.Persistence.EF.csproj ./Main/Persistence/MicroManagement.Persistence.EF/
COPY Main/Persistence/MicroManagement.Persistence.Migrations.AzureSQL/MicroManagement.Persistence.Migrations.AzureSQL.csproj ./Main/Persistence/MicroManagement.Persistence.Migrations.AzureSQL/
COPY Main/Persistence/MicroManagement.Persistence.SQLite.MigrationsApplier/MicroManagement.Persistence.Migrations.SQLite.csproj ./Main/Persistence/MicroManagement.Persistence.SQLite.MigrationsApplier/
COPY Core/MicroManagement.Core/MicroManagement.Core.csproj ./Core/MicroManagement.Core/

COPY Main/MicroManagement.Service.sln ./Main/

RUN dotnet restore ./Main/MicroManagement.Service.sln

# Copy the entire solution and restore dependencies
COPY . ./

# Publish the specific Web API project
RUN dotnet publish ./Main/MicroManagement.Service/MicroManagement.Service.WebAPI.csproj -c Release -o /out

# Use the runtime image for a smaller final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the build output from the previous stage
COPY --from=build /out .

# Expose the port the application will run on
EXPOSE 81

# Define the entry point for the container
ENTRYPOINT ["dotnet", "MicroManagement.Service.WebAPI.dll", "--urls", "http://0.0.0.0:81"]