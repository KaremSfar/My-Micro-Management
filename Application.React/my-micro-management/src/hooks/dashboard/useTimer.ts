import { useEffect } from "react";
import { ProjectDTO } from "../../DTOs/ProjectDto";

export const useTimer = (runningProjectId: string | null, setProjects: React.Dispatch<React.SetStateAction<ProjectDTO[]>>) => {
    useEffect(() => {
        if (!runningProjectId) return;

        const interval = setInterval(() => {
            setProjects((projects) =>
                projects.map((project) => {
                    if (project.id !== runningProjectId) return project;

                    return {
                        ...project,
                        timeSpentCurrentSession: project.timeSpentCurrentSession + 1,
                        timeSpentTotal: project.timeSpentTotal + 1,
                    } as ProjectDTO;
                })
            );
        }, 1000);

        return () => clearInterval(interval);
    }, [runningProjectId, setProjects]);
};
