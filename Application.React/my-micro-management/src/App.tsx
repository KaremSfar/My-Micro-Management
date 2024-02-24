import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import './App.css';
import { useAuth } from './Auth/AuthContext';
import LoginForm from './Auth/LoginForm';
import Dashboard from './Pages/Dashboard';
import Layout from './Components/Layout';
import Analytics from './Pages/Analytics';

// LEARN: React components are usually functions that return tsx code
function App() {
  const { accessToken, isLoading } = useAuth(); // Use the useAuth hook to access the accessToken


  return (
    <div className="bg-gray-200 min-h-screen flex justify-center items-center bg-gradient-to-r from-orange-300 to-orange-400">
      <div style={{ height: '75vh' }} className="w-3/4 flex justify-center items-center bg-white border border-gray-300 rounded-lg shadow-xl p-8 m-4">
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
            </Routes>
          </BrowserRouter>}

      </div>
    </div>
  );
}

export default App;
