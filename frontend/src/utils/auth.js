import React, { createContext, useContext, useState } from 'react';
import { Navigate } from 'react-router-dom';
import jwtDecode from 'jwt-decode';
import api from '../api'; 

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  const login = async (username, password, callback) => {
    try {
      const response = await api.post('/user/auth', {
        username: username,
        password: password,
      });

      if (response.status === 200) {
        const { Token } = response.data; 
        localStorage.setItem('token', Token);

        const decodedToken = jwtDecode(Token);
        const userRoles = [decodedToken.role]; 

        setUser({ username: decodedToken.unique_name, roles: userRoles });
        callback();
      } else {
        alert('Неверный логин или пароль!');
      }
    } catch (error) {
      console.error(error);
      alert('Неверный логин или пароль!');
    }
  };

  const logout = (callback) => {
    // Удаляем токен из localStorage
    localStorage.removeItem('token');
    setUser(null);
    callback();
  };

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);

export const RequireAuth = ({ children, roles }) => {
  const { user } = useAuth();

  if (!user) {
    return <Navigate to="/login" />;
  }

  const token = localStorage.getItem('token');
  if (token) {
    const decodedToken = jwtDecode(token);
    const currentTime = Date.now() / 1000;
    if (decodedToken.exp < currentTime) {
      return <Navigate to="/login" />;
    }
  } else {
    return <Navigate to="/login" />;
  }

  if (roles && !roles.some((role) => user.roles.includes(role))) {
    return <div>Доступ запрещен</div>;
  }

  return children;
};