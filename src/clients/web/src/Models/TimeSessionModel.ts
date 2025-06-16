import { ProjectSessionDTO } from "../DTOs/ProjectDto";

export interface TimeSessionDTO {
    startTime: string; // ISO date string
    endDate?: string; // ISO date string, optional as per your C# DTO
    Project: ProjectSessionDTO[]; // Array of project IDs
}