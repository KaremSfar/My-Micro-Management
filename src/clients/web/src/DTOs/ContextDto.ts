export interface GetContextDto {
    id: string;
    name: string;
    icon: string | null;
}

export interface CreateContextDTO {
    name?: string;
    icon?: string;
}
