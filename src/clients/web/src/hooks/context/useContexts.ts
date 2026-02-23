import { useQuery } from '@tanstack/react-query';
import { useAuth } from '../../Auth/AuthContext';
import { GetContextDto } from '../../DTOs/ContextDto';

export const useContexts = () => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['contexts'],
        queryFn: async () => {
            const response = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/contexts`, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${accessToken}`,
                },
            });

            if (!response.ok) {
                if (response.status === 401) {
                    console.error('Unauthorized - token may have expired');
                }
                throw new Error(`Failed to fetch contexts: ${response.status}`);
            }

            return response.json() as Promise<GetContextDto[]>;
        },
        enabled: !!accessToken,
    });
};
