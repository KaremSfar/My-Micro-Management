interface IProjectProps {
    projectName: string;
    projectColor: string;
}

function ProjectCard(props: IProjectProps) {

    const colorRgb = hexToRgb(props.projectColor);
    const darkerShade = darkerColor(colorRgb);

    const backgroundColor = `rgb(${colorRgb[0]}, ${colorRgb[1]}, ${colorRgb[2]})`;
    const borderColor = `rgb(${darkerShade[0]}, ${darkerShade[1]}, ${darkerShade[2]})`;
    const color = borderColor;

    return <div className="h-32 w-48 border-2 rounded-lg shadow-md" style={{ backgroundColor, borderColor }}>
        <div className="flex flex-col justify-items font-bold" style={{ color }}>
            <span className="p-2">
                {props.projectName}
            </span>
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