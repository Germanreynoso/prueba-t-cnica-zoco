# 🚀 Prueba Técnica - Full Stack Developer (.NET + React)

Esta es una solución integral para la gestión de usuarios, estudios y direcciones, cumpliendo con todos los requisitos técnicos y de seguridad solicitados.

## 🛠️ Tecnologías Utilizadas

### Backend
- **Framework:** .NET 8 (C#)
- **ORM:** Entity Framework Core
- **Base de Datos:** SQL Server
- **Autenticación:** JWT (JSON Web Tokens)
- **Documentación:** Swagger UI
- **Arquitectura:** Patrón Repository y Services (Capas)

### Frontend
- **Framework:** React (Vite)
- **Estilos:** Tailwind CSS
- **Gestión de Estado:** Context API
- **Enrutamiento:** React Router DOM
- **HTTP Client:** Axios

---

## 🌐 Deploys (Demostrativos)
Aunque la consigna requiere ejecución local, se han realizado despliegues para demostrar habilidades en DevOps y Docker:
- **Frontend (Netlify):** [https://germanreynoso-zoco.netlify.app/](https://germanreynoso-zoco.netlify.app/)
- **Frontend (Netlify):** [https://germanreynoso-zoco.netlify.app/](https://germanreynoso-zoco.netlify.app/)
- **Backend:** Ejecución local (ver instrucciones abajo). *Nota: Se optó por mantener el backend local para garantizar la estabilidad de la conexión con SQL Server.*

---

## ⚙️ Configuración Local

### 1. Requisitos Previos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express o LocalDB)
- [Node.js](https://nodejs.org/) (v18 o superior)

### 2. Configuración del Backend
1. Navega a la carpeta del backend:
   ```bash
   cd BackendApi
   ```
2. Configura tu cadena de conexión en `appsettings.json`. Por defecto está configurada para buscar una instancia local:
   ```json
   "DefaultConnection": "Server=DESKTOP-QL9OU6S;Database=BackendDb;Trusted_Connection=True;TrustServerCertificate=True"
   ```
3. Ejecuta las migraciones para crear la base de datos y los datos iniciales (Seed):
   ```bash
   dotnet ef database update
   ```
4. Inicia la API:
   ```bash
   dotnet run
   ```
   *La API estará disponible en `http://localhost:5152` y Swagger en `http://localhost:5152/index.html`*

### 3. Configuración del Frontend
1. Navega a la carpeta del frontend:
   ```bash
   cd FrontendApp
   ```
2. Instala las dependencias:
   ```bash
   npm install
   ```
3. Crea un archivo `.env` en la raíz de `FrontendApp` (si no existe) con la URL de la API local:
   ```env
   VITE_API_URL=http://localhost:5152/api
   ```
4. Inicia la aplicación:
   ```bash
   npm run dev
   ```

---

## 🔑 Credenciales de Acceso
La base de datos se inicializa automáticamente con los siguientes usuarios para pruebas:

| Rol | Usuario | Contraseña |
| :--- | :--- | :--- |
| **Admin** | `admin` | `admin123` |
| **Usuario** | `usuario` | `user123` |

---

## 🛡️ Funcionalidades Destacadas
- **Seguridad:** Middleware de autorización que valida no solo el rol, sino también la propiedad de los recursos (un usuario no puede editar estudios de otro).
- **Sesión:** Registro automático en la tabla `SessionLogs` al iniciar y cerrar sesión, incluyendo captura de IP.
- **Validaciones:** Control estricto de nulos y tipos de datos en .NET 8.
- **Diseño:** Interfaz moderna, responsiva y con estados de carga.

---

## 📁 Estructura del Proyecto
```text
├── BackendApi/
│   ├── Controllers/    # Endpoints de la API
│   ├── Models/         # Entidades de base de datos
│   ├── Repositories/   # Acceso a datos (Patrón Repository)
│   ├── Services/       # Lógica de negocio
│   └── DTOs/           # Objetos de transferencia de datos
├── FrontendApp/
│   ├── src/
│   │   ├── context/    # AuthContext (JWT & SessionStorage)
│   │   ├── components/ # Componentes reutilizables
│   │   ├── pages/      # Vistas (Dashboard, Login, etc.)
│   │   └── services/   # Cliente API (Axios)
└── netlify.toml        # Configuración de deploy
```

---
*Desarrollado por German Reynoso para la prueba técnica de Zoco.*
