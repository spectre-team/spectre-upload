/*
 * DownloadControllerTest.cs
 * Tests DownloadController
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

using System.IO.Abstractions.TestingHelpers;
using System.Net.Http;
using Moq;
using Spectre.UploadApi.Controllers;
using Spectre.UploadApi.Models;
using Spectre.UploadApi.Services;
using Xunit;

namespace Spectre.UploadApi.Test.Controllers
{
    public class DownloadControllerTest
    {
        private const string Url = "http://www.good.url/";
        private const string DatasetName = "my precious";
        private readonly DownloadJob _job = new DownloadJob(Url, DatasetName);
        private readonly Mock<DatasetFetchService> _fetchService;
        private readonly DownloadController _controller;

        public DownloadControllerTest()
        {
            var fileSystem = new MockFileSystem();
            var downloader = new Mock<DownloadService>(new object[]{new HttpClient(), fileSystem});
            var placementService = new Mock<DatasetPlacementService>(new object[]{fileSystem});
            _fetchService = new Mock<DatasetFetchService>(new object[]{downloader.Object, fileSystem, placementService.Object});

            _controller = new DownloadController(_fetchService.Object);
        }

        [Fact]
        public void redirects_post_directly_to_fetch_service()
        {
            _controller.Post(_job).Wait();
            _fetchService.Verify(s => s.FetchAsync(Url, DatasetName), Times.Exactly(1));
            _fetchService.VerifyNoOtherCalls();
        }
    }
}
