import { useQuery } from "@tanstack/react-query";
import { ProjectSessionDTO } from "../../DTOs/ProjectDto";
import { useAuth } from "../../Auth/AuthContext";

export const useProjects = () => {
    const { accessToken } = useAuth();

    // Fetch projects using React Query
    const {
        data: projects = [],
        isLoading,
        isError,
        error,
        refetch,
    } = useQuery({
        queryKey: ["projects", accessToken],
        queryFn: async () => {
            const response = await fetch(
                `${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/projects`,
                {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${accessToken}`,
                    },
                }
            );

            if (!response.ok) {
                if (response.status === 401) {
                    console.error("Unauthorized - token may have expired");
                }
                throw new Error(`Failed to fetch projects: ${response.status}`);
            }

            return response.json() as Promise<ProjectSessionDTO[]>;
        },
        enabled: !!accessToken, // Only run query when accessToken is available
        staleTime: 5 * 60 * 1000, // 5 minutes
    });

    // Derive the running project ID from projects data
    const runningProjectId = projects.find(p => p.isRunning)?.id ?? null;

    return {
        projects,
        isLoading,
        isError,
        error,
        runningProjectId,
        refetch,
    };
};
