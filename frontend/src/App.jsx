import { useState } from 'react'
import Login from './components/Login'
import Notes from './components/Notes'
import authService from './api/authService'
import './App.css'

function App() {
  // Inicializar estado basado en token guardado
  const [isAuthenticated, setIsAuthenticated] = useState(() => authService.isAuthenticated())
  const [user, setUser] = useState(() => {
    // Extraer username del token almacenado
    if (authService.isAuthenticated()) {
      const token = authService.getToken()
      if (token) {
        try {
          const payload = JSON.parse(atob(token.split('.')[1]))
          return payload.username || null
        } catch (err) {
          console.error('Error parsing token:', err)
          return null
        }
      }
    }
    return null
  })

  /**
   * Maneja el login exitoso
   * @param {AuthResponse} response - Datos del usuario autenticado
   */
  const handleLoginSuccess = (response) => {
    setIsAuthenticated(true)
    setUser(response.username)
    console.log('✓ Autenticado como:', response.username)
  }

  /**
   * Maneja el logout
   */
  const handleLogout = () => {
    authService.logout()
    setIsAuthenticated(false)
    setUser(null)
    console.log('✓ Sesión cerrada')
  }

  // Mostrar Login si no está autenticado
  if (!isAuthenticated) {
    return <Login onLoginSuccess={handleLoginSuccess} />
  }

  // Mostrar contenido principal si está autenticado
  return (
    <>
      <header className="app-header">
        <div className="header-content">
          <span>Bienvenido, <strong>{user}</strong></span>
          <div className="user-info">
            <button className="btn-logout" onClick={handleLogout}>
              Cerrar Sesión
            </button>
          </div>
        </div>
      </header>

      <main className="app-main">
        <Notes />
      </main>
    </>
  )
}

export default App
