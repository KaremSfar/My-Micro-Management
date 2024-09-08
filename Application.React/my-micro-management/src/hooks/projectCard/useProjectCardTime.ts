import { useEffect, useState } from 'react';
import { useStopwatch } from 'react-timer-hook';

export const useProjectCardTime = (isCurrentProjectRunning: boolean, onStart: () => void) => {
    const { seconds, minutes, start, isRunning, pause, reset } = useStopwatch();
    const [totalTimeToday, setTotalTimeToday] = useState<number>(0);

    useEffect(() => {
        if (seconds > 0) {
            setTotalTimeToday((prevTotal: number) => prevTotal + 1);
        }
    }, [seconds]);

    useEffect(() => {
        if (!isCurrentProjectRunning && isRunning) {
            reset();
            pause();
            console.log("Stopped the timer");
        }
    }, [isCurrentProjectRunning, isRunning, pause, reset]);

    const handleStart = () => {
        start();
        onStart();
    };

    return { seconds, minutes, isRunning, pause, handleStart, totalTimeToday };
};