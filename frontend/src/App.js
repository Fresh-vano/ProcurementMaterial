// src/App.js
import React from 'react';
import { BrowserRouter as Router, Navigate, Route, Routes, useNavigate } from 'react-router-dom';
import Login from './components/Auth/Login';
import ManagerDashboard from './components/Manager/Dashboard';
import PurchaserRequestView from './components/Purchaser/RequestView';
import ReportGroupChartBuilder from './components/ReportGroup/ChartBuilder';
import { AuthProvider, RequireAuth, useAuth } from './utils/auth';

function App() {
  return (
    <AuthProvider>
      <Router>
        <AppRoutes />
      </Router>
    </AuthProvider>
  );
}

function AppRoutes() {
  const { user, loading } = useAuth();
  const navigate = useNavigate();

  React.useEffect(() => {
    if (!loading && user) {
      // Пользователь аутентифицирован, перенаправляем на соответствующую страницу
      const userRole = user.roles[0];
      switch (userRole) {
        case 'manager':
          navigate('/manager');
          break;
        case 'purchaser':
          navigate('/purchaser');
          break;
        case 'report_group':
          navigate('/report-group');
          break;
        default:
          navigate('/login');
      }
    }
  }, [user, loading, navigate]);

  if (loading) {
    // Можно отобразить индикатор загрузки
    return null;
  }

  return (
    <Routes>
      <Route
        path="/manager"
        element={
          <RequireAuth roles={['manager']}>
            <ManagerDashboard />
          </RequireAuth>
        }
      />
      <Route
        path="/purchaser"
        element={
          <RequireAuth roles={['purchaser']}>
            <PurchaserRequestView />
          </RequireAuth>
        }
      />
      <Route
        path="/report-group"
        element={
          <RequireAuth roles={['report_group']}>
            <ReportGroupChartBuilder />
          </RequireAuth>
        }
      />
      <Route path="/login" element={<Login />} />
      <Route path="/" element={<Login />} />
      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  );
}

export default App;
