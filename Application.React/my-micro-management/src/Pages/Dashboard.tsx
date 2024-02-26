import { useEffect, useState } from "react";
import ProjectCard from "../Components/ProjectCard";
import { ProjectDTO } from "../DTOs/ProjectDto";
import { useAuth } from "../Auth/AuthContext";

function Dashboard() {
    const { accessToken } = useAuth();

    const [projects, setProjects] = useState<ProjectDTO[]>([]);

    useEffect(() => {
        const fetchProjects = async () => {
            const response = await fetch('https://localhost:7114/api/projects', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`, // Set the Authorization header
                }
            });
            const data: ProjectDTO[] = await response.json();
            setProjects(data); // Assuming the API response is an array of project objects
        };

        fetchProjects();
    }, [accessToken]); // Empty dependency array means this effect runs once on mount


    return (
        <div className="grid  lg:grid-cols-4 grid-cols-1 justify-start lg my-8 gap-4 overflow-auto">
            {projects.map((project) => (
                <ProjectCard
                    key={project.id}
                    projectName={project.name}
                    projectColor={project.color}
                />
            ))}
        </div>
    );
}

export default Dashboard;