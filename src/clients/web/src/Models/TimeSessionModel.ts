import { ProjectSessionDTO } from "../DTOs/ProjectDto";

export interface TimeSessionModel {
    startTime: Date; // ISO date string
    endTime?: Date; // ISO date string, optional as per your C# DTO
    project: ProjectSessionDTO;
}