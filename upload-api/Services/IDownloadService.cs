using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace upload_api.Services
{
    public interface IDownloadService
    {
        Task DownloadAsync(string url, string destination);
    }
}
