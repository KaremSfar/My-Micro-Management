import { createContext, ReactNode, useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { useAuth } from '../Auth/AuthContext';
import { CreateContextDTO, GetContextDto } from '../DTOs/ContextDto';

interface IContextContext {
    contexts: GetContextDto[];
    isLoadingContexts: boolean;
    selectedContextId: string | null;
    setSelectedContextId: (contextId: string | null) => void;
    createNewContext: (createContextDto: CreateContextDTO) => Promise<GetContextDto | null>;
}

const ContextContext = createContext<IContextContext | undefined>(undefined);

export const ContextProvider = ({ children }: { children: ReactNode }) => {
    const { accessToken } = useAuth();
    const [contexts, setContexts] = useState<GetContextDto[]>([]);
    const [isLoadingContexts, setIsLoadingContexts] = useState(true);
    const [selectedContextId, setSelectedContextId] = useState<string | null>(null);

    useEffect(() => {
        const fetchContexts = async () => {
            if (!accessToken) return;

            try {
                setIsLoadingContexts(true);
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
                setSelectedContextId((previousSelectedContextId) => {
                    if (!previousSelectedContextId && data.length > 0) {
                        return data[0].id;
                    }

                    if (previousSelectedContextId && !data.some(c => c.id === previousSelectedContextId)) {
                        return null;
                    }

                    return previousSelectedContextId;
                });
            } catch (error) {
                console.error('Error fetching contexts:', error);
            } finally {
                setIsLoadingContexts(false);
            }
        };

        if (accessToken) {
            fetchContexts();
        } else {
            setIsLoadingContexts(false);
        }
    }, [accessToken]);

    const createNewContext = useCallback(async (createContextDto: CreateContextDTO): Promise<GetContextDto | null> => {
        if (!accessToken) return null;

        const response = await fetch(`${import.meta.env.VITE_MAIN_SERVICE_BASE_URL}/api/contexts`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${accessToken}`,
            },
            body: JSON.stringify(createContextDto),
        });

        if (!response.ok) {
            if (response.status === 401) {
                console.error('Unauthorized - token may have expired');
            }
            throw new Error(`Failed to create context: ${response.status}`);
        }

        const createdContext = await response.json() as GetContextDto;
        setContexts(prev => [...prev, createdContext]);
        setSelectedContextId((previousSelectedContextId) => previousSelectedContextId ?? createdContext.id);
        return createdContext;
    }, [accessToken]);

    const value = useMemo(() => ({
        contexts,
        isLoadingContexts,
        selectedContextId,
        setSelectedContextId,
        createNewContext,
    }), [contexts, isLoadingContexts, selectedContextId, createNewContext]);

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
