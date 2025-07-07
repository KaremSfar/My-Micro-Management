export interface TimeSessionDTO {
    startTime: Date; // ISO date string
    endTime?: Date; // ISO date string, optional as per your C# DTO
    projectId: string; // Array of project IDs
}