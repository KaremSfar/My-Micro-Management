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


    useEffect(() => {
        if (Notification.permission !== 'granted') {
            Notification.requestPermission();
        }
    }, []);

    const showNotification = (message: string) => {
        if (Notification.permission === 'granted') {
            new Notification(message);
        }
    };

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

            setIsActive(false);
            setTimeLeft(FOCUS_DURATION);



        } else {

            if (isActive && runningProjectId) {
                setPausedProjectId(runningProjectId);
                handleProjectClick(runningProjectId);
            }
            setIsActive(!isActive);
        }
    };

    const handleNextPhase = () => {

        if (runningProjectId) {
            setPausedProjectId(runningProjectId);
            handleProjectClick(runningProjectId);
        }


        setIsActive(false);


        if (currentMode === 'focus') {
            showNotification('Focus session complete! Time for a break.');
            const newPomodoroCount = pomodoroCount + 1;
            setPomodoroCount(newPomodoroCount);
            
            if (newPomodoroCount % SESSIONS_FOR_LONG_BREAK === 0) {
                setCurrentMode('longBreak');
                setTimeLeft(LONG_BREAK_DURATION);
            } else {
                setCurrentMode('shortBreak');
                setTimeLeft(SHORT_BREAK_DURATION);
            }


            if (pausedProjectId) {
                handleProjectClick(pausedProjectId);
                setPausedProjectId(null);
                setIsActive(true);
            }

        } else {
            showNotification('Break time over! Time to focus.');
            setCurrentMode('focus');
            setTimeLeft(FOCUS_DURATION);
        }
    };


    const startTimerAndProject = () => {

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

            return 'bg-rose-500';
        }

        return 'bg-sky-500';
    };

    const isBreakMode = currentMode === 'shortBreak' || currentMode === 'longBreak';

    return (
        <div className={`flex items-center justify-between p-2 rounded-lg text-white min-w-[130px] ${getBackgroundColor()}`}>
            <span className="text-xl font-bold tabular-nums">
                {formatTime(timeLeft)}
            </span>

            {!(isActive && isBreakMode) && (
                <button
                    onClick={() => {
                        handleStartPauseReset();


                        if (!isActive && currentMode === 'focus') {
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
