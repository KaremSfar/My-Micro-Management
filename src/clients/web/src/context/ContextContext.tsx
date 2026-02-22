import { createContext, ReactNode, useContext, useEffect, useMemo, useState } from 'react';
import { useAuth } from '../Auth/AuthContext';
import { GetContextDto } from '../DTOs/ContextDto';

interface IContextContext {
    contexts: GetContextDto[];
    selectedContextId: string | null;
    setSelectedContextId: (contextId: string | null) => void;
}

const ContextContext = createContext<IContextContext | undefined>(undefined);

export const ContextProvider = ({ children }: { children: ReactNode }) => {
    const { accessToken } = useAuth();
    const [contexts, setContexts] = useState<GetContextDto[]>([]);
    const [selectedContextId, setSelectedContextId] = useState<string | null>(null);

    useEffect(() => {
        const fetchContexts = async () => {
            if (!accessToken) return;

            try {
                const response = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/contexts`, {
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${accessToken}`,
                    },
                });

                if (!response.ok) {
                    if (response.status === 401) {
                        console.error('Unauthorized - token may have expired');
                    }
                    throw new Error(`Failed to fetch contexts: ${response.status}`);
                }

                const data: GetContextDto[] = await response.json();
                setContexts(data);

                if (selectedContextId && !data.some(c => c.id === selectedContextId)) {
                    setSelectedContextId(null);
                }
            } catch (error) {
                console.error('Error fetching contexts:', error);
            }
        };

        if (accessToken) {
            fetchContexts();
        }
    }, [accessToken]);

    const value = useMemo(() => ({
        contexts,
        selectedContextId,
        setSelectedContextId,
    }), [contexts, selectedContextId]);

    return (
        <ContextContext.Provider value={value}>
            {children}
        </ContextContext.Provider>
    );
};

export const useContextContext = () => {
    const context = useContext(ContextContext);
    if (context === undefined) {
        throw new Error('useContextContext must be used within a ContextProvider');
    }
    return context;
};
