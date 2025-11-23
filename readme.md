# My-Micro-Management

My-Micro-Management is a microservices-based productivity platform for time tracking, activity management, and analytics. It features a modern React web client and .NET backend services, orchestrated via Docker Compose and Nginx.

## Architecture Overview

- **Frontend:** React + Vite + TypeScript web client (see `src/clients/web`)
- **Backend Services:**
	- **Auth Service:** Authentication, JWT, Google OAuth (`src/services/Auth`)
		- Endpoints: `/Auth/register`, `/Auth/login`, `/Auth/logout`, `/Auth/refresh-token`, `/google-login`, `/google-callback`
	- **Main Service:** Core business logic, time sessions, projects (`src/services/Main`)
		- Endpoints: `/api/TimeSessions`, `/api/Projects`
	- **Activity Service:** Activity/event management, SSE (`src/services/Activity`)
		- Endpoint: `/events`
- **Supporting Components:** Redis, RabbitMQ, PostgreSQL, Jaeger, Nginx

## Quick Start

1. **Clone the repository:**
	 ```bash
	 git clone https://github.com/KaremSfar/My-Micro-Management.git
	 cd My-Micro-Management
	 ```

2. **Build and start all services (development):**
	 ```bash
	 docker compose -f docker-compose.yml -f docker-compose.dev.yml up --build
	 ```

3. **Access the web client:**
	 - Open [http://localhost:8080](http://localhost:8080) in your browser (default Nginx port)

## Web Client (React)

- Source: `src/clients/web`
- Features: Authentication (email/password, Google OAuth), dashboard, analytics, project/activity management
- Local development:
	```bash
	cd src/clients/web
	npm install
	npm run dev
	```

## Backend Services

- **Auth Service:** Handles user registration, login, JWT, Google OAuth
- **Main Service:** Manages time sessions, projects, aggregates data
- **Activity Service:** Streams user activities/events via SSE

## Technologies

- React, Vite, TypeScript, Tailwind CSS
- .NET (C#), Entity Framework
- Redis, RabbitMQ, PostgreSQL
- Nginx, Docker Compose, Jaeger

## API Endpoints

### Auth Service
- `POST /Auth/register` — Register user
- `POST /Auth/login` — Login
- `POST /Auth/logout` — Logout
- `POST /Auth/refresh-token` — Refresh JWT
- `GET /google-login` — Google OAuth login
- `GET /google-callback` — Google OAuth callback

### Main Service
- `GET /api/TimeSessions` — List time sessions
- `POST /api/TimeSessions/start` — Start session
- `POST /api/TimeSessions/stop` — Stop session
- `GET /api/Projects` — List projects
- `POST /api/Projects` — Add project

### Activity Service
- `GET /events` — Server-Sent Events stream

## Documentation

- [ARCHITECTURE.md](ARCHITECTURE.md): Detailed architecture, technology stack, and service interactions
- [src/clients/web/README.md](src/clients/web/README.md): Web client details
- Each service folder contains its own README and API details

## Security & Best Practices

- JWT authentication, OAuth2 (Google)
- Role-based authorization
- Environment variables for secrets
- TLS via Nginx (recommended for production)

## Contributing

See [ARCHITECTURE.md](ARCHITECTURE.md) for development patterns, service templates, and extension guidelines.

## License

MIT