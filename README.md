# SGVF - Backend API

Backend de **SGVF (Sistema de Gestión de Venta Frutihortícola)**.

API REST desarrollada para centralizar la lógica de negocio y la gestión de datos de un comercio mayorista frutihortícola.

El backend administra ventas, productos, stock, clientes, proveedores, cuentas corrientes, pagos, autenticación y generación de tickets.

## Tecnologías utilizadas

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger / OpenAPI
- QuestPDF

## Arquitectura

El proyecto utiliza una arquitectura basada en separación de responsabilidades.

```text
Controllers
    ↓
Services
    ↓
Entity Framework Core
    ↓
SQL Server
```

La estructura principal del backend es:

```text
sgvf-api/
├── Controllers/
├── Data/
├── DTOs/
│   ├── Auth/
│   ├── Clientes/
│   ├── PagoCliente/
│   ├── Productos/
│   └── Ventas/
├── Entities/
├── Enums/
├── Services/
│   ├── Interfaces/
│   └── Pdf/
├── Program.cs
└── appsettings.json
```

## Funcionalidades

### Autenticación

- Inicio de sesión.
- Validación de credenciales.
- Generación de tokens JWT.
- Protección de endpoints mediante `[Authorize]`.
- Identificación del usuario autenticado mediante claims.

### Productos

- Obtener todos los productos.
- Obtener producto por ID.
- Crear productos.
- Actualizar productos.
- Eliminar productos.
- Gestión de stock.
- Control de stock mínimo.
- Registro de movimientos de stock.

### Clientes

- Obtener todos los clientes.
- Obtener cliente por ID.
- Crear clientes.
- Actualizar clientes.
- Eliminar clientes.
- Gestión del saldo pendiente.
- Registro de deudas.
- Registro de cobros.
- Consulta de pagos por cliente.

### Proveedores

- Gestión de proveedores.
- Consulta de proveedores.
- Gestión de cuentas corrientes.
- Registro de pagos.

### Ventas

- Registrar ventas.
- Consultar todas las ventas.
- Consultar una venta por ID.
- Registrar múltiples productos en una venta.
- Calcular subtotales.
- Calcular el total de la venta.
- Validar disponibilidad de stock.
- Descontar stock automáticamente.
- Registrar movimientos de stock.
- Registrar ventas pagadas.
- Registrar ventas pendientes.
- Actualizar la deuda del cliente.
- Cancelar ventas.
- Restaurar stock al cancelar una venta.
- Revertir deuda al cancelar una venta pendiente.
- Generar tickets en PDF.

## Reglas de negocio de ventas

Al registrar una venta, el backend realiza distintas validaciones antes de guardar la operación.

Entre ellas:

1. Valida la existencia del cliente cuando fue seleccionado.
2. Verifica que exista al menos un producto.
3. Valida que cada producto exista.
4. Verifica que los productos estén activos.
5. Valida que las cantidades sean mayores a cero.
6. Valida los precios ingresados.
7. Verifica que exista stock suficiente.
8. Calcula el subtotal de cada producto.
9. Calcula el total de la venta.
10. Descuenta el stock correspondiente.
11. Registra los movimientos de stock.
12. Guarda la venta y sus detalles.
13. Si la venta es pendiente, actualiza el saldo pendiente del cliente.

Ejemplo:

```text
Stock inicial:      20 cajones
Cantidad vendida:   5 cajones
Stock resultante:   15 cajones
```

En una venta pendiente:

```text
Deuda anterior:     $50.000
Venta pendiente:    $20.000
Nueva deuda:        $70.000
```

## Cancelación de ventas

Cuando una venta es cancelada, el backend revierte las operaciones relacionadas.

El sistema:

1. Verifica que la venta exista.
2. Verifica que no haya sido cancelada anteriormente.
3. Recupera los productos involucrados.
4. Restaura las cantidades al stock.
5. Marca la venta como cancelada.
6. Si la venta tenía saldo pendiente asociado a un cliente, revierte la deuda correspondiente.

De esta forma se mantiene la consistencia entre:

```text
Venta
  ↕
Stock
  ↕
Cliente
  ↕
Saldo pendiente
```

## Pagos de clientes

Los clientes pueden registrar pagos sobre su saldo pendiente.

Al registrar un cobro, el backend:

