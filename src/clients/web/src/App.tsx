import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import './App.css';
import { useAuth } from './Auth/AuthContext';
import LoginForm from './Auth/LoginForm';
import Dashboard from './Pages/Dashboard';
import Layout from './Components/Layout';
import Analytics from './Pages/Analytics';
import SignupForm from './Auth/SignupForm';
import GoogleAuthCallback from './Auth/GoogleAuthCallback';

// LEARN: React components are usually functions that return tsx code
function App() {
  const { accessToken, isLoading } = useAuth(); // Use the useAuth hook to access the accessToken

  return (
    <div className="min-h-screen flex justify-center items-center bg-gradient-to-r from-orange-300 to-orange-400">
      <div className="border-2 border-black lg:h-[75vh] w-3/4 flex justify-center items-center bg-white rounded-lg shadow-xl p-5 m-4">
        {isLoading ?
          <div className="flex justify-center items-center">
            <div className="spinner-border animate-spin inline-block w-8 h-8 border-4 rounded-full" role="status">
              <span className="visually-hidden"></span>
            </div>
          </div>
          :
          <BrowserRouter>
            <Routes>
              <Route path="/" element={accessToken ? <Layout><Dashboard /></Layout> : <Navigate to="/login" replace />} />
              <Route path="/analytics" element={accessToken ? <Layout><Analytics /></Layout> : <Navigate to="/login" replace />} />
              <Route path="/login" element={accessToken ? <Navigate to="/" replace /> : <LoginForm />} />
              <Route path="/signup" element={accessToken ? <Navigate to="/" replace /> : <SignupForm />} />
              <Route path="/google-login-success" element={<GoogleAuthCallback />} />
              <Route path="*" element={accessToken ? <Layout><Dashboard /></Layout> : <Navigate to="/login" replace />} /> {/* Reroute all to / if not found */}
            </Routes>
          </BrowserRouter>}

      </div>
    </div>
  );
}

export default App;
