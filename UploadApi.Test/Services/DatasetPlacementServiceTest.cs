/*
 * DatasetPlacementServiceTest.cs
 * Tests service responsible for placement of datasets on the disk.
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

using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Spectre.UploadApi.Services;
using Xunit;

namespace Spectre.UploadApi.Test.Services
{
    public class DatasetPlacementServiceTest
    {
        private readonly IFileSystem _fileSystem;
        private readonly DatasetPlacementService _service;

        public DatasetPlacementServiceTest()
        {
            _fileSystem = new MockFileSystem(files: new Dictionary<string, MockFileData>());
            _service = new DatasetPlacementService(_fileSystem);
        }

        [Fact]
        public void temporary_download_location_changes()
        {
            var first = _service.GetTemporaryDownloadLocation();
            var second = _service.GetTemporaryDownloadLocation();
            Assert.NotEqual(first, second);
        }

        [Theory]
        [InlineData("str@ng3?dataset__/name", "data/str_ng3_dataset___name/text_data/data.txt")]
        [InlineData("some name", "data/some_name/text_data/data.txt")]
        public void estimates_destination_inside_local_data_root(string datasetName, string expectedLocation)
        {
            expectedLocation = expectedLocation.Replace('/', _fileSystem.Path.DirectorySeparatorChar);
            expectedLocation = _fileSystem.Path.Combine(_fileSystem.GetRoot(), expectedLocation);
            var location = _service.GetDestinationLocation(datasetName);
            Assert.Equal(expectedLocation, location);
        }

        [Theory]
        [InlineData("str@ng3?dataset__/name", "str_ng3_dataset___name")]
        [InlineData("some name", "some_name")]
        [InlineData("???", "___")]
        public void conversion_to_directory_name_replaces_prohibited_charaters(string givenName, string expectedName)
        {
            var directoryName = _service.AsValidDirectoryName(givenName);
            Assert.Equal(expectedName, directoryName);
        }
    }
}
