using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FiscalApi.Abstractions;
using FiscalApi.Common;
using FiscalApi.Http;
using FiscalApi.Models;

namespace FiscalApi.Services
{
   

    public class ApiKeyService : BaseFiscalApiService<ApiKey>, IApiKeyService
    {
        public ApiKeyService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "apikeys", apiVersion)
        {
        }


    }
}
