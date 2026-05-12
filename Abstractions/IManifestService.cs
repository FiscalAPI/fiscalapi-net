using Fiscalapi.Common;
using Fiscalapi.Models;
using System.Threading.Tasks;

namespace Fiscalapi.Abstractions
{
    public interface IManifestService
    {
        Task<ApiResponse<FileResponse>> SignAsync(SignManifestRequest requestModel);
    }
}
