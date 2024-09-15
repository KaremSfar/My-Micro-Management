export interface ProjectSessionDTO {
    id: string;
    name: string;
    color: string;

    //TO-DO karem remove these fields below from here
    timeSpentCurrentSession: number;
    timeSpentTotal: number;
    isRunning: boolean;
}

export interface CreateProjectDTO {
    name?: string;
    color?: string;
}

export interface GetProjectDto {
    id: string;
    name: string;
    color: string;
}