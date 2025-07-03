import { useState, useEffect } from 'react';
import { TimeSessionDTO } from '../DTOs/TimeSessionDto';
import { useAuth } from '../Auth/AuthContext';
import { TimeSessionModel } from '../Models/TimeSessionModel';
import { ProjectSessionDTO } from '../DTOs/ProjectDto';

export const useTimeSessions = () => {
    const { accessToken } = useAuth();
    const [timeSessions, setTimeSessions] = useState<TimeSessionModel[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // Fetch time sessions
    useEffect(() => {
        const fetchTimeSessions = async () => {
            if (!accessToken) return;

            setLoading(true);
            setError(null);

            try {
                const timeSessionsResponse = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/timeSessions`, {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`,
                    },
                });


                if (!timeSessionsResponse.ok) {
                    throw new Error('Failed to fetch time sessions');
                }

                const timeSessionsDtos: TimeSessionDTO[] = (await timeSessionsResponse.json()).map((ts: any) => {
                    return {
                        ...ts,
                        startTime: new Date(ts.startTime),
                        endDate: ts.endDate ? new Date(ts.endDate) : undefined,
                    }
                });

                const projectsResponse = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/projects`, {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`,
                    },
                });

                if (!projectsResponse.ok) {
                    throw new Error('Failed to fetch projects');
                }

                const projectsData: ProjectSessionDTO[] = await projectsResponse.json();

                const timeSessions: TimeSessionModel[] = timeSessionsDtos.map(ts => {
                    return {
                        startTime: ts.startTime,
                        endDate: ts.endDate,
                        project: projectsData.find(p => p.id === ts.projectIds[0])!
                    }
                });

                setTimeSessions(timeSessions);
            } catch (error) {
                console.error('Error fetching time sessions:', error);
                setError(error instanceof Error ? error.message : 'Failed to fetch time sessions');
            } finally {
                setLoading(false);
            }
        };

        if (accessToken) {
            fetchTimeSessions();
        }
    }, [accessToken]);

    return { timeSessions, loading, error };
};