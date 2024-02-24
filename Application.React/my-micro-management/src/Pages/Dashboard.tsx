import ProjectCard from "../Components/ProjectCard";

function Dashboard() {
    return (
        <div className="flex flex-wrap justify-start border border-black lg my-8 gap-4">
            <ProjectCard projectName="External Sollicitations" projectColor="#ff7675"/>
            <ProjectCard projectName="Pull Requests" projectColor="#74b9ff"/>
            <ProjectCard projectName="Internal Sollicitations" projectColor="#ff7675"/>
            <ProjectCard projectName="Bug Fixes" projectColor="#a29bfe"/>
            <ProjectCard projectName="Documentation Writing" projectColor="#ff7675"/>
            <ProjectCard projectName="Some other thing" projectColor="#ff7675"/>
        </div>
    );
}

export default Dashboard;