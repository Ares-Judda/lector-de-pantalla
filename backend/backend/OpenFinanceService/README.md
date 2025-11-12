# Documentación: API de Finanzas Abiertas (OpenFinanceService)

Esta API se encarga de gestionar las conexiones a instituciones financieras externas (Finanzas Abiertas), permitiendo al usuario ver productos como tarjetas y préstamos de otros bancos.

**Seguridad:** Este servicio está protegido por JWT. Asume que un AuthProfileService ya ha generado un token y este servicio solo lo valida.  
**CORS:** Este servicio **NO REQUIERE CORS**. Está diseñado para ser llamado exclusivamente a través del API Gateway (Ocelot), que es quien maneja las políticas de CORS.

---

## 1. Configuración de Secretos (User Secrets)

Para evitar exponer contraseñas y claves en el código fuente (ej. `appsettings.json`), usamos la herramienta `dotnet user-secrets`.

### Comandos de Configuración

Abre una terminal en la carpeta raíz de tu proyecto **OpenFinanceService**.

1. **Inicializar User Secrets (solo una vez):**  
   `dotnet user-secrets init`

2. **Guardar la Cadena de Conexión:**  
   `dotnet user-secrets set "ConnectionStrings:OpenFinanceDbConnection" "Server=ALECER\SQLEXPRESS;Database=openfinance_db;User Id=AleDataBase;Password=123SPEI;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;"`

3. **Guardar la Clave Secreta de JWT:**  
   (Debe ser idéntica a la clave usada en **AuthProfileService** y el **API Gateway**).  
   `dotnet user-secrets set "JwtSettings:Key" "ESTA-ES-UNA-CLAVE-SECRETA-MUY-LARGA-Y-SEGURA-PARA-EL-HACKATHON-123!"`

### Ejemplo de Archivo `secrets.json` (Resultado)

Estos comandos crearán un archivo `secrets.json` en una carpeta oculta de tu perfil de Windows/Mac.  
El contenido se verá así:

```{
  "ConnectionStrings": {
    "OpenFinanceDbConnection": "Server=ALECER\\SQLEXPRESS;Database=openfinance_db;User Id=AleDataBase;Password=123SPEI;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Key": "ESTA-ES-UNA-CLAVE-SECRETA-MUY-LARGA-Y-SEGURA-PARA-EL-HACKATHON-123!"
  }
}
```

---

## 2. Dependencias a instalar

- Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.22
- Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.22
- Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.22
- Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 8.0.22
- Install-Package Swashbuckle.AspNetCore -Version 6.6.2

---

## 3. Documentación de Endpoints

### OpenFinanceController (Endpoints Protegidos)

**Ruta base:** `/api/openfinance`
**IMPORTANTE:** Todos los endpoints en este controlador requieren un token JWT válido enviado en el header `Authorization`:

`Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`

---

### GET /api/openfinance/connections

Obtiene una lista de todas las conexiones a bancos externos que el usuario ha registrado.

**Request Body:** Ninguno.

**Response (Éxito 200 OK):**
Devuelve un array de `ConnectionDto`.

```[
  {
    "id": "c1d2e3f4-...",
    "providerName": "BBVA",
    "status": "ACTIVE",
    "lastSync": "2025-11-10T09:00:00Z"
  }
]
```

---

### POST /api/openfinance/connections

Crea una nueva conexión a un banco externo. En un escenario real, esto iniciaría un flujo OAuth2. Para el hackathon, simula la creación exitosa.

**Request Body (`CreateConnectionDto`):**

```{
  "providerName": "BBVA",
  "scopes": "accounts_read, transactions_read",
  "authToken": "TOKEN_SIMULADO_DEL_OTRO_BANCO"
}
```

**Atributos del Request:**

- `providerName` (string, Requerido): El nombre del banco o institución.
- `scopes` (string, Requerido): Los permisos concedidos (ej. `"accounts_read"`).
- `authToken` (string, Requerido): El token de acceso (simulado).

**Response (Éxito 201 Created):**
Devuelve la `ConnectionDto` recién creada.

```{
  "id": "c1d2e3f4-...",
  "providerName": "BBVA",
  "status": "ACTIVE",
  "lastSync": "2025-11-11T12:00:00Z"
}
```

---

### DELETE /api/openfinance/connections/{id}

Elimina una conexión a un banco externo (revoca el consentimiento). El `{id}` es el GUID de la conexión. Esto también borrará todos los productos externos asociados a ella (**ON DELETE CASCADE**).

**Request Body:** Ninguno.

**Response (Éxito 204 No Content):**
No devuelve contenido, solo confirma que fue borrado.

**Response (Error 404 Not Found):**

`{
  "message": "Conexión no encontrada o no pertenece al usuario."
}`

---

### GET /api/openfinance/products

Obtiene una lista de todos los productos externos (tarjetas, préstamos) que pertenecen al usuario, obtenidos de todas sus conexiones activas.

**Request Body:** Ninguno.

**Response (Éxito 200 OK):**
Devuelve un array de `ExternalProductDto`.

```
[
  {
    "id": "p1a2b3c4-...",
    "connectionId": "c1d2e3f4-...",
    "provider": "BBVA",
    "productType": "CREDIT_CARD",
    "name": "Tarjeta Oro",
    "balance": -5400.50,
    "currency": "MXN",
    "nextPaymentAmount": 1500.00,
    "nextPaymentDate": "2025-11-20",
    "lastSync": "2025-11-11T12:05:00Z"
  }
]
```

---

### POST /api/openfinance/sync/{connectionId}

Fuerza una "sincronización" manual con un banco externo. El `{connectionId}` es el GUID de la conexión. Para el demo, esto simula una llamada a la API del otro banco, borra los productos antiguos de esa conexión y crea una nueva lista de productos de ejemplo.

**Request Body:** Ninguno.

**Response (Éxito 200 OK):**
Devuelve la lista de nuevos productos sincronizados.

```[
  {
    "id": "p9x8y7z6-...",
    "connectionId": "c1d2e3f4-...",
    "provider": "BBVA",
    "productType": "CREDIT_CARD",
    "name": "Tarjeta Oro (Sincronizada)",
    "balance": -5800.00,
    "currency": "MXN",
    "nextPaymentAmount": 1600.00,
    "nextPaymentDate": "2025-11-20",
    "lastSync": "2025-11-11T12:10:00Z"
  }
]
```

---
