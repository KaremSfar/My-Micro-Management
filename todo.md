- [ ] Update Open Telemetry Stuff - Use Collector and enrich
- [ ] Sync Pomodoro Across devices as well
- [ ] Update Auth Service - use ready methods 
- [ ] Turn into Progressive web app to install
- [ ] Add Migration creator automation
- [ ] Clean up env variables mess and only keep needed ones in github vs needed ones in VPS (find a secrets manager)
- [ ] Use Remote Caches to avoid building everything everytime
- [ ] Add Unit Tests :)
- [ ] Swap datetimes with DateTimeOffsets
- [x] Change Web site title and icon

## NEXT
- [ ] Add Context Concept
- [ ] On Pomodoro Paused projects, actually pause, do not restart the whole project !
    - [ ] Start by implementing a Pause feature on the Projects card (Service changes :( ) 
- [ ] Redo Readme

## Bugs
- [ ] Fix 401 token staleness
- [ ] Console errors Websockets

## Big Features / Epics
- [x] Basics: Auth, SSO, Deploying 
- [x] Time sessions and Projects creating etc..
- [ ] Add mail stuff (for Auth endpoints)
- [ ] Context Stuff
- [ ] CLI app
- [ ] Screenshots cli service, connect with Agent 
- [ ] Outside: Create an n8n agent for Work (Scrum ceremonies, Planner ...) And integrate with mmgmt


## History
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
----