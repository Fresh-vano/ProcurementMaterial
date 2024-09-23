// src/utils/auth.js
import React, { createContext, useContext, useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import api from '../api';
import { Navigate } from 'react-router-dom';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  // Флаг для обозначения завершения проверки токена
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decodedToken = jwtDecode(token);
        const currentTime = Date.now() / 1000;
        if (decodedToken.exp > currentTime) {
          const userRole = decodedToken.role.toLowerCase();
          setUser({ username: decodedToken.unique_name, roles: [userRole] });
        } else {
          // Токен истёк
          localStorage.removeItem('token');
        }
      } catch (error) {
        console.error('Ошибка при декодировании токена:', error);
        localStorage.removeItem('token');
      }
    }
    setLoading(false);
  }, []);

  const login = async (username, password, callback) => {
    try {
      const response = await api.post('/user/auth', {
        username: username,
        password: password,
      });

      if (response.status === 200) {
        const { token } = response.data;
        localStorage.setItem('token', token);

        const decodedToken = jwtDecode(token);
        const userRole = decodedToken.role.toLowerCase();

        setUser({ username: decodedToken.unique_name, roles: [userRole] });
        callback(userRole);
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
    <AuthContext.Provider value={{ user, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);

// src/utils/auth.js

export const RequireAuth = ({ children, roles }) => {
  const { user, loading } = useAuth();

  if (loading) {
    // Можно отобразить индикатор загрузки или ничего не отображать
    return null;
  }

  if (!user) {
    return <Navigate to="/login" />;
  }

  // Проверяем токен на валидность
  const token = localStorage.getItem('token');
  if (token) {
    try {
      const decodedToken = jwtDecode(token);
      const currentTime = Date.now() / 1000;
      if (decodedToken.exp < currentTime) {
        // Токен истёк
        localStorage.removeItem('token');
        return <Navigate to="/login" />;
      }
    } catch (error) {
      console.error('Ошибка при декодировании токена:', error);
      localStorage.removeItem('token');
      return <Navigate to="/login" />;
    }
  } else {
    return <Navigate to="/login" />;
  }

  // Проверяем роль пользователя
  const userRoles = user.roles.map((role) => role.toLowerCase());
  const requiredRoles = roles.map((role) => role.toLowerCase());

  if (roles && !requiredRoles.some((role) => userRoles.includes(role))) {
    return <div>Доступ запрещен</div>;
  }

  return children;
};
