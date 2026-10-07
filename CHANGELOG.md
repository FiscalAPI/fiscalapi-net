# Changelog

Cambios del SDK y notas de comportamiento del API de FiscalAPI que afectan a quien usa el SDK.

## [Sin publicar]

### Cambios incompatibles (BREAKING)

- Se elimina `Person.CapitalRegime` (estaba `[Obsolete]`): el API no tiene régimen de capital, ignoraba el valor y nunca lo devolvía. El código que lo asigna o lo lee deja de compilar: quite toda asignación o lectura de `CapitalRegime` (por ejemplo `CapitalRegime = "S.A. de C.V."` en un inicializador de `Person`) y envíe la razón social sin régimen de capital en `LegalName`.
- El API deja de devolver `stripeCustomerId` y `subscriptionStatus` en las personas (`/api/v4/people` y la persona de las reglas de descarga): eran datos internos de Stripe. Este SDK nunca los modeló en `Person`, así que el código que usa el modelo no cambia; si su integración los lee de la respuesta JSON cruda, quite esa lectura: el campo ya no llega.

### Modelo `Person`

- Nuevos miembros (solo lectura en el API salvo `PhoneNumber`):
  - `PhoneNumber`: teléfono de la persona.
  - `Balances`: saldos por tipo de crédito (`CreditBalance`: `CreditType` y `Available`). Solo aparecen los tipos que la persona ha tenido; un tipo ausente tiene saldo 0.
  - `IsOwner`: `true` si la persona es el owner de su tenant.
  - `ValidTo` (`DateTime?`): fin de vigencia de la persona. La asigna el API; casi siempre es `null` y es informativa (no limita el timbrado ni el acceso al API).
- `Password`: requerida al crear; en `UpdateAsync`, `null` o vacía conserva la contraseña actual. El API nunca la devuelve.
- `UserTypeId`: `"C"` (cliente, el valor por omisión al crear) o `"U"` (usuario). `"T"` (tenant) solo llega en respuestas: el API lo rechaza al crear y al actualizar solo lo acepta si la persona ya es `"T"`.

### Notas del API (sin cambio de forma en el SDK)

- `Person.TaxPassword` es la contraseña de la llave privada (.key) que la persona guarda en su perfil; el API no la usa para sellar (al timbrar usa la contraseña de los certificados registrados o la de `TaxCredentials`). Solo la reciben con valor la propia persona y el owner del tenant; los demás reciben `null`. En `UpdateAsync`, `null` la conserva (el SDK omite los nulos al serializar) y `""` la borra.
- `ValidationFailure.AttemptedValue`: en un 400 de validación, el valor de un secreto (contraseñas, códigos, tokens, archivos y contraseñas de CSD/FIEL) llega enmascarado como `"[masked: n]"` (`n` es su longitud), o `"[masked]"` si la falla es de un objeto o una lista que lo contiene.
- `TaxCredential.Password` solo se exige en la llave privada (.key); en el certificado (.cer) es opcional y el API no la usa.
- `TaxFile.Tin` es opcional en `TaxFiles.CreateAsync`: si se omite (`null` o vacío), el API usa el RFC de la persona. Si se envía, debe ser el RFC de la persona (sin distinguir mayúsculas); si no, el API responde 400 con la falla en `Tin` (después de comprobar que puede gestionar los certificados de la persona; si no, 403). El API no guarda el valor enviado: el `Tin` del archivo siempre es el RFC de la persona.
- El API ya no devuelve `twoFactorEnabled` en las personas (este SDK nunca lo modeló).
- Las personas ya no traen `stripeCustomerId` ni `subscriptionStatus` (datos internos de Stripe). `ValidTo` y `CommittedBalance` son de solo lectura: el API los ignora al crear o actualizar. `CommittedBalance` es un campo heredado que el API ya no calcula y siempre vale 0; para el saldo use `AvailableBalance`, `AvailableValidationBalance` o `Balances`.
