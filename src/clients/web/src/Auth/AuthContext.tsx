// src/contexts/AuthContext.tsx
import { createContext, useContext, useState, useEffect, ReactNode } from 'react';

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

    const login = async (email: string, password: string) => {
        const response = await fetch(`${import.meta.env.VITE_AUTH_SERVICE_BASE_URL}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password }),
            credentials: 'include'
        });
        const data = await response.json();
        setAccessToken(data.accessToken);
    };

    const singup = async (firstName: string, lastName: string, email: string, password: string) => {
        const response = await fetch(`${import.meta.env.VITE_AUTH_SERVICE_BASE_URL}/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ firstName, lastName, email, password }),
            credentials: 'include'
        });
        const data = await response.json();
        setAccessToken(data.accessToken);
    }

    const refreshAuthToken = async () => {
        setIsLoading(true);
        try {
            const response = await fetch(`${import.meta.env.VITE_AUTH_SERVICE_BASE_URL}/auth/refresh-token`, {
                method: 'POST',
                credentials: 'include', // Necessary to send the HttpOnly cookie
            });

            if (!response.ok) {
                throw new Error('Failed to refresh token');
            }

            const data = await response.json();
            setAccessToken(data.accessToken); // Update the access token

            // Optionally, handle successful refresh (e.g., update UI or state)

        } catch (error) {
            // Handle absence or invalidity of refresh token here
            // For example, redirect to login page or show a login prompt
        } finally {
            setIsLoading(false);
        }
    };

    const logout = async () => {
        await fetch(`${import.meta.env.VITE_AUTH_SERVICE_BASE_URL}/auth/logout`, {
            method: 'POST',
            credentials: 'include',
        });

        setAccessToken(null);
    }

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
        refreshAuthToken();
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
