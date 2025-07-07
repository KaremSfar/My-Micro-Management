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

function App() {
  const { isAuthenticated } = useAuth();

  return (
    <div className="min-h-screen flex justify-center items-center bg-gradient-to-r from-orange-300 to-orange-400">
      <div className="border-2 border-black lg:h-[75vh] w-3/4 flex justify-center items-center bg-white rounded-lg shadow-xl p-5 m-4">
          <BrowserRouter>
            {isAuthenticated ? (
              <ProjectProvider> {/* Wrap authenticated routes with the provider */}
                <Routes>
                  <Route path="/" element={<Layout><Dashboard /></Layout>} />
                  <Route path="/analytics" element={<Layout><Analytics /></Layout>} />
                  <Route path="/login" element={<Navigate to="/" replace />} />
                  <Route path="/signup" element={<Navigate to="/" replace />} />
                  <Route path="/google-login-success" element={<GoogleAuthCallback />} />
                  <Route path="*" element={<Layout><Dashboard /></Layout>} />
                </Routes>
              </ProjectProvider>
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
