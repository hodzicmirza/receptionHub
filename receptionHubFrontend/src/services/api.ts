import axios from 'axios';

export const API_URL = import.meta.env.VITE_BACKEND_URL || 'http://localhost:5143/api';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor za token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Interceptor za greške
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    // Ako je 401 Unauthorized i nije pokušaj refresh-a
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      // Možeš implementirati refresh token ovdje ako backend podržava
      // Za sada samo logout
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }

    return Promise.reject(error);
  }
);

export default api;
