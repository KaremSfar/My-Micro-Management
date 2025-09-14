# Comprehensive Project Architecture Blueprint

This blueprint analyzes the codebase to provide a definitive reference for architectural consistency, extensibility, and onboarding. Update as the codebase evolves.

## 1. Architecture Detection and Analysis

### Technology Stacks Detected
- **.NET (C#)**: Backend microservices (Activity, Auth, Main)
- **React + Vite + TypeScript**: Frontend web client
- **Redis, RabbitMQ, PostgreSQL**: Data stores and messaging
- **Nginx**: Reverse proxy
- **Jaeger**: Distributed tracing
- **Docker Compose**: Deployment orchestration

### Architectural Pattern(s)
- **Microservices**: Each major domain is a separate service (Activity, Auth, Main)
- **Layered Architecture**: Each .NET service uses layers (WebAPI, Abstractions, Persistence)
- **Event-Driven**: RabbitMQ for inter-service communication
- **API Gateway/Reverse Proxy**: Nginx routes requests to services

## 2. Architectural Overview
- **Guiding Principles**: Separation of concerns, scalability, extensibility, clear service boundaries
- **Boundaries**: Each service is independently deployable, communicates via HTTP or messaging
- **Hybrid Patterns**: Layered microservices, event-driven workflows

## 3. Architecture Visualization (Textual)

[User] <--> [Web Client] <--> [Nginx] <--> [Auth Service] <--> [Postgres]
                                 |           |
                                 |           +--> [Jaeger]
                                 |           +--> [Redis]
                                 |           +--> [RabbitMQ]
                                 +--> [Main Service] <--> [RabbitMQ]
                                 +--> [Activity Service] <--> [Redis]

## 4. Core Architectural Components

### 4.1. Web Client (React)
- **Purpose**: User interface for authentication, activity management, dashboards
- **Internal Structure**: Components, pages, services, state management
- **Interaction**: REST API calls to backend, JWT for auth
- **Extensibility**: Add new pages/components, extend API services

### 4.2. Auth Service (.NET)
- **Purpose**: Authentication, JWT issuance, OAuth
- **Internal Structure**: Controllers, services, data contracts, persistence
- **Interaction**: REST endpoints, communicates with Postgres, issues JWT
- **Extensibility**: Add new auth providers, extend user model
- **API Endpoints**:
    - POST /Auth/register
    - POST /Auth/login
    - POST /Auth/logout
    - POST /Auth/refresh-token
    - GET /google-login
    - GET /google-callback

### 4.3. Main Service (.NET)
- **Purpose**: Core business logic, aggregates data
- **Internal Structure**: Controllers, services, data contracts, persistence
- **Interaction**: REST endpoints, RabbitMQ, aggregates from Activity/Auth
- **Extensibility**: Add new business workflows, extend data contracts
- **API Endpoints**:
    - GET /api/TimeSessions
    - POST /api/TimeSessions/start
    - POST /api/TimeSessions/stop
    - GET /api/Projects
    - POST /api/Projects

### 4.4. Activity Service (.NET)
- **Purpose**: Manages activities/events, persistence in Redis
- **Internal Structure**: Controllers, event streaming, persistence
- **Interaction**: SSE endpoint, RabbitMQ, Redis
- **Extensibility**: Add new event types, extend activity model
- **API Endpoints**:
    - GET /events

### 4.5. Supporting Components
- **Jaeger**: Tracing
- **RabbitMQ**: Messaging
- **Redis**: Caching, fast data
- **Postgres**: Relational data
- **Nginx**: Routing

## 5. Architectural Layers and Dependencies
- **Layered Structure**: WebAPI (controllers) → Services → Abstractions → Persistence
- **Dependency Rules**: Controllers depend on services, services on abstractions, abstractions on persistence
- **Abstraction Mechanisms**: Interfaces, DTOs
- **Dependency Injection**: .NET DI container
- **No circular dependencies detected**

## 6. Data Architecture
- **Domain Models**: Users, Activities, Events, Projects, TimeSessions
- **Entity Relationships**: Users own projects, projects have activities/events
- **Data Access Patterns**: Repository pattern in .NET services
- **Data Transformation**: DTOs for API contracts
- **Caching**: Redis for fast access
- **Validation**: Model validation in controllers/services

## 7. Cross-Cutting Concerns Implementation
- **Authentication & Authorization**: JWT, OAuth2, role-based checks
- **Error Handling & Resilience**: Exception filters, retries, circuit breakers (recommended)
- **Logging & Monitoring**: Jaeger tracing, recommend structured logging
- **Validation**: Model validation, DTO validation
- **Configuration Management**: Environment variables, Docker secrets

## 8. Service Communication Patterns
- **Boundaries**: Each service exposes REST endpoints
- **Protocols**: HTTP (REST), AMQP (RabbitMQ)
- **Sync/Async**: Sync via HTTP, async via RabbitMQ
- **API Versioning**: Not detected, recommend for future
- **Service Discovery**: Static in Docker Compose
- **Resilience**: RabbitMQ for decoupling, retries recommended

## 9. Technology-Specific Architectural Patterns

### .NET
- Host/application model, DI container, middleware pipeline
- Entity Framework for ORM
- Controllers for API

### React
- Component-based, hooks, context/state management
- API service layer for backend communication

## 10. Implementation Patterns
- **Interface Design**: Segregated interfaces, DTOs
- **Service Implementation**: Scoped lifetimes, DI
- **Repository Implementation**: EF repositories
- **Controller/API**: Attribute routing, model binding
- **Domain Model**: Entities, value objects

## 11. Testing Architecture
- **Strategies**: Unit (xUnit/NUnit), integration, E2E (frontend)
- **Test Doubles**: Mocks, stubs
- **Test Data**: Seed data, fixtures
- **Tools**: Jest (frontend), xUnit/NUnit (backend)

## 12. Deployment Architecture
- **Topology**: Docker Compose, each service/container
- **Environment Adaptation**: Env vars, Docker secrets
- **Runtime Dependency**: Container links
- **Orchestration**: Docker Compose
- **Cloud Integration**: Not detected, adaptable

## 13. Extension and Evolution Patterns
- **Feature Addition**: Add new service or controller, extend DTOs
- **Modification**: Update service logic, maintain API contracts
- **Integration**: Add adapters, use RabbitMQ for new events
- **Configuration**: Extend env vars, Docker secrets

## 14. Architecture Governance
- **Consistency**: Layered structure, DI, DTOs
- **Automated Checks**: Recommend static analysis, code review
- **Documentation**: Update ARCHITECTURE.md, READMEs

## 15. Blueprint for New Development
- **Workflow**: Create new service/component in src/services, follow layered pattern
- **Templates**: Use existing service/controller/repository as template
- **Pitfalls**: Avoid circular dependencies, keep API contracts stable
- **Testing**: Add unit/integration tests for new features
- **Documentation**: Update ARCHITECTURE.md and service README

---

# Architecture Overview
This document serves as a critical, living template designed to equip agents with a rapid and comprehensive understanding of the codebase's architecture, enabling efficient navigation and effective contribution from day one. Update this document as the codebase evolves.

## 1. Project Structure
This section provides a high-level overview of the project's directory and file structure, categorised by architectural layer or major functional area. It is essential for quickly navigating the codebase, locating relevant files, and understanding the overall organization and separation of concerns.

[Project Root]/
├── src/                      # Main source code for all services and clients
│   ├── clients/              # Frontend web client
│   │   └── web/              # Web client (React + Vite)
│   │       ├── src/          # Frontend source code
│   │       ├── public/       # Static assets
│   │       └── ...           # Configs, Dockerfiles
│   └── services/             # Backend microservices
│       ├── Activity/         # Activity service (C#/.NET)
│       ├── Auth/             # Authentication service (C#/.NET)
│       ├── Core/             # Core shared logic
│       └── Main/             # Main service (C#/.NET)
├── devops/                   # DevOps scripts and automation
├── docs/                     # Project documentation and images
├── docker-compose.yml        # Main Docker Compose file
├── nginx.conf                # Nginx reverse proxy configuration
├── ARCHITECTURE.md           # This document
├── README.md                 # Project overview and quick start guide
└── ...                       # Other config and support files

## 2. High-Level System Diagram

[User] <--> [Web Client] <--> [Nginx] <--> [Auth Service] <--> [Postgres]
                                 |           |
                                 |           +--> [Jaeger]
                                 |           +--> [Redis]
                                 |           +--> [RabbitMQ]
                                 +--> [Main Service] <--> [RabbitMQ]
                                 +--> [Activity Service] <--> [Redis]

## 3. Core Components

### 3.1. Frontend

Name: Web Client
Description: The main user interface for interacting with the system, allowing users to authenticate, manage activities, and view dashboards. Built with React and Vite, styled with Tailwind CSS.
Technologies: React, Vite, Tailwind CSS, TypeScript
Deployment: Docker container, served via Nginx

### 3.2. Backend Services

#### 3.2.1. Auth Service
Name: Authentication Service
Description: Handles user authentication, JWT issuance, and OAuth integrations (e.g., Google).
Technologies: C#/.NET, Entity Framework
Deployment: Docker container
API Endpoints:
    - POST /Auth/register — Register a new user
    - POST /Auth/login — Login with email and password
    - POST /Auth/logout — Logout user
    - POST /Auth/refresh-token — Refresh JWT tokens
    - GET /google-login — Initiate Google OAuth login
    - GET /google-callback — Google OAuth callback

#### 3.2.2. Main Service
Name: Main MicroManagement Service
Description: Core business logic, orchestrates activities and user data.
Technologies: C#/.NET
Deployment: Docker container
API Endpoints:
    - GET /api/TimeSessions — Get all time sessions for the current user
    - POST /api/TimeSessions/start — Start a new time session for a project
    - POST /api/TimeSessions/stop — Stop the current time session
    - GET /api/Projects — Get all projects for the current user
    - POST /api/Projects — Add a new project for the current user

#### 3.2.3. Activity Service
Name: Activity Service
Description: Manages user activities, events, and persistence (Redis).
Technologies: C#/.NET, Redis
Deployment: Docker container
API Endpoints:
    - GET /events — Server-Sent Events stream for user activity events

#### 3.2.4. Supporting Services
- Jaeger: Distributed tracing
- RabbitMQ: Message broker for inter-service communication
- Redis: Caching and fast data store
- Postgres: Main relational database
- Nginx: Reverse proxy and static asset serving

## 4. Data Stores

### 4.1. PostgreSQL
Name: Main Database
Type: PostgreSQL
Purpose: Stores persistent user data, activities, and core business entities.
Key Schemas/Collections: users, activities, events

### 4.2. Redis
Name: Cache & Fast Data Store
Type: Redis
Purpose: Caching, fast access to activity/event data, session management.

### 4.3. RabbitMQ
Name: Message Queue
Type: RabbitMQ
Purpose: Inter-service communication, event-driven workflows.

## 5. External Integrations / APIs

- Google OAuth: User authentication via Google
- Jaeger: Distributed tracing

## 6. Deployment & Infrastructure

Cloud Provider: Not specified (Docker-based, can be deployed on any cloud or on-premise)
Key Services Used: Docker, Docker Compose, Nginx, Postgres, Redis, RabbitMQ, Jaeger
CI/CD Pipeline: Not specified (recommend GitHub Actions or similar)
Monitoring & Logging: Jaeger (tracing), recommend Prometheus/Grafana for metrics

## 7. Security Considerations

Authentication: JWT, OAuth2 (Google)
Authorization: Role-based (details in code)
Data Encryption: TLS in transit (via Nginx), recommend DB encryption at rest
Key Security Tools/Practices: Environment variables for secrets, Docker isolation

## 8. Development & Testing Environment

Local Setup Instructions: See README.md
Testing Frameworks: Jest (frontend), xUnit/NUnit (backend)
Code Quality Tools: ESLint (frontend), .NET analyzers (backend)

## 9. Future Considerations / Roadmap

- Expand microservices for additional domains
- Implement event-driven architecture for real-time updates
- Add more external integrations (e.g., payment, notifications)

## 10. Project Identification

Project Name: My-Micro-Management
Repository URL: https://github.com/KaremSfar/My-Micro-Management
Primary Contact/Team: Karem Sfar
Date of Last Update: 2025-09-14

## 11. Glossary / Acronyms

- JWT: JSON Web Token
- OAuth2: Open Authorization 2.0
- RBAC: Role-Based Access Control
- CI/CD: Continuous Integration / Continuous Deployment
- API: Application Programming Interface
- DTO: Data Transfer Object
