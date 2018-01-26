/*
 * DownloadServiceTest.cs
 * Tests download service
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
using System.IO.Abstractions.TestingHelpers;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using Spectre.UploadApi.Services;
using WorldDomination.Net.Http;
using Xunit;

namespace Spectre.UploadApi.Test.Services
{
    public class DownloadServiceTest
    {
        // to see details, check https://github.com/dotnet/corefx/issues/1624
        // to check tutorial, check https://github.com/PureKrome/HttpClient.Helpers/wiki/A-single-endpoint

        private const string GoodUrl = "http://www.good.url/";
        private const string BadUrl = "http://www.bad.url/";
        private const string Destination = "wololo";
        private const string ResponseData = "{\"fake\": \"response-data\"}";
        private readonly HttpMessageOptions[] _messages;
        private readonly MockFileSystem _fileSystem;

        public DownloadServiceTest()
        {
            _fileSystem = new MockFileSystem(
                files: new Dictionary<string, MockFileData>(),
                currentDirectory: new MockFileSystem().GetRoot());
            var responseData = FakeHttpMessageHandler.GetStringHttpResponseMessage(ResponseData);
            _messages = new []
            {
                new HttpMessageOptions
                {
                    RequestUri = new Uri(GoodUrl),
                    HttpResponseMessage = FakeHttpMessageHandler.GetStringHttpResponseMessage(ResponseData, HttpStatusCode.OK)
                },
                new HttpMessageOptions
                {
                    RequestUri = new Uri(BadUrl),
                    HttpResponseMessage = FakeHttpMessageHandler.GetStringHttpResponseMessage(ResponseData, HttpStatusCode.InternalServerError)
                }
            };
        }

        [Fact]
        public void downloads_directly_from_url_to_file()
        {
            if (!RunningOnWindows)
            {
                ReportSkip(nameof(downloads_directly_from_url_to_file));
                return;
            }
            using (var messageHandler = new FakeHttpMessageHandler(_messages))
            using (var httpClient = new HttpClient(messageHandler))
            {
                var service = new DownloadService(httpClient, _fileSystem);
                service.DownloadAsync(GoodUrl, Destination).Wait();
            }

            Assert.True(_fileSystem.FileExists(Destination), "file is missing");

            var fileContent = _fileSystem.File.ReadAllText(Destination);
            Assert.Equal(fileContent, ResponseData);
        }

        [Fact]
        public void throws_without_saving_file_when_got_return_code_else_than_2XX()
        {
            if (!RunningOnWindows)
            {
                ReportSkip(nameof(throws_without_saving_file_when_got_return_code_else_than_2XX));
                return;
            }
            using (var messageHandler = new FakeHttpMessageHandler(_messages))
            using (var httpClient = new HttpClient(messageHandler))
            {
                var service = new DownloadService(httpClient, _fileSystem);
                Assert.ThrowsAsync<DownloadFailureException>(() => service.DownloadAsync(BadUrl, Destination)).Wait();
            }
            Assert.False(_fileSystem.FileExists(Destination), "saves file on disk");
        }

        [Fact]
        public void throws_without_saving_file_when_connectivity_failed()
        {
            using (var messageHandler = new FakeHttpMessageHandler(new HttpRequestException()))
            using (var httpClient = new HttpClient(messageHandler))
            {
                var service = new DownloadService(httpClient, _fileSystem);
                Assert.ThrowsAsync<DownloadFailureException>(() => service.DownloadAsync(GoodUrl, Destination)).Wait();
            }
            Assert.False(_fileSystem.FileExists(Destination), "saves file on disk");
        }

        private static readonly bool RunningOnWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        private static void ReportSkip(string testName)
            => Console.WriteLine(
                $"Skipping {nameof(DownloadServiceTest)}.{testName}. Reason: mock uses ACL specific to Windows.");
    }
}
