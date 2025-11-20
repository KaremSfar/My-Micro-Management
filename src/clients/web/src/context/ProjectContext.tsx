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
    // Fetch initial projects only once on mount - DO NOT refetch on token refresh
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

                if (!response.ok) {
                    if (response.status === 401) {
                        console.error('Unauthorized - token may have expired');
                        // Token is invalid, will be handled by auth context
                    }
                    throw new Error(`Failed to fetch projects: ${response.status}`);
                }

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

    // WebSocket connection is managed internally by useWebSocket hook
    useWebSocket(startProject, stopProjects);

    const handleProjectClick = useCallback(async (projectId: string) => {
        if (!accessToken) {
            console.error('No access token available');
            return;
        }

        if (projectId === runningProjectId) {
            stopProjects();
            const response = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/timesessions/stop`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`,
                },
            });

            if (!response.ok && response.status === 401) {
                console.error('Unauthorized - token may have expired');
            }
        } else {
            startProject(projectId);
            const response = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/timesessions/start`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`,
                },
                body: JSON.stringify(projectId),
            });

            if (!response.ok && response.status === 401) {
                console.error('Unauthorized - token may have expired');
            }
        }
    }, [runningProjectId, startProject, stopProjects, accessToken]);

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
