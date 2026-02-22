- [ ] Swap datetimes with DateTimeOffsets

## Developer Experience
- [ ] Add Unit Tests :)
- [ ] Add Migration creator automation

## NEXT
- [ ] Sync Pomodoro Across devices as well
- [ ] Redo Readme
- [ ] Turn into Progressive web app to install

## Bugs
- [ ] Fix Time Sync across devices (minor milliseconds)
- [ ] Resiliency++ (The Inactivity service)
- [ ] User Creation should create a default Context with it

## Big Features / Epics
- [ ] Sync Pomodoro Across devices as well
- [ ] Add mail stuff (for Auth endpoints) - if using .NET baked in
- [ ] CLI app
- [ ] Screenshots cli service, connect with Agent 
- [ ] Outside: Create an n8n agent for Work (Scrum ceremonies, Planner ...) And integrate with mmgmt
- [ ] On Pomodoro Paused projects, actually pause, do not restart the whole project !
    - [ ] Start by implementing a Pause feature on the Projects card (Service changes :( ) 


## History
---
- [x] Context Stuff
- [x] Add Context Concept

--- 
- [x] Extract .env files somewhere, maybe self hosted
    => Infisical
- [x] Get rid of the docker compose files found everywhere - add a prelaunch task for the Services
- [x] Telemetry should be on even for local devving
- [x] Update Open Telemetry Stuff - Use Collector and enrich
    - [x] Try out signoz and consider migrating (or grafana stack)
- [x] Use Remote Caches to avoid building everything everytime
- [x] Clean up projects - get rid of old migration stuff and empty projects

- [x] Why arent websockets WORKIIING ??
    => Combination of wrong header upgrade + forgot to add .UseWebSockets in Startup
- [x] Fix 401 token staleness (Page reloads on token refresh)
- [x] Console errors Websockets

- [x] Basics: Auth, SSO, Deploying 
- [x] Time sessions and Projects creating etc..


- [x] Change Web site title and icon
- [x] Fix deployment on new VPS
---
- [x] GET BACK ON TRACK: local development experience back on track: local services + web + dockers in 2-3 clicks 

- [x] Add Open Telemetry and bundle Graphana / or stuff like that with it
- [x] Add Nginx routing config to docker and source control

- [x] Deploy and Run Bolt Diy => mega flop

- [x] Make sure timers and connections are not lost when switching pages

- [x] Introduce the Event Bus stuff to stop time sessions
- [x] User connections state service
    - [x] Add new Web API Authenticated
    - [x] Link With Events Smth (Redis Stream; RabbitMQ etc)
    - [x] Add UserActions Controller
        => Active connections ?
    - [x] Time since last connections => Closes current TimeSessions (buggy)

- [x] Dockerize - NGINX - Deploy - test - changes after activity service

- [x] Add Pomodoro to the App 
    - [x] Sticky Pomodoro Widget (maybe in the navbar)
    - [x] On Pomodoro Pause, pauses the timers and restarts on itself
    - [x] On Pomodoro Next Clicks - Auto Start Pomodoro and Projects


- [x] Fix Timers Ticks (Main + Pomodoro)

- [x] Add Initial Crude Analytics page
    - [x] Empty TimeTable
    - [x] Time Table with time sessions
    - [x] Colored, can be focused TimeSessions
    - [x] Fix 401 session issue => i think fixed with websockets ?

- [x] FIX TIME SESSION MODEL (one project  only)
- [x] Fix the event not reconnecting and the access token not refreshing 

- [x] Go back to using Websockets lol
- [x] Switch domain names
- [x] Make clients super dumb, only listen to ws and call http
- [x] Update Time Sessions feature 
---