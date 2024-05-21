// src/utils/auth.js
import {Navigate} from "react-router-dom"
import React, { createContext, useContext, useState } from 'react';
import axios from "axios";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  const login = async (username, password, callback) => {
    const response = await axios.post('http://localhost:8080/User/auth', {
      username,
      password,
    });

    if (response.status === 200) {
      const role = response.data;
      setUser({ username, roles: [role] });
      callback();
    } else {
      alert('Invalid credentials');
    }
  };

  const logout = (callback) => {
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

  if (roles && !roles.some((role) => user.roles.includes(role))) {
    return <div>Access Denied</div>;
  }

  return children;
};
