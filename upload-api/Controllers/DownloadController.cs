using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using upload_api.Services;

namespace upload_api.Controllers
{
    [Route("api/[controller]")]
    public class DownloadController : Controller
    {
        public DownloadController(IDownloadService download)
        {
            _downloadService = download;
        }

        // POST api/download
        [HttpPost]
        public async Task Post([FromBody]string value)
        {
            await _downloadService.DownloadAsync(value, "new-file.txt");
        }

        private IDownloadService _downloadService;
    }
}
