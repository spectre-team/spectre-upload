using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadApi.Services
{
    public interface IDownloadService
    {
        Task DownloadAsync(string url, string destination);
    }
}
