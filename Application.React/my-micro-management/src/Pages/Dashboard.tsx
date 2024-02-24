import ProjectCard from "../Components/ProjectCard";

function Dashboard() {
    return (
        <div className="flex flex-wrap justify-start border border-black lg my-8 gap-4 overflow-auto">
            <ProjectCard projectName="External Sollicitations" projectColor="#ff7675"/>
            <ProjectCard projectName="Pull Requests" projectColor="#74b9ff"/>
            <ProjectCard projectName="Internal Sollicitations" projectColor="#ffbe76"/>
            <ProjectCard projectName="Bug Fixes" projectColor="#a29bfe"/>
            <ProjectCard projectName="Documentation Writing" projectColor="#badc58"/>
            <ProjectCard projectName="Some other thing" projectColor="#686de0"/>
        </div>
    );
}

export default Dashboard;