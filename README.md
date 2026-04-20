# Prueba Técnica - Sistema de Notas

## Descripción del proyecto

Este proyecto es un sistema backend para la gestión de notas personales con autenticación de usuarios. Está desarrollado utilizando **arquitectura limpia** (Clean Architecture) y se compone de dos microservicios independientes:

- **SecurityService**: Responsable de la autenticación, registro y login de usuarios (JWT).
- **NotesService**: Responsable de la creación, lectura, actualización y eliminación de notas (CRUD).

El sistema permite a los usuarios registrados crear y gestionar sus notas de forma segura.

### Tecnologías utilizadas
- **Backend**: C# .NET 10 (ASP.NET Core)
- **Arquitectura**: Clean Architecture (separación en Api, Domain e Infrastructure)
- **Base de datos**: postgreSQL (con Entity Framework Core + Code First)
- **Autenticación**: JWT Bearer
- **Contenerización**: Docker + Docker Compose
- **Otros**: Swagger (OpenAPI), Repository Pattern

Verificar
Docker Desktop
Visual studio 2026
Clonar proyecto
.Net SDK 10

---

## Arquitectura utilizada

El proyecto sigue los principios de **Clean Architecture** y está organizado en tres capas por cada servicio:

- **.Api** → Capa de presentación (Controllers, Middlewares, configuración de servicios, Swagger)
- **.Domain** → Capa de dominio (Entidades, Interfaces, Excepciones de dominio, Lógica de negocio)
- **.Infrastructure** → Capa de infraestructura (EF Core DbContext, Repositorios, Migrations, configuración de base de datos)

Cada servicio es independiente y se comunica a través de HTTP + JWT.

---

## Pasos para ejecutar cada servicio

### 1. Con Docker

# Clonar el repositorio
git clone https://github.com/jsaponte31/PruebaTecnicaNotas.git
cd PruebaTecnicaNotas

# Levantar todos los servicios (API + Base de datos)
docker-compose up --build
Los servicios quedarán disponibles en:

SecurityService: https://localhost:7237
NotesService: https://localhost:8080

Ejecución local

Abrir la solución PruebaTecnicaNotas.sln con Visual Studio 2026.
Configurar múltiples proyectos de inicio:
SecurityService.Api
NotesService.Api

Actualizar la cadena de conexión en los archivos appsettings.json de ambos proyectos.
Ejecutar las migraciones de base de datos (ver sección de BD).

Iniciar ambos proyectos.

Configuración de base de datos
El proyecto utiliza SQL Server con Entity Framework Core.
Pasos para configurar la BD:

Aplicar las migraciones:

Bash# Para SecurityService
cd SecurityService.Infrastructure
dotnet ef database update --startup-project ../SecurityService.Api

# Para NotesService
cd NotesService.Infrastructure
dotnet ef database update --startup-project ../NotesService.Api
O desde Visual Studio usando la Consola de Administrador de Paquetes.
La base de datos se crea automáticamente al levantar el docker-compose.
docker compose up -d

Cómo probar los endpoints
El proyecto incluye Swagger en ambos servicios para facilitar las pruebas.

Ejecuta los servicios.
Abre en el navegador:
SecurityService: https://localhost:7237/swagger/index.html
NotesService: https://localhost:8080/swagger/index.html


Flujo recomendado de prueba:

Registrar un usuario (POST /api/auth/register)
Iniciar sesión (POST /api/auth/login) → obtendrás un token JWT
Usar el token en el botón Authorize de Swagger para autenticarte.
Probar los endpoints de notas (CRUD) en NotesService.

Supuestos realizados

Se utiliza PosgreSQL como base de datos
La autenticación se basa en JWT Bearer Token.
Las contraseñas se almacenan hasheadas
Se siguió Clean Architecture para mantener la separación de responsabilidades y facilitar el mantenimiento y escalabilidad.
Se utilizó Docker Compose para simplificar el entorno de desarrollo (API + Base de datos).


Autor: Jhojan Aponte
Fecha: Abril 2026
Proyecto: Prueba Técnica - Sistema de Gestión de Notas
