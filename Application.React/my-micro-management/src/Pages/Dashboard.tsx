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

    const webSocketConnectionRef = useWebSocket(startProject);

    const handleProjectClick = (projectId: string) => {
        if (projectId === runningProjectId) {
            return;
        }

        webSocketConnectionRef.current?.send("StartTimeSession", projectId);

        startProject(projectId);
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
                    onClick={() => handleProjectClick(project.id)}
                />
            ))}
            <NewProjectCard onProjectCreated={addNewProject} />
        </div>
    );
}

export default Dashboard;