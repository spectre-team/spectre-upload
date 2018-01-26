/*
 * DatasetFetchServiceTest.cs
 * Tests service fetching datasets
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

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Moq;
using Spectre.UploadApi.Services;
using Xunit;

namespace Spectre.UploadApi.Test.Services
{
    public class DatasetFetchServiceTest
    {
        private const string Url = "http://www.good.url/";
        private const string DatasetName = "my precious";
        private const string FinalDestination = "blah";
        private const string TemporaryLocation = "wololo";
        private const string DatasetContent = "IO-EO-IO-EEEE";
        private readonly MockFileSystem _fileSystem;
        private readonly Mock<DownloadService> _downloadService;
        private readonly DatasetFetchService _service;

        public DatasetFetchServiceTest()
        {
            if (!RunningOnWindows)
            {
                // more reporting in each test
                // source of the problem: https://github.com/tathamoddie/System.IO.Abstractions/blob/master/TestingHelpers/MockFileData.cs#L63
                return;
            }
            
            _fileSystem = new MockFileSystem();
            // if you wonder why the hell next line is sooo long, check this:
            // https://github.com/tathamoddie/System.IO.Abstractions/issues/208
            var appRoot = _fileSystem.Path.Combine(_fileSystem.GetRoot(), "app") + _fileSystem.Path.DirectorySeparatorChar.ToString();
            _fileSystem = new MockFileSystem(
                files: new Dictionary<string, MockFileData>
                {
                    {appRoot, new MockDirectoryData()}
                },
                currentDirectory: appRoot);

            var task = new Task(() => { });
            task.Start();
            _downloadService = new Mock<DownloadService>(new object[]{new HttpClient(), (IFileSystem)_fileSystem});
            _downloadService
                .Setup(s => s.DownloadAsync(Url, TemporaryLocation))
                .Callback(() => _fileSystem.File.WriteAllText(TemporaryLocation, DatasetContent))
                .Returns(task);

            var placementService = new Mock<DatasetPlacementService>(new object[]{(IFileSystem)_fileSystem});
            placementService
                .Setup(s => s.GetTemporaryDownloadLocation())
                .Returns(TemporaryLocation);
            placementService
                .Setup(s => s.GetDestinationLocation(DatasetName))
                .Returns(FinalDestination);
            
            _service = new DatasetFetchService(_downloadService.Object, _fileSystem, placementService.Object);
        }

        [Fact]
        public void downloads_dataset_and_moves_it_into_default_store()
        {
            if (!RunningOnWindows)
            {
                ReportSkip(nameof(downloads_dataset_and_moves_it_into_default_store));
                return;
            }
            _service.FetchAsync(Url, DatasetName).Wait();
            Assert.True(_fileSystem.FileExists(FinalDestination), "Dataset absent at default storage");
            Assert.Equal(DatasetContent, _fileSystem.File.ReadAllText(FinalDestination));
        }

        [Fact]
        public void removes_temporary_file()
        {
            if (!RunningOnWindows)
            {
                ReportSkip(nameof(removes_temporary_file));
                return;
            }
            _service.FetchAsync(Url, DatasetName).Wait();
            Assert.False(_fileSystem.FileExists(TemporaryLocation), "Temporary file present");
        }

        [Fact]
        public void throws_on_download_failure()
        {
            if (!RunningOnWindows)
            {
                ReportSkip(nameof(throws_on_download_failure));
                return;
            }
            _downloadService.Reset();
            _downloadService
                .Setup(s => s.DownloadAsync(Url, TemporaryLocation))
                .ThrowsAsync(new DownloadFailureException(Url, HttpStatusCode.NotFound));
            Assert.ThrowsAsync<DownloadFailureException>(() => _service.FetchAsync(Url, DatasetName)).Wait();
        }

        [Fact]
        public void throws_on_owerwrite_attempt()
        {
            if (!RunningOnWindows)
            {
                ReportSkip(nameof(throws_on_owerwrite_attempt));
                return;
            }
            _service.FetchAsync(Url, DatasetName).Wait();
            Assert.ThrowsAsync<OverwriteAttemptException>(() => _service.FetchAsync(Url, DatasetName)).Wait();
        }

        private static readonly bool RunningOnWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        private static void ReportSkip(string testName) 
            => Console.WriteLine($"Skipping {nameof(DatasetFetchServiceTest)}.{testName}. Reason: mock uses ACL specific to Windows.");
    }
}
