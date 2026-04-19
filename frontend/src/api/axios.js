import axios from 'axios';

const securityBaseUrl = import.meta.env.VITE_API_URL_SECURITY;
const notesBaseUrl = import.meta.env.VITE_API_URL_NOTES;

export const authApi = axios.create({
    baseURL: securityBaseUrl,
});

export const notesApi = axios.create({
    baseURL: notesBaseUrl,
});

// Interceptor para el token
notesApi.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});