# Reservas - Aplicación de Gestión de Reservas

## Descripción
Esta es una aplicación desarrollada en **.NET Core 8** para la gestión de reservas en espacios determinados. La base de datos utilizada es **SQL Server**.

## Características Principales

- Creación y gestión de reservas.
- Validación de reservas para evitar conflictos de tiempo y espacio.
- Filtros para listar reservas por espacio, usuario, y rangos de fecha.
- Listado de espacios y usuarios disponibles.
- Manejador global de excepciones.
- Implementación de limitación de peticiones (rate limiting).
- Integración con Swagger para documentación de la API.
- Configuración de CORS para permitir solicitudes desde clientes externos como Angular.

## Requisitos Previos

1. **Software Requerido**:
   - .NET Core SDK 8.0
   - SQL Server
   - Visual Studio o Visual Studio Code
   - Angular (para el cliente, si se requiere probar desde el frontend)

2. **Configuración de la Base de Datos**:
   - Se proporciona un archivo de respaldo de la base de datos.
   - Restaure la base de datos usando SQL Server Management Studio (SSMS) o una herramienta equivalente antes de ejecutar la aplicación.

## Restauración de la Base de Datos

1. Abra SQL Server Management Studio (SSMS).
2. Conéctese al servidor donde desea restaurar la base de datos.
3. Haga clic derecho en "Bases de datos" y seleccione "Restaurar base de datos".
4. Seleccione el archivo de respaldo proporcionado.
5. Siga los pasos indicados para completar la restauración.
6. Verifique que las tablas y datos estén disponibles.

## Configuración de la Aplicación

1. **Conexión a la Base de Datos**:
   - Actualice la cadena de conexión en el archivo `appsettings.json` con la información de su servidor SQL Server:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
     }
     ```

2. **Configuración de CORS**:
   - Se incluye una política para permitir solicitudes desde `http://localhost:4200` (cliente Angular por defecto). Puede modificarla según sea necesario en `Program.cs`.

3. **Ejecución de la Aplicación**:
   - Ejecute el siguiente comando en la terminal para iniciar la aplicación:
     ```bash
     dotnet run
     ```
   - La aplicación estará disponible en `https://localhost:5001`.

## Endpoints Principales

### Reservas
- **Crear Reserva**: `POST /api/ReservationManagement`
- **Eliminar Reserva**: `DELETE /api/ReservationManagement/{id}`
- **Listar Reservas**: `GET /api/ReservationManagement`

### Espacios
- **Listar Espacios Disponibles**: `GET /api/ReservationManagement/available-spaces`

### Usuarios
- **Listar Usuarios Disponibles**: `GET /api/ReservationManagement/available-users`

## Manejador Global de Excepciones
La aplicación incluye un filtro global para manejar excepciones comunes:

- **Errores de validación de datos** (`DataValidationException`)
- **Errores de argumentos inválidos** (`ArgumentException`)
- **Errores inesperados**

Los errores se registran usando **Serilog** y se devuelven con un formato consistente.

## Pruebas Unitarias
Se incluyen pruebas unitarias para el servicio de reservas utilizando **Moq** y **xUnit**. Ejecute las pruebas con el siguiente comando:
```bash
dotnet test
```

## Swagger
Swagger está habilitado para documentar y probar la API. Acceda a la interfaz de Swagger en:
```
https://localhost:5001/swagger
```

## Limitación de Peticiones (Rate Limiting)
La aplicación utiliza **AspNetCoreRateLimit** para limitar las peticiones:
- Límite: 60 peticiones por minuto.

## Configuración de Registro
Los logs están configurados con **Serilog**. Los registros de errores y solicitudes se almacenan en archivos configurados en `SerilogConfiguration.ConfigureLogging`.

## Contacto
Para consultas o problemas, puede comunicarse con Fredy Mendoza.

