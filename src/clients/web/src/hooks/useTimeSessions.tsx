import { useQuery } from '@tanstack/react-query';
import { TimeSessionDTO } from '../DTOs/TimeSessionDto';
import { useAuth } from '../Auth/AuthContext';
import { TimeSessionModel } from '../Models/TimeSessionModel';
import { ProjectSessionDTO } from '../DTOs/ProjectDto';

export const useTimeSessions = () => {
    const { accessToken } = useAuth();

    const {
        data: timeSessions = [],
        isLoading: loading,
        error,
        refetch,
    } = useQuery({
        queryKey: ['timeSessions', accessToken],
        queryFn: async () => {
            const timeSessionsResponse = await fetch(
                `${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/timeSessions`,
                {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`,
                    },
                }
            );

            if (!timeSessionsResponse.ok) {
                if (timeSessionsResponse.status === 401) {
                    console.error('Unauthorized - token may have expired');
                    throw new Error('Authentication failed - please refresh the page');
                }
                throw new Error(`Failed to fetch time sessions: ${timeSessionsResponse.status}`);
            }

            const timeSessionsDtos: TimeSessionDTO[] = (await timeSessionsResponse.json()).map((ts: any) => ({
                ...ts,
                startTime: new Date(ts.startTime),
                endTime: ts.endTime ? new Date(ts.endTime) : undefined,
            }));

            const projectsResponse = await fetch(
                `${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/projects`,
                {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`,
                    },
                }
            );

            if (!projectsResponse.ok) {
                if (projectsResponse.status === 401) {
                    console.error('Unauthorized - token may have expired');
                    throw new Error('Authentication failed - please refresh the page');
                }
                throw new Error(`Failed to fetch projects: ${projectsResponse.status}`);
            }

            const projectsData: ProjectSessionDTO[] = await projectsResponse.json();

            return timeSessionsDtos.map(ts => ({
                startTime: ts.startTime,
                endTime: ts.endTime,
                project: projectsData.find(p => p.id === ts.projectId)!,
            })) as TimeSessionModel[];
        },
        enabled: !!accessToken,
        staleTime: 5 * 60 * 1000, // 5 minutes
    });

    return { timeSessions, loading, error, refetch };
};