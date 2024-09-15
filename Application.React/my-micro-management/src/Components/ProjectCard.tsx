import { PlayIcon } from '@heroicons/react/24/outline';
import { useProjectCardColors } from '../hooks/projectCard/useProjectCardColors';

interface IProjectCardProps {
    projectName: string;
    projectColor: string;
    projectId: string;

    isCurrentProjectRunning: boolean;
    timeSpentTotal: number;
    timeSpentCurrently: number;

    onStart: () => void;
}

function ProjectCard(props: IProjectCardProps) {
    const { backgroundColor, borderColor, color } = useProjectCardColors(props.projectColor);

    return (
        <div onClick={() => props.onStart()}
            className="lg:aspect-[5/3] sm:min-w-48 border-2 rounded-lg shadow-md min-w-full m-1 hover:scale-[1.01] transition-transform hover:cursor-pointer"
            style={{ backgroundColor, borderColor }}>
            <div className="flex flex-col h-full justify-between font-bold p-2">
                <span className="w-full" style={{ color }}>
                    {props.projectName}
                </span>
                <span className="w-full text-center py-4 text-white text-xl">
                    {!props.isCurrentProjectRunning ?
                        "--:--" :
                        `${String(Math.floor(props.timeSpentCurrently / 60)).padStart(2, '0')}:${String(props.timeSpentCurrently % 60).padStart(2, '0')}`
                    }
                </span>
                <div className="flex w-full justify-between" style={{ color }}>
                    <span>{formatTime(props.timeSpentTotal)}</span>
                    <button className=" ">
                        <PlayIcon className="h-6 w-6"></PlayIcon>
                    </button>
                </div>
            </div>
        </div>
    );
}

const formatTime = (totalSeconds: number) => {
    const date = new Date(0);
    date.setSeconds(totalSeconds);
    return date.toISOString().substring(11, 19);
};

export default ProjectCard;
