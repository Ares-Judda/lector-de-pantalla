# Documentación: API de Analíticas y "Nudging" (AnalyticsService)

Este microservicio es el "observador" del sistema. Su trabajo es recolectar eventos de telemetría del usuario (qué hace, dónde se atasca) y registrar las "ayudas" (nudges) que se le muestran.

**Seguridad:** Este servicio está protegido por JWT para identificar al usuario y prevenir abuso.  
**CORS:** Este servicio **SÍ REQUIERE CORS**, ya que será llamado directamente desde el frontend (React/JS).

## 1. Configuración de Secretos (User Secrets)

Para evitar exponer contraseñas y claves en el código fuente (ej. appsettings.json), usamos la herramienta dotnet user-secrets.

### Comandos de Configuración

Abre una terminal en la carpeta raíz de tu proyecto AnalyticsService.

#### 1. Inicializar User Secrets (solo una vez):

```
dotnet user-secrets init
```

#### 2. Guardar la Cadena de Conexión:

```
dotnet user-secrets set "ConnectionStrings:AnalyticsDbConnection" "Server=usuario;Database=analytics_db;User Id=usuario;Password=password;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;"
```

#### 3. Guardar la Clave Secreta de JWT:

(Debe ser idéntica a la clave usada en AuthProfileService y el API Gateway).

```
dotnet user-secrets set "JwtSettings:Key" "CLAVE-SECRETA-MUY-LARGA"
```

### Ejemplo de Archivo secrets.json (Resultado)

El contenido del secrets.json de este proyecto se verá así:

```
{
  "ConnectionStrings": {
    "AnalyticsDbConnection": "Server=server;Database=analytics_db;User Id=usuario;Password=password;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Key": "CLAVE-SECRETA-MUY-LARGA"
  }
}
```

## 2. Dependencias a instalar

Estos son los paquetes NuGet necesarios para este servicio:

- Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.22
- Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.22
- Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.22
- Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 8.0.22
- Install-Package Swashbuckle.AspNetCore -Version 6.6.2

## 3. Documentación de Endpoints

### AnalyticsController (Endpoints Protegidos)

#### Ruta base: /api/analytics

¡IMPORTANTE! Todos los endpoints en este controlador requieren un token JWT válido enviado en el header Authorization:

Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

### POST /api/analytics/events

Registra un evento de telemetría (uso de la app) del usuario. Esta es una llamada "disparar y olvidar" (fire-and-forget).

#### Request Body (CreateUsageEventDto):

```
{
  "screen": "TRANSACTION_AMOUNT_SCREEN",
  "eventType": "USER_IDLE_15_SECONDS",
  "details": "{\"valor_monto\": 1500}"
}
```
