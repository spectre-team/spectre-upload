/*
 * DatasetPlacementService.cs
 * Specifies locations of datasets.
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
using System.Text.RegularExpressions;

namespace Spectre.UploadApi.Services
{
    /// <summary>
    /// Specifies locations of datasets.
    /// </summary>
    public class DatasetPlacementService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetPlacementService"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        public DatasetPlacementService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            var fileSystemRoot = _fileSystem.Path.GetPathRoot(_fileSystem.Path.GetFullPath("."));
            const string dataDirectory = "data";
            _dataRoot = _fileSystem.Path.Combine(fileSystemRoot, dataDirectory);
        }

        /// <summary>
        /// Gets the temporary download location.
        /// </summary>
        /// <returns>Random location that should be used to download file.</returns>
        public virtual string GetTemporaryDownloadLocation() => _fileSystem.Path.GetRandomFileName();

        /// <summary>
        /// Gets the destination location.
        /// </summary>
        /// <param name="datasetName">Name of the dataset.</param>
        /// <returns>Location under which dataset should be placed in default storage.</returns>
        public virtual string GetDestinationLocation(string datasetName)
        {
            var datasetDirectoryName = AsValidDirectoryName(datasetName);
            const string subdirectoryName = "text_data";
            var destinationDirectory = _fileSystem.Path.Combine(_dataRoot, datasetDirectoryName, subdirectoryName);
            const string fileName = "data.txt";
            var destinationFileName = _fileSystem.Path.Combine(destinationDirectory, fileName);
            return destinationFileName;
        }

        /// <summary>
        /// Converts the dataset's name to the valid directory name.
        /// </summary>
        /// <param name="datasetName">Name of the directory.</param>
        /// <returns>Valid directory name.</returns>
        public string AsValidDirectoryName(string datasetName)
        {
            return Regex.Replace(datasetName, DisallowedCharacters, "_");
        }

        private readonly IFileSystem _fileSystem;
        private readonly string _dataRoot;
        private const string DisallowedCharacters = "[^a-zA-Z0-9_-]";
    }
}
