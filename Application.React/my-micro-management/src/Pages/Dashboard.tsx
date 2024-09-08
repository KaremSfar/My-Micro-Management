import { useEffect, useState } from "react";
import ProjectCard from "../Components/ProjectCard";
import { ProjectDTO } from "../DTOs/ProjectDto";
import { useAuth } from "../Auth/AuthContext";
import NewProjectCard from "../Components/NewProjectCard"; // Assuming this is the correct import path

function Dashboard() {
    const { accessToken } = useAuth();

    useEffect(() => {
        const fetchProjects = async () => {
            const response = await fetch(`${process.env.REACT_APP_MAIN_SERVICE_BASE_URL}/api/projects`, {
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

    const addNewProject = (newProject: ProjectDTO) => {
        setProjects((prevProjects) => [...prevProjects, newProject]);
    };

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
            <NewProjectCard onProjectCreated={addNewProject} /> {/* Pass the function here */}
        </div>
        
    );
}

export default Dashboard;