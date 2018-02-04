/*
 * DownloadController.cs
 * API controller exhibiting file download.
 *
   Copyright 2018 Spectre Team

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spectre.UploadApi.Models;
using Spectre.UploadApi.Services;

namespace Spectre.UploadApi.Controllers
{
    /// <summary>
    /// API controller for file downloads.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    public class DownloadController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadController"/> class.
        /// </summary>
        /// <param name="fetchService">The fetch service.</param>
        public DownloadController(DatasetFetchService fetchService)
        {
            _fetchService = fetchService;
        }

        // POST api/download
        /// <summary>
        /// Downloads file from the URL.
        /// </summary>
        /// <param name="job">Download job.</param>
        [HttpPost]
        public async Task Post([FromBody]DownloadJob job)
        {
            await _fetchService.FetchAsync(job.Url, job.DatasetName);
        }

        private readonly DatasetFetchService _fetchService;
    }
}
