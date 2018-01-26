/*
 * DatasetFetchService.cs
 * Downloads datasets and puts them into default storage.
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
using System.Threading.Tasks;

namespace Spectre.UploadApi.Services
{
    /// <summary>
    /// Downloads datasets and puts them into default storage.
    /// </summary>
    public class DatasetFetchService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetFetchService"/> class.
        /// </summary>
        /// <param name="downloadService">The download service.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="placementService">The placement service.</param>
        public DatasetFetchService(DownloadService downloadService, IFileSystem fileSystem, DatasetPlacementService placementService)
        {
            _downloadService = downloadService;
            _fileSystem = fileSystem;
            _placementService = placementService;
        }

        /// <summary>
        /// Fetches the specified URL as a named dataset.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="datasetName">Name of the dataset.</param>
        /// <exception cref="DownloadFailureException">download failed</exception>
        /// <exception cref="OverwriteAttemptException">overwrite of existing dataset attempt</exception>
        public virtual async Task FetchAsync(string url, string datasetName)
        {
            var fileName = _placementService.GetTemporaryDownloadLocation();
            await _downloadService.DownloadAsync(url, fileName)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted && task.Exception.InnerException is DownloadFailureException)
                    {
                        throw new DownloadFailureException((DownloadFailureException)task.Exception.InnerException);
                    }
                    var destination = _placementService.GetDestinationLocation(datasetName);
                    if (_fileSystem.File.Exists(destination))
                    {
                        throw new OverwriteAttemptException(destination, datasetName);
                    }
                    if (_fileSystem.File.Exists(fileName))
                    {
                        _fileSystem.File.Move(fileName, destination);
                    }
                });
        }

        private readonly DownloadService _downloadService;
        private readonly IFileSystem _fileSystem;
        private readonly DatasetPlacementService _placementService;
    }
}
