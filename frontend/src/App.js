// src/App.js
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Login from './components/Auth/Login';
import ManagerDashboard from './components/Manager/Dashboard';
import PurchaserRequestView from './components/Purchaser/RequestView';
import ReportGroupChartBuilder from './components/ReportGroup/ChartBuilder';
import PrivateRoute from './routes/PrivateRoute';
import { AuthProvider, RequireAuth } from './utils/auth';

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<Login />} />
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
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
