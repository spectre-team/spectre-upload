/*
 * FileSystemExtensions.cs
 * Adds functionality to IFileSystem.
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

namespace Spectre.UploadApi
{
    /// <summary>
    /// Adds functionality to IFileSystem.
    /// </summary>
    public static class FileSystemExtensions
    {
        /// <summary>
        /// Gets the root directory of the filesystem.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <returns>Root directory.</returns>
        public static string GetRoot(this IFileSystem fileSystem)
        {
            var directory = fileSystem.Directory.GetCurrentDirectory();
            return fileSystem.Path.GetPathRoot(directory);
        }
    }
}
