/*
 * DownloadJob.cs
 * Model of the scheduled job.
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
using System.Linq;
using System.Threading.Tasks;

namespace Spectre.UploadApi.Models
{
    /// <summary>
    /// Model of the scheduled job.
    /// </summary>
    public class DownloadJob
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL to file.
        /// </value>
        public string Url { get; }

        /// <summary>
        /// Gets the name of the dataset.
        /// </summary>
        /// <value>
        /// The name of the dataset to be fetched.
        /// </value>
        public string DatasetName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadJob"/> class.
        /// </summary>
        /// <param name="url">The URL to file.</param>
        /// <param name="datasetName">Name of the dataset to be fetched.</param>
        public DownloadJob(string url, string datasetName)
        {
            Url = url;
            DatasetName = datasetName;
        }
    }
}
