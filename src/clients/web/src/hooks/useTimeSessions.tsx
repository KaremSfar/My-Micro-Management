import { useState, useEffect } from 'react';
import { TimeSessionDTO } from '../DTOs/TimeSessionDto';
import { useAuth } from '../Auth/AuthContext';

export const useTimeSessions = () => {
    const { accessToken } = useAuth();
    const [timeSessions, setTimeSessions] = useState<TimeSessionDTO[]>([]);
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

                const data: TimeSessionDTO[] = await timeSessionsResponse.json();

                const projectsResponse = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/projects`, {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`,
                    },
                });

                if (!projectsResponse.ok) {
                    throw new Error('Failed to fetch projects');
                }

                const projectsData = await projectsResponse.json();

                setTimeSessions(data);
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