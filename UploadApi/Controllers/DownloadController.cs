﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UploadApi.Services;

namespace UploadApi.Controllers
{
    [Route("api/[controller]")]
    public class DownloadController : Controller
    {
        public DownloadController(DownloadService download)
        {
            _downloadService = download;
        }

        // POST api/download
        [HttpPost]
        public async Task Post([FromBody]string value)
        {
            await _downloadService.DownloadAsync(value, "new-file.txt");
        }

        private DownloadService _downloadService;
    }
}
