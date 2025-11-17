import { createContext, useContext, useState, useEffect, ReactNode, useCallback } from 'react';
import { GetProjectDto, ProjectSessionDTO } from '../DTOs/ProjectDto';
import { useAuth } from '../Auth/AuthContext';
import { useTimer } from '../hooks/dashboard/useTimer';
import { useWebSocket } from '../hooks/useWebSockets';

interface IProjectContext {
    projects: ProjectSessionDTO[];
    runningProjectId: string | null;
    pausedProjectId: string | null; // New field to track paused project
    handleProjectClick: (projectId: string) => void;
    addNewProject: (newProject: GetProjectDto) => void;
    setPausedProjectId: (projectId: string | null) => void; // New method to set paused project
}

const ProjectContext = createContext<IProjectContext | undefined>(undefined);

export const ProjectProvider = ({ children }: { children: ReactNode }) => {
    const { accessToken } = useAuth();
    const [projects, setProjects] = useState<ProjectSessionDTO[]>([]);
    const [runningProjectId, setRunningProjectId] = useState<string | null>(null);
    const [pausedProjectId, setPausedProjectId] = useState<string | null>(null); // New state for paused project

    // Fetch initial projects
    useEffect(() => {
        const fetchProjects = async () => {
            if (!accessToken) return;

            try {
                const response = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/projects`, {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`,
                    },
                });
                const data: ProjectSessionDTO[] = await response.json();
                setProjects(data);

                // Check if any project is running from the backend data and set it
                const runningProject = data.find(p => p.isRunning);
                if (runningProject) {
                    setRunningProjectId(runningProject.id);
                }
            } catch (error) {
                console.error('Error fetching projects:', error);
            }
        };

        if (accessToken) {
            fetchProjects();
        }

    }, []);

    // Timer logic is now managed within the provider
    useTimer(runningProjectId, setProjects);

    // WebSocket logic and handlers are also managed here
    const startProject = useCallback((projectId: string) => {
        setProjects(prevProjects => prevProjects.map(project => ({
            ...project,
            timeSpentCurrentSession: 0,
        })));
        setRunningProjectId(projectId);
        setPausedProjectId(null); // Clear paused project when starting a new one
    }, []);

    const stopProjects = useCallback(() => {
        setProjects(prevProjects => prevProjects.map(project => ({
            ...project,
            timeSpentCurrentSession: 0,
        })));
        setRunningProjectId(null);
    }, []);

    const sseConnectionRef = useWebSocket(startProject, stopProjects);

    const handleProjectClick = useCallback(async (projectId: string) => {
        if (projectId === runningProjectId) {
            stopProjects();
            await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/timesessions/stop`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`,
                },
            });

        } else {
            startProject(projectId);
            await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/timesessions/start`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`,
                },
                body: JSON.stringify(projectId),
            });

        }
    }, [runningProjectId, startProject, stopProjects, sseConnectionRef]);

    const addNewProject = useCallback((newProject: GetProjectDto) => {
        const addedProject: ProjectSessionDTO = {
            ...newProject,
            isRunning: false,
            timeSpentCurrentSession: 0,
            timeSpentTotal: 0,
        };
        setProjects(prevProjects => [...prevProjects, addedProject]);
    }, []);

    const value = {
        projects,
        runningProjectId,
        pausedProjectId,
        handleProjectClick,
        addNewProject,
        setPausedProjectId, // Expose the method to set paused project
    };

    return (
        <ProjectContext.Provider value={value}>
            {children}
        </ProjectContext.Provider>
    );
};

export const useProjectContext = () => {
    const context = useContext(ProjectContext);
    if (context === undefined) {
        throw new Error('useProjectContext must be used within a ProjectProvider');
    }
    return context;
};
