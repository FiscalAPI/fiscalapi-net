# FiscalAPI SDK para .NET
[![.NET](https://img.shields.io/badge/.NET-6.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/6.0)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![NuGet](https://img.shields.io/nuget/v/FiscalApi.svg)](https://www.nuget.org/packages/FiscalApi/)
[![License](https://img.shields.io/github/license/FiscalAPI/fiscalapi-net)](https://github.com/FiscalAPI/fiscalapi-net/blob/master/LICENSE.txt) 

**SDK oficial de FiscalAPI para .NET**, la API de facturación CFDI y otros servicios fiscales en México. Simplifica la integración con los servicios de facturación electrónica, eliminando las complejidades del SAT y facilitando la generación de facturas, notas de crédito, complementos de pago, nómina, carta porte, y más. ¡Factura sin dolor!



## 📋 Facturación CFDI 4.0
- **Soporte completo para CFDI 4.0** con todas las especificaciones oficiales
- **Timbrado de facturas de ingreso** con validación automática
- **Timbrado de notas de crédito** (facturas de egreso)
- **Timbrado de complementos de pago** en MXN, USD y EUR.
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
    var settings = new FiscalApiOptions
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
    Tin = "EKU9003173C9"
};

var clavePrivadaCsd = new TaxFile
{
    PersonId = "984708c4-fcc0-43bd-9d30-ec017815c20e",
    Base64File = "MIIFDjBABgkqhkiG9w0BBQ0...==", // Llave privada .key codificada en Base64
    FileType = FileType.PrivateKeyCsd,
    Password = "12345678a",
    Tin = "EKU9003173C9"
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


### 7. Búsqueda en Catálogos del SAT

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
