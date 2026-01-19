using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
using System;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
    public class PersonService : BaseFiscalApiService<Person>, IPersonService
    {
        public EmployerService Employer { get; }
        public EmployeeService Employee { get; }

        public PersonService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "people", apiVersion)
        {
            Employer = new EmployerService(httpClient);
            Employee = new EmployeeService(httpClient);
        }
    }


    public class EmployeeData : BaseDto
    {
        public string Curp {  get; set; }
        public string EmployerPersonId { get; set; }
        public string EmployeePersonId { get; set; }
        public string EmployeeNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string LaborRelationStartDate { get; set; }
        public CatalogDto SatContractType { get; set; }
        public CatalogDto SatTaxRegimeType { get; set; }
        public CatalogDto SatWorkdayType { get; set; }
        public CatalogDto SatJobRisk { get; set; }
        public CatalogDto SatPaymentPeriodicity { get; set; }
        public CatalogDto SatBank { get; set; }
        public CatalogDto SatPayrollState { get; set; }
        public CatalogDto SatUnionizedStatus { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Seniority { get; set; }
        public string SatUnionizedStatusId { get; set; }
        public string SatContractTypeId { get; set; }
        public string SatWorkdayTypeId { get; set; }
        public string SatTaxRegimeTypeId { get; set; }
        public string SatJobRiskId { get; set; }
        public string SatPaymentPeriodicityId { get; set; }
        public string SatBankId { get; set; }
        public string SatPayrollStateId { get; set; }
        public string BankAccount { get; set; }
        public decimal BaseSalaryForContributions { get; set; }
        public decimal IntegratedDailySalary { get; set; }
        public string SubcontractorRfc { get; set; }
        public decimal TimePercentage { get; set; }
    }

    public class EmployerData : BaseDto
    {
        public string PersonId { get; set; }
        public string EmployerRegistration { get; set; }
        public string OriginEmployerTin { get; set; }
        public CatalogDto SatFundSource { get; set; } // Rename to SatFundingSource
        public string SatFundSourceId { get; set; }
        public decimal OwnResourceAmount { get; set; }
    }

    public class EmployerService
    {
        private IFiscalApiHttpClient HttpClient { get; }
        private string baseEndpoint = "api/v4/people";

        public EmployerService(IFiscalApiHttpClient fiscalApiHttpClient)
        {
            HttpClient = fiscalApiHttpClient;
        }

        // GET /api/v4/people/{personId}/employer
        public async Task<ApiResponse<EmployerData>> GetByIdAsync(string id)
        {
            string endpoint = $"{baseEndpoint}/{id}/employer";

            return await HttpClient.GetAsync<EmployerData>(endpoint);
        }

        //POST /api/v4/people/{personId}/employer
        //var employer = await fiscalApi.People.Employer(personId).CreateAsync(employerData);
        public async Task<ApiResponse<EmployerData>> CreateAsync(EmployerData requestModel)
        {
            string endpoint = $"{baseEndpoint}/{requestModel.PersonId}/employer";

            return await HttpClient.PostAsync<EmployerData>(endpoint, requestModel);
        }

        // PUT /api/v4/people/{personId}/employer
        //var employer = await fiscalApi.People.Employer(personId).UpdateAsync(employerData);
        public async Task<ApiResponse<EmployerData>> UpdateAsync(EmployerData requestModel)
        {
            string endpoint = $"{baseEndpoint}/{requestModel.PersonId}/employer";

            return await HttpClient.PutAsync<EmployerData>(endpoint, requestModel);
        }

        // DELETE /api/v4/people/{personId}/employer
        //await fiscalApi.People.Employer(personId).DeleteAsync();
        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            string endpoint = $"{baseEndpoint}/{id}/employer";

            return await HttpClient.DeleteAsync(endpoint);
        }
    }

    public class EmployeeService
    {
        private IFiscalApiHttpClient HttpClient { get; }
        private string baseEndpoint = "api/v4/people";

        public EmployeeService(IFiscalApiHttpClient fiscalApiHttpClient)
        {
            HttpClient = fiscalApiHttpClient;
        }

        // GET /api/v4/people/{personId}/employer
        public async Task<ApiResponse<EmployeeData>> GetByIdAsync(string id)
        {
            string endpoint = $"{baseEndpoint}/{id}/employer";

            return await HttpClient.GetAsync<EmployeeData>(endpoint);
        }

        //POST /api/v4/people/{personId}/employer
        //var employer = await fiscalApi.People.Employer(personId).CreateAsync(employerData);
        public async Task<ApiResponse<EmployeeData>> CreateAsync(EmployeeData requestModel)
        {
            string endpoint = $"{baseEndpoint}/{requestModel.EmployerPersonId}/employer";

            return await HttpClient.PostAsync<EmployeeData>(endpoint, requestModel);
        }

        // PUT /api/v4/people/{personId}/employer
        //var employer = await fiscalApi.People.Employer(personId).UpdateAsync(employerData);
        public async Task<ApiResponse<EmployeeData>> UpdateAsync(EmployeeData requestModel)
        {
            string endpoint = $"{baseEndpoint}/{requestModel.EmployerPersonId}/employer";

            return await HttpClient.PutAsync<EmployeeData>(endpoint, requestModel);
        }

        // DELETE /api/v4/people/{personId}/employer
        //await fiscalApi.People.Employer(personId).DeleteAsync();
        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            string endpoint = $"{baseEndpoint}/{id}/employer";

            return await HttpClient.DeleteAsync(endpoint);
        }
    }
}