# FiscalAPI SDK para .NET
[![.NET](https://img.shields.io/badge/.NET-6.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/6)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/9)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/10)
[![NuGet](https://img.shields.io/nuget/v/FiscalApi.svg)](https://www.nuget.org/packages/FiscalApi/)
[![License](https://img.shields.io/github/license/FiscalAPI/fiscalapi-net)](https://github.com/FiscalAPI/fiscalapi-net/blob/master/LICENSE.txt) 

**SDK oficial de https://fiscalapi.com para .NET**, la API de facturación CFDI y otros servicios fiscales en México. Simplifica la integración con los servicios de facturación electrónica, eliminando las complejidades del SAT y facilitando la generación de facturas, notas de crédito, complementos de pago, nómina, carta porte, y más. ¡Factura sin dolor!



## 📋 Facturación CFDI 4.0
- **Soporte completo para CFDI 4.0** con todas las especificaciones oficiales
- **Timbrado de facturas de ingreso** con validación automática
- **Timbrado de notas de crédito** (facturas de egreso)
- **Timbrado de complementos de pago** en MXN, USD y EUR
- **Timbrado de facturas de nómina** 
- **Timbrado de facturas de carta porte** 
- **Timbrado de facturas de comercio exterior**  
- **Consulta del estatus de facturas** en el SAT en tiempo real
- **Cancelación de facturas** 
- **Generación de archivos PDF** de las facturas con formato profesional
- **Personalización de logos y colores** en los PDF generados
- **Envío de facturas por correo electrónico** automatizado
- **Descarga de archivos XML** con estructura completa
- **Almacenamiento y recuperación** de facturas por 5 años.
- Dos [modos de operación](https://docs.fiscalapi.com/modes-of-operation): **Por valores** o **Por referencias**

## 📥 Descarga Masiva
- **Acceso a catálogos de descarga masiva** del SAT
- **Descarga de CFDI y Metadatos** en lotes grandes
- **Descarga masiva XML** con filtros personalizados
- **Reglas de descarga automática por RFC** 
- **Solicitudes de descarga** via API y Dashboard.
- **Automatización de solicitudes de descarga**

## 👥 Gestión de Personas
- **Administración de personas** (emisores, receptores, clientes, usuarios, etc.)
- **Gestión de certificados CSD y FIEL** (subir archivos .cer y .key a FiscalAPI)
- **Configuración de datos fiscales** (RFC, domicilio fiscal, régimen fiscal)
- **Datos de empleado** (agrega/actualiza/elimina datos de empleado a una persona. CFDI Nómina)
- **Datos de empleador** (agrega/actualiza/elimina datos de empleador a una persona. CFDI Nómina)

## 🎖️ Gestión de Timbres
- **Gestión de folios fiscales** Compra timbres a FiscalAPI y transfiere/retira a las personas de tu organización según tus reglas de negocio.
- **Créditos de validación** Transfiere créditos de validación SAT (`CreditType.Validation`) con la misma operación de timbres y consulta el saldo `AvailableValidationBalance` de cada persona.
- **Créditos de ticket** Transfiere créditos de ticket (`CreditType.Ticket`) con la misma operación de timbres y consulta su saldo en `Balances` de cada persona.

## ✅ Validaciones SAT de CFDI
- **Estructura del XML** contra el Anexo 20 y sus complementos (verificador oficial del SAT)
- **Vigencia del certificado** del emisor a la fecha de emisión
- **Sello del CFDI y sello del SAT** (TimbreFiscalDigital)
- **Estatus del comprobante** en el SAT (vigente, cancelado, no encontrado)
- **Listas negras 69-B y 69-B Bis** por CFDI o directamente por RFC
- **Catálogo de tipos y estatus** de validación consultable desde el SDK

## 🛍️ Gestión de Productos/Servicios
- **Gestión de productos y servicios** con catálogo personalizable
- **Administración de impuestos aplicables** (IVA, ISR, IEPS)

## 📚 Consulta de Catálogos SAT
- **Consulta en catálogos oficiales del SAT** actualizados
- **Consulta en catálogos oficiales de Descarga masiva del SAT** actualizados
- **Búsqueda de información** en catálogos del SAT con filtros avanzados
- **Acceso y búsqueda** en catálogos completos

## 📖 Recursos Adicionales
- **Cientos de ejemplos de código** disponibles en múltiples lenguajes de programación
- Documentación completa con guías paso a paso
- Ejemplos prácticos para casos de uso comunes
- Soporte técnico especializado
- Actualizaciones regulares conforme a cambios del SAT



## 📦 Instalación
Compatible con múltiples versiones de .NET (desde **.NET Framework 4.6.1** hasta **.NET 8**)

**NuGet Package Manager**:

```bash
NuGet\Install-Package Fiscalapi
```

**.NET CLI**:

```bash
dotnet add package Fiscalapi
```

## ⚙️ Configuración

Puedes usar el SDK tanto en aplicaciones sin inyección de dependencias (WinForms, Consolas, WPF, etc.) como en proyectos que usan DI (ASP.NET Core, Blazor, etc.). A continuación se describen ambas formas:

### A) Aplicaciones sin Inyección de Dependencias

1. **Crea tu objeto de configuración** con [tus credenciales](https://docs.fiscalapi.com/credentials-info):
    ```csharp
    var settings = new FiscalapiSettings
    {
        ApiUrl = "https://test.fiscalapi.com", // https://live.fiscalapi.com (producción)
        ApiKey = "<tu_api_key>",
        Tenant = "<tenant>"
    };
    ```

2. **Crea la instancia del cliente**:
    ```csharp
    var fiscalApi = FiscalApiClient.Create(settings);
    ```

Para ejemplos completos, consulta [winforms-console](https://github.com/FiscalAPI/fiscalapi-samples-net-winforms).

---

### B) Aplicaciones con Inyección de Dependencias (ASP.NET, Blazor, etc.)

1. **Agrega la sección de configuración** en tu `appsettings.json`:
    ```jsonc
    {
      "FiscalapiSettings": {
        "ApiUrl": "https://test.fiscalapi.com", // https://live.fiscalapi.com (producción)
        "ApiKey": "<YourApiKeyHere>",
        "Tenant": "<YourTenantHere>"
      }
    }
    ```

2. **Registra los servicios** en el contenedor (por ejemplo, en `Program.cs`):
    ```csharp
    builder.Services.AddFiscalApi();
    ```

Posteriormente, podrás **inyectar** `IFiscalApiClient` donde lo requieras:

```csharp
public class InvoicesController : Controller
{
    private readonly IFiscalApiClient _fiscalApi;

    public InvoicesController(IFiscalApiClient fiscalApi)
    {
        _fiscalApi = fiscalApi;
    }
    
    // Usa _fiscalApi en tus métodos de controlador...
}
```

Para más ejemplos, revisa [samples-asp-net](https://github.com/FiscalAPI/fiscalapi-samples-net-aspnet).


## 🔄 Modos de Operación

FiscalAPI admite dos [modos de operación](https://docs.fiscalapi.com/modes-of-operation):

- **Por Referencias**: Envía solo IDs de objetos previamente creados en el dashboard de FiscalAPI.  
  Ideal para integraciones ligeras.

- **Por Valores**: Envía todos los campos requeridos en cada petición, con mayor control sobre los datos.  
  No se requiere configuración previa en el dashboard.


## 📝 Ejemplos de Uso

A continuación se muestran algunos ejemplos básicos para ilustrar cómo utilizar el SDK. Puedes encontrar más ejemplos en la [documentación oficial](https://docs.fiscalapi.com).

### 1. Crear una Persona (Emisor o Receptor)

```csharp
var fiscalApi = FiscalApiClient.Create(Settings);

var request = new Person
{
    LegalName = "Persona de Prueba",
    Email = "someone@somewhere.com",
    Password = "YourStrongPassword123!",
};

var apiResponse = await fiscalApi.Persons.CreateAsync(request);
```

Para un **receptor extranjero** (necesario para facturar Comercio Exterior por referencias), agrega la
residencia fiscal y la identificación tributaria del país de residencia:

```csharp
var request = new Person
{
    LegalName = "Persona Fisica Extranjera",
    Email = "someone@somewhere.com",
    Password = "YourStrongPassword123!",
    Tin = "XEXX010101000",
    ZipCode = "42501",
    SatTaxRegimeId = "616",
    SatCfdiUseId = "S01",
    CountryId = "USA",          // Residencia fiscal, catálogo c_Pais. Va a cfdi:Receptor@ResidenciaFiscal
    ForeignTin = "123456789"    // Va a cfdi:Receptor@NumRegIdTrib
};

var apiResponse = await fiscalApi.Persons.CreateAsync(request);

// El id devuelto es el que se usa como Recipient.Id al facturar por referencias.
var recipientId = apiResponse.Data.Id;
```

### 2. Subir Certificados CSD
[Descarga certificados de prueba](https://docs.fiscalapi.com/tax-files-info)

```csharp
var fiscalApi = FiscalApiClient.Create(Settings);

var certificadoCsd = new TaxFile
{
    PersonId = "984708c4-fcc0-43bd-9d30-ec017815c20e",
    Base64File = "MIIFsDCCA5igAwIBAgI...==", // Certificado .cer codificado en Base64
    FileType = FileType.CertificateCsd,
    Password = "12345678a",
    Tin = "EKU9003173C9" // RFC de la persona (opcional)
};

var clavePrivadaCsd = new TaxFile
{
    PersonId = "984708c4-fcc0-43bd-9d30-ec017815c20e",
    Base64File = "MIIFDjBABgkqhkiG9w0BBQ0...==", // Llave privada .key codificada en Base64
    FileType = FileType.PrivateKeyCsd,
    Password = "12345678a",
    Tin = "EKU9003173C9" // RFC de la persona (opcional)
};

var apiResponseCer = await fiscalApi.TaxFiles.CreateAsync(certificadoCsd);
var apiResponseKey = await fiscalApi.TaxFiles.CreateAsync(clavePrivadaCsd);
```

### 3. Crear un Producto o Servicio

```csharp
var fiscalApi = FiscalApiClient.Create(Settings);

var request = new Product
{
    Description = "Servicios contables",
    UnitPrice = 100,
    SatUnitMeasurementId = "E48",
    SatTaxObjectId = "02",
    SatProductCodeId = "84111500"
};

var apiResponse = await fiscalApi.Products.CreateAsync(request);
```

### 4. Actualizar Impuestos de un Producto

```csharp
var fiscalApi = FiscalApiClient.Create(Settings);

var request = new Product
{
    Id = "310301b3-1ae9-441b-b463-51a8f9ca8ba2",
    Description = "Servicios contables",
    UnitPrice = 100, 
    SatUnitMeasurementId = "E48",
    SatTaxObjectId = "02",
    SatProductCodeId = "84111500",
    ProductTaxes = new List<ProductTax>
    {
        new ProductTax { Rate = 0.16m, TaxId = "002", TaxFlagId = "T", TaxTypeId = "Tasa" },  // IVA 16%
        new ProductTax { Rate = 0.10m, TaxId = "001", TaxFlagId = "R", TaxTypeId = "Tasa" },  // ISR 10%
        new ProductTax { Rate = 0.10666666666m, TaxId = "002", TaxFlagId = "R", TaxTypeId = "Tasa" } // IVA 2/3 partes
    }
};

var apiResponse = await fiscalApi.Products.UpdateAsync(request.Id, request);
```

### 5. Crear una Factura de Ingreso (Por Referencias)

```csharp
var fiscalApi = FiscalApiClient.Create(Settings);

var invoice = new Invoice
{
    VersionCode = "4.0",
    Series = "SDK-F",
    Date = DateTime.Now,
    PaymentFormCode = "01",
    CurrencyCode = "MXN",
    TypeCode = "I",
    ExpeditionZipCode = "42501",
    Issuer = new InvoiceIssuer
    {
        Id = "<id-emisor-en-fiscalapi>"
    },
    Recipient = new InvoiceRecipient
    {
        Id = "<id-receptor-en-fiscalapi>"
    },
    Items = new List<InvoiceItem>
    {
        new InvoiceItem
        {
            Id = "<id-producto-en-fiscalapi>",
            Quantity = 1,
            Discount = 10.85m
        }
    },
    PaymentMethodCode = "PUE",
};

var apiResponse = await fiscalApi.Invoices.CreateAsync(invoice);
```

### 6. Crear la Misma Factura de Ingreso (Por Valores)

```csharp
var fiscalApi = FiscalApiClient.Create(settings);

// Agregar sellos CSD, Emisor, Receptor, Items, etc.
var invoice = new Invoice
{
    VersionCode = "4.0",
    Series = "SDK-F",
    Date = DateTime.Now,
    PaymentFormCode = "01",
    CurrencyCode = "MXN",
    TypeCode = "I",
    ExpeditionZipCode = "42501",
    Issuer = new InvoiceIssuer
    {
        Tin = "EKU9003173C9",
        LegalName = "ESCUELA KEMPER URGATE",
        TaxRegimeCode = "601",
        TaxCredentials  = new List<TaxCredential>()
         {
             new TaxCredential
             {
                 Base64File ="certificate_base64...",
                 FileType = FileType.CertificateCsd,
                 Password = "12345678a"
             },
             new TaxCredential
             {
                 Base64File ="private_key_base64...",
                 FileType = FileType.PrivateKeyCsd,
                 Password = "12345678a"
             }
         }
    },
    Recipient = new InvoiceRecipient
    {
        Tin = "EKU9003173C9",
        LegalName = "ESCUELA KEMPER URGATE",
        ZipCode = "42501",
        TaxRegimeCode = "601",
        CfdiUseCode = "G01",
        Email = "someone@somewhere.com"
    },
    Items = new List<InvoiceItem>
    {
        new InvoiceItem
        {
            ItemCode = "01010101",
            Quantity = 9.5m,
            UnitOfMeasurementCode = "E48",
            Description = "Invoicing software as a service",
            UnitPrice = 3587.75m,
            TaxObjectCode = "02",
            Discount = 255.85m,
            ItemTaxes = new List<InvoiceItemTax>
            {
                new InvoiceItemTax
                {
                    TaxCode = "002", // IVA
                    TaxTypeCode = "Tasa",
                    TaxRate = 0.16m,
                    TaxFlagCode = "T"
                }
            }
        }
    },
    PaymentMethodCode = "PUE",
};

var apiResponse = await fiscalApi.Invoices.CreateAsync(invoice);
```


### 7. Crear Factura con Complemento de Comercio Exterior (Por Valores)

```csharp
var fiscalApi = FiscalApiClient.Create(settings);

var invoice = new Invoice
{
    VersionCode = "4.0",
    PaymentFormCode = "99",
    PaymentMethodCode = "PPD",
    CurrencyCode = "MXN",
    TypeCode = "I",
    ExpeditionZipCode = "42501",
    Series = "CCE",
    Date = DateTime.Parse("2025-05-02T15:56:40"),
    PaymentConditions = "CondicionesDePago",
    ExportCode = "02",
    Issuer = new InvoiceIssuer
    {
        Tin = "EKU9003173C9",
        LegalName = "ESCUELA KEMPER URGATE",
        TaxRegimeCode = "601",
        TaxCredentials = new List<TaxCredential>
        {
            new TaxCredential { Base64File = "certificate_base64...", FileType = FileType.CertificateCsd, Password = "12345678a" },
            new TaxCredential { Base64File = "private_key_base64...", FileType = FileType.PrivateKeyCsd, Password = "12345678a" }
        }
    },
    Recipient = new InvoiceRecipient
    {
        Tin = "XEXX010101000",
        LegalName = "Persona Fisica Extranjera",
        ZipCode = "42501",
        TaxRegimeCode = "616",
        CfdiUseCode = "S01",
        CountryId = "USA",
        ForeignTin = "123456789"
    },
    Items = new List<InvoiceItem>
    {
        new InvoiceItem
        {
            ItemCode = "50211503",
            ItemSku = "131494-1055",
            Quantity = 2,
            UnitOfMeasurementCode = "H87",
            Description = "Cigarros",
            UnitPrice = 200.00m,
            TaxObjectCode = "02",
            ItemTaxes = new List<InvoiceItemTax>
            {
                new InvoiceItemTax { TaxCode = "002", TaxTypeCode = "Tasa", TaxRate = 0.160000m, TaxFlagCode = "T" },
                new InvoiceItemTax { TaxCode = "001", TaxTypeCode = "Tasa", TaxRate = 0.100000m, TaxFlagCode = "R" },
                new InvoiceItemTax { TaxCode = "002", TaxTypeCode = "Tasa", TaxRate = 0.106666m, TaxFlagCode = "R" }
            }
        }
    },
    Complement = new Complement
    {
        ComercioExterior = new Fiscalapi.Models.ForeignTrade.ComercioExterior
        {
            ClaveDePedimentoId = "A1",
            CertificadoOrigen = 0,
            IncotermId = "FOB",
            TipoCambioUSD = 17.4948m,
            Emisor = new Fiscalapi.Models.ForeignTrade.Emisor
            {
                Domicilio = new Fiscalapi.Models.ForeignTrade.EmisorDomicilio
                {
                    Calle = "CALLE DEL PAPEL",
                    ColoniaId = "0214",
                    LocalidadId = "01",
                    MunicipioId = "014",
                    EstadoId = "QUE",
                    PaisId = "MEX",
                    CodigoPostalId = "76199"
                }
            },
            Receptor = new Fiscalapi.Models.ForeignTrade.Receptor
            {
                NumRegIdTrib = "123456789",
                Domicilio = new Fiscalapi.Models.ForeignTrade.ReceptorDomicilio
                {
                    Calle = "ST. A",
                    Estado = "TX",
                    PaisId = "USA",
                    CodigoPostal = "00000"
                }
            },
            Mercancias = new List<Fiscalapi.Models.ForeignTrade.Mercancia>
            {
                new Fiscalapi.Models.ForeignTrade.Mercancia
                {
                    NoIdentificacion = "131494-1055",
                    FraccionArancelariaId = "2402200100",
                    CantidadAduana = 2.00m,
                    UnidadAduanaId = "01",
                    ValorUnitarioAduana = 11.74m,
                    ValorDolares = 23.47m
                }
            }
        }
    }
};

var apiResponse = await fiscalApi.Invoices.CreateAsync(invoice);
```

> Para la versión **Por Referencias**, sustituye los objetos `Issuer` y `Recipient` por `new InvoiceIssuer { Id = "..." }` y `new InvoiceRecipient { Id = "..." }` manteniendo el resto idéntico.

### 8. Firmar Manifiesto

```csharp
var fiscalApi = FiscalApiClient.Create(settings);

var request = new SignManifestRequest
{
    Base64Cer = "MIIFsDCCA5igAwIBAgI...==", // .cer en Base64
    Base64Key = "MIIFDjBABgkqhkiG9w0BBQ0...==", // .key en Base64
    Password  = "12345678a"
};

var apiResponse = await fiscalApi.Manifests.SignAsync(request);

if (apiResponse.Succeeded)
{
    var bytes = Convert.FromBase64String(apiResponse.Data.Base64File);
    // FileName ya incluye la extensión (por ejemplo "EKU9003173C9.pdf")
    File.WriteAllBytes(apiResponse.Data.FileName, bytes);
}
```

Al firmar, todas las personas del tenant con el RFC del certificado quedan con `ManifestStatusId` en `Signed` (`ManifestStatusIds.Signed`).

### 9. Búsqueda en Catálogos del SAT

```csharp
// Busca los registros que contengan 'inter' en el catalogo 'SatUnitMeasurements' (pagina 1, tamaño pagina 10)
var apiResponse = await fiscalApi.Catalogs.SearchCatalogAsync("SatUnitMeasurements", "inter", 1, 10);

if (apiResponse.Succeeded)
{
    foreach (var item in apiResponse.Data.Items)
    {
        Console.WriteLine($"Unidad: {item.Description}");
    }
}
else
{
    Console.WriteLine(apiResponse.Message);
}
```

### 10. Validar un CFDI ante el SAT

Cada tipo de validación consume un crédito de validación. Los resultados vienen en el orden del catálogo y `Status.Details` trae los hechos del caso (RFC y corte del listado, número de certificado, estado en el SAT) cuando aplica.

```csharp
var fiscalApi = FiscalApiClient.Create(settings);

var request = new SatValidationRequest
{
    Xml = Convert.ToBase64String(File.ReadAllBytes("factura-timbrada.xml")), // CFDI timbrado en Base64
    ValidationTypes = new List<string>
    {
        SatValidationTypeIds.XmlStructure,
        SatValidationTypeIds.CertificateValidity,
        SatValidationTypeIds.CfdiSello,
        SatValidationTypeIds.TfdSello,
        SatValidationTypeIds.CfdiStatus,
        SatValidationTypeIds.Blacklist69B,
        SatValidationTypeIds.Blacklist69BBis
    }
};

var apiResponse = await fiscalApi.SatValidations.ValidateAsync(request);

if (apiResponse.Succeeded)
{
    foreach (var result in apiResponse.Data)
    {
        Console.WriteLine($"{result.Type.Id}: {result.Status.Id} (passed: {result.Passed}) {result.Status.Details}");
    }
}
else
{
    Console.WriteLine(apiResponse.Details); // 400 sin cobro, 402 créditos insuficientes, etc.
}
```

Para consultar únicamente listas negras basta el RFC, sin XML:

```csharp
var request = new SatValidationRequest
{
    Tin = "XAXX010101000",
    ValidationTypes = new List<string> { SatValidationTypeIds.Blacklist69B, SatValidationTypeIds.Blacklist69BBis }
};

var apiResponse = await fiscalApi.SatValidations.ValidateAsync(request);
```

El catálogo de tipos y los estatus posibles de cada tipo también están disponibles:

```csharp
var types = await fiscalApi.SatValidations.GetTypesAsync();                                   // GET /sat-validations
var type = await fiscalApi.SatValidations.GetTypeByIdAsync(SatValidationTypeIds.CfdiStatus);  // GET /sat-validations/{id}
var statuses = await fiscalApi.SatValidations.GetStatusesAsync(SatValidationTypeIds.CfdiStatus); // GET /sat-validations/{id}/statuses

foreach (var item in statuses.Data)
{
    Console.WriteLine($"{item.Id}: {item.Description}");
}
```

### 11. Transferir timbres, créditos de validación o créditos de ticket

```csharp
var apiResponse = await fiscalApi.Stamps.TransferStamps(new StampTransactionParams
{
    FromPersonId = "<id_persona_origen>",
    ToPersonId = "<id_persona_destino>",
    Amount = 10,
    Comments = "Créditos para validar CFDI",
    CreditType = CreditType.Validation // CreditType.Stamp (por defecto) transfiere timbres
});

// Créditos de ticket: la misma operación con CreditType.Ticket
var ticketResponse = await fiscalApi.Stamps.TransferStamps(new StampTransactionParams
{
    FromPersonId = "<id_persona_origen>",
    ToPersonId = "<id_persona_destino>",
    Amount = 100,
    Comments = "Créditos de ticket",
    CreditType = CreditType.Ticket
});

// El saldo de cada persona se consulta en Persons.GetByIdAsync(...): AvailableBalance (timbres), AvailableValidationBalance (validaciones)
// y Balances (por tipo de crédito; un tipo ausente tiene saldo 0)
var person = (await fiscalApi.Persons.GetByIdAsync("<id_persona_destino>")).Data;
var ticketBalance = person.Balances.FirstOrDefault(b => b.CreditType == CreditType.Ticket)?.Available ?? 0;
```

### 12. Tickets de Auto CFDI: ingesta y consulta

```csharp
// Ingesta de una venta pagada (cobra 1 crédito de ticket). La Idempotency-Key es opcional: repetir la misma key con el
// mismo cuerpo responde 200 con el ticket original sin cobrar otra vez.
var ticket = new Ticket
{
    Issuer = new TicketIssuer { Id = "<id_persona_emisora>" },
    Folio = "F-1001",
    Series = "A",
    PaymentDate = new DateTime(2026, 10, 15, 12, 30, 0), // hora civil de México
    PaymentFormCode = "01",
    Total = 116.00m, // debe cuadrar al centavo con los conceptos e impuestos
    Items = new List<TicketItem>
    {
        new TicketItem
        {
            ItemCode = "50202306",
            ItemSku = "SKU-1",
            Quantity = 1,
            UnitOfMeasurementCode = "H87",
            Description = "Refresco",
            UnitPrice = 100.00m,
            TaxObjectCode = "02",
            ItemTaxes = new List<TicketItemTax>
            {
                new TicketItemTax { TaxCode = "002", TaxTypeCode = "Tasa", TaxRate = 0.160000m, TaxFlagCode = "T" }
            }
        }
    }
};

var created = await fiscalApi.Tickets.CreateAsync(ticket, idempotencyKey: "pos-01-F-1001");
if (created.Succeeded)
{
    // Imprime el código y el QR en el ticket de venta
    Console.WriteLine($"{created.Data.TicketCode} {created.Data.QrUrl}");
}
else
{
    // 409: folio y serie ya activos; 422: total que no cuadra, emisor sin CSD o sin créditos de ticket, etc.
    // ErrorData trae el data del error: existingTicketId (409) o expectedTotal y computedTotal (422).
    Console.WriteLine($"{created.HttpStatusCode}: {created.Details} {created.ErrorData}");
}

// Tickets pendientes de un emisor en septiembre (por ejemplo, para armar la factura global)
var page = await fiscalApi.Tickets.GetListAsync(new TicketListFilters
{
    Status = new List<string> { TicketStatus.Pending },
    PersonId = "<id_persona_emisora>",
    From = new DateTime(2026, 9, 1),
    To = new DateTime(2026, 9, 30),
    PageNumber = 1,
    PageSize = 50
});

// Detalle con conceptos e historia, código para reimprimir y cancelación (solo en estado pending)
var detail = await fiscalApi.Tickets.GetByIdAsync("<id_ticket>", details: true);
var code = await fiscalApi.Tickets.GetCodeAsync("<id_ticket>");
var cancelled = await fiscalApi.Tickets.CancelAsync("<id_ticket>");
```

`Status` (y `Type`/`ActorType` de los eventos) llegan como texto camelCase; compáralos con las constantes de `TicketStatus`, `TicketEventType` y `TicketActorType`. Un valor que esta versión del SDK no conoce llega tal cual, sin error.

### 13. Factura global de tickets (Auto CFDI)

La global de tickets es una factura de ingreso estándar (`Invoices.CreateAsync`) con `GlobalInformation`, el emisor por referencia (`Issuer.Id`) y `TicketId` en cada concepto. Cada ticket va en un concepto por grupo de objeto de impuesto e impuestos (un ticket con IVA 16 % y 0 % lleva dos conceptos con el mismo `TicketId`), y la suma de sus conceptos debe reflejar el ticket. Una global lleva una sola moneda, tipo de cambio y lugar de expedición, y hasta 1,500 conceptos: este ejemplo asume tickets en MXN de un solo lugar de expedición. Una global sin `TicketId` no marca los tickets como facturados.

```csharp
// Tickets pendientes del emisor en septiembre, con sus conceptos
var issuerId = "<id_persona_emisora>";
var tickets = new List<Ticket>();
for (var pageNumber = 1; ; pageNumber++)
{
    var page = (await fiscalApi.Tickets.GetListAsync(new TicketListFilters
    {
        Status = new List<string> { TicketStatus.Pending },
        PersonId = issuerId,
        From = new DateTime(2026, 9, 1),
        To = new DateTime(2026, 9, 30),
        PageNumber = pageNumber,
        PageSize = 50
    })).Data;
    foreach (var summary in page.Items)
        tickets.Add((await fiscalApi.Tickets.GetByIdAsync(summary.Id, details: true)).Data);
    if (!page.HasNextPage)
        break;
}

// Un concepto por ticket y grupo de impuestos: valor unitario = suma de cantidad × valor unitario (6 decimales por concepto)
// y descuento = suma de descuentos del grupo
string TaxSignature(TicketItem item) => item.TaxObjectCode + ";" + string.Join(";", (item.ItemTaxes ?? new List<TicketItemTax>())
    .Select(tax => $"{tax.TaxCode}|{tax.TaxTypeCode}|{tax.TaxRate.ToString("0.000000", CultureInfo.InvariantCulture)}|{tax.TaxFlagCode}")
    .OrderBy(signature => signature, StringComparer.Ordinal));

var items = tickets.SelectMany(ticket => ticket.Items.GroupBy(TaxSignature).Select(group => new InvoiceItem
{
    TicketId = ticket.Id,
    ItemCode = "01010101",
    ItemSku = ticket.Folio, // NoIdentificacion: folio del ticket
    Quantity = 1,
    UnitOfMeasurementCode = "ACT",
    Description = "Venta",
    UnitPrice = group.Sum(item => Math.Round(item.Quantity * item.UnitPrice, 6, MidpointRounding.AwayFromZero)),
    Discount = group.Sum(item => item.Discount ?? 0m),
    TaxObjectCode = group.First().TaxObjectCode,
    ItemTaxes = (group.First().ItemTaxes ?? new List<TicketItemTax>())
        .Select(tax => new InvoiceItemTax
        {
            TaxCode = tax.TaxCode, TaxTypeCode = tax.TaxTypeCode, TaxRate = tax.TaxRate, TaxFlagCode = tax.TaxFlagCode
        })
        .ToList()
})).ToList();

var expeditionZipCode = tickets[0].ExpeditionZipCode;
var globalInvoice = new Invoice
{
    Series = "G",
    Date = DateTime.Now,
    PaymentFormCode = tickets.OrderByDescending(ticket => ticket.Total).First().PaymentFormCode, // la del mayor monto
    PaymentMethodCode = "PUE",
    CurrencyCode = "MXN",
    TypeCode = "I",
    ExpeditionZipCode = expeditionZipCode,
    Issuer = new InvoiceIssuer { Id = issuerId },
    Recipient = new InvoiceRecipient
    {
        Tin = "XAXX010101000",
        LegalName = "PUBLICO EN GENERAL",
        ZipCode = expeditionZipCode,
        TaxRegimeCode = "616",
        CfdiUseCode = "S01"
    },
    GlobalInformation = new GlobalInformation { PeriodicityCode = "04", MonthCode = "09", Year = 2026 },
    Items = items
};

var global = await fiscalApi.Invoices.CreateAsync(globalInvoice);
if (!global.Succeeded && global.HttpStatusCode == 409)
{
    // Algún ticket ya no estaba pendiente: no se cobró el timbre ni quedó tomado ningún ticket.
    foreach (var unavailable in global.ErrorData["unavailableTickets"])
        Console.WriteLine($"{unavailable["ticketId"]}: {unavailable["status"]}");
}
```

Un 400 en `items[i].ticketId` indica que los conceptos de ese ticket no lo reflejan (importes, descuentos o impuestos). Sin `TicketId` (null), el concepto se envía igual que antes: la propiedad no viaja en el JSON.

### 14. Recibir webhooks

FiscalAPI firma cada entrega con [Standard Webhooks](https://www.standardwebhooks.com) y el secreto `whsec_…` de su endpoint. `Webhook.Verify` comprueba la firma (en tiempo constante) y que `webhook-timestamp` no difiera más de 5 minutos del reloj del receptor, y devuelve el `WebhookEvent`. La firma es la única prueba de que la entrega viene de FiscalAPI.

**Verifique el cuerpo crudo**: léalo como texto antes de deserializarlo. Un cuerpo deserializado y vuelto a serializar (por ejemplo, con `[FromBody]`) cambia sus bytes y la firma deja de coincidir.

```csharp
using System.Text;
using System.Threading.Channels;
using Fiscalapi.Abstractions;
using Fiscalapi.Models;
using Fiscalapi.Webhooks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Program.cs:
//   builder.Services.AddFiscalApi();
//   builder.Services.AddSingleton(Channel.CreateUnbounded<WebhookEvent>());
//   builder.Services.AddHostedService<FiscalApiWebhookWorker>();

[ApiController]
public class FiscalApiWebhooksController : ControllerBase
{
    private readonly Channel<WebhookEvent> _events;
    private readonly string _webhookSecret; // whsec_… del endpoint, desde su configuración segura

    public FiscalApiWebhooksController(Channel<WebhookEvent> events, IConfiguration configuration)
    {
        _events = events;
        _webhookSecret = configuration["FiscalApi:WebhookSecret"];
    }

    [HttpPost("webhooks/fiscalapi")]
    public async Task<IActionResult> Receive()
    {
        string body;
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            body = await reader.ReadToEndAsync();

        WebhookEvent webhookEvent;
        try
        {
            var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
            webhookEvent = Webhook.Verify(headers, body, _webhookSecret);
        }
        catch (WebhookVerificationException)
        {
            return BadRequest(); // firma inválida, header faltante o timestamp fuera de tolerancia
        }

        // Responda 2xx rápido y procese después: FiscalAPI corta cada intento a los 15 s y lo reintenta.
        await _events.Writer.WriteAsync(webhookEvent);
        return Ok();
    }
}

// Procesa los eventos fuera de la petición. La cola en memoria es ilustrativa: en producción use una cola durable.
public class FiscalApiWebhookWorker : BackgroundService
{
    private readonly Channel<WebhookEvent> _events;
    private readonly IServiceScopeFactory _scopes;

    public FiscalApiWebhookWorker(Channel<WebhookEvent> events, IServiceScopeFactory scopes)
    {
        _events = events;
        _scopes = scopes;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var webhookEvent in _events.Reader.ReadAllAsync(stoppingToken))
        {
            // La entrega es al menos una vez: webhookEvent.Id (el header webhook-id) se repite en cada reintento;
            // descarte los ya procesados. El orden de llegada no está garantizado: el estado autoritativo se lee del recurso.
            using (var scope = _scopes.CreateScope())
            {
                var fiscalApi = scope.ServiceProvider.GetRequiredService<IFiscalApiClient>();
                switch (webhookEvent.Type)
                {
                    case WebhookEventTypes.InvoiceCanceled:
                        var invoice = await fiscalApi.Invoices.GetByIdAsync(webhookEvent.Data.RelatedObject.Id);
                        break;
                    case WebhookEventTypes.DownloadRequestCompleted:
                        var request = await fiscalApi.DownloadRequests.GetByIdAsync(webhookEvent.Data.RelatedObject.Id);
                        break;
                    default:
                        break; // un tipo que no conoce se ignora: el catálogo puede crecer
                }
            }
        }
    }
}
```

- `WebhookEvent` es el mismo sobre para todo evento: `Id`, `Type` (texto; compárelo con las constantes de `WebhookEventTypes`), `CreatedAt` (UTC), `PersonId` (null si el recurso no tiene persona) y `Data` con `RelatedObject` (`Id`, `Type`, `Url`), `Status` y `PreviousStatus`.
- `RelatedObject.Url` es la ruta relativa del GET v4 del recurso (`/api/v4/invoices/{id}`); para leerlo, use el servicio de su tipo con `RelatedObject.Id`.
- El botón "Enviar prueba" del webhook (`POST /api/v4/webhooks/{id}/test`) envía `WebhookEventTypes.WebhookTest` (`webhook.test`), cuyo `RelatedObject` es el webhook mismo (`Type` `webhook`, `Url` `/api/v4/webhooks/{id}`), sin persona ni estados; no se suscribe.
- Durante una rotación del secreto, FiscalAPI firma con el nuevo y con el anterior por 24 h: pase ambos, `Webhook.Verify(headers, body, new[] { secretoNuevo, secretoAnterior })`.
- La tolerancia es configurable (`Webhook.Verify(headers, body, secreto, TimeSpan.FromMinutes(2))`); el valor por omisión es `Webhook.DefaultTolerance`.
- Un secreto mal copiado (que no es base64 canónico: con un espacio o un salto de línea de más, o truncado) o una lista de secretos vacía lanza `ArgumentException` con la posición del secreto (nunca su valor): es un error de configuración, no de la entrega.

---

## ⏳ Operaciones Asíncronas y Sincrónicas

- **Asíncrono**:
    ```csharp
    var apiResponse = await fiscalApi.Invoices.GetByIdAsync(<id>);
    ```
- **Sincrónico** (use esto solo en .NET Framework 4.X.X)
    ```csharp
    var apiResponse = Task.Run(async () => await fiscalApi.Invoices.GetByIdAsync(<id>)).Result;
    ```

## 📋 Operaciones Principales

- **Facturas (CFDI)**  
  Crear facturas de ingreso, notas de crédito, complementos de pago, cancelaciones, generación de PDF/XML.
- **Personas (Clientes/Emisores)**  
  Alta y administración de personas, gestión de certificados (CSD).
- **Productos y Servicios**  
  Administración de catálogos de productos, búsqueda en catálogos SAT.
- **Validaciones SAT**  
  Validación de CFDI (estructura, certificado, sellos, estatus, listas negras 69-B / 69-B Bis) y consulta del catálogo de tipos y estatus.
- **Timbres, créditos de validación y créditos de ticket**  
  Transferencia de timbres, créditos de validación y créditos de ticket entre personas, consulta de transacciones y saldos.
- **Tickets (Auto CFDI)**  
  Ingesta de ventas pagadas del punto de venta con Idempotency-Key opcional, consulta con filtros, código para reimprimir y cancelación.
- **Webhooks**  
  Verificación de la firma Standard Webhooks de las entregas y lectura del evento (`Webhook.Verify`, `WebhookEvent`).


## 🤝 Contribuir

1. Haz un fork del repositorio.  
2. Crea una rama para tu feature: `git checkout -b feature/AmazingFeature`.  
3. Realiza commits de tus cambios: `git commit -m 'Add some AmazingFeature'`.  
4. Sube tu rama: `git push origin feature/AmazingFeature`.  
5. Abre un Pull Request en GitHub.


## 🐛 Reportar Problemas

1. Asegúrate de usar la última versión del SDK.  
2. Verifica si el problema ya fue reportado.  
3. Proporciona un ejemplo mínimo reproducible.  
4. Incluye los mensajes de error completos.


## 📄 Licencia

Este proyecto está licenciado bajo la Licencia **MPL**. Consulta el archivo [LICENSE](LICENSE.txt) para más detalles.


## 🔗 Enlaces Útiles

- [Documentación Oficial](https://docs.fiscalapi.com)  
- [Portal de FiscalAPI](https://fiscalapi.com)  
- [Ejemplos WinForms/WPF/Console](https://github.com/FiscalAPI/fiscalapi-samples-net-winforms)  
- [Ejemplos ASP.NET](https://github.com/FiscalAPI/fiscalapi-samples-net-aspnet)
- [Postman Collection](https://documenter.getpostman.com/view/4346593/2sB2j4eqXr)
- [SDKs](https://docs.fiscalapi.com/sdks)

---

Desarrollado con ❤️ por [Fiscalapi](https://www.fiscalapi.com)
