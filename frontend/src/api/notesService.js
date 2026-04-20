import { notesApi } from './axios';

/**
 * @typedef {import('../models/notes.model').Note} Note
 * @typedef {import('../models/notes.model').NoteInput} NoteInput
 */

/**
 * Servicio de Notas
 * Maneja operaciones CRUD para notas
 */
class NotesService {
  /**
   * Obtiene todas las notas del usuario
   * @returns {Promise<Note[]>} Lista de notas
   */
  async getAllNotes() {
    try {
      const response = await notesApi.get('/Notes');
      return response.data;
    } catch (error) {
      throw this._handleError(error);
    }
  }

  /**
   * Obtiene una nota por ID
   * @param {string|number} id - ID de la nota
   * @returns {Promise<Note>} Datos de la nota
   */
  async getNoteById(id) {
    try {
      const response = await notesApi.get(`/Notes/${id}`);
      return response.data;
    } catch (error) {
      throw this._handleError(error);
    }
  }

  /**
   * Crea una nueva nota
   * @param {NoteInput} noteData - Datos de la nota (title y content)
   * @returns {Promise<Note>} Nota creada con id y timestamps
   */
  async createNote(noteData) {
    try {
      const response = await notesApi.post('/Notes', noteData);
      return response.data;
    } catch (error) {
      throw this._handleError(error);
    }
  }

  /**
   * Elimina una nota
   * @param {string|number} id - ID de la nota
   * @returns {Promise<void>} Respuesta del servidor
   */
  async deleteNote(id) {
    try {
      const response = await notesApi.delete(`/Notes/${id}`);
      return response.data;
    } catch (error) {
      throw this._handleError(error);
    }
  }

  /**
   * Manejo de errores centralizado
   * @param {Error} error - Error a procesar
   * @returns {{status: number|null, message: string, data: any}} Objeto de error normalizado
   * @private
   */
  _handleError(error) {
    if (error.response) {
      return {
        status: error.response.status,
        message: error.response.data?.message || 'Error al procesar notas',
        data: error.response.data
      };
    } else if (error.request) {
      return {
        status: null,
        message: 'No se recibió respuesta del servidor',
        data: error.request
      };
    } else {
      return {
        status: null,
        message: error.message || 'Error desconocido',
        data: error
      };
    }
  }
}

// Exportar instancia singleton
export default new NotesService();
