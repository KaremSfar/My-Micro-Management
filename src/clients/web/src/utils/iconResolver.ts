import { IconType } from 'react-icons';
import { FaBook, FaBriefcase, FaBullseye, FaCode, FaDumbbell, FaFolder, FaGamepad, FaHeart, FaHouse, FaMusic, FaPlane, FaUtensils } from 'react-icons/fa6';

export const contextIconOptions = [
    { value: 'home', label: 'Home' },
    { value: 'briefcase', label: 'Briefcase' },
    { value: 'code', label: 'Code' },
    { value: 'gamepad', label: 'Gamepad' },
    { value: 'music', label: 'Music' },
    { value: 'heart', label: 'Heart' },
    { value: 'gym', label: 'Gym' },
    { value: 'food', label: 'Food' },
    { value: 'travel', label: 'Travel' },
    { value: 'target', label: 'Target' },
    { value: 'book', label: 'Book' },
];

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
