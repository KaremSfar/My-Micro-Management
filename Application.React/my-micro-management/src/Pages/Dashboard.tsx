import ProjectCard from "../Components/ProjectCard";
import { ProjectDTO } from "../DTOs/ProjectDto";
import NewProjectCard from "../Components/NewProjectCard"; // Assuming this is the correct import path
import { useProjects } from "../hooks/dashboard/useProjects";
import { useWebSocket } from "../hooks/dashboard/useWebSocket";
import { useTimer } from "../hooks/dashboard/useTimer";

function Dashboard() {
    const { projects, setProjects, runningProjectId, setRunningProjectId } = useProjects();

    useTimer(runningProjectId, setProjects);

    const startProject: (projectId: string) => void = (projectId: string) => {
        setProjects(prevProjects => prevProjects.map(project => ({
            ...project,
            timeSpentCurrentSession: 0,
        })));

        setRunningProjectId(projectId);
    };

    const stopProjects: () => void = () => {
        setProjects(prevProjects => prevProjects.map(project => ({
            ...project,
            timeSpentCurrentSession: 0,
        })));

        setRunningProjectId(null);
    };

    const webSocketConnectionRef = useWebSocket(startProject, stopProjects);

    const handleProjectClick = (projectId: string) => {
        // When clicking on a same project - we stop the session
        if (projectId === runningProjectId) {
            webSocketConnectionRef.current?.send("StopTimeSessions");
            stopProjects();
        }
        else { // Other wise we start a new one
            webSocketConnectionRef.current?.send("StartTimeSession", projectId);

            startProject(projectId);
        }
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
                    isRunning={runningProjectId === project.id}
                    timeSpentTotal={project.timeSpentTotal}
                    timeSpentCurrently={project.timeSpentCurrentSession}
                    onClick={() => handleProjectClick(project.id)}
                />
            ))}
            <NewProjectCard onProjectCreated={addNewProject} />
        </div>
    );
}

export default Dashboard;