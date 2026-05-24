# AI Hub — Backend

API REST del proyecto AI Hub desarrollada con ASP.NET Core 9.0. Provee endpoints para autenticación, gestión de categorías y herramientas de inteligencia artificial.

## Tecnologías

- ASP.NET Core 9.0 Web API
- Entity Framework Core — ORM
- ASP.NET Core Identity — autenticación y gestión de usuarios
- JWT Bearer Authentication
- SQL Server
- Swagger / Swashbuckle — documentación de endpoints

## Estructura del proyecto
ai-hub-backend/
├── Controllers/
│   ├── AuthController.cs         # Registro, login y generación de JWT
│   ├── CategoriesController.cs   # CRUD de categorías
│   └── ToolsController.cs        # CRUD de herramientas con filtrado y paginación
├── Models/
│   ├── User.cs                   # Entidad de usuario con constantes de roles
│   ├── Category.cs               # Entidad de categoría
│   └── Tool.cs                   # Entidad de herramienta con FK a Category
├── Data/
│   └── ApplicationDbContext.cs   # Contexto de EF Core con Identity
├── DTOs/
│   ├── LoginDto.cs               # Datos de entrada para login
│   ├── RegisterDto.cs            # Datos de entrada para registro
│   ├── ToolDto.cs                # Datos de entrada para herramientas
│   └── CategoryDto.cs            # Datos de entrada para categorías
├── Services/                     # Servicios de negocio (a futuro)
├── Middleware/                   # Middlewares personalizados (a futuro)
├── Migrations/                   # Migraciones de EF Core
├── Program.cs                    # Configuración de servicios y middlewares
├── appsettings.json              # Cadena de conexión y configuración JWT
└── global.json                   # Versión del SDK de .NET fijada a 9.0.314

## Requisitos

- .NET SDK 9.0.314 o superior
- SQL Server (local o Express)
- SQL Server Management Studio (opcional)

## Instalación

```bash
# Clonar el repositorio
git clone https://github.com/tu-usuario/ai-hub-backend
cd ai-hub-backend

# Restaurar dependencias
dotnet restore

# Aplicar migraciones y crear la base de datos
dotnet ef database update

# Iniciar el servidor
dotnet run
```

La API estará disponible en `http://localhost:5221`.
La documentación Swagger estará en `http://localhost:5221/swagger`.

## Configuración

Edita `appsettings.json` con tus valores:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AIHubDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "tu-clave-secreta-minimo-32-caracteres",
    "Issuer": "AIHubBackend",
    "Audience": "AIHubFrontend"
  }
}
```

## Endpoints

### Autenticación

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| POST | `/api/auth/register` | Registrar nuevo usuario | Público |
| POST | `/api/auth/login` | Iniciar sesión, retorna JWT | Público |

### Categorías

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| GET | `/api/categories` | Listar todas las categorías | Público |
| GET | `/api/categories/{id}` | Obtener categoría por ID | Público |
| GET | `/api/categories/{id}/tools` | Herramientas de una categoría | Público |
| POST | `/api/categories` | Crear categoría | Admin |
| PUT | `/api/categories/{id}` | Actualizar categoría | Admin |
| DELETE | `/api/categories/{id}` | Eliminar categoría | Admin |

### Herramientas

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| GET | `/api/tools` | Listar herramientas (con filtros) | Público |
| GET | `/api/tools/{id}` | Obtener herramienta por ID | Público |
| POST | `/api/tools` | Crear herramienta | Admin |
| PUT | `/api/tools/{id}` | Actualizar herramienta | Admin |
| DELETE | `/api/tools/{id}` | Eliminar herramienta | Admin |

### Parámetros de filtrado en GET /api/tools
| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| category | int | ID de la categoría |
| page | int | Número de página (default: 1) |
| limit | int | Resultados por página (default: 10) |

## Autenticación JWT

Al hacer login exitoso el endpoint retorna:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "role": "admin"
}
```

Para consumir endpoints protegidos incluye el header:
Authorization: Bearer {token}
## Base de datos

### Tablas propias

| Tabla | Descripción |
|-------|-------------|
| Categories | Categorías de herramientas IA |
| Tools | Herramientas IA con FK a Categories |

### Tablas de Identity

| Tabla | Descripción |
|-------|-------------|
| AspNetUsers | Usuarios del sistema |
| AspNetRoles | Roles disponibles (admin, user) |
| AspNetUserRoles | Relación usuario-rol |

## Crear usuario admin

Después de registrar un usuario, asígnale el rol admin desde SQL Server:

```sql
-- Obtener IDs
SELECT Id, Email FROM AspNetUsers;
SELECT Id, Name FROM AspNetRoles;

-- Asignar rol admin
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('id-del-usuario', 'id-del-rol-admin');
```

## Paquetes instalados
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Swashbuckle.AspNetCore
