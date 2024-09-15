import { useEffect, useState } from "react";
import { ProjectDTO } from "../../DTOs/ProjectDto";
import { useAuth } from "../../Auth/AuthContext";

export const useProjects = () => {
    const { accessToken } = useAuth();

    const [projects, setProjects] = useState<ProjectDTO[]>([]);
    const [runningProjectId, setRunningProjectId] = useState<string | null>(null);

    useEffect(() => {
        const fetchProjects = async () => {
            if (!accessToken || projects?.length)
                return;

            try {
                const response = await fetch(`${process.env.REACT_APP_MAIN_SERVICE_BASE_URL}/api/projects`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`,
                    }
                });

                const data: ProjectDTO[] = await response.json();
                setProjects(data);

                const runningProject = data.find(p => p.isRunning);
                if (runningProject)
                    setRunningProjectId(runningProject.id);
            } catch (error) {
                console.error('Error fetching projects:', error);
            }
        };

        fetchProjects();
    }, [accessToken, projects]);

    return { projects, setProjects, runningProjectId, setRunningProjectId };
};
