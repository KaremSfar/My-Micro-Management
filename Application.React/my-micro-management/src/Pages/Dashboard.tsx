import { useEffect, useState } from "react";
import ProjectCard from "../Components/ProjectCard";
import { ProjectDTO } from "../DTOs/ProjectDto";
import { useAuth } from "../Auth/AuthContext";

function Dashboard() {
    const { accessToken } = useAuth();

    useEffect(() => {
        const fetchProjects = async () => {
            const response = await fetch('https://micomanagement-service.azurewebsites.net/api/projects', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`,
                }
            });
            const data: ProjectDTO[] = await response.json();
            setProjects(data);
        };

        fetchProjects();
    }, []);

    const [runningProjectId, setRunningProjectId] = useState<string | null>(null);

    const handleStart = (projectId: string) => {
        console.log("called");
        setRunningProjectId(projectId);
    };

    const [projects, setProjects] = useState<ProjectDTO[]>([]);

    return (
        <div className="grid lg:grid-cols-4 grid-cols-1 justify-start lg my-8 gap-4 overflow-auto">
            {projects.map((project) => (
                <ProjectCard
                    key={project.id}
                    projectName={project.name}
                    projectColor={project.color}
                    projectId={project.id}
                    isCurrentProjectRunning={runningProjectId === project.id}
                    onStart={() => handleStart(project.id)}
                />
            ))}
        </div>
    );
}

export default Dashboard;