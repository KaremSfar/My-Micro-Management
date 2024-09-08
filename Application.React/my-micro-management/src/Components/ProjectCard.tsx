import { useStopwatch } from 'react-timer-hook';
import { PauseIcon, PlayIcon } from '@heroicons/react/24/outline';
import { useEffect, useState } from 'react';
import { useAuth } from '../Auth/AuthContext';

interface IProjectCardProps {
    projectName: string;
    projectColor: string;
    projectId: string;
    isCurrentProjectRunning: boolean;
    onStart: () => void;
}

function ProjectCard(props: IProjectCardProps) {
    const {
        seconds,
        minutes,
        start,
        isRunning,
        pause,
        reset
    } = useStopwatch();

    const colorRgb = hexToRgb(props.projectColor);
    const darkerShade = darkerColor(colorRgb);

    const backgroundColor = `rgb(${colorRgb[0]}, ${colorRgb[1]}, ${colorRgb[2]})`;
    const luminance = calculateLuminance(colorRgb);
    const isDark = luminance < 0.3;

    const borderColor = isDark ? `rgb(${lighterColor(colorRgb)[0]}, ${lighterColor(colorRgb)[1]}, ${lighterColor(colorRgb)[2]})` : `rgb(${darkerShade[0]}, ${darkerShade[1]}, ${darkerShade[2]})`;
    const color = isDark ? 'white' : borderColor;

    const [totalTimeToday, setTotalTimeToday] = useState<number>(0);

    useEffect(() => {
        const incrementTotalTime = () => {
            setTotalTimeToday((prevTotal: number) => prevTotal + 1);
        };

        if (seconds > 0)
            incrementTotalTime();

    }, [seconds]);

    useEffect(() => {
        if (!props.isCurrentProjectRunning && isRunning) {
            reset();
            pause();
            console.log("Stopped the timer");
        }
    }, [props.isCurrentProjectRunning, isRunning, pause, reset])

    const handleStart = () => {
        start();
        props.onStart();
    }

    const { accessToken } = useAuth();

    useEffect(() => {
        const createTimeSession = async () => {
            const endDate = new Date().toISOString();
            const startTime = new Date(new Date().getTime() - seconds * 1000).toISOString();

            const body = {
                startTime,
                endDate,
                projectIds: [
                    props.projectId
                ]
            };

            try {
                const response = await fetch(`${process.env.REACT_APP_MAIN_SERVICE_BASE_URL}/api/timeSessions`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`
                    },
                    body: JSON.stringify(body)
                });

                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }

                const data = await response.json();
                console.log('Time session created:', data);
            } catch (error) {
                console.error('Failed to create time session:', error);
            }
        };

        if (!props.isCurrentProjectRunning && isRunning && seconds > 10) {
            createTimeSession();
        }
    }, [props.isCurrentProjectRunning, isRunning]);

    return <div onClick={isRunning ? pause : handleStart}
        className="lg:aspect-[5/3] sm:min-w-48 border-2 rounded-lg shadow-md min-w-full m-1 hover:scale-[1.01] transition-transform hover:cursor-pointer"
        style={{ backgroundColor, borderColor }}>
        <div className="flex flex-col h-full justify-between font-bold p-2">
            <span className="w-full" style={{ color }}>
                {props.projectName}
            </span>
            <span className="w-full text-center py-4 text-white text-xl">
                {!props.isCurrentProjectRunning ?
                    "--:--" :
                    `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`
                }
            </span>
            <div className="flex w-full justify-between" style={{ color }}>
                <span>{formatTime(totalTimeToday)}</span>
                <button className=" ">
                    {isRunning
                        ? <PauseIcon onClick={pause} className="h-6 w-6"></PauseIcon>
                        : <PlayIcon onClick={handleStart} className="h-6 w-6"></PlayIcon>}
                </button>
            </div>
        </div>
    </div>
}

export default ProjectCard;

const hexToRgb = (hex: string): number[] => {
    const hexRgb: string = hex.replace(/^#?([a-f\d])([a-f\d])([a-f\d])$/i,
        (m, r, g, b) => '#' + r + r + g + g + b + b);

    return hexRgb
        .substring(1).match(/.{2}/g)!
        .map(x => parseInt(x, 16));
}

const darkerColor = (rbg: number[]): number[] => {
    return rbg.map(x => x * 0.6);
}

const lighterColor = (rgb: number[]): number[] => {
    return rgb.map(x => Math.min(255, x + 100));
}

const calculateLuminance = (rgb: number[]): number => {
    const [r, g, b] = rgb.map(x => x / 255);
    const a = [r, g, b].map(v => v <= 0.03928 ? v / 12.92 : Math.pow((v + 0.055) / 1.055, 2.4));
    return 0.2126 * a[0] + 0.7152 * a[1] + 0.0722 * a[2];
}

const formatTime = (totalSeconds: number) => {
    const date = new Date(0);
    date.setSeconds(totalSeconds);
    return date.toISOString().substring(11, 19);
};