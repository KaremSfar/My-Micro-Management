import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '../../Auth/AuthContext';
import { CreateContextDTO, GetContextDto } from '../../DTOs/ContextDto';

export const useCreateContext = (onCreated?: (context: GetContextDto) => void) => {
    const { accessToken } = useAuth();
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (createContextDto: CreateContextDTO) => {
            const response = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/contexts`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${accessToken}`,
                },
                body: JSON.stringify(createContextDto),
            });

            if (!response.ok) {
                if (response.status === 401) {
                    console.error('Unauthorized - token may have expired');
                }
                throw new Error(`Failed to create context: ${response.status}`);
            }

            return response.json() as Promise<GetContextDto>;
        },
        onSuccess: (createdContext) => {
            queryClient.setQueryData<GetContextDto[]>(['contexts'], (old = []) => [...old, createdContext]);
            onCreated?.(createdContext);
        },
    });
};
