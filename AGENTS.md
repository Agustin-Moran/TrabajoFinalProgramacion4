# AGENTS.md — GymTurnos (Trabajo Práctico Integrador - Programación IV)

Documento de contexto integral y guía de desarrollo para agentes y desarrolladores que trabajen en la solución **GymTurnos**.

---

## 1. Información General del Proyecto

* **Materia:** Programación IV (Cátedra universitaria - Backend .NET).
* **Objetivo:** Sistema de gestión de turnos y reservas de camas para un gimnasio de Pilates.
* **Meta de Evaluación:** **Aprobación Directa (Promoción)**.
* **Tecnología Base:** .NET 10 / C# con ASP.NET Core Web API y Entity Framework Core (Code First).
* **Base de Datos:** MySQL (con Pomelo EF Core).

---

## 2. Requerimientos Académicos para Aprobación Directa

1. **Clean Architecture Estricta:** Separación en 4 capas respetando la regla de dependencias:
   $$\text{Controller} \longrightarrow \text{IService} \longrightarrow \text{Service} \longrightarrow \text{IRepository} \longrightarrow \text{Repository} \longrightarrow \text{AppDbContext}$$
2. **DTOs (Data Transfer Objects):** Ningún controlador expone entidades directas ni contraseñas.
3. **Persistencia Relacional Code First:** EF Core completamente operativo con migraciones.
4. **Autenticación y Autorización Funcional (JWT):**
   * Endpoint de login (y registro) en `AuthenticationController`.
   * Contraseñas hasheadas (PBKDF2/SHA256).
   * Generación y validación de tokens JWT con claims de `Id`, `Email` y `Rol`.
   * El secret del token JWT debe leerse de configuración / variables de entorno de Azure.
   * Endpoints protegidos según rol mediante `[Authorize(Roles = "...")]`.
5. **Consumo de Servicio de Terceros:**
   * La API debe consumir en al menos uno de sus endpoints un servicio externo utilizando **`IHttpClientFactory`** (o Typed Client).
6. **Pipeline de CI/CD (GitHub Actions):**
   * Archivo workflow en `.github/workflows/` que compile, empaquete y despliegue automáticamente en Azure al hacer push a la rama principal.
7. **Despliegue Operativo en Azure:**
   * La API debe estar publicada en Azure App Service con una URL/dominio accesible externamente para la mesa de examen oral.
8. **Swagger / OpenAPI:**
   * Documentación interactiva habilitada con soporte para autorización JWT (botón *Authorize*).
9. **Patrón Generic Repository:** *(Opcional / Sube nota)*. No es mandatorio.

---

## 3. Modelo de Negocio y Tipos de Usuarios

El sistema modela un gimnasio de **Pilates** con control de camas por clase y tres roles:

### Roles del Sistema (`RolUsuario` enum: `Cliente = 1`, `Profesor = 2`, `Administrador = 3`)

* **Cliente:**
  * Inicia sesión (y registro).
  * Consulta clases disponibles y camas libres.
  * Reserva una cama (valida no duplicación y cupo máximo de la clase).
  * Recibe notificación por email automática al reservar (vía Resend).
  * Consulta sus turnos en `/api/reservas/mis-reservas` (obteniendo el id del cliente del JWT Claim).
  * Cancela un turno (`PUT /api/reservas/{id}/cancelar`), liberando la cama.
* **Profesor:**
  * Asignado a clases.
  * Consulta clases y alumnos inscriptos.
* **Administrador:**
  * Gestión completa de clases, camas y usuarios.

---

## 4. Estructura de la Solución

```
GymTurnos/
├── GymTurnos.Domain/               # Entidades (Cama, Clase, Reserva, Usuario) y Enums (RolUsuario)
├── GymTurnos.Application/          # DTOs, Interfaces y Lógica de negocio (Servicios)
│   ├── DTOs/                       # Camas, Clases, Reservas, Usuarios, Auth
│   ├── Interfaces/                 # Repositorios y Servicios (incluye IEmailService)
│   └── Services/                   # CamaService, ClaseService, ReservaService, UsuarioService
├── GymTurnos.Infrastructure/       # Acceso a datos, seguridad y servicios externos
│   ├── ExternalServices/          # ResendEmailService (Consumo de API de Resend vía HttpClient)
│   ├── Persistence/                # AppDbContext
│   ├── Repositories/               # CamaRepository, ClaseRepository, ReservaRepository, UsuarioRepository
│   └── Security/                   # JwtService, PasswordHasher
└── GymTurnos.API/                  # Presentación HTTP
    ├── Controllers/                # AuthenticationController, CamasController, ClasesController, EmailController, ReservasController, UsuariosController
    ├── Program.cs                  # Inyección de dependencias, JWT Bearer, Typed HttpClient, Swagger UI / OpenAPI
    └── appsettings.json            # ConnectionStrings, JWT Secret
```

---

## 5. Estado Actual del Repositorio (Actualizado al 03/11/2026)

| Componente | Estado | Detalle |
| :--- | :---: | :--- |
| **Clean Architecture** | ✅ **Cumple** | 4 capas correctamente desacopladas. |
| **Entidades de Dominio** | ✅ **Cumple** | Usuario, Cama, Clase, Reserva y RolUsuario. |
| **DTOs** | ✅ **Cumple** | Implementados para todas las entidades y la autenticación. |
| **Controladores** | ✅ **Cumple** | `AuthenticationController`, `CamasController`, `ClasesController`, `EmailController`, `ReservasController` y `UsuariosController` funcionales. |
| **Autenticación JWT** | ✅ **Cumple** | `AddJwtBearer` configurado, `JwtService`, `PasswordHasher`, endpoints protegidos con roles y lectura de claims. Swagger configurado con `BearerSecuritySchemeTransformer`. |
| **Servicios y Lógica** | ✅ **Cumple** | Validaciones de cupo, disponibilidad de cama, cancelación y hash de contraseñas. |
| **Repositorios** | ✅ **Cumple** | CRUD completo y consultas específicas de negocio (`GetByClienteIdAsync`, `ContarReservasActivasPorClaseAsync`, etc.). |
| **Servicio de Terceros (`HttpClientFactory`)** | ✅ **Cumple** | Implementado `ResendEmailService` registrado con `AddHttpClient<IEmailService, ResendEmailService>`, expuesto en `EmailController` e integrado en `ReservaService` para confirmar reservas por correo. |
| **Persistencia en la Nube** | ⚠️ **Parcial** | Funciona localmente en MySQL. Falta conectar a la base de datos en Azure. |
| **Pipeline CI/CD (GitHub Actions)** | ❌ **Pendiente** | Falta el workflow `.github/workflows/deploy.yml`. |
| **Deploy en Azure (URL pública)** | ❌ **Pendiente** | Falta publicar en Azure App Service. Condición excluyente para rendir. |
| **Configuración de Variables en Azure** | ❌ **Pendiente** | Configurar `ConnectionStrings__DefaultConnection`, `Jwt__Secret`, `Resend__ApiKey`, `Resend__From` en Azure. |
