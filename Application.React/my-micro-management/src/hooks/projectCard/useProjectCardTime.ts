import { useEffect, useState } from 'react';
import { useStopwatch } from 'react-timer-hook';

export const useProjectCardTime = (isCurrentProjectRunning: boolean, initialTotalSpentTime: number, initialCurrentSpentTime: number, onStart: (fromClick: boolean) => void) => {
    const { totalSeconds, start, isRunning, pause, reset } = useStopwatch();
    const [totalTimeToday, setTotalTimeToday] = useState<number>(initialTotalSpentTime);

    useEffect(() => {
        if (totalSeconds > 0) {
            setTotalTimeToday((prevTotal: number) => prevTotal + 1);
        }
    }, [totalSeconds, isCurrentProjectRunning]);

    const handleStartTimer = (fromClick: boolean) => {
        reset();
        start();
        onStart(fromClick);
    };

    return { totalSeconds: totalSeconds + initialCurrentSpentTime, handleStartTimer, totalTimeToday };
};