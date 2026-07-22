# Sistema de Gestión de Venta Frutihortícola (SGVF)

## Descripción

SGVF es una API REST desarrollada con ASP.NET Core Web API para gestionar un negocio mayorista de frutas y verduras.

El sistema permite administrar clientes, proveedores, productos, ventas, pagos y estadísticas comerciales.

Actualmente el proyecto se encuentra en desarrollo.

---

## Tecnologías utilizadas

- ASP.NET Core 8
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger / OpenAPI

---

## Arquitectura

El proyecto utiliza una arquitectura por capas.

```
Controllers
│
Services
│
DTOs
│
Entities
│
Data
```

- Controllers: reciben las peticiones HTTP.
- Services: contienen la lógica de negocio.
- DTOs: representan los datos enviados y recibidos.
- Entities: representan las tablas de la base de datos.
- Data: configuración del DbContext.

---

## Funcionalidades implementadas

### Autenticación

- Login mediante JWT.
- Protección de endpoints con `[Authorize]`.

### Productos

- CRUD completo.
- Validaciones.
- Documentación Swagger.

### Clientes

- CRUD completo.
- Validaciones.
- JWT.

### Proveedores

- CRUD completo.
- Validaciones.
- JWT.

---

## Requisitos

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 o Visual Studio Code

---

## Cómo ejecutar el proyecto

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
```

### 2. Restaurar paquetes

```bash
dotnet restore
```

### 3. Configurar la cadena de conexión

Editar el archivo:

```
appsettings.json
```

Modificar la sección:

```json
"ConnectionStrings": {
  "DefaultConnection": "TU_CADENA_DE_CONEXION"
}
```

---

### 4. Ejecutar migraciones

```bash
dotnet ef database update
```

---

### 5. Ejecutar la API

```bash
dotnet run
```

---

## Swagger

Una vez iniciada la aplicación acceder a:

```
https://localhost:xxxx/swagger
```

Autenticarse mediante el botón **Authorize** utilizando:

```
Bearer <token>
```

---

## Estado del proyecto

Actualmente se encuentran implementados:

- Autenticación JWT
- CRUD Productos
- CRUD Clientes
- CRUD Proveedores

Próximamente:

- Ventas
- Pagos
- Compras
- Estadísticas
- Gestión de stock
