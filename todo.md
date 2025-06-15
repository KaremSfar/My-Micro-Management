- [x] Add Open Telemetry and bundle Graphana / or stuff like that with it
- [x] Add Nginx routing config to docker and source control
- [ ] Next? Use Remote Caches to avoid building everything everytime

- [x] Deploy and Run Bolt Diy 

- [] Add Context Concept
- [x] Make sure timers and connections are not lost when switching pages

- [] Introduce the Event Bus stuff
- [] User connections state service
    => Active connections ?
    => Time since last connections => Closes current TimeSessions

- [] Add Pomodoro to the App 
    - [x] Sticky Pomodoro Widget (maybe in the navbar)
    - [x] On Pomodoro Pause, pauses the timers and restarts on itself
    - [ ] On Pomodoro Next Clicks - Auto Start Pomodoro and Projects
    - [ ] On Pomodoro Paused projects, actually pause, do not restart the whole project !
        - [ ] Start by implementing a Pause feature on the Projects card (Service changes :( ) 
