using FiscalApi.Models;
using System.Threading.Tasks;
using FiscalApi.Common;

namespace FiscalApi.Abstractions
{
    public interface IPersonService : IFiscalApiService<Person>
    {
    }
}