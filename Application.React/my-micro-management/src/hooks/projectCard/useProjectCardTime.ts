import { useEffect, useState } from 'react';
import { useStopwatch } from 'react-timer-hook';

export const useProjectCardTime = (isCurrentProjectRunning: boolean, initialTotalSpentTime: number, onStart: () => void) => {
    const { seconds, minutes, start, isRunning, pause, reset } = useStopwatch();
    const [totalTimeToday, setTotalTimeToday] = useState<number>(initialTotalSpentTime);

    useEffect(() => {
        if (seconds > 0) {
            setTotalTimeToday((prevTotal: number) => prevTotal + 1);
        }
    }, [seconds]);

    useEffect(() => {
        if (!isCurrentProjectRunning && isRunning) {
            reset();
            pause();
        }
    }, [isCurrentProjectRunning, isRunning, pause, reset]);

    const handleStartTimer = () => {
        start();
        onStart();
    };

    return { seconds, minutes, isRunning, pause, handleStartTimer, totalTimeToday };
};