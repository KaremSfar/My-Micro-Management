import React, { useState, useEffect } from 'react';
import { PlayIcon, StopIcon, ForwardIcon } from '@heroicons/react/24/outline';
import { useProjectContext } from '../context/ProjectContext';

const FOCUS_DURATION = 25 * 60; // 25 minutes in seconds
const SHORT_BREAK_DURATION = 5 * 60; // 5 minutes in seconds
const LONG_BREAK_DURATION = 15 * 60; // 15 minutes in seconds
const SESSIONS_FOR_LONG_BREAK = 4;

type Mode = 'focus' | 'shortBreak' | 'longBreak';

const PomodoroWidget: React.FC = () => {
    const { 
        runningProjectId, 
        pausedProjectId, 
        handleProjectClick, 
        setPausedProjectId 
    } = useProjectContext();

    const [currentMode, setCurrentMode] = useState<Mode>('focus');
    const [timeLeft, setTimeLeft] = useState<number>(FOCUS_DURATION);
    const [isActive, setIsActive] = useState<boolean>(false);
    const [pomodoroCount, setPomodoroCount] = useState<number>(0);

    // Timer effect
    useEffect(() => {
        let interval: NodeJS.Timeout | null = null;

        if (isActive && timeLeft > 0) {
            interval = setInterval(() => {
                setTimeLeft((prevTime) => prevTime - 1);
            }, 1000);
        } else if (timeLeft === 0 && isActive) {
            handleNextPhase();
        }

        return () => {
            if (interval) {
                clearInterval(interval);
            }
        };
    }, [isActive, timeLeft, currentMode, pomodoroCount]);

    const handleStartPauseReset = () => {
        if (isActive && currentMode === 'focus') {
            // If active and in focus mode, reset the timer
            setIsActive(false);
            setTimeLeft(FOCUS_DURATION);
            // IMPORTANT: Do NOT stop the running project here.
            // The project should continue running in the background.
            // setPausedProjectId(null); // No need to clear pausedProjectId if not stopping project
        } else {
            // If not active, or in break mode, behave as a start/pause button
            if (isActive && runningProjectId) {
                setPausedProjectId(runningProjectId);
                handleProjectClick(runningProjectId);
            }
            setIsActive(!isActive);
        }
    };

    const handleNextPhase = () => {
        // Stop current running project if exists
        if (runningProjectId) {
            setPausedProjectId(runningProjectId);
            handleProjectClick(runningProjectId);
        }

        // Reset active state
        setIsActive(false);

        // Determine next phase logic
        if (currentMode === 'focus') {
            const newPomodoroCount = pomodoroCount + 1;
            setPomodoroCount(newPomodoroCount);
            
            if (newPomodoroCount % SESSIONS_FOR_LONG_BREAK === 0) {
                setCurrentMode('longBreak');
                setTimeLeft(LONG_BREAK_DURATION);
            } else {
                setCurrentMode('shortBreak');
                setTimeLeft(SHORT_BREAK_DURATION);
            }

            // Automatically start project if manually skipping from focus phase
            if (pausedProjectId) {
                handleProjectClick(pausedProjectId);
                setPausedProjectId(null);
                setIsActive(true);
            }
        } else { // currentMode was 'shortBreak' or 'longBreak'
            setCurrentMode('focus');
            setTimeLeft(FOCUS_DURATION);
        }
    };

    // Start timer and project together
    const startTimerAndProject = () => {
        // Only start project if in focus mode and timer is about to start
        if (currentMode === 'focus' && pausedProjectId && !isActive) {
            handleProjectClick(pausedProjectId);
            setPausedProjectId(null);
            setIsActive(true);
        }
    };

    const formatTime = (seconds: number): string => {
        const mins = Math.floor(seconds / 60);
        const secs = seconds % 60;
        return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
    };

    const getBackgroundColor = () => {
        if (currentMode === 'focus') {
            return 'bg-rose-500'; // Reddish
        }
        return 'bg-sky-500'; // Bluish
    };

    const isBreakMode = currentMode === 'shortBreak' || currentMode === 'longBreak';

    return (
        <div className={`flex items-center justify-between p-2 rounded-lg text-white min-w-[130px] ${getBackgroundColor()}`}>
            <span className="text-xl font-bold tabular-nums">
                {formatTime(timeLeft)}
            </span>
            {!(isActive && isBreakMode) && ( // Hide play button when active and in break mode
                <button
                    onClick={() => {
                        handleStartPauseReset();
                        // Only start project if we are actually starting the timer, not resetting
                        if (!isActive && currentMode === 'focus') { // Check if it's about to become active and in focus
                            startTimerAndProject();
                        }
                    }}
                    className="ml-2 p-1 bg-white/20 hover:bg-white/30 rounded focus:outline-none focus:ring-2 focus:ring-white/50"
                    aria-label={isActive && !isBreakMode ? "Reset timer" : "Start timer"}
                >
                    {isActive && !isBreakMode ? <StopIcon className="h-5 w-5"></StopIcon> : <PlayIcon className="h-5 w-5"></PlayIcon>}
                </button>
            )}
            <button
                onClick={handleNextPhase}
                className="ml-1 p-1 bg-white/20 hover:bg-white/30 rounded focus:outline-none focus:ring-2 focus:ring-white/50"
                aria-label="Next phase"
            >
                <ForwardIcon className="h-5 w-5"></ForwardIcon>
            </button>
        </div>
    );
};

export default PomodoroWidget;
