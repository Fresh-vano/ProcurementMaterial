// src/components/Admin/AdminCreateUser.js
import React, { useState } from 'react';
import {
  TextField,
  Button,
  Snackbar,
  Alert,
  Select,
  MenuItem,
  InputLabel,
  FormControl,
  Box,
  Typography,
  CircularProgress,
  AppBar,
  Toolbar,
} from '@mui/material';
import api from '../../api';
import LogoutButton from '../Auth/LogoutButton';

const AdminCreateUser = () => {
  const [username, setUserName] = useState('');
  const [userShortName, setUserShortName] = useState('');
  const [userRole, setUserRole] = useState(0);
  const [password, setPassword] = useState('');
  const [openSuccess, setOpenSuccess] = useState(false);
  const [openError, setOpenError] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [loading, setLoading] = useState(false);

  const handleClose = () => {
    setOpenSuccess(false);
    setOpenError(false);
    setSuccessMessage('');
    setErrorMessage('');
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!username || !userShortName || !userRole || !password) {
      setErrorMessage('Все поля обязательны для заполнения.');
      setOpenError(true);
      return;
    }

    const newUser = {
      username,
      userShortName,
      userRole: Number(userRole),
      password,
    };

    setLoading(true);

    try {
      const response = await api.post('/user/register', newUser);

      if (response.status === 201 || response.status === 204) {
        setSuccessMessage('Пользователь успешно создан.');
        setOpenSuccess(true);
        setUserName('');
        setUserShortName('');
        setUserRole('');
        setPassword('');
      } else {
        setErrorMessage('Не удалось создать пользователя.');
        setOpenError(true);
      }
    } catch (error) {
      if (error.response && error.response.status === 409) {
        setErrorMessage('Пользователь с таким именем уже существует.');
      } else {
        setErrorMessage('Произошла ошибка при создании пользователя.');
      }
      setOpenError(true);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
        <AppBar position="static">
            <Toolbar>
            <Typography variant="h6" style={{ flexGrow: 1 }}>
                Страница администратора
            </Typography>
            <LogoutButton/>
            </Toolbar>
        </AppBar>
        <Box sx={{ mt: 5, mx: 'auto', width: '50%' }}>
        <Typography variant="h5" gutterBottom>
            Создание нового пользователя
        </Typography>
        <form onSubmit={handleSubmit}>
            <TextField
            label="Имя пользователя"
            variant="outlined"
            fullWidth
            required
            value={username}
            onChange={(e) => setUserName(e.target.value)}
            sx={{ mb: 2 }}
            />
            <TextField
            label="Краткое имя"
            variant="outlined"
            fullWidth
            required
            value={userShortName}
            onChange={(e) => setUserShortName(e.target.value)}
            sx={{ mb: 2 }}
            />
            <FormControl fullWidth required sx={{ mb: 2 }}>
            <InputLabel id="user-role-label">Роль</InputLabel>
            <Select
                labelId="user-role-label"
                value={userRole}
                label="Роль"
                onChange={(e) => setUserRole(e.target.value)}
            >
                <MenuItem value="1">Manager</MenuItem>
                <MenuItem value="2">Purchaser</MenuItem>
                <MenuItem value="3">Report Group</MenuItem>
            </Select>
            </FormControl>
            <TextField
            label="Пароль"
            variant="outlined"
            type="password"
            fullWidth
            required
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            sx={{ mb: 2 }}
            />
            <Button variant="contained" color="primary" type="submit" disabled={loading} fullWidth>
            {loading ? <CircularProgress size={24} /> : 'Создать пользователя'}
            </Button>
        </form>
        <Snackbar open={openSuccess} autoHideDuration={6000} onClose={handleClose}>
            <Alert onClose={handleClose} severity="success" sx={{ width: '100%' }}>
                {successMessage}
            </Alert>
        </Snackbar>
        <Snackbar open={openError} autoHideDuration={6000} onClose={handleClose}>
            <Alert onClose={handleClose} severity="error" sx={{ width: '100%' }}>
                {errorMessage}
            </Alert>
        </Snackbar>
        </Box>
    </>
  );
};

export default AdminCreateUser;
