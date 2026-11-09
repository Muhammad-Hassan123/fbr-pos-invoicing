import { createContext, useContext, useState, useEffect } from 'react';
import { api, authAPI } from '../services/api';
import toast from 'react-hot-toast';

const AuthContext = createContext();

function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}

function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [authChecked, setAuthChecked] = useState(false);
  


  useEffect(() => {
    if (!authChecked) {
      checkAuthStatus();
    }
  }, [authChecked]);

  const checkAuthStatus = async () => {
    try {
      const token = localStorage.getItem('token');
      console.log('Checking auth status, token exists:', !!token);
      if (token) {
        console.log('Token found, calling profile API...');
        const response = await authAPI.getProfile();
        console.log('Profile API response:', response.data);
        setUser(response.data);
        setIsAuthenticated(true);
      } else {
        console.log('No token found, user not authenticated');
      }
    } catch (error) {
      console.error('Auth check failed:', error);
      console.error('Error response:', error.response);
      // Only logout if it's a 401 error, not other network errors
      if (error.response?.status === 401) {
        logout();
      }
    } finally {
      setLoading(false);
      setAuthChecked(true);
    }
  };

  const login = async (email, password) => {
    try {
      console.log('Attempting login with:', { email });
      const response = await authAPI.login({ email, password });
      console.log('Login response:', response.data);
      const { Token: token, User: user } = response.data;
      
      localStorage.setItem('token', token);
      
      setUser(user);
      setIsAuthenticated(true);
      setAuthChecked(true); // Mark auth as checked after successful login
      
      toast.success('Login successful!');
      return { success: true };
    } catch (error) {
      console.error('Login error:', error);
      console.error('Error response:', error.response);
      const message = error.response?.data?.message || 'Login failed';
      toast.error(message);
      return { success: false, error: message };
    }
  };

  const register = async (userData) => {
    try {
      console.log('Attempting registration with:', { email: userData.email, firstName: userData.firstName });
      const response = await authAPI.register(userData);
      console.log('Registration response:', response.data);
      const { Token: token, User: user } = response.data;
      
      localStorage.setItem('token', token);
      
      setUser(user);
      setIsAuthenticated(true);
      
      toast.success('Registration successful!');
      return { success: true };
    } catch (error) {
      console.error('Registration error:', error);
      console.error('Error response:', error.response);
      const message = error.response?.data?.message || 'Registration failed';
      toast.error(message);
      return { success: false, error: message };
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
    setIsAuthenticated(false);
    toast.success('Logged out successfully');
  };

  const changePassword = async (currentPassword, newPassword) => {
    try {
      await authAPI.changePassword({
        currentPassword,
        newPassword
      });
      toast.success('Password changed successfully!');
      return { success: true };
    } catch (error) {
      const message = error.response?.data?.message || 'Password change failed';
      toast.error(message);
      return { success: false, error: message };
    }
  };

  const value = {
    user,
    isAuthenticated,
    loading,
    login,
    register,
    logout,
    changePassword
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export { useAuth, AuthProvider }; 