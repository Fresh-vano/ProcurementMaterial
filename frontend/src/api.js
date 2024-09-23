import axios from 'axios';
import cookie from 'react-cookies';

// Создаем экземпляр Axios
const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL || 'http://localhost:8080', 
});

// Добавляем интерцептор запросов
api.interceptors.request.use(
  (request) => {
    // Добавляем XSRF токен из cookie
    const xsrfToken = cookie.load('.AspNetCore.Xsrf');
    if (xsrfToken) {
      request.headers['x-xsrf-token'] = xsrfToken;
    }

    // Добавляем JWT токен из localStorage
    const token = localStorage.getItem('token');
    if (token) {
      request.headers['Authorization'] = `Bearer ${token}`;
    }

    return request;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response && error.response.status === 401) {
        localStorage.removeItem('token');
        window.location.href = '/login';
      }
      return Promise.reject(error);
    }
  );

export default api;
