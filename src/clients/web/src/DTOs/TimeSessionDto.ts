export interface TimeSessionDTO {
    startTime: Date; // ISO date string
    endDate?: Date; // ISO date string, optional as per your C# DTO
    projectIds: string[]; // Array of project IDs
}