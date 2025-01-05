using System.Threading.Tasks;
using FiscalApi.Abstractions;
using FiscalApi.Common;
using FiscalApi.Http;
using FiscalApi.Models;

namespace FiscalApi.Services
{
    public class PersonService : BaseFiscalApiService<Person>, IPersonService
    {
        public PersonService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "users", apiVersion)
        {
        }

        public Task<ApiResponse<string>> CreateAsync(Person model)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<string>> UpdateAsync(string id, Person model)
        {
            throw new System.NotImplementedException();
        }
    }
}