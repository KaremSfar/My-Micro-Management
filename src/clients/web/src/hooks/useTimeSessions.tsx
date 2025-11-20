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

    // Fetch time sessions only once on mount - keep cache, but use latest token for all API calls
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
                    if (timeSessionsResponse.status === 401) {
                        console.error('Unauthorized - token may have expired');
                        throw new Error('Authentication failed - please refresh the page');
                    }
                    throw new Error(`Failed to fetch time sessions: ${timeSessionsResponse.status}`);
                }

                const timeSessionsDtos: TimeSessionDTO[] = (await timeSessionsResponse.json()).map((ts: any) => {
                    return {
                        ...ts,
                        startTime: new Date(ts.startTime),
                        endTime: ts.endTime ? new Date(ts.endTime) : undefined,
                    }
                });

                const projectsResponse = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/projects`, {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`,
                    },
                });

                if (!projectsResponse.ok) {
                    if (projectsResponse.status === 401) {
                        console.error('Unauthorized - token may have expired');
                        throw new Error('Authentication failed - please refresh the page');
                    }
                    throw new Error(`Failed to fetch projects: ${projectsResponse.status}`);
                }

                const projectsData: ProjectSessionDTO[] = await projectsResponse.json();

                const timeSessions: TimeSessionModel[] = timeSessionsDtos.map(ts => {
                    return {
                        startTime: ts.startTime,
                        endTime: ts.endTime,
                        project: projectsData.find(p => p.id === ts.projectId)!
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
    }, []);

    return { timeSessions, loading, error };
};