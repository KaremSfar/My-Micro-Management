export interface ProjectDTO {
    id: string;
    name: string;
    color: string;

    //TO-DO karem remove these fields below from here
    timeSpentCurrentSession: number;
    timeSpentTotal: number;
}
