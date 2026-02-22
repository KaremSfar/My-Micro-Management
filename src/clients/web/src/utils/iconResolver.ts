import { IconType } from 'react-icons';
import { FaBook, FaBriefcase, FaBullseye, FaCode, FaDumbbell, FaFolder, FaGamepad, FaHeart, FaHouse, FaMusic, FaPlane, FaUtensils } from 'react-icons/fa6';

const iconMap: Record<string, IconType> = {
    house: FaHouse,
    home: FaHouse,
    briefcase: FaBriefcase,
    work: FaBriefcase,
    code: FaCode,
    coding: FaCode,
    gamepad: FaGamepad,
    games: FaGamepad,
    music: FaMusic,
    heart: FaHeart,
    gym: FaDumbbell,
    fitness: FaDumbbell,
    food: FaUtensils,
    travel: FaPlane,
    target: FaBullseye,
    focus: FaBullseye,
    book: FaBook,
};

export const resolveContextIcon = (iconName: string | null | undefined): IconType => {
    if (!iconName || !iconName.trim()) {
        return FaFolder;
    }

    const normalized = iconName.trim().toLowerCase();
    return iconMap[normalized] ?? FaFolder;
};
