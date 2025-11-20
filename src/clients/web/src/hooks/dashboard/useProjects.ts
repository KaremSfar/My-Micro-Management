import { useEffect, useState } from "react";
import { ProjectSessionDTO } from "../../DTOs/ProjectDto";
import { useAuth } from "../../Auth/AuthContext";

export const useProjects = () => {
    const { accessToken } = useAuth();

    const [projects, setProjects] = useState<ProjectSessionDTO[]>([]);
    const [runningProjectId, setRunningProjectId] = useState<string | null>(null);

    // Fetch projects only once on mount - keep cache, but use latest token for all API calls
    useEffect(() => {
        const fetchProjects = async () => {
            if (!accessToken || projects?.length)
                return;

            try {
                const response = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/projects`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`,
                    }
                });

                if (!response.ok) {
                    if (response.status === 401) {
                        console.error('Unauthorized - token may have expired');
                    }
                    throw new Error(`Failed to fetch projects: ${response.status}`);
                }

                const data: ProjectSessionDTO[] = await response.json();
                setProjects(data);

                const runningProject = data.find(p => p.isRunning);
                if (runningProject)
                    setRunningProjectId(runningProject.id);
            } catch (error) {
                console.error('Error fetching projects:', error);
            }
        };

        fetchProjects();
    }, []);

    return { projects, setProjects, runningProjectId, setRunningProjectId };
};
