
# Activity Service

This folder contains the Activity microservice for My-Micro-Management. It manages user activities, events, and persistence using Redis.

## Purpose
Handles creation, tracking, and management of user activities and events. Integrates with Redis for fast data access and RabbitMQ for inter-service communication.

## Main Features
- Create, update, and delete activities
- Track user events and activity logs
- Real-time updates via RabbitMQ
- Fast data access and caching with Redis

## API Endpoints
- `GET /events` — Server-Sent Events stream for user activity events

## Folder Structure
- `MicroManagement.Activity.WebAPI/` - Main web API project
- `MicroManagement.Activity.Abstractions/` - Shared interfaces and contracts
- `Persistence/` - Data persistence and migrations

## Technologies
- C#/.NET
- Redis
- RabbitMQ

## Usage
Configure environment variables for Redis and RabbitMQ in `docker-compose.yml`. Build and run using Docker or .NET CLI. See main project README.md for setup instructions.
