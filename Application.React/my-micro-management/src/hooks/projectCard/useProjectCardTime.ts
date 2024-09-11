import { useEffect, useState } from 'react';
import { useStopwatch } from 'react-timer-hook';

export const useProjectCardTime = (isCurrentProjectRunning: boolean, initialTotalSpentTime: number, initialCurrentSpentTime: number) => {
    const { totalSeconds, start, pause, reset } = useStopwatch();
    const [totalTimeToday, setTotalTimeToday] = useState<number>(initialTotalSpentTime);

    useEffect(() => {
        if (!isCurrentProjectRunning) {
            reset();
            pause();
            return;
        }

        if (totalSeconds > 0) {
            setTotalTimeToday((prevTotal: number) => prevTotal + 1);
        }
    }, [totalSeconds, isCurrentProjectRunning]);

    const handleStartTimer = (fromClick: boolean) => {
        reset();
        start();
    };

    return { totalSeconds: totalSeconds + initialCurrentSpentTime, handleStartTimer, totalTimeToday };
};