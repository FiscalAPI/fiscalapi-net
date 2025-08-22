using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Models;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the download request service.
    /// </summary>
    public interface IDownloadRequestService : IFiscalApiService<DownloadRequest>
    {
        /// <summary>
        /// Lista los xmls descargados para un requestId.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns>List of Xmls objects</returns>
        Task<ApiResponse<PagedList<Xml>>> GetXmlsAsync(string requestId);

        /// <summary>
        /// Lista los meta-items descargados para un requestId.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns>List of meta-Items objects</returns>
        Task<ApiResponse<PagedList<MetadataItem>>> GetMetadataItemsAsync(string requestId);

        /// <summary>
        /// Downloads la lista de paquetes (archivos .zip) de un requestId.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns>Lista of FileResponses</returns>
        Task<ApiResponse<List<FileResponse>>> DownloadPackageAsync(string requestId);

        /// <summary>
        /// Descarga el archivo crudo de solicitud SAT para un requestId.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns>File response object</returns>
        Task<ApiResponse<FileResponse>> DownloadSatRequestAsync(string requestId);

        /// <summary>
        /// Descarga la respuesta SAT para un requestId.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns>File response object</returns>
        Task<ApiResponse<FileResponse>> DownloadSatResponseAsync(string requestId);

        /// <summary>
        /// Searches for download requests created at a specific date.
        /// </summary>
        /// <param name="createdAt"></param>
        /// <returns></returns>
        Task<ApiResponse<List<DownloadRequest>>> SearchAsync(DateTime createdAt);
    }
}