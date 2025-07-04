import { useEffect, useRef } from "react";
import { fetchEventSource } from '@microsoft/fetch-event-source';
import { useAuth } from "../../Auth/AuthContext";

export const useServerSideEvents = (startProject: (projectId: string) => void, stopProjects: () => void) => {
    const { accessToken } = useAuth();
    const abortControllerRef = useRef<AbortController | null>(null);

    useEffect(() => {
        if (!accessToken) return;

        const abortController = new AbortController();
        abortControllerRef.current = abortController;

        fetchEventSource(`${import.meta.env.VITE_ACTIVITY_SERVICE_BASE_URL}/events`, {
            headers: {
                'Authorization': `Bearer ${accessToken}`,
            },
            signal: abortController.signal,
            onmessage(event) {
                try {
                    const data = JSON.parse(event.data);

                    if (data.userActivityEventType === "TimeSessionStarted" && data.eventData?.projectId) {
                        startProject(data.eventData.projectId);
                    } else if (data.userActivityEventType === "TimeSessionStopped") {
                        stopProjects();
                    }
                } catch (error) {
                    console.error("Error parsing SSE message:", error);
                }
            },
            onopen(_) {
                console.log("SSE connection opened");
                return Promise.resolve(); // Return a promise to indicate readiness
            },
            onerror(error) {
                console.error("SSE connection error:", error);
                // Returns true to retry, false to stop
                return 1000; // Retry after 1 second
            },
            openWhenHidden: true, // Keep connection open when tab is hidden
        });

        return () => {
            abortController.abort();
        };
    }, [accessToken, startProject, stopProjects]);

    return abortControllerRef;
};