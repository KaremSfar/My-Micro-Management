export interface TimeSessionDTO {
    startTime: string; // ISO date string
    endDate?: string; // ISO date string, optional as per your C# DTO
    projectIds: string[]; // Array of project IDs
}