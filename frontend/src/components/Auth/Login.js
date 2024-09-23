// src/components/Auth/Login.js
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { TextField, Button, Container, Typography } from '@mui/material';
import { useAuth } from '../../utils/auth';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { user, login } = useAuth();

  useEffect(() => {
    if (user) {
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
  }, [user, navigate]);

  const handleSubmit = (event) => {
    event.preventDefault();
    login(username, password, (userRole) => {
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
    });
  };

  return (
    <Container component="main" maxWidth="xs">
      <Typography component="h1" variant="h5">
        Вход
      </Typography>
      <form onSubmit={handleSubmit}>
        <TextField
          variant="outlined"
          margin="normal"
          required
          fullWidth
          label="Логин"
          autoFocus
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
        <TextField
          variant="outlined"
          margin="normal"
          required
          fullWidth
          label="Пароль"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <Button type="submit" fullWidth variant="contained" color="primary">
          Войти
        </Button>
      </form>
    </Container>
  );
};

export default Login;
