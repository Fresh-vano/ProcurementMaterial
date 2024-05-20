// src/routes/PrivateRoute.js
import React from 'react';
import { Route, Navigate } from 'react-router-dom';
import { useAuth } from '../utils/auth';

const PrivateRoute = ({ element: Element, roles, ...rest }) => {
  const { user } = useAuth();

  if (!user) {
    return <Navigate to="/login" />;
  }

  if (roles && !roles.some((role) => user.roles.includes(role))) {
    return <div>Access Denied</div>;
  }

  return <Route {...rest} element={<Element />} />;
};

export default PrivateRoute;
