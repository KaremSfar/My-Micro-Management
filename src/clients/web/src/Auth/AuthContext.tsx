// src/contexts/AuthContext.tsx
import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';

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
}

const AuthContext = createContext<IAuthContext | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [accessToken, setAccessToken] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(true); // Add a loading state

    const login = async (email: string, password: string) => {
        const response = await fetch(`${process.env.REACT_APP_AUTH_SERVICE_BASE_URL}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password }),
            credentials: 'include'
        });
        const data = await response.json();
        setAccessToken(data.accessToken);
    };

    const singup = async (firstName: string, lastName: string, email: string, password: string) => {
        const response = await fetch(`${process.env.REACT_APP_AUTH_SERVICE_BASE_URL}/auth/register`, {
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
            const response = await fetch(`${process.env.REACT_APP_AUTH_SERVICE_BASE_URL}/auth/refresh-token`, {
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
        await fetch(`${process.env.REACT_APP_AUTH_SERVICE_BASE_URL}/auth/logout`, {
            method: 'POST',
            credentials: 'include',
        });

        setAccessToken(null);
    }

    const loginWithGoogle = () => {
        window.location.href = `${process.env.REACT_APP_AUTH_SERVICE_BASE_URL}/google-login`;
    };

    const handleGoogleAuthResponse = async () => {
        try {
            // Assuming Refresh token is appendeds
            await refreshAuthToken();
        } catch (error) {
            console.error('Error during Google authentication:', error);
            // Handle error (e.g., show error message to user)
        }
    };

    // Attempt to refresh the token on app startup
    useEffect(() => {
        refreshAuthToken();
    }, []);

    return (
        <AuthContext.Provider value={{ accessToken, setAccessToken, login, singup, refreshAuthToken, isLoading, logout, loginWithGoogle, handleGoogleAuthResponse }}>
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
