# Changelog

Cambios del SDK y notas de comportamiento del API de FiscalAPI que afectan a quien usa el SDK.

## [Sin publicar]

### Cambios incompatibles (BREAKING)

- Se elimina `Person.CapitalRegime` (estaba `[Obsolete]`): el API no tiene régimen de capital, ignoraba el valor y nunca lo devolvía. El código que lo asigna o lo lee deja de compilar: quite toda asignación o lectura de `CapitalRegime` (por ejemplo `CapitalRegime = "S.A. de C.V."` en un inicializador de `Person`) y envíe la razón social sin régimen de capital en `LegalName`.

### Modelo `Person`

- Nuevos miembros (solo lectura en el API salvo `PhoneNumber`):
  - `PhoneNumber`: teléfono de la persona.
  - `Balances`: saldos por tipo de crédito (`CreditBalance`: `CreditType` y `Available`). Solo aparecen los tipos que la persona ha tenido; un tipo ausente tiene saldo 0.
  - `IsOwner`: `true` si la persona es el owner de su tenant.
- `Password`: requerida al crear; en `UpdateAsync`, `null` o vacía conserva la contraseña actual. El API nunca la devuelve.
- `UserTypeId`: `"C"` (cliente, el valor por omisión al crear) o `"U"` (usuario). `"T"` (tenant) solo llega en respuestas: el API lo rechaza al crear y al actualizar solo lo acepta si la persona ya es `"T"`.

### Notas del API (sin cambio de forma en el SDK)

- `Person.TaxPassword` es la contraseña de la llave privada (.key) que la persona guarda en su perfil; el API no la usa para sellar (al timbrar usa la contraseña de los certificados registrados o la de `TaxCredentials`). Solo la reciben con valor la propia persona y el owner del tenant; los demás reciben `null`. En `UpdateAsync`, `null` la conserva (el SDK omite los nulos al serializar) y `""` la borra.
- `ValidationFailure.AttemptedValue`: en un 400 de validación, el valor de un secreto (contraseñas, códigos, tokens, archivos y contraseñas de CSD/FIEL) llega enmascarado como `"[masked: n]"` (`n` es su longitud), o `"[masked]"` si la falla es de un objeto o una lista que lo contiene.
- `TaxCredential.Password` solo se exige en la llave privada (.key); en el certificado (.cer) es opcional y el API no la usa.
- El API ya no devuelve `twoFactorEnabled` en las personas (este SDK nunca lo modeló).
