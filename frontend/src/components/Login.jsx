import { useState } from 'react';
import authService from '../api/authService';
import './Login.css';

/**
 * Componente de Login
 * Maneja la autenticación de usuarios
 * @returns {JSX.Element} Formulario de login
 */
export default function Login({ onLoginSuccess }) {
  const [formData, setFormData] = useState({
    username: '',
    password: ''
  });

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);

  /**
   * Maneja los cambios en los campos del formulario
   * @param {Event} e - Evento del input
   */
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    // Limpiar error cuando el usuario empieza a escribir
    if (error) {
      setError(null);
    }
  };

  /**
   * Valida los datos del formulario
   * @returns {boolean} True si los datos son válidos
   */
  const validateForm = () => {
    if (!formData.username.trim()) {
      setError('Por favor ingresa tu nombre de usuario');
      return false;
    }
    if (!formData.password.trim()) {
      setError('Por favor ingresa tu contraseña');
      return false;
    }
    if (formData.password.length < 3) {
      setError('La contraseña debe tener al menos 3 caracteres');
      return false;
    }
    return true;
  };

  /**
   * Maneja el envío del formulario
   * @param {Event} e - Evento del formulario
   */
  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);

    // Validar formulario
    if (!validateForm()) {
      return;
    }

    setIsLoading(true);

    try {
      // Llamar al servicio de autenticación
      const response = await authService.login(formData);

      if (response.token) {
        setSuccess(true);
        setFormData({ username: '', password: '' });

        // Ejecutar callback si existe (para redirigir, etc.)
        if (onLoginSuccess) {
          onLoginSuccess(response);
        }
      } else {
        setError('No se recibió token de autenticación');
      }
    } catch (err) {
      // Manejo de errores del servicio
      const errorMessage = err.message || 'Error al iniciar sesión';
      setError(errorMessage);
      console.error('Error de login:', err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h2>Iniciar Sesión</h2>

        {error && (
          <div className="alert alert-error">
            <span>{error}</span>
            <button
              className="alert-close"
              onClick={() => setError(null)}
              aria-label="Cerrar alerta"
            >
              ✕
            </button>
          </div>
        )}

        {success && (
          <div className="alert alert-success">
            <span>¡Autenticación exitosa!</span>
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="username">Nombre de Usuario</label>
            <input
              type="text"
              id="username"
              name="username"
              value={formData.username}
              onChange={handleChange}
              placeholder="Ingresa tu nombre de usuario"
              disabled={isLoading}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Contraseña</label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              placeholder="Ingresa tu contraseña"
              disabled={isLoading}
              required
            />
          </div>

          <button
            type="submit"
            className="btn btn-primary"
            disabled={isLoading}
          >
            {isLoading ? 'Iniciando sesión...' : 'Iniciar Sesión'}
          </button>
        </form>
      </div>
    </div>
  );
}
