/*
 * DownloadService.cs
 * Downloads files on disk.
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

using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spectre.UploadApi.Services
{
    /// <summary>
    /// Downloads files on disk.
    /// </summary>
    public class DownloadService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="fileSystem">The file system.</param>
        public DownloadService(HttpClient httpClient, IFileSystem fileSystem)
        {
            _httpClient = httpClient;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Downloads the file from url in asynchronous manner.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="destination">The destination file path.</param>
        public virtual async Task DownloadAsync(string url, string destination)
        {
            try
            {
                using (var content = await _httpClient.GetAsync(url))
                using (var outputFile = _fileSystem.File.Create(destination))
                {
                    if (!content.IsSuccessStatusCode)
                        throw new DownloadFailureException(url, content.StatusCode);
                    await content.Content.CopyToAsync(outputFile);
                }
            }
            catch(HttpRequestException exception)
            {
                _fileSystem.File.Delete(destination);
                throw new DownloadFailureException(url, exception);
            }
        }

        private readonly HttpClient _httpClient;
        private readonly IFileSystem _fileSystem;
    }
}
