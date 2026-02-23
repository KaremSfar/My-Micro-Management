import { createContext, ReactNode, useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { useAuth } from '../Auth/AuthContext';
import { CreateContextDTO, GetContextDto } from '../DTOs/ContextDto';
import { useContexts } from '../hooks/context/useContexts';
import { useCreateContext } from '../hooks/context/useCreateContext';

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
    const [selectedContextId, setSelectedContextId] = useState<string | null>(null);

    const { data: contexts = [], isLoading: isLoadingContexts } = useContexts();

    useEffect(() => {
        if (!contexts.length) return;
        setSelectedContextId((prev) => {
            if (!prev && contexts.length > 0) return contexts[0].id;
            if (prev && !contexts.some(c => c.id === prev)) return null;
            return prev;
        });
    }, [contexts]);

    const { mutateAsync: createContextMutate } = useCreateContext((createdContext) => {
        setSelectedContextId((prev) => prev ?? createdContext.id);
    });

    const createNewContext = useCallback(async (createContextDto: CreateContextDTO): Promise<GetContextDto | null> => {
        if (!accessToken) return null;
        return createContextMutate(createContextDto);
    }, [accessToken, createContextMutate]);

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
