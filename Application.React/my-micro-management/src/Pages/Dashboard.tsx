import { useEffect, useRef, useState } from "react";
import ProjectCard from "../Components/ProjectCard";
import { ProjectDTO } from "../DTOs/ProjectDto";
import { useAuth } from "../Auth/AuthContext";
import NewProjectCard from "../Components/NewProjectCard"; // Assuming this is the correct import path
import { HubConnection, HubConnectionBuilder, JsonHubProtocol } from "@microsoft/signalr";

function Dashboard() {
    const { accessToken } = useAuth();
    const [projects, setProjects] = useState<ProjectDTO[]>([]);
    const [runningProjectId, setRunningProjectId] = useState<string | null>(null);
    const webSocketConnectionRef = useRef<HubConnection | null>(null);

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

        if (accessToken) {
            fetchProjects();
        }
    }, []);

    useEffect(() => {
        let connection = webSocketConnectionRef.current;

        if (!connection) {
            connection = new HubConnectionBuilder()
                .withUrl(`${process.env.REACT_APP_MAIN_SERVICE_BASE_URL}/hub/timesessionshub`, {
                    accessTokenFactory: () => accessToken!
                })
                .withAutomaticReconnect()
                .withHubProtocol(new JsonHubProtocol())
                .build();

            webSocketConnectionRef.current = connection;

            connection.start();
        }

        connection.onclose((error) => {
            console.error("WebSocket connection closed:", error);
        });

        connection.on("TimeSessionsStopped", () => {
            setRunningProjectId(projects[0].id);
        });

    }, [accessToken]);

    const handleStart = (projectId: string) => {
        console.log("called");
        webSocketConnectionRef.current!.send("TimeSessionStarted", projectId);
        setRunningProjectId(projectId);
    };

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
            <NewProjectCard onProjectCreated={addNewProject} />
        </div>
    );
}

export default Dashboard;