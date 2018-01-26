/*
 * OverwriteAttemptException.cs
 * Thrown when dataset would overwrite existing one.
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

using System.IO;

namespace Spectre.UploadApi.Services
{
    /// <summary>
    /// Thrown when dataset would overwrite existing one.
    /// </summary>
    /// <seealso cref="System.IO.IOException" />
    public class OverwriteAttemptException: IOException
    {
        /// <summary>
        /// Gets the existing dataset path.
        /// </summary>
        /// <value>
        /// The existing dataset path.
        /// </value>
        public string ExistingDatasetPath { get; }
        /// <summary>
        /// Gets the name of the dataset.
        /// </summary>
        /// <value>
        /// The name of the dataset.
        /// </value>
        public string DatasetName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OverwriteAttemptException"/> class.
        /// </summary>
        /// <param name="existingDatasetPath">The existing dataset path.</param>
        /// <param name="datasetName">Name of the overwriting dataset.</param>
        public OverwriteAttemptException(string existingDatasetPath, string datasetName)
        {
            ExistingDatasetPath = existingDatasetPath;
            DatasetName = datasetName;
        }
    }
}
