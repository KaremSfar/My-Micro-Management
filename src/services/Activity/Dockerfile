# Use the official .NET 9 SDK image from Microsoft as the build environment
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

# Set the working directory inside the container
WORKDIR /app

COPY Activity/MicroManagement.Activity.WebAPI/MicroManagement.Activity.WebAPI.csproj ./Activity/MicroManagement.Activity.WebAPI/
COPY Core/MicroManagement.Shared.Events/MicroManagement.Shared.Events.csproj ./Core/MicroManagement.Shared.Events/

# Copy the solution file and project files first to restore dependencies
COPY Activity/MicroManagement.Activity.sln ./Activity/

# Restore dependencies only
RUN dotnet restore ./Activity/MicroManagement.Activity.sln

# Copy the remaining source files
COPY . ./

# Publish the specific Web API project
RUN dotnet publish ./Activity/MicroManagement.Activity.WebAPI/MicroManagement.Activity.WebAPI.csproj -c Release -o /out

# Use the runtime image for a smaller final image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the build output from the previous stage
COPY --from=build /out .

# Expose the port the application will run on
EXPOSE 81

# Define the entry point for the container
ENTRYPOINT ["dotnet", "MicroManagement.Activity.WebAPI.dll", "--urls", "http://0.0.0.0:81"]
