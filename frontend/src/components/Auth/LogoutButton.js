import React from 'react';
import { Button } from '@mui/material';
import { useAuth } from '../../utils/auth';
import { useNavigate } from 'react-router-dom';

const LogoutButton = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout(() => {
      navigate('/login');
    });
  };

  return (
    <Button color="inherit" onClick={handleLogout}>
      Выйти
    </Button>
  );
};

export default LogoutButton;
