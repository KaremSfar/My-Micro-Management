import React, { useState, useEffect } from 'react';
import { PlayIcon, StopIcon, ForwardIcon } from '@heroicons/react/24/outline';

const FOCUS_DURATION = 25 * 60; // 25 minutes in seconds
const SHORT_BREAK_DURATION = 5 * 60; // 5 minutes in seconds
const LONG_BREAK_DURATION = 15 * 60; // 15 minutes in seconds
const SESSIONS_FOR_LONG_BREAK = 4;

type Mode = 'focus' | 'shortBreak' | 'longBreak';

const PomodoroWidget: React.FC = () => {
    const [currentMode, setCurrentMode] = useState<Mode>('focus');
    const [timeLeft, setTimeLeft] = useState<number>(FOCUS_DURATION);
    const [isActive, setIsActive] = useState<boolean>(false);
    const [pomodoroCount, setPomodoroCount] = useState<number>(0); // Completed focus sessions

    // Timer effect
    useEffect(() => {
        let interval: NodeJS.Timeout | null = null;

        if (isActive && timeLeft > 0) {
            interval = setInterval(() => {
                setTimeLeft((prevTime) => prevTime - 1);
            }, 1000);
        } else if (timeLeft === 0 && isActive) { // Timer reached 0 while active
            handleNextPhase(); // Automatically advance to next phase
        }

        return () => {
            if (interval) {
                clearInterval(interval);
            }
        };
    }, [isActive, timeLeft, currentMode, pomodoroCount]);

    const handleStartPause = () => {
        setIsActive(!isActive);
    };

    const handleNextPhase = () => {
        setIsActive(false); // Pause timer when manually advancing

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
        } else { // currentMode was 'shortBreak' or 'longBreak'
            setCurrentMode('focus');
            setTimeLeft(FOCUS_DURATION);
        }
        // Future enhancement: Play a sound notification
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

    return (
        <div className={`flex items-center justify-between p-2 rounded-lg text-white min-w-[130px] ${getBackgroundColor()}`}>
            <span className="text-xl font-bold tabular-nums"> {/* Increased font size and added bold */}
                {formatTime(timeLeft)}
            </span>
            <button
                onClick={handleStartPause}
                className="ml-2 p-1 bg-white/20 hover:bg-white/30 rounded focus:outline-none focus:ring-2 focus:ring-white/50" // Smaller padding, rounded for squareness
                aria-label={isActive ? "Pause timer" : "Start timer"}
            >
                {isActive ? <StopIcon className="h-5 w-5"></StopIcon> : <PlayIcon className="h-5 w-5"></PlayIcon>} {/* Smaller icons */}
            </button>
            <button
                onClick={handleNextPhase}
                className="ml-1 p-1 bg-white/20 hover:bg-white/30 rounded focus:outline-none focus:ring-2 focus:ring-white/50" // Smaller padding, rounded for squareness
                aria-label="Next phase"
            >
                <ForwardIcon className="h-5 w-5"></ForwardIcon> {/* Smaller icon */}
            </button>
        </div>
    );
};

export default PomodoroWidget;
