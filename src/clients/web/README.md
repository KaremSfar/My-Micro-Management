
# Web Client

This folder contains the source code for the main web client of My-Micro-Management. It provides the user interface for authentication, activity management, dashboards, and more.

## Purpose
Delivers a modern, responsive UI for users to interact with all core features of the platform. Communicates with backend services via REST APIs and is served via Nginx in production.

## Main Features
- User authentication (login, registration, Google OAuth)
- Activity and event management
- Dashboard and analytics views
- Responsive design with Tailwind CSS
- Fast development and HMR with Vite

## Folder Structure
- `src/` - Main source code
  - `Auth/` - Authentication pages and logic
  - `Components/` - Reusable UI components
  - `Pages/` - Application views/pages
  - `context/` - React context providers
  - `hooks/` - Custom React hooks
  - `Models/` - TypeScript models/interfaces
  - `DTOs/` - Data transfer objects
- `public/` - Static assets (favicon, manifest, etc.)
- `Dockerfile` - Container build instructions
- `nginx.conf` - Nginx configuration for serving the app

## Technologies
- React
- Vite
- TypeScript
- Tailwind CSS

## Usage
Install dependencies with `npm install`. Run locally with `npm run dev`. For production, build with `npm run build` and serve via Nginx or Docker. See main project README.md for setup instructions.
