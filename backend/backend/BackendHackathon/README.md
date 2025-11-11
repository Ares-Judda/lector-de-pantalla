# Documentación: API de Autenticación y Perfil (AuthProfileService)

Esta API se encarga de la identidad del usuario, la autenticación (JWT) y la gestión de sus perfiles de accesibilidad y consentimientos.

---

## 1. Configuración de Secretos (User Secrets)

Para evitar exponer contraseñas y claves en el código fuente (ej. appsettings.json), usamos la herramienta **dotnet user-secrets**.

### Comandos de Configuración

Abre una terminal en la carpeta raíz de tu proyecto (donde está el .csproj).

**1. Inicializar User Secrets (solo una vez):**
```
dotnet user-secrets init
```

**2. Guardar la Cadena de Conexión:**  
(Reemplaza con tu servidor y contraseña real)
```
dotnet user-secrets set "ConnectionStrings:AuthDbConnection" "Server=localhost;Database=auth_db;User Id=user;Password=password!;Trusted_Connection=False;Encrypt=False;"
```

**3. Guardar la Clave Secreta de JWT:**  
(Usa un generador de claves seguras para este valor)
```
dotnet user-secrets set "JwtSettings:Key" "CLAVE-SECRETA-MUY-LARGA"
```

### Ejemplo de Archivo secrets.json (Resultado)
Estos comandos crearán un archivo `secrets.json` en una carpeta oculta de tu perfil de Windows/Mac.  
El contenido se verá así:
```json
{
  "ConnectionStrings": {
    "AuthDbConnection": "Server=localhost;Database=auth_db;User Id=user;Password=password!;Trusted_Connection=False;Encrypt=False;"
  },
  "JwtSettings": {
    "Key": "CLAVE-SECRETA-MUY-LARGA"
  }
}
```
---

## 2. Dependencias a instalar


- Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.22

- Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.22

- Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.22

- Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 8.0.22

- Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 8.0.22

- Install-Package Swashbuckle.AspNetCore -Version 6.6.2

---

## 3. Documentación de Endpoints

---

### **AuthController (Endpoints Públicos)**

**Ruta base:** `/api/auth`

---

#### **POST /api/auth/register**

Registra un nuevo usuario en el sistema.  
Al registrarse, también se crea automáticamente su perfil de accesibilidad por defecto.

**Request Body (RegisterRequestDto):**
```json
{
  "alias": "Alejandro",
  "password": "miPasswordSegura123",
  "email": "ale@correo.com",
  "phoneNumber": "5512345678"
}
```

**Atributos del Request:**

- **alias (string, Requerido):**  
  Significado: El nombre o apodo que el usuario usará.  
  Validación: 3-50 caracteres.  
  *(Anteriormente se validaba como único, pero ahora se trata como un nombre y puede repetirse).*

- **password (string, Requerido):**  
  Significado: La contraseña del usuario.  
  Validación: Mínimo 8 caracteres.  
  Nota: Este valor se hashea usando BCrypt antes de guardarse en la BD.

- **email (string, Opcional):**  
  Significado: El email del usuario. Debe ser único si se proporciona.  
  Validación: Formato de email válido.

- **phoneNumber (string, Opcional):**  
  Significado: El teléfono del usuario. Debe ser único si se proporciona.  
  Validación: Formato de teléfono válido.

**Response (Éxito 200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "alias": "Alejandro"
}
```

**Response (Error 400 Bad Request):**
```json
{
  "message": "El email ya está en uso."
}
```

---

#### **POST /api/auth/login**

Autentica a un usuario usando su alias y password y devuelve un token JWT.

**Request Body (LoginRequestDto):**
```json
{
  "alias": "Alejandro",
  "password": "miPasswordSegura123"
}
```

**Atributos del Request:**
- **alias (string, Requerido):** El alias que el usuario usó para registrarse.  
- **password (string, Requerido):** La contraseña del usuario.

**Response (Éxito 200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "alias": "Alejandro"
}
```

**Response (Error 401 Unauthorized):**
```json
{
  "message": "Usuario o contraseña incorrecta."
}
```

---

### **ProfileController (Endpoints Protegidos)**

**Ruta base:** `/api/profile`  
**¡IMPORTANTE!** Todos los endpoints en este controlador requieren un token JWT válido enviado en el header **Authorization**:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

