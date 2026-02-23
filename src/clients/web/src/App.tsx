import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import './App.css';
import { useAuth } from './Auth/AuthContext';
import LoginForm from './Auth/LoginForm';
import Dashboard from './Pages/Dashboard';
import Layout from './Components/Layout';
import Analytics from './Pages/Analytics';
import SignupForm from './Auth/SignupForm';
import GoogleAuthCallback from './Auth/GoogleAuthCallback';
import { ProjectProvider } from './context/ProjectContext';
import { ContextProvider } from './context/ContextContext';

function App() {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return (
      <div className="min-h-screen flex justify-center items-center bg-gradient-to-r from-orange-300 to-orange-400">
        <div className="text-center">
          <div className="animate-spin rounded-full h-16 w-16 border-b-4 border-white mx-auto mb-4"></div>
          <p className="text-white font-medium text-lg">Loading...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen flex justify-center items-center bg-gradient-to-r from-orange-300 to-orange-400">
      <div className="border-2 border-black lg:h-[75vh] w-3/4 flex justify-center items-center bg-white rounded-lg shadow-xl p-5 m-4">
          <BrowserRouter>
            {isAuthenticated ? (
              <ContextProvider>
                <ProjectProvider>
                  <Routes>
                    <Route path="/" element={<Layout><Dashboard /></Layout>} />
                    <Route path="/analytics" element={<Layout><Analytics /></Layout>} />
                    <Route path="/login" element={<Navigate to="/" replace />} />
                    <Route path="/signup" element={<Navigate to="/" replace />} />
                    <Route path="/google-login-success" element={<GoogleAuthCallback />} />
                    <Route path="*" element={<Layout><Dashboard /></Layout>} />
                  </Routes>
                </ProjectProvider>
              </ContextProvider>
            ) : (
              <Routes>
                <Route path="/login" element={<LoginForm />} />
                <Route path="/signup" element={<SignupForm />} />
                <Route path="/google-login-success" element={<GoogleAuthCallback />} />
                <Route path="*" element={<Navigate to="/login" replace />} />
              </Routes>
            )}
          </BrowserRouter>
      </div>
    </div>
  );
}

export default App;
