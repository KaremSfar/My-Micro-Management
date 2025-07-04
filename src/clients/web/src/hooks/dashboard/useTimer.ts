import { useEffect, useRef } from "react";
import { ProjectSessionDTO } from "../../DTOs/ProjectDto";

export const useTimer = (
    runningProjectId: string | null,
    setProjects: React.Dispatch<React.SetStateAction<ProjectSessionDTO[]>>
) => {
    let lastTimestamp = useRef<number>(Date.now());

    useEffect(() => {
        if (!runningProjectId)
            return;

        const interval = setInterval(() => {
            const now = Date.now();
            const elapsedSeconds = Math.floor((now - lastTimestamp.current) / 1000);

            if (elapsedSeconds > 0) {
                lastTimestamp.current = now;

                setProjects((projects) =>
                    projects.map((project) => {
                        if (project.id !== runningProjectId) return project;

                        return {
                            ...project,
                            timeSpentCurrentSession: project.timeSpentCurrentSession + elapsedSeconds,
                            timeSpentTotal: project.timeSpentTotal + elapsedSeconds,
                        } as ProjectSessionDTO;
                    })
                );
            }
        }, 250);

        // Reset the timer when starting
        lastTimestamp.current = Date.now();

        return () => clearInterval(interval);
    }, [runningProjectId, setProjects]);
};