#### **GET /api/profile/me**

Obtiene la información pública del perfil del usuario autenticado (el dueño del token).

**Request Body:** Ninguno.

**Response (Éxito 200 OK):**
```json
{
  "id": "a1b2c3d4-...",
  "alias": "Alejandro",
  "email": "ale@correo.com",
  "phoneNumber": "5512345678",
  "preferredLanguage": "es-MX",
  "demoMode": false
}
```

---

#### **PUT /api/profile/me**

Actualiza la información del perfil del usuario autenticado.  
Solo actualiza los campos que se envían (los campos nulos en el JSON se ignoran).

**Request Body (UpdateUserProfileDto):**
```json
{
  "email": "nuevo.email@correo.com",
  "phoneNumber": "5599998888"
}
```

**Atributos del Request:**
- **alias (string, Opcional):** Nuevo alias (3-50 caracteres).  
- **email (string, Opcional):** Nuevo email (debe ser único).  
- **phoneNumber (string, Opcional):** Nuevo teléfono (debe ser único).  
- **preferredLanguage (string, Opcional):** (ej. "en-US").

**Response (Éxito 200 OK):**
```json
{
  "id": "a1b2c3d4-...",
  "alias": "Alejandro",
  "email": "nuevo.email@correo.com",
  "phoneNumber": "5599998888",
  "preferredLanguage": "es-MX",
  "demoMode": false
}
```

---

#### **GET /api/profile/accessibility**

Obtiene el perfil de accesibilidad del usuario autenticado.

**Request Body:** Ninguno.

**Response (Éxito 200 OK):**
```json
{
  "theme": "light",
  "screenReaderMode": false,
  "fontScale": 1.0,
  "nudgingLevel": "medium",
  "voiceFeedback": false
}
```

---

#### **PUT /api/profile/accessibility**

Actualiza todo el perfil de accesibilidad del usuario.  
Este endpoint espera el objeto completo.

**Request Body (AccessibilityProfileDto):**
```json
{
  "theme": "high-contrast",
  "screenReaderMode": true,
  "fontScale": 1.5,
  "nudgingLevel": "high",
  "voiceFeedback": true
}
```

**Atributos y Posibles Valores:**

- **theme (string, Requerido):** `"light"`, `"dark"`, `"high-contrast"`.  
- **screenReaderMode (bool, Requerido):** `true` o `false`.  
- **fontScale (decimal, Requerido):** Escala de fuente (ej. 1.0, 1.5, 2.0). Validado entre 0.5 y 3.0.  
- **nudgingLevel (string, Requerido):** Nivel de ayuda proactiva. `"none"`, `"medium"`, `"high"`.  
- **voiceFeedback (bool, Requerido):** `true` o `false`.

**Response (Éxito 200 OK):**
```json
{
  "theme": "high-contrast",
  "screenReaderMode": true,
  "fontScale": 1.5,
  "nudgingLevel": "high",
  "voiceFeedback": true
}
```

---

#### **GET /api/profile/consent**

Obtiene una lista de todos los registros de consentimiento del usuario.

**Request Body:** Ninguno.

**Response (Éxito 200 OK):**
```json
[
  {
    "id": "c1d2e3f4-...",
    "type": "MARKETING_EMAIL",
    "granted": false,
    "timestamp": "2025-11-11T10:30:00Z"
  },
  {
    "id": "e5f6g7h8-...",
    "type": "OPEN_FINANCE_READ",
    "granted": true,
    "timestamp": "2025-11-10T09:00:00Z"
  }
]
```

---

#### **PUT /api/profile/consent**

Actualiza (o crea si no existe) un tipo específico de consentimiento para el usuario.

**Request Body (UpdateConsentRequestDto):**
```json
{
  "type": "MARKETING_EMAIL",
  "granted": true
}
```

**Atributos del Request:**
- **type (string, Requerido):** El identificador único del consentimiento (ej. `"MARKETING_EMAIL"`, `"OPEN_FINANCE_READ"`).  
- **granted (bool, Requerido):** El nuevo estado (`true` para aceptado, `false` para revocado).

**Response (Éxito 200 OK):**
```json
{
  "id": "c1d2e3f4-...",
  "type": "MARKETING_EMAIL",
  "granted": true,
  "timestamp": "2025-11-11T12:00:00Z"
}
```
