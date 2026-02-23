// src/contexts/AuthContext.tsx
import { createContext, useCallback, useContext, useState, useEffect, ReactNode } from 'react';
import { useMutation } from '@tanstack/react-query';

interface IAuthContext {
    accessToken: string | null;
    isLoading: boolean;
    setAccessToken: (token: string | null) => void;
    login: (email: string, password: string) => Promise<void>;
    singup: (firstName: string, lastName: string, email: string, password: string) => Promise<void>;
    refreshAuthToken: () => Promise<void>;
    logout: () => Promise<void>;
    loginWithGoogle: () => void;
    handleGoogleAuthResponse: () => Promise<void>;
    isAuthenticated: boolean;
}

const AuthContext = createContext<IAuthContext | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [accessToken, setAccessTokenInternal] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(true); // Add a loading state
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const setAccessToken = (token: string | null) => {
        setAccessTokenInternal(token);

        if (!!token !== isAuthenticated)
            setIsAuthenticated(!!token);
    };

    const loginMutation = useMutation({
        mutationFn: async ({ email, password }: { email: string; password: string }) => {
            const response = await fetch(`${import.meta.env.VITE_AUTH_SERVICE_BASE_URL}/auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password }),
                credentials: 'include'
            });
            return response.json();
        },
        onSuccess: (data) => setAccessToken(data.accessToken),
    });

    const singupMutation = useMutation({
        mutationFn: async ({ firstName, lastName, email, password }: { firstName: string; lastName: string; email: string; password: string }) => {
            const response = await fetch(`${import.meta.env.VITE_AUTH_SERVICE_BASE_URL}/auth/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ firstName, lastName, email, password }),
                credentials: 'include'
            });
            return response.json();
        },
        onSuccess: (data) => setAccessToken(data.accessToken),
    });

    const refreshMutation = useMutation({
        mutationFn: async () => {
            const response = await fetch(`${import.meta.env.VITE_AUTH_SERVICE_BASE_URL}/auth/refresh-token`, {
                method: 'POST',
                credentials: 'include',
            });
            if (!response.ok) throw new Error('Failed to refresh token');
            return response.json();
        },
        onSuccess: (data) => setAccessToken(data.accessToken),
    });

    const logoutMutation = useMutation({
        mutationFn: async () => {
            await fetch(`${import.meta.env.VITE_AUTH_SERVICE_BASE_URL}/auth/logout`, {
                method: 'POST',
                credentials: 'include',
            });
        },
        onSuccess: () => setAccessToken(null),
    });

    const login = async (email: string, password: string) => {
        await loginMutation.mutateAsync({ email, password });
    };

    const singup = async (firstName: string, lastName: string, email: string, password: string) => {
        await singupMutation.mutateAsync({ firstName, lastName, email, password });
    };

    const refreshAuthToken = useCallback(async (showLoading = false) => {
        if (showLoading) setIsLoading(true);
        try {
            await refreshMutation.mutateAsync();
        } catch {
            // Handle absence or invalidity of refresh token here
        } finally {
            if (showLoading) setIsLoading(false);
        }
    }, [refreshMutation]);

    const logout = async () => {
        await logoutMutation.mutateAsync();
    };

    const loginWithGoogle = () => {
        window.location.href = `${import.meta.env.VITE_AUTH_SERVICE_BASE_URL}/google-login?returnUrl=${window.location.origin}`;
    };

    const handleGoogleAuthResponse = async () => {
        try {
            // Assuming Refresh token is appended
            await refreshAuthToken();
        } catch (error) {
            console.error('Error during Google authentication:', error);
            // Handle error (e.g., show error message to user)
        }
    };

    // Auto-refresh token every 5 minutes (before 15min expiry)
    useEffect(() => {
        if (!accessToken) return;

        // Refresh immediately on mount/when token changes
        const timer = setInterval(() => {
            refreshAuthToken();
        }, 5 * 60 * 1000); // 5 minutes

        return () => clearInterval(timer);
    }, [accessToken]); // Depend on the actual token, not just its existence


    // Attempt to refresh the token on app startup
    useEffect(() => {
        refreshAuthToken(true);
    }, []);

    return (
        <AuthContext.Provider value={{ accessToken, setAccessToken, login, singup, refreshAuthToken, isLoading, logout, loginWithGoogle, handleGoogleAuthResponse, isAuthenticated }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};
