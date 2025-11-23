import { useEffect, useRef } from "react";
import { HubConnection, HubConnectionBuilder, JsonHubProtocol } from "@microsoft/signalr";
import { useAuth } from "../Auth/AuthContext";

// UserActivityEventType enum equivalent to C# enum
type UserActivityEventType = "TimeSessionStarted" | "TimeSessionStopped";

// UserActivityEvent type equivalent to C# record
export type UserActivityEvent = {
    userActivityEventType: UserActivityEventType;
    userId: string;
    eventData: { projectId: string, userId: string };
};


export const useWebSocket = (startProject: (projectId: string) => void, stopProjects: () => void) => {
    const { accessToken } = useAuth();
    const webSocketConnectionRef = useRef<HubConnection | null>(null);

    useEffect(() => {
        if (!accessToken) return;

        webSocketConnectionRef.current = new HubConnectionBuilder()
            .withUrl(`${import.meta.env.VITE_ACTIVITY_SERVICE_BASE_URL}/hub/timesessionshub`, {
                accessTokenFactory: () => accessToken!,
            })
            .withAutomaticReconnect()
            .withHubProtocol(new JsonHubProtocol())
            .build();

        webSocketConnectionRef.current.start();

        webSocketConnectionRef.current.on("ReceiveEvent", (projectId: any) => {
            switch (projectId.userActivityEventType) {
                case "TimeSessionStarted":
                    startProject(projectId.eventData.projectId);
                    break;
                case "TimeSessionStopped":
                    stopProjects();
                    break;
                default:
                    console.warn("Unknown event type received:", projectId.userActivityEventType);
            }
        });

        webSocketConnectionRef.current.onclose((error) => {
            console.error("WebSocket connection closed:", error);
        });

        return () => {
            webSocketConnectionRef.current?.stop();
            webSocketConnectionRef.current = null;
        };
    }, [accessToken]);

    return webSocketConnectionRef;
};
