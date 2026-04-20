import { authApi } from './axios';

/**
 * @typedef {import('../models/auth.model').AuthRequest} AuthRequest
 * @typedef {import('../models/auth.model').AuthResponse} AuthResponse
 */

/**
 * Servicio de Autenticación
 * Maneja el registro, login y validación de tokens
 */
class AuthService {
  /**
   * Registra un nuevo usuario
   * @param {AuthRequest} credentials - Credenciales del usuario
   * @returns {Promise<AuthResponse>} Respuesta del servidor con token y username
   */
  async register(credentials) {
    try {
      const response = await authApi.post('/Auth/register', credentials);
      return response.data;
    } catch (error) {
      throw this._handleError(error);
    }
  }

  /**
   * Inicia sesión con credenciales
   * @param {AuthRequest} credentials - Credenciales del usuario
   * @returns {Promise<AuthResponse>} Respuesta del servidor con token y username
   */
  async login(credentials) {
    try {
      const response = await authApi.post('/Auth/login', credentials);
      const { token } = response.data;
      
      if (token) {
        localStorage.setItem('token', token);
      }
      
      return response.data;
    } catch (error) {
      throw this._handleError(error);
    }
  }

  /**
   * Valida un token
   * @param {string} token - Token a validar
   * @returns {Promise<AuthResponse>} Resultado de la validación con información del usuario
   */
  async validateToken(token) {
    try {
      const response = await authApi.post('/Auth/validate', null, {
        params: { token }
      });
      return response.data;
    } catch (error) {
      throw this._handleError(error);
    }
  }

  /**
   * Cierra la sesión del usuario
   * @returns {void}
   */
  logout() {
    localStorage.removeItem('token');
  }

  /**
   * Obtiene el token almacenado
   * @returns {string|null} Token almacenado o null
   */
  getToken() {
    return localStorage.getItem('token');
  }

  /**
   * Verifica si el usuario está autenticado
   * @returns {boolean} True si hay token válido
   */
  isAuthenticated() {
    return !!this.getToken();
  }

  /**
   * Manejo de errores centralizado
   * @param {Error} error - Error a procesar
   * @returns {{status: number|null, message: string, data: any}} Objeto de error normalizado
   * @private
   */
  _handleError(error) {
    if (error.response) {
      // Error de respuesta del servidor
      return {
        status: error.response.status,
        message: error.response.data?.message || 'Error en la autenticación',
        data: error.response.data
      };
    } else if (error.request) {
      // Error de solicitud (sin respuesta)
      return {
        status: null,
        message: 'No se recibió respuesta del servidor',
        data: error.request
      };
    } else {
      // Error general
      return {
        status: null,
        message: error.message || 'Error desconocido',
        data: error
      };
    }
  }
}

// Exportar instancia singleton
export default new AuthService();
