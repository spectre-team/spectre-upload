/*
 * DownloadFailureException.cs
 * Thrown when file download fails.
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
using System.Net;
using System.Net.Http;

namespace Spectre.UploadApi.Services
{
    /// <summary>
    /// Thrown when file download fails.
    /// </summary>
    /// <seealso cref="System.Net.Http.HttpRequestException" />
    public sealed class DownloadFailureException : HttpRequestException
    {
        /// <summary>
        /// Gets the cause.
        /// </summary>
        /// <value>
        /// The cause of the download failure. Either bad response code or connectivity error.
        /// </value>
        public object Cause { get; }
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL of file downloaded.
        /// </value>
        public string Url { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFailureException"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="connectivityProblem">The connectivity problem.</param>
        public DownloadFailureException(string url, HttpRequestException connectivityProblem) :
            base($"Download of file {url} failed due to connectivity issues: {connectivityProblem.Message}.")
        {
            Url = url;
            Cause = connectivityProblem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFailureException"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="errorCode">The error code.</param>
        public DownloadFailureException(string url, HttpStatusCode errorCode) :
            base($"Download of file {url} failed due to error code: {errorCode.ToString()}")
        {
            Url = url;
            Cause = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFailureException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public DownloadFailureException(DownloadFailureException innerException) :
            base(innerException.Message, innerException)
        {
            Url = innerException.Url;
            Cause = innerException.Cause;
        }
    }
}
