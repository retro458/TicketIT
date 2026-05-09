# TicketIT — Sistema de Gestión de Soporte Técnico

Sistema de tickets para gestión de incidencias técnicas en organizaciones pequeñas y medianas.
Arquitectura desacoplada: API REST + Frontend MVC.

---

## Tecnologías

| Capa | Tecnología |
|---|---|
| Backend | ASP.NET Core 9 Web API |
| Frontend | ASP.NET Core 9 MVC |
| Base de datos | PostgreSQL |
| ORM | Entity Framework Core 9 |
| Autenticación | JWT + Cookies HttpOnly |
| Chat en tiempo real | SignalR |
| IDE Backend | VS Code |
| IDE Frontend | VS Code / Visual Studio |

---

## Requisitos previos

- .NET 9 SDK
- PostgreSQL 16+
- Node.js (opcional, para herramientas frontend)
- Docker y Docker Compose (para despliegue)

---

## Estructura del proyectoTicketIT/
---

├── TicketIT.API/          → API REST (puerto 5065)
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── Models/
│   ├── DTOs/
│   ├── Hubs/              → SignalR
│   └── Data/              → AppDbContext
├── TicketIT.Web/          → Frontend MVC (puerto 5265)
│   ├── Controllers/
│   ├── Views/
│   ├── ViewModels/
│   └── Services/          → ApiService (HttpClient)
└── README.md
---

## Endpoints principales de la API

| Método | Endpoint | Rol requerido |
|---|---|---|
| POST | /api/auth/login | Público |
| POST | /api/auth/register | Público |
| GET | /api/tickets | Administrador |
| GET | /api/tickets/{id} | Autenticado |
| POST | /api/tickets | Cliente |
| PATCH | /api/tickets/{id} | Tecnico, Administrador |
| GET | /api/comentarios/ticket/{id} | Autenticado |
| POST | /api/comentarios | Autenticado |
| GET | /api/chat/{ticketId} | Autenticado |
| POST | /api/chat | Autenticado |

---
## Guía de configuración para el equipo (Windows + DBeaver)

### 1. Requisitos previos
- Instalar [PostgreSQL 16 para Windows](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)
  - Durante la instalación anota el password que le pongas a `postgres`
  - El puerto por defecto es 5432, déjalo así
- Instalar [DBeaver Community](https://dbeaver.io/download/)
- Instalar [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- Instalar [VS Code](https://code.visualstudio.com/) con la extensión **C# Dev Kit**

---

### 2. Restaurar la base de datos desde el backup

1. Abre DBeaver
2. Click en **Nueva Conexión** → selecciona **PostgreSQL** → Next
3. Configura:
   - Host: `localhost`
   - Port: `5432`
   - Database: `postgres`
   - Username: `postgres`
   - Password: la que pusiste al instalar
4. Click en **Test Connection** → si dice OK → **Finish**
5. En el panel izquierdo clic derecho en la conexión → **SQL Editor**
6. Ejecuta esto para crear la base:
```sql
   CREATE DATABASE ticketsit;
```
7. Clic derecho en la conexión → **Edit Connection** → cambia Database a `ticketsit` → **Test Connection** → **Finish**
8. Clic derecho sobre `ticketsit` en el panel izquierdo → **Tools** → **Restore**
9. En **Backup file** selecciona el archivo `ticketsit.backup`
10. Click en **Start** y espera que termine
11. Refresca la conexión — deberías ver las 10 tablas en `public`

---

### 3. Insertar datos iniciales

Si la restauración no incluyó datos, ejecuta esto en el SQL Editor de DBeaver:

```sql
INSERT INTO roles (nombre) VALUES ('Cliente'), ('Tecnico'), ('Administrador');
INSERT INTO estado (nombre) VALUES ('Abierto'), ('En Progreso'), ('En Espera'), ('Resuelto'), ('Cerrado');
INSERT INTO prioridades (nombre) VALUES ('Baja'), ('Media'), ('Alta'), ('Critica');
INSERT INTO comentarios_tipo (nombre) VALUES ('Publico'), ('Interno'), ('Sistema');
INSERT INTO categorias (nombre, prefijo) VALUES 
    ('Software', 'SW'), ('Hardware', 'HW'), ('Red', 'NET'), ('Accesos', 'ACC');
```

---

### 4. Configurar el proyecto

Clona o descarga el repositorio y abre la carpeta en VS Code.

Abre dos terminales en VS Code:

**Terminal 1 — Configurar secrets de la API:**
```bash
cd TicketIT.API
dotnet user-secrets init
dotnet user-secrets set "JwtSettings:SecretKey" "UnaClaveSeguraDeMinimo32Caracteres!!"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=ticketsit;Username=postgres;Password=TU_PASSWORD"
```

**Terminal 2 — Verificar que el Web apunta a la API:**

Abre `TicketIT.Web/appsettings.json` y confirma que diga:
```json
"ApiSettings": {
    "BaseUrl": "http://localhost:5065/"
}
```

---

### 5. Ejecutar el proyecto

**Terminal 1 — API:**
```bash
cd TicketIT.API
dotnet run
```
Espera hasta ver `Now listening on: http://localhost:5065`

**Terminal 2 — Web:**
```bash
cd TicketIT.Web
dotnet run
```
Espera hasta ver `Now listening on: http://localhost:5265`

Abre el navegador en `http://localhost:5265`

---

### 6. Crear el primer usuario administrador

En Swagger (`http://localhost:5065/swagger`) usa `POST /api/auth/register`:
```json
{
  "nombre": "Tu Nombre",
  "email": "tu@email.com",
  "password": "Password123!",
  "rolId": 3
}
```
`rolId: 3` = Administrador, `rolId: 2` = Tecnico, `rolId: 1` = Cliente

---

### 7. Posibles errores comunes

| Error | Causa | Solución |
|---|---|---|
| `Connection refused` en la API | PostgreSQL no está corriendo | Abre el panel de servicios de Windows y verifica que `postgresql-x64-16` esté iniciado |
| `401 Unauthorized` en el Web | Token expirado o no configurado | Cierra sesión y vuelve a entrar |
| `No such host` al conectar DBeaver | Host mal escrito | Usa `localhost` no `127.0.0.1` |
| Tablas vacías después del restore | El backup no incluyó datos | Ejecuta el script de datos iniciales del paso 3 |
| `dotnet: command not found` | .NET no instalado | Instala .NET 9 SDK y reinicia la terminal |