import ProjectCard from "../Components/ProjectCard";
import NewProjectCard from "../Components/NewProjectCard";
import { useProjectContext } from "../context/ProjectContext";

function Dashboard() {
    // All state and logic is now consumed from the central context.
    // This component is now much simpler and only handles rendering.
    const { projects, runningProjectId, handleProjectClick, addNewProject } = useProjectContext();

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
