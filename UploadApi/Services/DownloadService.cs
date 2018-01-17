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

using System.Net.Http;
using System.Threading.Tasks;

namespace Spectre.UploadApi.Services
{
    public class DownloadService
    {
        public async Task DownloadAsync(string url, string destination)
        {
            using (var client = new HttpClient())
            using (var content = await client.GetAsync(url))
            using (var outputFile = System.IO.File.Create(destination))
                await content.Content.CopyToAsync(outputFile);
        }
    }
}
