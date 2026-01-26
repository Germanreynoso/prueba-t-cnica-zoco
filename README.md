# 🚀 Prueba Técnica - Full Stack Developer (.NET + React)

🔗 **Frontend en producción:**  
https://prueba-tecnica-zoco.netlify.app/


Aplicación web Full Stack para la gestión de usuarios, estudios y direcciones, con autenticación basada en JWT, control de roles y registro de sesiones.
La solución cumple con todos los requisitos funcionales, técnicos y de seguridad solicitados en la consigna.
----

## 📋 Características Principales

### 🧱 Backend (.NET 8)
- **Seguridad:** Autenticación JWT y Middleware de Autorización basado en Roles y Propiedad de Recursos.
- **Base de Datos:** SQL Server con Entity Framework Core (Code First).
- **Auditoría:** Sistema de `SessionLogs` que registra inicios y cierres de sesión, duración.
- **Documentación:** API completamente documentada con Swagger UI.

### 🎨 Frontend (React + Vite)
- **UI Moderna:** Diseño responsivo y estético utilizando **Tailwind CSS**.
- **Gestión de Estado:** Uso de Context API para manejo global de autenticación y sesión.
- **UX:** Feedback visual con spinners de carga, modales animados y notificaciones.
- **Rutas Protegidas:** Sistema de navegación seguro que restringe el acceso según el estado de autenticación.

---

## 🛠️ Stack Tecnológico

| Área | Tecnologías |
| :--- | :--- |
| **Backend** | .NET 8, Entity Framework Core, SQL Server, JWT, Swagger |
| **Frontend** | React 18, Vite, Tailwind CSS, Axios, React Router DOM |
| **Herramientas** | Git, Visual Studio / VS Code, Postman |

---

## ⚙️ Guía de Instalación y Ejecución

### 1. Requisitos Previos
Asegurarse de tener instalado:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Developer, Express o LocalDB)
- [Node.js](https://nodejs.org/) (v18 o superior)

### 2. Configuración del Backend

1. **Clonar el repositorio:**
   ```bash
   git clone <url-del-repositorio>
   cd prueba-t-cnica-zoco
   ```

2. **Configurar Base de Datos:**
   Abre el archivo `BackendApi/appsettings.json` y actualiza la cadena de conexión `DefaultConnection` según tu entorno local.
   
   *Ejemplo para SQL Express:*
   ```json
   "DefaultConnection": "Server=.\\SQLEXPRESS;Database=BackendDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
   ```
   *Ejemplo para LocalDB:*
   ```json
   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BackendDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
   ```

3. **Aplicar Migraciones (Opción A - Recomendada):**
   Desde la terminal en la carpeta `BackendApi`:
   ```bash
   cd BackendApi
   dotnet ef database update
   ```

   **Usar Script SQL (Opción B):**
   Si prefieres, puedes ejecutar el archivo `BackendApi/script.sql` directamente en tu servidor SQL para crear la base de datos y tablas.

4. **Iniciar el Servidor:**
   ```bash
   dotnet run
   ```
   🚀 La API estará disponible en: `http://localhost:5152`
   📄 Documentación Swagger: `http://localhost:5152/index.html`

### 3. Configuración del Frontend

1. **Navegar al directorio:**
   ```bash
   cd ../FrontendApp
   ```

2. **Instalar dependencias:**
   ```bash
   npm install
   ```

3. **Configurar Variables de Entorno:**
   El proyecto ya incluye una configuración por defecto para conectar con el backend local. Si tu puerto es diferente al 5152, crea un archivo `.env` en la raíz de `FrontendApp`:
   ```env
   VITE_API_URL=http://localhost:5152/api
   ```

4. **Iniciar la Aplicación:**
   ```bash
   npm run dev
   ```
   🌐 Abre tu navegador en la URL que muestra la terminal (usualmente `http://localhost:5173`).

---

## 🔑 Usuarios de Prueba (Seed Data)

La base de datos se inicializa automáticamente con los siguientes usuarios para facilitar las pruebas:

| Rol | Usuario | Contraseña | Permisos |
| :--- | :--- | :--- | :--- |
| **Admin** | `admin` | `admin123` | Gestión total (Usuarios, Estudios, Direcciones, Logs) |
| **Usuario** | `usuario` | `user123` | Gestión de su propio Perfil, Estudios y Dirección |

---

## 📂 Estructura del Proyecto

```text
prueba-t-cnica-zoco/
├── BackendApi/                 # API REST en .NET 8
│   ├── Controllers/            # Controladores (Endpoints)
│   ├── Models/                 # Entidades de Dominio (User, Study, etc.)
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Repositories/           # Capa de Acceso a Datos
│   ├── Services/               # Lógica de Negocio
│   ├── Migrations/             # Historial de cambios de BD
│   └── appsettings.json        # Configuración (Connection Strings, JWT)
│
├── FrontendApp/                # SPA en React + Vite
│   ├── src/
│   │   ├── components/         # Componentes UI reutilizables (Modal, Forms)
│   │   ├── context/            # Estado Global (AuthContext)
│   │   ├── pages/              # Vistas Principales (Dashboard, Login)
│   │   ├── services/           # Comunicación con API (Axios)
│   │   └── hooks/              # Custom Hooks
│   └── tailwind.config.js      # Configuración de Estilos
│
└── README.md                   # Documentación del proyecto
```

---

## 🧪 Endpoints Principales

Puedes probar todos estos endpoints directamente desde Swagger UI.

- **Auth:** `/api/auth/login`, `/api/auth/logout`
- **Users:** `/api/users` (CRUD completo para Admin)
- **Studies:** `/api/studies` (CRUD con validación de propiedad)
- **Addresses:** `/api/addresses` (Gestión de direcciones)
- **SessionLogs:** `/api/sessionlogs` (Solo lectura para Admin)

---

## ✨ Notas Adicionales
- **Validación de Roles:** El backend valida estrictamente que un usuario normal no pueda modificar datos de otros usuarios, incluso si intenta manipular las peticiones.
- **Sesiones:** El sistema registra automáticamente cuándo un usuario inicia sesión y cuándo la cierra (o cuando expira su token), permitiendo auditoría de accesos.

---
*Desarrollado por Germán Gonzalo Reynoso*
