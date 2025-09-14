
# Auth Service

This folder contains the Authentication microservice for My-Micro-Management. It handles user authentication, JWT issuance, and OAuth integrations (Google).

## Purpose
Manages user login, registration, token issuance, and third-party authentication. Secures API endpoints and provides user identity to other services.

## Main Features
- User registration and login
- JWT access and refresh token management
- Google OAuth integration
- Role-based authorization

## API Endpoints
- `POST /Auth/register` — Register a new user
- `POST /Auth/login` — Login with email and password
- `POST /Auth/logout` — Logout user
- `POST /Auth/refresh-token` — Refresh JWT tokens
- `GET /google-login` — Initiate Google OAuth login
- `GET /google-callback` — Google OAuth callback

## Folder Structure
- `MicroManagement.Auth.WebAPI/` - Main web API project
- `MicroManagement.Auth.DataContracts/` - DTOs and contracts
- `MicroManagement.Auth.EFDbContext/` - Entity Framework DB context
- `Persistence/` - Database migrations

## Technologies
- C#/.NET
- Entity Framework
- JWT

## Usage
Configure database and JWT secrets in `docker-compose.yml` or environment variables. Build and run using Docker or .NET CLI. See main project README.md for setup instructions.