1. Valida que el cliente exista y esté activo.
2. Verifica que posea deuda.
3. Valida que el monto sea mayor a cero.
4. Evita registrar un pago superior a la deuda existente.
5. Registra el pago.
6. Reduce el saldo pendiente del cliente.
7. Actualiza la fecha del último cobro.
8. Actualiza el monto del último cobro.

También es posible consultar los pagos correspondientes a un cliente determinado.

## Endpoints principales

### Autenticación

```http
POST /api/Auth/login
```

### Productos

```http
GET    /api/Producto
GET    /api/Producto/{id}
POST   /api/Producto
PUT    /api/Producto/{id}
DELETE /api/Producto/{id}
```

### Clientes

```http
GET    /api/Cliente
GET    /api/Cliente/{id}
POST   /api/Cliente
PUT    /api/Cliente/{id}
DELETE /api/Cliente/{id}
```

### Pagos de clientes

```http
GET    /api/PagoCliente
GET    /api/PagoCliente/{id}
GET    /api/PagoCliente/cliente/{clienteId}
POST   /api/PagoCliente
DELETE /api/PagoCliente/{id}
```

### Ventas

```http
GET    /api/Venta
GET    /api/Venta/{id}
POST   /api/Venta
DELETE /api/Venta/{id}
GET    /api/Venta/{id}/ticket
```

## Autenticación JWT

Los endpoints protegidos requieren un JWT válido.

El token debe enviarse mediante el encabezado:

```http
Authorization: Bearer <token>
```

La autenticación se configura en `Program.cs`.

Ejemplo de configuración:

```json
{
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE"
  }
}
```

> Nunca se deben subir claves JWT reales, contraseñas o credenciales al repositorio público.

## Base de datos

El proyecto utiliza **SQL Server** y **Entity Framework Core** para persistencia de datos.

La conexión se configura mediante `appsettings.json`.

Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
  }
}
```

La cadena de conexión debe adaptarse al entorno donde se ejecute el proyecto.

> No incluir usuarios, contraseñas ni información sensible en un repositorio público.

## CORS

Durante el desarrollo, el backend permite solicitudes desde el frontend de React.

Ejemplo:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
```

## Swagger

La API utiliza Swagger/OpenAPI para facilitar la documentación y prueba de los endpoints durante el desarrollo.

Con el backend ejecutándose en el entorno local, Swagger estará disponible normalmente en:

```text
https://localhost:7153/swagger
```

El puerto puede variar dependiendo de la configuración local.

Swagger también permite utilizar autenticación Bearer para probar endpoints protegidos.

## Generación de tickets

Los tickets de venta se generan en formato PDF mediante **QuestPDF**.

El endpoint utilizado es:

```http
GET /api/Venta/{id}/ticket
```

El ticket incluye información como:

- Número de venta.
- Fecha.
- Cliente.
- Productos.
- Cantidades.
- Precios.
- Total de la operación.

Los tickets generados son comprobantes no fiscales.

## Instalación

### 1. Clonar el repositorio

```bash
git clone <URL_DEL_REPOSITORIO_BACKEND>
```

### 2. Ingresar al proyecto

```bash
cd sgvf-api
```

### 3. Restaurar dependencias

```bash
dotnet restore
```

### 4. Configurar la base de datos

Configurar la cadena de conexión correspondiente en:

```text
appsettings.json
```

### 5. Configurar JWT

Agregar la configuración correspondiente:

```json
{
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE"
  }
}
```

### 6. Ejecutar el proyecto

```bash
dotnet run
```

## Frontend

El frontend de SGVF fue desarrollado con React y TypeScript.

Repositorio:

<URL_DEL_REPOSITORIO_FRONTEND>

## Seguridad

Antes de publicar o clonar el proyecto en un repositorio público, se recomienda verificar que no se encuentren versionados:

- Contraseñas de SQL Server.
- Connection strings con credenciales.
- Claves JWT reales.
- Tokens de autenticación.
- Archivos con información sensible.

Para valores sensibles se recomienda utilizar variables de entorno, User Secrets o mecanismos equivalentes.

## Autores

Desarrollado por:

- Juan Cruz Manzo
- Camila Ernaga

## Estado del proyecto

Proyecto funcional desarrollado como aplicación web full stack.

---

**SGVF - Sistema de Gestión de Venta Frutihortícola**
