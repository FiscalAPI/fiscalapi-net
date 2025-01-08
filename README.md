# FiscalAPI SDK para .NET

[![NuGet](https://img.shields.io/nuget/v/FiscalApi.svg)](https://www.nuget.org/packages/FiscalApi/)
[![License](https://img.shields.io/github/license/FiscalAPI/fiscalapi-net)](https://github.com/FiscalAPI/fiscalapi-net/blob/main/LICENSE)

SDK oficial de FiscalAPI para .NET, la API de facturación CFDI y otros servicios fiscales en México. Simplifica la integración con los servicios de facturación electrónica, eliminando las complejidades del de la autoridad tributaria (SAT) y facilitando la generación de facturas, notas de crédito y complementos de pago, etc.

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
Install-Package FiscalApi
```

O vía .NET CLI:

```bash
dotnet add package FiscalApi
```

## ⚙️ Configuración

Configura el cliente con tus credenciales:

```csharp
var settings = new FiscalApiOptions
{
    ApiUrl = "https://test.fiscalapi.com", // Usa https://fiscalapi.com para producción
    ApiKey = "<tu_api_key>",
    ApiVersion = "v4",
    Tenant = "<tenant>",
    TimeZone = "America/Mexico_City"
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
var invoice = new Invoice
{
    VersionCode = "4.0",
    Series = "A",
    Date = DateTime.Now,
    PaymentFormCode = "01",
    CurrencyCode = "MXN",
    TypeCode = "I",
    ExpeditionZipCode = "42501",
    Issuer = new InvoiceIssuer { Id = "id-del-emisor" },
    Recipient = new InvoiceRecipient { Id = "id-del-receptor" },
    Items = new List<InvoiceItem>
    {
        new InvoiceItem { Id = "id-del-producto", Quantity = 1 }
    },
    PaymentMethodCode = "PUE",
};

var response = await fiscalApi.Invoices.CreateAsync(invoice);
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

Este proyecto está licenciado bajo la Licencia MPL. Consulta el archivo [LICENSE](LICENSE) para más detalles.

## 🔗 Enlaces Úúles

- [Documentación Oficial](https://docs.fiscalapi.com)
- [Portal de FiscalAPI](https://fiscalapi.com)
- [Ejemplos](https://github.com/FiscalAPI/fiscalapi-samples-net-winforms)

