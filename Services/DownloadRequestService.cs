using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;

namespace Fiscalapi.Services
{
    public class DownloadRequestService : BaseFiscalApiService<DownloadRequest>, IDownloadRequestService
    {
        public DownloadRequestService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "download-requests", apiVersion)
        {
        }


        public Task<ApiResponse<PagedList<Xml>>> GetXmlsAsync(string requestId)
        {
            // GET /api/v4/download-requests/<requestId>/xmls
            var path = $"{requestId}/xmls";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<PagedList<Xml>>(endpoint);
        }

        public Task<ApiResponse<PagedList<MetadataItem>>> GetMetadataItemsAsync(string requestId)
        {
            // GET /api/v4/download-requests/<requestId>/meta-items
            var path = $"{requestId}/meta-items";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<PagedList<MetadataItem>>(endpoint);
        }

        public Task<ApiResponse<List<FileResponse>>> DownloadPackageAsync(string requestId)
        {
            // GET /api/v4/download-requests/<requestId>/package
            var path = $"{requestId}/package";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<List<FileResponse>>(endpoint);
        }

        public Task<ApiResponse<FileResponse>> DownloadSatRequestAsync(string requestId)
        {
            // GET /api/v4/download-requests/<requestId>/raw-request
            var path = $"{requestId}/raw-request";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<FileResponse>(endpoint);
        }

        public Task<ApiResponse<FileResponse>> DownloadSatResponseAsync(string requestId)
        {
            // GET /api/v4/download-requests/<requestId>/raw-response
            var path = $"{requestId}/raw-response";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<FileResponse>(endpoint);
        }

        public Task<ApiResponse<List<DownloadRequest>>> SearchAsync(DateTime createdAt)
        {
            //api/v4/download-requests/search?createdAt=2025-08-21
            var path = $"search?createdAt={createdAt:yyyy-MM-dd}";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<List<DownloadRequest>>(endpoint);
        }
    }
}