import './App.css';
import LoginForm from './Auth/LoginForm';

// LEARN: React components are usually functions that return tsx code
function App() {
  const handleLogin = (email: string, password: string) => {
    console.log('Login Attempt:', email, password);
    // Here we'll later add the logic to call the API
  };

  return (
    <div className="bg-gray-200 min-h-screen flex justify-center items-center bg-gradient-to-r from-orange-300 to-orange-400">
      <div className="w-3/4 h-3/4 bg-white border border-gray-300 rounded-lg shadow-xl p-8 m-4 overflow-auto">
        <LoginForm onLogin={handleLogin} />
      </div>
    </div>
  );
}

export default App;
