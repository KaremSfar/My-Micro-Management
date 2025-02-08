import { useEffect, useRef } from "react";
import { HubConnection, HubConnectionBuilder, JsonHubProtocol } from "@microsoft/signalr";
import { useAuth } from "../../Auth/AuthContext";

export const useWebSocket = (startProject: (projectId: string) => void, stopProjects: () => void) => {
    const { accessToken } = useAuth();
    const webSocketConnectionRef = useRef<HubConnection | null>(null);

    useEffect(() => {
        if (!accessToken) return;

        webSocketConnectionRef.current = new HubConnectionBuilder()
            .withUrl(`${process.env.REACT_APP_MAIN_SERVICE_BASE_URL}/hub/timesessionshub`, {
                accessTokenFactory: () => accessToken!,
            })
            .withAutomaticReconnect()
            .withHubProtocol(new JsonHubProtocol())
            .build();

        webSocketConnectionRef.current.start();

        webSocketConnectionRef.current.on("TimeSessionStarted", (projectId: string) => {
            startProject(projectId);
        });

        webSocketConnectionRef.current.on("TimeSessionsStopped", () => {
            stopProjects();
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
