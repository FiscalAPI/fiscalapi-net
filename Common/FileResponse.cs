using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalApi.Common
{
    public class FileResponse
    {
        public string Base64File { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}