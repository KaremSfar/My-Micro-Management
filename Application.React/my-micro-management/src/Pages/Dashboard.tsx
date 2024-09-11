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
            const runningProject = data.find(p => p.isRunning);

            if (runningProject) {
                setRunningProjectId(runningProject.id);
            }
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

        connection.on("TimeSessionStarted", (projectId: string) => {
            setRunningProjectId(projectId);
        });

    }, [accessToken]);

    const handleStart = (projectId: string, fromClick: boolean) => {
        if (fromClick) {
            webSocketConnectionRef.current?.send("StartTimeSession", projectId);
        }

        for (const project of projects) {
            if (project.id !== projectId) {
                project.timeSpentCurrentSession = 0;
            }
        }

        setProjects(projects);
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
                    initialTimeSpentTotal={project.timeSpentTotal}
                    initialTimeSpentCurrent={project.timeSpentCurrentSession}
                    onStart={(fromClick) => handleStart(project.id, fromClick)}
                />
            ))}
            <NewProjectCard onProjectCreated={addNewProject} />
        </div>
    );
}

export default Dashboard;