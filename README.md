# Full Stack User Management System (.NET + React)

Este proyecto es una aplicación web Full Stack para gestionar usuarios, sus estudios y direcciones personales.

## Tecnologías Utilizadas

### Backend
- .NET 6 Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger
- BCrypt.Net para hashing de contraseñas

### Frontend
- React (Vite)
- Tailwind CSS
- React Router DOM
- Context API
- Axios

## Requisitos Previos
- .NET 6 SDK o superior
- Node.js (v16+)
- SQL Server (LocalDB o instancia completa)

## Instrucciones para el Backend

1. Navega a la carpeta `BackendApi`:
   ```bash
   cd BackendApi
   ```
2. Restaura las dependencias:
   ```bash
   dotnet restore
   ```
3. Configura la cadena de conexión en `appsettings.json` si es necesario. Por defecto usa `(localdb)\\mssqllocaldb`.
4. Ejecuta las migraciones para crear la base de datos:
   ```bash
   dotnet ef database update
   ```
   *Nota: Si no tienes `dotnet-ef` instalado, puedes instalarlo con `dotnet tool install --global dotnet-ef`.*
5. Ejecuta la aplicación:
   ```bash
   dotnet run
   ```
   La API estará disponible en `https://localhost:7287` (o el puerto configurado en `launchSettings.json`).

## Instrucciones para el Frontend

1. Navega a la carpeta `FrontendApp`:
   ```bash
   cd FrontendApp
   ```
2. Instala las dependencias:
   ```bash
   npm install
   ```
3. Ejecuta la aplicación en modo desarrollo:
   ```bash
   npm run dev
   ```
4. Abre tu navegador en `http://localhost:5173`.

## Credenciales de Prueba (Sugeridas tras migraciones)
- **Admin**: `admin` / `admin123`
- **Usuario**: `usuario` / `user123`

## Funcionalidades
- **Autenticación**: Login y Logout con persistencia en `sessionStorage`.
- **Roles**:
  - **Admin**: Puede ver, crear, editar y eliminar cualquier usuario.
  - **Usuario**: Solo puede ver y editar su propio perfil, estudios y dirección.
- **Seguridad**: Middleware de propiedad (Ownership) que valida que un usuario solo acceda a sus propios recursos.
- **Registro de Sesión**: Se guardan logs de inicio y cierre de sesión en la base de datos.
- **Diseño Responsivo**: Interfaz moderna construida con Tailwind CSS, adaptable a móviles.
