# Use the official .NET 8 SDK image from Microsoft as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the solution file and project files first to restore dependencies
COPY ./My-Micro-Management.sln ./

# Copy the remaining source files
COPY . ./

# Restore dependencies only
RUN dotnet restore ./My-Micro-Management.sln

# Publish the specific Web API project
RUN dotnet publish ./AuthenticationService/MicroManagement.Auth.WebAPI/MicroManagement.Auth.WebAPI.csproj -c Release -o /out

# Use the runtime image for a smaller final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
RUN apt-get update && apt-get install -y curl

# Set the working directory inside the container
WORKDIR /app

# Copy the build output from the previous stage
COPY --from=build /out .

# Expose the port the application will run on
EXPOSE 82

# Define the entry point for the container
ENTRYPOINT ["dotnet", "MicroManagement.Auth.WebAPI.dll", "--urls", "http://0.0.0.0:82"]
