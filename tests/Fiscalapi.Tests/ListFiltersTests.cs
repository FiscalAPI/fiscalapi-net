using Fiscalapi.Models;
using Fiscalapi.Services;

namespace Fiscalapi.Tests;

/// <summary>
/// Filtros de las listas de facturas, personas y productos (GET /api/v4/invoices, /people y /products): cada filtro
/// asignado viaja en camelCase y escapado, uno en null no se envía, y sin filtros la query queda vacía. La sobrecarga
/// GetListAsync(pageNumber, pageSize) sigue igual.
/// </summary>
public class ListFiltersTests
{
    private const string EmptyPage = """
        {"data":{"items":[],"pageNumber":1,"totalPages":0,"totalCount":0,"hasPreviousPage":false,"hasNextPage":false},
        "succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private readonly FakeApi _api = new();

    public ListFiltersTests() => _api.RespondWith(EmptyPage);

    private InvoiceService Invoices => new(_api.HttpClient, FakeApi.ApiVersion);
    private PersonService Persons => new(_api.HttpClient, FakeApi.ApiVersion);
    private ProductService Products => new(_api.HttpClient, FakeApi.ApiVersion);

    private RecordedRequest SingleRequest() => Assert.Single(_api.Requests);

    [Fact]
    public async Task Invoices_SendsEveryFilter_InTheContractOrder()
    {
        await Invoices.GetListAsync(new InvoiceListFilters
        {
            From = new DateTime(2026, 9, 1),
            To = new DateTime(2026, 9, 30, 18, 45, 0),
            IssuerTin = "EKU9003173C9",
            RecipientTin = "FUNK671228PH6",
            TypeCode = "I",
            StatusId = "V",
            Series = "SDK-F",
            Number = "1001",
            Uuid = "5f2c7a1e-0000-4000-8000-0000000000c3",
            Order = InvoiceListOrder.Desc,
            PageNumber = 2,
            PageSize = 50
        });

        var request = SingleRequest();
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/v4/invoices", request.Uri.AbsolutePath);
        Assert.Equal(
            "?from=2026-09-01&to=2026-09-30&issuerTin=EKU9003173C9&recipientTin=FUNK671228PH6&typeCode=I&statusId=V" +
            "&series=SDK-F&number=1001&uuid=5f2c7a1e-0000-4000-8000-0000000000c3&order=desc&pageNumber=2&pageSize=50",
            request.Uri.Query);
        Assert.Null(request.Body);
    }

    [Fact]
    public async Task Invoices_SendsOnlyTheAssignedFilters()
    {
        await Invoices.GetListAsync(new InvoiceListFilters { StatusId = "X", Order = InvoiceListOrder.Asc });

        Assert.Equal("?statusId=X&order=asc", SingleRequest().Uri.Query);
    }

    [Fact]
    public async Task Invoices_WithoutFilters_SendsNoQuery()
    {
        await Invoices.GetListAsync(new InvoiceListFilters());
        await Invoices.GetListAsync((InvoiceListFilters)null!);

        Assert.All(_api.Requests, request =>
        {
            Assert.Equal("/api/v4/invoices", request.Uri.AbsolutePath);
            Assert.Equal("", request.Uri.Query);
        });
    }

    [Fact]
    public async Task Invoices_PageOverload_IsUnchanged()
    {
        await Invoices.GetListAsync(1, 10);

        Assert.Equal("?PageNumber=1&PageSize=10", SingleRequest().Uri.Query);
    }

    [Fact]
    public async Task Persons_SendsSearchUserTypeAndPage_Escaped()
    {
        await Persons.GetListAsync(new PersonListFilters
        {
            Search = "Ana María & Co",
            UserTypeId = "C",
            PageNumber = 1,
            PageSize = 20
        });

        var request = SingleRequest();
        Assert.Equal("/api/v4/people", request.Uri.AbsolutePath);
        Assert.Equal("?search=Ana%20Mar%C3%ADa%20%26%20Co&userTypeId=C&pageNumber=1&pageSize=20", request.Uri.Query);
    }

    [Fact]
    public async Task Persons_SendsOnlyTheAssignedFilters()
    {
        await Persons.GetListAsync(new PersonListFilters { UserTypeId = "U" });

        Assert.Equal("?userTypeId=U", SingleRequest().Uri.Query);
    }

    [Fact]
    public async Task Persons_WithoutFilters_SendsNoQuery()
    {
        await Persons.GetListAsync((PersonListFilters)null!);

        var request = SingleRequest();
        Assert.Equal("/api/v4/people", request.Uri.AbsolutePath);
        Assert.Equal("", request.Uri.Query);
    }

    [Fact]
    public async Task Products_SendsSearchAndPage()
    {
        await Products.GetListAsync(new ProductListFilters { Search = "01010101", PageNumber = 3, PageSize = 5 });

        var request = SingleRequest();
        Assert.Equal("/api/v4/products", request.Uri.AbsolutePath);
        Assert.Equal("?search=01010101&pageNumber=3&pageSize=5", request.Uri.Query);
    }

    [Fact]
    public async Task Products_WithoutFilters_SendsNoQuery()
    {
        await Products.GetListAsync(new ProductListFilters());

        Assert.Equal("", SingleRequest().Uri.Query);
    }
}
