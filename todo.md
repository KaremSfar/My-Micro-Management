- [x] GET BACK ON TRACK: local development experience back on track: local services + web + dockers in 2-3 clicks 

- [x] Add Open Telemetry and bundle Graphana / or stuff like that with it
- [x] Add Nginx routing config to docker and source control

- [x] Deploy and Run Bolt Diy 

- [] Redo Readme

- [x] Make sure timers and connections are not lost when switching pages

- [x] Introduce the Event Bus stuff to stop time sessions
- [x] User connections state service
    - [x] Add new Web API Authenticated
    - [x] Link With Events Smth (Redis Stream; RabbitMQ etc)
    - [x] Add UserActions Controller
        => Active connections ?
    - [x] Time since last connections => Closes current TimeSessions (buggy)

- [x] Dockerize - NGINX - Deploy - test - changes after activity service

- [] Add Pomodoro to the App 
    - [x] Sticky Pomodoro Widget (maybe in the navbar)
    - [x] On Pomodoro Pause, pauses the timers and restarts on itself
    - [ ] On Pomodoro Next Clicks - Auto Start Pomodoro and Projects


- [x] Fix Timers Ticks (Main + Pomodoro)

- [x] Add Initial Crude Analytics page
    - [x] Empty TimeTable
    - [x] Time Table with time sessions
    - [x] Colored, can be focused TimeSessions
    - [x] Fix 401 session issue => i think fixed with websockets ?

- [x] FIX TIME SESSION MODEL (one project  only)
- [x] Fix the event not reconnecting and the access token not refreshing 

- [x] Go back to using Websockets lol
- [] Sync Pomodoro Across devices as well

## NEXT
- [ ] Turn into Progressive web app to install
- [ ] Add Context Concept
- [ ] Make clients super dumb, only listen to ws and call http
- [ ] Update Auth Service - use ready methods 
- [x] Update Time Sessions feature 
- [ ] Use Remote Caches to avoid building everything everytime
- [ ] On Pomodoro Paused projects, actually pause, do not restart the whole project !
    - [ ] Start by implementing a Pause feature on the Projects card (Service changes :( ) 
- [ ] Add Migration creator automation
- [ ] Add Unit Tests :)
- [ ] Swap datetimes with DateTimeOffsets
- [ ] Update Open Telemetry Stuff - Use Collector and enrich
- [ ] Clean up env variables mess and only keep needed ones in github vs needed ones in VPS (find a secrets manager)