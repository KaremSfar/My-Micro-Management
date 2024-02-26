import { useStopwatch } from 'react-timer-hook';
import { PauseIcon, PlayIcon } from '@heroicons/react/24/outline';

interface IProjectProps {
    projectName: string;
    projectColor: string;
}

function ProjectCard(props: IProjectProps) {
    const {
        seconds,
        minutes,
        hours,
        start,
        isRunning,
        pause,
    } = useStopwatch();

    const colorRgb = hexToRgb(props.projectColor);
    const darkerShade = darkerColor(colorRgb);

    const backgroundColor = `rgb(${colorRgb[0]}, ${colorRgb[1]}, ${colorRgb[2]})`;
    const borderColor = `rgb(${darkerShade[0]}, ${darkerShade[1]}, ${darkerShade[2]})`;
    const color = borderColor;

    return <div onClick={isRunning ? pause : start}
        className="lg:aspect-[5/3] sm:min-w-48 border-2 rounded-lg shadow-md min-w-full m-1 hover:scale-[1.01] transition-transform hover:cursor-pointer"
        style={{ backgroundColor, borderColor }}>
        <div className="flex flex-col h-full justify-between font-bold p-2">
            <span className="w-full" style={{ color }}>
                {props.projectName}
            </span>
            <span className="w-full text-center py-4 text-white text-xl">
                {minutes + seconds + hours === 0 ?
                    "--:--" :
                    `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`
                }
            </span>
            <div className="flex w-full justify-between" style={{ color }}>
                <span>00:00:00</span>
                <button className=" ">
                    {isRunning
                        ? <PauseIcon onClick={pause} className="h-6 w-6"></PauseIcon>
                        : <PlayIcon onClick={start} className="h-6 w-6"></PlayIcon>}
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