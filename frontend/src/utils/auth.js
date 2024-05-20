// src/utils/auth.js
import {Navigate} from "react-router-dom"
import React, { createContext, useContext, useState } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  const login = (username, password, callback) => {
    const users = {
      manager: { password: '123', roles: ['manager'] },
      purchaser: { password: '123', roles: ['purchaser'] },
      report_group: { password: '123', roles: ['report_group'] },
    };

    if (users[username] && users[username].password === password) {
      setUser({ username, roles: users[username].roles });
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
