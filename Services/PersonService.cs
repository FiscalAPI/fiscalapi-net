using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
using Fiscalapi.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
    public class PersonService : BaseFiscalApiService<Person>, IPersonService
    {
        public EmployeerService  Employeer { get;}
        public PersonService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "people", apiVersion)
        {
            Employeer = new EmployeerService(httpClient);
        }

        // GET /api/v4/people/{personId}/employer
        //var employer = await fiscalApi.People.Employer(personId).GetAsync();
    

        //POST /api/v4/people/{personId}/employer
        //var employer = await fiscalApi.People.Employer(personId).CreateAsync(employerData);

        // PUT /api/v4/people/{personId}/employer
        //var employer = await fiscalApi.People.Employer(personId).UpdateAsync(employerData);

        // DELETE /api/v4/people/{personId}/employer
        //await fiscalApi.People.Employer(personId).DeleteAsync();


    }




    public class EmployeerData : BaseDto
    {
        public string personId { get; set; }
        public string employerRegistration { get; set; }
        public string originEmployerTin { get; set; }
        public CatalogDto satFundSource { get; set; } // Rename to SatFundingSource
        public decimal ownResourceAmount { get; set; }
     
    }

  


}

public class EmployeerService {

    public IFiscalApiHttpClient HttpClient { get; }
    public EmployeerService(IFiscalApiHttpClient fiscalApiHttpClient)
    {
        HttpClient = fiscalApiHttpClient;
    }

   


    // GET /api/v4/people/{personId}/employer
    public async  Task<ApiResponse<EmployeerData>> GetByIdAsync(string id) { 
        var apiResponse = await HttpClient.GetAsync<EmployeerData>($"api/v4/people/{id}/employer");
        return apiResponse;
    }




    //POST /api/v4/people/{personId}/employer
    //var employer = await fiscalApi.People.Employer(personId).CreateAsync(employerData);

    // PUT /api/v4/people/{personId}/employer
    //var employer = await fiscalApi.People.Employer(personId).UpdateAsync(employerData);

    // DELETE /api/v4/people/{personId}/employer
    //await fiscalApi.People.Employer(personId).DeleteAsync();
}