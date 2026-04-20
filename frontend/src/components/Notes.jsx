import { useState, useEffect } from 'react';
import notesService from '../api/notesService';
import './Notes.css';

/**
 * Componente de Notas
 * Maneja la visualización, creación y eliminación de notas
 * @returns {JSX.Element} Interfaz de notas
 */
export default function Notes() {
  const [notes, setNotes] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [formData, setFormData] = useState({
    title: '',
    content: ''
  });
  const [isCreating, setIsCreating] = useState(false);
  const [deletingId, setDeletingId] = useState(null);
  const [selectedNote, setSelectedNote] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isLoadingModal, setIsLoadingModal] = useState(false);

  /**
   * Carga todas las notas del usuario
   */
  const loadNotes = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await notesService.getAllNotes();
      setNotes(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err.message || 'Error al cargar notas');
      console.error('Error loading notes:', err);
    } finally {
      setIsLoading(false);
    }
  };

  // Cargar notas al montar el componente
  useEffect(() => {
    const fetchNotes = async () => {
      setIsLoading(true);
      setError(null);
      try {
        const data = await notesService.getAllNotes();
        setNotes(Array.isArray(data) ? data : []);
      } catch (err) {
        setError(err.message || 'Error al cargar notas');
        console.error('Error loading notes:', err);
      } finally {
        setIsLoading(false);
      }
    };
    
    fetchNotes();
  }, []);

  /**
   * Abre el modal con los detalles de la nota
   * Realiza una petición getById para obtener los datos completos
   */
  const handleViewNote = async (note) => {
    setIsLoadingModal(true);
    setError(null);
    try {
      const fullNote = await notesService.getNoteById(note.id);
      setSelectedNote(fullNote);
      setIsModalOpen(true);
    } catch (err) {
      setError(err.message || 'Error al cargar los detalles de la nota');
      console.error('Error loading note details:', err);
    } finally {
      setIsLoadingModal(false);
    }
  };

  /**
   * Cierra el modal
   */
  const handleCloseModal = () => {
    setIsModalOpen(false);
    setSelectedNote(null);
  };

  /**
   * Maneja cambios en los campos del formulario
   * @param {Event} e - Evento del input
   */
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    if (error) {
      setError(null);
    }
  };

  /**
   * Valida los datos del formulario
   * @returns {boolean} True si es válido
   */
  const validateForm = () => {
    if (!formData.title.trim()) {
      setError('El título es requerido');
      return false;
    }
    if (!formData.content.trim()) {
      setError('El contenido es requerido');
      return false;
    }
    if (formData.title.trim().length < 3) {
      setError('El título debe tener al menos 3 caracteres');
      return false;
    }
    return true;
  };

  /**
   * Maneja la creación de una nueva nota
   * @param {Event} e - Evento del formulario
   */
  const handleCreateNote = async (e) => {
    e.preventDefault();
    setError(null);

    if (!validateForm()) {
      return;
    }

    setIsCreating(true);

    try {
      const newNote = await notesService.createNote({
        title: formData.title,
        content: formData.content
      });

      setNotes(prev => [newNote, ...prev]);
      loadNotes();
      setFormData({ title: '', content: '' });
    } catch (err) {
      setError(err.message || 'Error al crear nota');
      console.error('Error creating note:', err);
    } finally {
      setIsCreating(false);
    }
  };

  /**
   * Maneja la eliminación de una nota
   * @param {number} id - ID de la nota a eliminar
   */
  const handleDeleteNote = async (id) => {
    if (!window.confirm('¿Estás seguro que deseas eliminar esta nota?')) {
      return;
    }

    setDeletingId(id);
    setError(null);

    try {
      await notesService.deleteNote(id);
      setNotes(prev => prev.filter(note => note.id !== id));
    } catch (err) {
      setError(err.message || 'Error al eliminar nota');
      console.error('Error deleting note:', err);
    } finally {
      setDeletingId(null);
    }
  };

  return (
    <div className="notes-container">
      <div className="notes-header">
        <h2>Listado de Notas</h2>
        <button 
          className="btn-refresh"
          onClick={loadNotes}
          disabled={isLoading}
          title="Recargar notas"
        >
          ↻
        </button>
      </div>

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

      <div className="notes-content">
        <div className="notes-form-section">
          <h3>Crear Nueva Nota</h3>
          <form onSubmit={handleCreateNote} className="notes-form">
            <div className="form-group">
              <input
                type="text"
                name="title"
                value={formData.title}
                onChange={handleInputChange}
                placeholder="Título de la nota"
                disabled={isCreating}
                maxLength="20"
                required
              />
            </div>

            <div className="form-group">
              <textarea
                name="content"
                value={formData.content}
                onChange={handleInputChange}
                placeholder="Contenido de la nota"
                disabled={isCreating}
                rows="3"
                maxLength="200"
                required
              ></textarea>
            </div>

            <button
              type="submit"
              className="btn btn-primary"
              disabled={isCreating}
            >
              {isCreating ? 'Creando...' : 'Crear Nota'}
            </button>
          </form>
        </div>

        <div className="notes-list-section">
          <h3>
            {isLoading ? 'Cargando...' : `Mis Notas (${notes.length})`}
          </h3>

          {isLoading ? (
            <div className="loading">
              <p>Cargando notas...</p>
            </div>
          ) : notes.length === 0 ? (
            <div className="empty-state">
              <p>No tienes notas aún. ¡Crea tu primera nota!</p>
            </div>
          ) : (
            <div className="notes-table-wrapper">
              <table className="notes-table">
                <thead>
                  <tr>
                    <th>Título</th>
                    <th>Contenido</th>
                    <th>Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {notes.map(note => (
                    <tr key={note.id} className="note-row">
                      <td className="note-title">{note.title}</td>
                      <td className="note-content-cell">{note.content}</td>
                      <td className="note-actions-cell">
                        <button
                          className="btn-view-icon"
                          onClick={() => handleViewNote(note)}
                          disabled={isLoadingModal}
                          title="Ver nota completa"
                        >
                          👁
                        </button>
                        <button
                          className="btn-delete-icon"
                          onClick={() => handleDeleteNote(note.id)}
                          disabled={deletingId === note.id}
                          title="Eliminar nota"
                          aria-label="Eliminar nota"
                        >
                          🗑
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>

      {/* Modal para ver detalles de la nota */}
      {isModalOpen && selectedNote && (
        <div className="modal-overlay" onClick={handleCloseModal}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>Detalle de la Nota</h3>
              <button
                className="modal-close"
                onClick={handleCloseModal}
                aria-label="Cerrar modal"
              >
                ✕
              </button>
            </div>
            <div className="modal-body">
              {isLoadingModal ? (
                <div className="modal-loading">
                  <p>Cargando detalles de la nota...</p>
                </div>
              ) : (
                <div className="modal-detail">
                  <div className="detail-row">
                    <span className="detail-label">Título: </span>
                    <span className="detail-value">{selectedNote.title}</span>
                  </div>
                  <div className="detail-row">
                    <span className="detail-label">Contenido: </span>
                    <span className="detail-value detail-content">{selectedNote.content}</span>
                  </div>
                  <div className="detail-row">
                    <span className="detail-label">Autor: </span>
                    <span className="detail-value">{selectedNote.createdBy}</span>
                  </div>
                  <div className="detail-row">
                    <span className="detail-label">Fecha: </span>
                    <span className="detail-value">{new Date(selectedNote.createdAt).toLocaleString('es-ES')}</span>
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
