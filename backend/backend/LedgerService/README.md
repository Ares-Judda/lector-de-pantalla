# Documentación: API de Cuentas y Transacciones (LedgerService)

Este microservicio actúa como el "núcleo bancario" de la aplicación. Es responsable de gestionar las cuentas, saldos, beneficiarios y el historial de transacciones del usuario.

**Importante:** Este servicio está protegido. Asume que un AuthProfileService ya ha autenticado al usuario y ha generado un token JWT. Este servicio valida dicho token en cada petición.

---

## 1. Configuración de Dependencias (NuGet)

Abre la Consola del Administrador de Paquetes (PMC), selecciona el proyecto **LedgerService** y ejecuta los siguientes comandos para instalar las dependencias necesarias:

```
# --- Entity Framework Core (Base de Datos) ---
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.22
Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.22
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.22

# --- Seguridad (Validación de JWT) ---
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 8.0.22

# --- Documentación (Swagger/OpenAPI) ---
Install-Package Swashbuckle.AspNetCore -Version 6.6.2
```

---

## 2. Configuración de Secretos (User Secrets)

Este servicio necesita dos secretos para funcionar:

- La llave de JWT (para validar los tokens creados por AuthProfileService).
- La cadena de conexión (para acceder a `ledger_db`).

Abre una terminal en la carpeta raíz de tu proyecto **LedgerService**:

1. **Inicializar User Secrets (solo una vez):**

```
dotnet user-secrets init
```

2. **Guardar la Cadena de Conexión (Ledger):**

```
dotnet user-secrets set "ConnectionStrings:LedgerDbConnection" "Server=server;Database=ledger_db;User Id=usuario;Password=password;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;"
```

3. **Guardar la Clave Secreta de JWT:**  
   (¡MUY IMPORTANTE! Esta clave debe ser idéntica a la que usaste en _AuthProfileService_).

```
dotnet user-secrets set "JwtSettings:Key" "CLAVE-SECRETA-MUY-LARGA"
```

---

## 3. Documentación de Endpoints

**Ruta base:** `/api/ledger`  
**¡IMPORTANTE!** Todos los endpoints en este controlador están protegidos por `[Authorize]`.  
Requieren un token JWT válido en el header `Authorization: Bearer ...`.

El servicio leerá el **ID de Usuario (`user_id`)** desde el token para filtrar todas las consultas, garantizando que un usuario solo pueda ver sus propios datos.

---

### Cuentas (Accounts)

#### **GET /api/ledger/accounts**

Obtiene una lista de todas las cuentas (saldos) que pertenecen al usuario autenticado.

**Request Body:** Ninguno.

**Response (Éxito 200 OK):**  
Devuelve un array de `AccountDto`.

```json
[
  {
    "id": "a1b2c3d4-...",
    "accountNumber": "1234567890",
    "accountType": "DEFAULT",
    "balance": 5000.0,
    "currency": "MXN"
  }
]
```

---

### Beneficiarios (Beneficiaries)

#### **GET /api/ledger/beneficiaries**

Obtiene la lista de "contactos de confianza" (beneficiarios) que el usuario ha registrado.

**Request Body:** Ninguno.

**Response (Éxito 200 OK):**

```json
[
  {
    "id": "b1c2d3e4-...",
    "name": "Hijo Carlos",
    "alias": "Carlitos",
    "accountNumber": "0123456789",
    "bankName": "SPEI Bank",
    "isFavorite": true
  }
]
```

---

#### **POST /api/ledger/beneficiaries**

Agrega un nuevo beneficiario a la lista del usuario.

**Request Body (CreateBeneficiaryDto):**

```json
{
  "name": "Doctora Ana",
  "alias": "Dra. Ana M.",
  "accountNumber": "9876543210",
  "bankName": "Banco de la Salud"
}
```

**Atributos del Request:**

- **name (string, Requerido):** El nombre completo del beneficiario (para reconocimiento).
- **alias (string, Opcional):** Un apodo corto.
- **accountNumber (string, Requerido):** La CLABE o número de cuenta.
- **bankName (string, Requerido):** El nombre del banco destino.

**Response (Éxito 201 Created):**

```json
{
  "id": "f1e2d3c4-...",
  "name": "Doctora Ana",
  "alias": "Dra. Ana M.",
  "accountNumber": "9876543210",
  "bankName": "Banco de la Salud",
  "isFavorite": false
}
```

**Response (Error 400 Bad Request):**

```json
{
  "message": "Este beneficiario ya existe."
}
```

---

#### **DELETE /api/ledger/beneficiaries/{id}**

Elimina un beneficiario de la lista del usuario. El `id` debe ser el GUID del beneficiario.

**Request Body:** Ninguno.

**Response (Éxito 204 No Content):**  
No devuelve contenido, solo confirma que fue borrado.

**Response (Error 404 Not Found):**

```json
{
  "message": "Beneficiario no encontrado o no pertenece al usuario."
}
```

---

### Transacciones (Transactions)

#### **GET /api/ledger/transactions**

Obtiene el historial de transacciones (envíos) del usuario.

**Request Body:** Ninguno.

**Response (Éxito 200 OK):**

```json
[
  {
    "id": "t1t2t3t4-...",
    "fromAccountId": "a1b2c3d4-...",
    "toBeneficiaryId": "b1c2d3e4-...",
    "beneficiaryName": "Hijo Carlos",
    "amount": 500.0,
    "status": "COMPLETED",
    "speiReference": "abc123xyz",
    "createdAt": "2025-11-11T15:30:00Z"
  }
]
```

---

#### **POST /api/ledger/transactions**

Crea una nueva transacción (un envío de dinero).  
Esta es la lógica de negocio más importante.

**Request Body (CreateTransactionDto):**

```json
{
  "fromAccountId": "a1b2c3d4-...",
  "toBeneficiaryId": "b1c2d3e4-...",
  "amount": 500.0
}
```

**Atributos del Request:**

- **fromAccountId (GUID, Requerido):** El id de la cuenta (del GET /api/ledger/accounts) de donde saldrá el dinero.
- **toBeneficiaryId (GUID, Requerido):** El id del beneficiario (del GET /api/ledger/beneficiaries) a quien se le enviará.
- **amount (decimal, Requerido):** La cantidad de dinero a enviar. Debe ser positiva.

---

#### **Lógica Interna (Resumen):**

- Verifica que `fromAccountId` pertenezca al usuario.
- Verifica que `toBeneficiaryId` pertenezca al usuario.
- Verifica que el balance de la cuenta sea mayor o igual al `amount`.
- Resta el `amount` del balance de la cuenta.
- Crea el registro de la `Transaction`.
- _(Opcional: Podría crear una SecurityAlert si el monto es alto)._

---

**Response (Éxito 200 OK):**  
Devuelve el `TransactionDto` de la transacción recién completada.

**Response (Error 400 Bad Request):**

```json
{
  "message": "Saldo insuficiente."
}
```

```json
{
  "message": "El beneficiario no existe o no pertenece al usuario."
}
```
