using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using Fiscalapi.Models;
using Fiscalapi.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fiscalapi.Tests;

/// <summary>
/// Consistencia entre SDK (SDK-059, DEC-136): los mismos escenarios viven en el SDK de Java y ya los cumplían Node.js,
/// Python y PHP:
/// 1. nómina: DaysPaid admite días fraccionarios (decimal) y viaja sin perder precisión;
/// 2. nómina por valores: Issuer.EmployerData.Curp viaja como issuer.employerData.curp;
/// 3. factura: Uuid (folio fiscal) en la raíz se lee de la respuesta;
/// 4. 400 de validación: Details une todas las fallas con "; ".
/// </summary>
public class PayrollAndInvoiceConsistencyTests
{
    private const string InvoiceId = "9c1d2e3f-0000-4000-8000-0000000000a1";
    private const string Uuid = "5F8C2D3E-1A2B-4C5D-8E9F-0A1B2C3D4E5F";

    private const string InvoiceResponse = """
        {"data":{"id":"9c1d2e3f-0000-4000-8000-0000000000a1","versionCode":"4.0","typeCode":"N",
        "uuid":"5F8C2D3E-1A2B-4C5D-8E9F-0A1B2C3D4E5F","consecutive":7,"total":1000.00,
        "complement":{"payroll":{"version":"1.2","payrollTypeCode":"O","daysPaid":15.5}}},
        "succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private const string TwoFailuresResponse = """
        {"data":[{"propertyName":"Items[0].Discount","errorMessage":"'discount' admite como máximo 6 decimales (se recibieron 17): redondéalo antes de enviarlo; FiscalAPI no lo redondea.","attemptedValue":0.30000000000000004},
        {"propertyName":"Complement.Payroll.DaysPaid","errorMessage":"'daysPaid' admite como máximo 3 decimales (se recibieron 4): redondéalo antes de enviarlo; FiscalAPI no lo redondea.","attemptedValue":15.1234}],
        "succeeded":false,"message":"Sorry, it's not me, it's you.","details":"One or more validation failures have occurred.","httpStatusCode":400}
        """;

    private readonly FakeApi _api = new();

    private InvoiceService Invoices => new(_api.HttpClient, FakeApi.ApiVersion);

    private static Invoice PayrollInvoice(decimal daysPaid) => new()
    {
        TypeCode = "N",
        Issuer = new InvoiceIssuer
        {
            Tin = "EKU9003173C9",
            EmployerData = new EmployerData { Curp = "XEXX010101HNEXXXA4", EmployerRegistration = "B5510768108" },
        },
        Complement = new Complement
        {
            Payroll = new Payroll { Version = "1.2", PayrollTypeCode = "O", DaysPaid = daysPaid },
        },
    };

    // Los números del cuerpo se leen como decimal para comparar sin pasar por double.
    private static JObject SentBody(RecordedRequest request) =>
        JObject.Load(new JsonTextReader(new StringReader(request.Body!)) { FloatParseHandling = FloatParseHandling.Decimal });

    // 1. DaysPaid decimal

    [Theory]
    [InlineData("15.5", "15.5")]
    [InlineData("15.125", "15.125")]
    [InlineData("30", "30.0")]
    public async Task Payroll_DaysPaid_IsSentAsTheExactDecimal(string daysPaid, string expectedJson)
    {
        _api.RespondWith(InvoiceResponse);
        var value = decimal.Parse(daysPaid, CultureInfo.InvariantCulture);

        await Invoices.CreateAsync(PayrollInvoice(value));

        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, "api/v4/invoices"), request.Uri);
        Assert.Matches($"\"daysPaid\":{Regex.Escape(expectedJson)}[,}}]", request.Body);
        Assert.Equal(value, SentBody(request).SelectToken("complement.payroll.daysPaid")!.Value<decimal>());
    }

    [Fact]
    public void Payroll_DaysPaid_IsDecimal()
    {
        Assert.Equal(typeof(decimal), typeof(Payroll).GetProperty(nameof(Payroll.DaysPaid))!.PropertyType);
    }

    [Fact]
    public async Task Payroll_DaysPaid_IsReadFromTheResponse()
    {
        _api.RespondWith(InvoiceResponse);

        var response = await Invoices.GetByIdAsync(InvoiceId);

        Assert.Equal(15.5m, response.Data.Complement.Payroll.DaysPaid);
    }

    // 2. Curp del empleador del emisor

    [Fact]
    public async Task IssuerEmployerData_Curp_IsSentAsCurp()
    {
        _api.RespondWith(InvoiceResponse);

        await Invoices.CreateAsync(PayrollInvoice(15m));

        var employerData = (JObject)SentBody(Assert.Single(_api.Requests)).SelectToken("issuer.employerData")!;
        Assert.Equal("XEXX010101HNEXXXA4", employerData.Value<string>("curp"));
        Assert.Equal("B5510768108", employerData.Value<string>("employerRegistration"));
    }

    [Fact]
    public async Task IssuerEmployerData_WithoutCurp_DoesNotSendIt()
    {
        _api.RespondWith(InvoiceResponse);
        var invoice = PayrollInvoice(15m);
        invoice.Issuer.EmployerData.Curp = null;

        await Invoices.CreateAsync(invoice);

        var employerData = (JObject)SentBody(Assert.Single(_api.Requests)).SelectToken("issuer.employerData")!;
        Assert.False(employerData.ContainsKey("curp"));
    }

    // 3. Uuid en la raíz de la factura

    [Fact]
    public async Task Invoice_Uuid_IsReadFromTheCreateResponse()
    {
        _api.RespondWith(InvoiceResponse);

        var response = await Invoices.CreateAsync(PayrollInvoice(15.5m));

        Assert.True(response.Succeeded);
        Assert.Equal(Uuid, response.Data.Uuid);
    }

    [Fact]
    public async Task Invoice_Uuid_IsReadFromGetById()
    {
        _api.RespondWith(InvoiceResponse);

        var response = await Invoices.GetByIdAsync(InvoiceId);

        Assert.Equal(new Uri(FakeApi.BaseAddress, $"api/v4/invoices/{InvoiceId}?details=false"), Assert.Single(_api.Requests).Uri);
        Assert.Equal(Uuid, response.Data.Uuid);
    }

    // 4. 400 de validación con varias fallas

    [Fact]
    public async Task ValidationFailure_Details_JoinsEveryFailureWithSemicolon()
    {
        _api.RespondWith(TwoFailuresResponse, HttpStatusCode.BadRequest);

        var response = await Invoices.CreateAsync(PayrollInvoice(15.1234m));

        Assert.False(response.Succeeded);
        Assert.Equal(400, response.HttpStatusCode);
        Assert.Equal("Sorry, it's not me, it's you.", response.Message);
        Assert.Equal(
            "Items[0].Discount: 'discount' admite como máximo 6 decimales (se recibieron 17): redondéalo antes de enviarlo; FiscalAPI no lo redondea.; " +
            "Complement.Payroll.DaysPaid: 'daysPaid' admite como máximo 3 decimales (se recibieron 4): redondéalo antes de enviarlo; FiscalAPI no lo redondea.",
            response.Details);
        Assert.Null(response.Data);
    }
}
