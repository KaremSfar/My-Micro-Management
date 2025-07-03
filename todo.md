- [x] Add Open Telemetry and bundle Graphana / or stuff like that with it
- [x] Add Nginx routing config to docker and source control

- [x] Deploy and Run Bolt Diy 

- [] Redo Readme

- [] Add Context Concept
- [x] Make sure timers and connections are not lost when switching pages

- [] Introduce the Event Bus stuff to stop time sessions
- [] User connections state service
    - [ ] Add new Web API Authenticated
    - [ ] Link With Events Smth (Redis Stream; RabbitMQ etc)
    - [ ] Add UserActions Controller (Start TimeSession, Stop TimeSession, Create TimeSession)
    => Active connections ?
    => Time since last connections => Closes current TimeSessions


- [] Add Pomodoro to the App 
    - [x] Sticky Pomodoro Widget (maybe in the navbar)
    - [x] On Pomodoro Pause, pauses the timers and restarts on itself
    - [ ] On Pomodoro Next Clicks - Auto Start Pomodoro and Projects


- [x] Fix Timers Ticks (Main + Pomodoro)

- [ ] Add Initial Crude Analytics page
    - [x] Empty TimeTable
    - [x] Time Table with time sessions
    - [ ] Colored, can be focused TimeSessions
    - [ ] Fix 401 session issue

- [ ] Update Auth Service - use ready methods 
- [x] FIX TIME SESSION MODEL (one project  only)


## NEXT
- [ ] Update Time Sessions feature 
- [ ] Use Remote Caches to avoid building everything everytime
- [ ] On Pomodoro Paused projects, actually pause, do not restart the whole project !
    - [ ] Start by implementing a Pause feature on the Projects card (Service changes :( ) 
- [ ] Add Migration creator automation
