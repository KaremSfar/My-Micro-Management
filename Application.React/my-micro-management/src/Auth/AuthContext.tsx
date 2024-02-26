// src/contexts/AuthContext.tsx
import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';

interface IAuthContext {
    accessToken: string | null;
    isLoading: boolean;
    setAccessToken: (token: string | null) => void;
    login: (email: string, password: string) => Promise<void>;
    refreshAuthToken: () => Promise<void>;
    logout: () => Promise<void>;
}

const AuthContext = createContext<IAuthContext | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [accessToken, setAccessToken] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(true); // Add a loading state

    const login = async (email: string, password: string) => {
        const response = await fetch('https://localhost:44325/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password }),
            credentials: 'include'
        });
        const data = await response.json();
        setAccessToken(data.accessToken); // Assuming your API returns the access token directly
    };

    const refreshAuthToken = async () => {
        setIsLoading(true);
        try {
            const response = await fetch('https://localhost:44325/auth/refresh-token', {
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
        await fetch('https://localhost:44325/auth/logout', {
            method: 'POST',
            credentials: 'include',
        });

        setAccessToken(null);
    }

    // Attempt to refresh the token on app startup
    useEffect(() => {
        refreshAuthToken();
    }, []);

    return (
        <AuthContext.Provider value={{ accessToken, setAccessToken, login, refreshAuthToken, isLoading, logout }}>
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
