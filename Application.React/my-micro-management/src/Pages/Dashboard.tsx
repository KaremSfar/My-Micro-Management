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
        webSocketConnectionRef.current = new HubConnectionBuilder()
            .withUrl(`${process.env.REACT_APP_MAIN_SERVICE_BASE_URL}/hub/timesessionshub`, {
                accessTokenFactory: () => accessToken!
            })
            .withAutomaticReconnect()
            .withHubProtocol(new JsonHubProtocol())
            .build();

        webSocketConnectionRef.current.start();

        webSocketConnectionRef.current.onclose((error) => {
            console.error("WebSocket connection closed:", error);
        });

        webSocketConnectionRef.current.on("TimeSessionStarted", (projectId: string) => {
            startProject(projectId);
        });

        return () => {
            webSocketConnectionRef.current?.stop();
            webSocketConnectionRef.current = null;
        };

    }, [accessToken]);

    useEffect(() => {
        if (!runningProjectId) // Foresight for the stop feature coming soon :)
            return;

        const interval = setInterval(() => {
            setProjects(projects => projects.map(project => {
                const addedTime = +(project.id === runningProjectId);

                if (!addedTime)
                    return project;

                return {
                    ...project,
                    timeSpentCurrentSession: project.timeSpentCurrentSession + addedTime,
                    timeSpentTotal: project.timeSpentTotal + addedTime,
                } as ProjectDTO
            }));
        }, 1000);

        return () => {
            if (interval) clearInterval(interval!);
        };

    }, [runningProjectId]);

    const handleStart = (projectId: string) => {
        if (projectId === runningProjectId) {
            return;
        }

        webSocketConnectionRef.current?.send("StartTimeSession", projectId);

        startProject(projectId);
    };

    const startProject: (projectId: string) => void = (projectId: string) => {
        setProjects(prevProjects => prevProjects.map(project => ({
            ...project,
            timeSpentCurrentSession: 0,
        })));

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
                    timeSpentTotal={project.timeSpentTotal}
                    timeSpentCurrently={project.timeSpentCurrentSession}
                    onStart={() => handleStart(project.id)}
                />
            ))}
            <NewProjectCard onProjectCreated={addNewProject} />
        </div>
    );
}

export default Dashboard;