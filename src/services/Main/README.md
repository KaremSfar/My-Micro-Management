
# Main Service

This folder contains the Main microservice for My-Micro-Management. It orchestrates core business logic, aggregates data from other services, and manages user data.

## Purpose
Acts as the central orchestrator for business workflows, aggregates data from Activity and Auth services, and exposes APIs for the web client.

## Main Features
- Aggregate user and activity data
- Expose unified API endpoints for frontend
- Coordinate workflows across microservices
- Integrate with RabbitMQ for messaging

## API Endpoints
- `GET /api/TimeSessions` — Get all time sessions for the current user
- `POST /api/TimeSessions/start` — Start a new time session for a project
- `POST /api/TimeSessions/stop` — Stop the current time session
- `GET /api/Projects` — Get all projects for the current user
- `POST /api/Projects` — Add a new project for the current user

## Folder Structure
- `MicroManagement.Service/` - Main service logic
- `MicroManagement.Service.Abstractions/` - Shared interfaces
- `MicroManagement.DataContracts/` - DTOs and contracts
- `Persistence/` - Data persistence

## Technologies
- C#/.NET
- RabbitMQ

## Usage
Configure environment variables for RabbitMQ and database in `docker-compose.yml`. Build and run using Docker or .NET CLI. See main project README.md for setup instructions.
