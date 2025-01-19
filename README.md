# FiscalAPI SDK para .NET

[![NuGet](https://img.shields.io/nuget/v/FiscalApi.svg)](https://www.nuget.org/packages/FiscalApi/)
[![License](https://img.shields.io/github/license/FiscalAPI/fiscalapi-net)](https://github.com/FiscalAPI/fiscalapi-net/blob/main/LICENSE)

SDK oficial de FiscalAPI para .NET, la API de facturación CFDI y otros servicios fiscales en México. Simplifica la integración con los servicios de facturación electrónica, eliminando las complejidades del de la autoridad tributaria (SAT) y facilitando la generación de facturas, notas de crédito y complementos de pago, nomina, carta portte, etc. Ahora es posible facturar sin dolor!! 

## 🚀 Características

- Soporte completo para CFDI 4.0
- Compatible con múltiples versiones de .NET (desde .NET Framework 4.6.1 hasta .NET 8)
- Operaciones asíncronas y sincrónicas
- Flexibilidad en modos de operación: por valores o referencias
- Manejo simplificado de errores
- Busqueda en todos los catálogo  del SAT.
- Documentación completa y ejemplos prácticos

## 📦 Instalación

Instala el paquete FiscalAPI vía NuGet Package Manager:

```bash
NuGet\Install-Package Fiscalapi
```

O vía .NET CLI:

```bash
dotnet add package Fiscalapi
```

## ⚙️ Configuración

Configura el cliente con [tus credenciales](https://docs.fiscalapi.com/credentials-info):

```csharp
var settings = new FiscalApiOptions
{
    ApiUrl = "https://test.fiscalapi.com", // Usa https://fiscalapi.com para producción
    ApiKey = "<tu_api_key>",
    Tenant = "<tenant>",
};

var fiscalApi = FiscalApiClient.Create(settings);
```

## 🔄 Modos de Operación

FiscalAPI soporta dos modos de operación:

### Por Referencias
- Envía solo los IDs de objetos previamente creados en el dashboard.
- Ideal para integraciones rápidas y ligeras.

### Por Valores
- Envía todos los campos requeridos en cada petición.
- Proporciona mayor control sobre los datos.
- No requiere configuración previa en el dashboard.

## 📝 Ejemplos de Uso

### Crear una Factura de Ingreso (Por Referencias)

```csharp
// Crear instancia de FiscalApiClient
var fiscalApi = FiscalApiClient.Create(Settings);

// Emisor KARLA FUENTE NOLASCO
var issuer = new InvoiceIssuer
{
    Id = "<id-emisor-en-fiscalapi>"
};

// Receptor ESCUELA KEMPER URGATE
var recipient = new InvoiceRecipient
{
    Id = "<id-receptor-en-fiscalapi>"
};

// Crear una lista de productos o servicios de la factura
var items = new List<InvoiceItem>()
{
    new InvoiceItem
    {
        Id = "<id-producto-en-fiscalapi>",
        Quantity = 1,
        Discount = 10.85m,
    },
};

// Crear la factura 
var invoice = new Invoice
{
    VersionCode = "4.0",
    Series = "SDK-F",
    Date = DateTime.Now,
    PaymentFormCode = "01",
    CurrencyCode = "MXN",
    TypeCode = "I",
    ExpeditionZipCode = "42501",
    Issuer = issuer,
    Recipient = recipient,
    Items = items,
    PaymentMethodCode = "PUE",
};

// Timbrar la factura
var apiResponse = await fiscalApi.Invoices.CreateAsync(invoice);
```

### Crear la misma factura de ingreso (Por valores)
```csharp
 // Crear fiscalapi client
var fiscalApi = FiscalApiClient.Create(settings);

// Crear certificados de prueba (EKU9003173C9)
var sellos = new List<TaxCredential>()
{
    new TaxCredential
    {
        Base64File = "MIIFsDCCA5igAwIBAgIUMzAwMDEwMDAwMDA1MDAwMDM0MTYwDQYJKoZIhvcNAQELBQAwggErMQ8wDQYDVQQDDAZBQyBVQVQxLjAsBgNVBAoMJVNFUlZJQ0lPIERFIEFETUlOSVNUUkFDSU9OIFRSSUJVVEFSSUExGjAYBgNVBAsMEVNBVC1JRVMgQXV0aG9yaXR5MSgwJgYJKoZIhvcNAQkBFhlvc2Nhci5tYXJ0aW5lekBzYXQuZ29iLm14MR0wGwYDVQQJDBQzcmEgY2VycmFkYSBkZSBjYWxpejEOMAwGA1UEEQwFMDYzNzAxCzAJBgNVBAYTAk1YMRkwFwYDVQQIDBBDSVVEQUQgREUgTUVYSUNPMREwDwYDVQQHDAhDT1lPQUNBTjERMA8GA1UELRMIMi41LjQuNDUxJTAjBgkqhkiG9w0BCQITFnJlc3BvbnNhYmxlOiBBQ0RNQS1TQVQwHhcNMjMwNTE4MTE0MzUxWhcNMjcwNTE4MTE0MzUxWjCB1zEnMCUGA1UEAxMeRVNDVUVMQSBLRU1QRVIgVVJHQVRFIFNBIERFIENWMScwJQYDVQQpEx5FU0NVRUxBIEtFTVBFUiBVUkdBVEUgU0EgREUgQ1YxJzAlBgNVBAoTHkVTQ1VFTEEgS0VNUEVSIFVSR0FURSBTQSBERSBDVjElMCMGA1UELRMcRUtVOTAwMzE3M0M5IC8gVkFEQTgwMDkyN0RKMzEeMBwGA1UEBRMVIC8gVkFEQTgwMDkyN0hTUlNSTDA1MRMwEQYDVQQLEwpTdWN1cnNhbCAxMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAtmecO6n2GS0zL025gbHGQVxznPDICoXzR2uUngz4DqxVUC/w9cE6FxSiXm2ap8Gcjg7wmcZfm85EBaxCx/0J2u5CqnhzIoGCdhBPuhWQnIh5TLgj/X6uNquwZkKChbNe9aeFirU/JbyN7Egia9oKH9KZUsodiM/pWAH00PCtoKJ9OBcSHMq8Rqa3KKoBcfkg1ZrgueffwRLws9yOcRWLb02sDOPzGIm/jEFicVYt2Hw1qdRE5xmTZ7AGG0UHs+unkGjpCVeJ+BEBn0JPLWVvDKHZAQMj6s5Bku35+d/MyATkpOPsGT/VTnsouxekDfikJD1f7A1ZpJbqDpkJnss3vQIDAQABox0wGzAMBgNVHRMBAf8EAjAAMAsGA1UdDwQEAwIGwDANBgkqhkiG9w0BAQsFAAOCAgEAFaUgj5PqgvJigNMgtrdXZnbPfVBbukAbW4OGnUhNrA7SRAAfv2BSGk16PI0nBOr7qF2mItmBnjgEwk+DTv8Zr7w5qp7vleC6dIsZFNJoa6ZndrE/f7KO1CYruLXr5gwEkIyGfJ9NwyIagvHHMszzyHiSZIA850fWtbqtythpAliJ2jF35M5pNS+YTkRB+T6L/c6m00ymN3q9lT1rB03YywxrLreRSFZOSrbwWfg34EJbHfbFXpCSVYdJRfiVdvHnewN0r5fUlPtR9stQHyuqewzdkyb5jTTw02D2cUfL57vlPStBj7SEi3uOWvLrsiDnnCIxRMYJ2UA2ktDKHk+zWnsDmaeleSzonv2CHW42yXYPCvWi88oE1DJNYLNkIjua7MxAnkNZbScNw01A6zbLsZ3y8G6eEYnxSTRfwjd8EP4kdiHNJftm7Z4iRU7HOVh79/lRWB+gd171s3d/mI9kte3MRy6V8MMEMCAnMboGpaooYwgAmwclI2XZCczNWXfhaWe0ZS5PmytD/GDpXzkX0oEgY9K/uYo5V77NdZbGAjmyi8cE2B2ogvyaN2XfIInrZPgEffJ4AB7kFA2mwesdLOCh0BLD9itmCve3A1FGR4+stO2ANUoiI3w3Tv2yQSg4bjeDlJ08lXaaFCLW2peEXMXjQUk7fmpb5MNuOUTW6BE=",
        FileType = FileType.CertificateCsd,
        Password = "12345678a"
    },
    new TaxCredential
    {
        Base64File = "MIIFDjBABgkqhkiG9w0BBQ0wMzAbBgkqhkiG9w0BBQwwDgQIAgEAAoIBAQACAggAMBQGCCqGSIb3DQMHBAgwggS/AgEAMASCBMh4EHl7aNSCaMDA1VlRoXCZ5UUmqErAbucoZQObOaLUEm+I+QZ7Y8Giupo+F1XWkLvAsdk/uZlJcTfKLJyJbJwsQYbSpLOCLataZ4O5MVnnmMbfG//NKJn9kSMvJQZhSwAwoGLYDm1ESGezrvZabgFJnoQv8Si1nAhVGTk9FkFBesxRzq07dmZYwFCnFSX4xt2fDHs1PMpQbeq83aL/PzLCce3kxbYSB5kQlzGtUYayiYXcu0cVRu228VwBLCD+2wTDDoCmRXtPesgrLKUR4WWWb5N2AqAU1mNDC+UEYsENAerOFXWnmwrcTAu5qyZ7GsBMTpipW4Dbou2yqQ0lpA/aB06n1kz1aL6mNqGPaJ+OqoFuc8Ugdhadd+MmjHfFzoI20SZ3b2geCsUMNCsAd6oXMsZdWm8lzjqCGWHFeol0ik/xHMQvuQkkeCsQ28PBxdnUgf7ZGer+TN+2ZLd2kvTBOk6pIVgy5yC6cZ+o1Tloql9hYGa6rT3xcMbXlW+9e5jM2MWXZliVW3ZhaPjptJFDbIfWxJPjz4QvKyJk0zok4muv13Iiwj2bCyefUTRz6psqI4cGaYm9JpscKO2RCJN8UluYGbbWmYQU+Int6LtZj/lv8p6xnVjWxYI+rBPdtkpfFYRp+MJiXjgPw5B6UGuoruv7+vHjOLHOotRo+RdjZt7NqL9dAJnl1Qb2jfW6+d7NYQSI/bAwxO0sk4taQIT6Gsu/8kfZOPC2xk9rphGqCSS/4q3Os0MMjA1bcJLyoWLp13pqhK6bmiiHw0BBXH4fbEp4xjSbpPx4tHXzbdn8oDsHKZkWh3pPC2J/nVl0k/yF1KDVowVtMDXE47k6TGVcBoqe8PDXCG9+vjRpzIidqNo5qebaUZu6riWMWzldz8x3Z/jLWXuDiM7/Yscn0Z2GIlfoeyz+GwP2eTdOw9EUedHjEQuJY32bq8LICimJ4Ht+zMJKUyhwVQyAER8byzQBwTYmYP5U0wdsyIFitphw+/IH8+v08Ia1iBLPQAeAvRfTTIFLCs8foyUrj5Zv2B/wTYIZy6ioUM+qADeXyo45uBLLqkN90Rf6kiTqDld78NxwsfyR5MxtJLVDFkmf2IMMJHTqSfhbi+7QJaC11OOUJTD0v9wo0X/oO5GvZhe0ZaGHnm9zqTopALuFEAxcaQlc4R81wjC4wrIrqWnbcl2dxiBtD73KW+wcC9ymsLf4I8BEmiN25lx/OUc1IHNyXZJYSFkEfaxCEZWKcnbiyf5sqFSSlEqZLc4lUPJFAoP6s1FHVcyO0odWqdadhRZLZC9RCzQgPlMRtji/OXy5phh7diOBZv5UYp5nb+MZ2NAB/eFXm2JLguxjvEstuvTDmZDUb6Uqv++RdhO5gvKf/AcwU38ifaHQ9uvRuDocYwVxZS2nr9rOwZ8nAh+P2o4e0tEXjxFKQGhxXYkn75H3hhfnFYjik/2qunHBBZfcdG148MaNP6DjX33M238T9Zw/GyGx00JMogr2pdP4JAErv9a5yt4YR41KGf8guSOUbOXVARw6+ybh7+meb7w4BeTlj3aZkv8tVGdfIt3lrwVnlbzhLjeQY6PplKp3/a5Kr5yM0T4wJoKQQ6v3vSNmrhpbuAtKxpMILe8CQoo=",
        FileType = FileType.PrivateKeyCsd,
        Password = "12345678a"
    }
};

// Emisor
var issuer = new InvoiceIssuer
{
    Tin = "EKU9003173C9",
    LegalName = "ESCUELA KEMPER URGATE",
    TaxRegimeCode = "601",
    TaxCredentials = sellos
};

// Receptor
var recipient = new InvoiceRecipient
{
    Tin = "EKU9003173C9",
    LegalName = "ESCUELA KEMPER URGATE",
    ZipCode = "42501",
    TaxRegimeCode = "601",
    CfdiUseCode = "G01",
    Email = "someone@somewhere.com"
};

// Crear una lista de productos o servicios de la factura
var items = new List<InvoiceItem>()
{
    new InvoiceItem
    {
        ItemCode = "01010101",
        Quantity = 9.5m,
        UnitOfMeasurementCode = "E48",
        UnitOfMeasurement = "Unidad de servicio",
        Description = "Invoicing software as a service",
        UnitPrice = 3587.75m,
        TaxObjectCode = "02",
        ItemSku = "7506022301697",
        Discount = 255.85m,
        ItemTaxes = new List<InvoiceItemTax>()
        {
            new InvoiceItemTax
            {
                TaxCode = "002", // IVA
                TaxTypeCode = "Tasa", // Tasa
                TaxRate = 0.160000m, // 16%
                TaxFlagCode = "T" // Traslado
            }
        }
    }
};

// Crear la factura 
var invoice = new Invoice
{
    VersionCode = "4.0",
    Series = "SDK-F",
    Date = DateTime.Now,
    PaymentFormCode = "01",
    CurrencyCode = "MXN",
    TypeCode = "I",
    ExpeditionZipCode = "42501",
    Issuer = issuer,
    Recipient = recipient,
    Items = items,
    PaymentMethodCode = "PUE",
};

// Timbrar la factura
var apiResponse = await fiscalApi.Invoices.CreateAsync(invoice);   
```




### Crear una Nota de Crédito (Por Valores)

```csharp
var creditNote = new Invoice
{
    VersionCode = "4.0",
    Series = "NC",
    TypeCode = "E", // E para egreso (nota de crédito)
    ExpeditionZipCode = "42501",
    RelatedInvoices = new List<RelatedInvoice>
    {
        new RelatedInvoice
        {
            Uuid = "UUID-de-factura-relacionada",
            RelationshipTypeCode = "01"
        }
    }
};

var response = await fiscalApi.Invoices.CreateAsync(creditNote);
```

### Buscar en Catálogos SAT

```csharp
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

## ⏳ Operaciones Asíncronas y Sincrónicas

### Asíncrono

```csharp
var apiResponse = await fiscalApi.Catalogs.SearchCatalogAsync("SatUnitMeasurements", "inter", 1, 10);
```

### Sincrónico

```csharp
var apiResponse = Task.Run(async () => await fiscalApi.Catalogs.SearchCatalogAsync("SatUnitMeasurements", "inter", 1, 10)).Result;
```

## 📋 Operaciones Principales

### Facturas (CFDI)
- Crear facturas de ingreso, notas de crédito y complementos de pago.
- Cancelación de facturas.
- Generación de PDF/XML.

### Personas (Clientes/Emisores)
- Alta y administración de personas.
- Gestión de certificados (CSD).

### Productos y Servicios
- Administración de catálogos de productos.
- Búsqueda en catálogos SAT.

## 🤝 Contribuir

1. Haz un fork del repositorio.
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`).
3. Realiza commits de tus cambios (`git commit -m 'Add some AmazingFeature'`).
4. Haz push a la rama (`git push origin feature/AmazingFeature`).
5. Abre un Pull Request.

## 🐛 Reportar Problemas

1. Usa la última versión del SDK.
2. Verifica si el problema ya ha sido reportado.
3. Proporciona un ejemplo mínimo reproducible.
4. Incluye los mensajes de error completos.

## 📄 Licencia

Este proyecto está licenciado bajo la Licencia MPL. Consulta el archivo [LICENSE](LICENSE.txt) para más detalles.

## 🔗 Enlaces Útiles

- [Documentación Oficial](https://docs.fiscalapi.com)
- [Portal de FiscalAPI](https://fiscalapi.com)
- [Ejemplos Winforms/Console](https://github.com/FiscalAPI/fiscalapi-samples-net-winforms)
- [Ejemplos ASP.NET](https://github.com/FiscalAPI/fiscalapi-samples-net-aspnet)

