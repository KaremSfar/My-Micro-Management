import React, { useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContext';

const GoogleAuthCallback: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { handleGoogleAuthResponse } = useAuth();

  useEffect(() => {
    const searchParams = new URLSearchParams(location.search);
    const code = searchParams.get('code');

    if (code) {
      handleGoogleAuthResponse()
        .catch((error) => {
          console.error('Error handling Google auth response:', error);
          navigate('/login'); // Redirect to login page on error
        });
    } else {
      console.error('No code found in URL');
      navigate('/login');
    }
  }, [location, handleGoogleAuthResponse, navigate]);

  return <div>Loading...</div>;
};

export default GoogleAuthCallback;
