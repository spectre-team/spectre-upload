/*
 * FileSystemExtensionsTest.cs
 * Tests FileSystemExtensions.
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
using System.Runtime.InteropServices;
using Xunit;

namespace Spectre.UploadApi.Test
{
    public class FileSystemExtensionsTest
    {
        [Theory]
        [InlineData("/app")]
        [InlineData("/app/data/or/many/different/nestings")]
        [InlineData("/")]
        public void default_nix_root_is_slash(string currentDirectory)
        {
            if (RunningOnWindows)   
            {
                Console.WriteLine($"Skipping {nameof(default_nix_root_is_slash)}. Reason: platform-specific test.");
                return;
            }

            var filesystem = MakeFilesystem(currentDirectory);
            var root = filesystem.GetRoot();
            Assert.Equal("/", root);
        }

        [Theory]
        [InlineData("C:\\", "C:\\")]
        [InlineData("C:\\Users\\SampleUser\\Desktop\\wololo", "C:\\")]
        [InlineData("D:\\my_projects\\spectre-upload", "D:\\")]
        public void default_windows_root_depends_on_drive(string currentDirectory, string expectedRoot)
        {
            if (!RunningOnWindows)
            {
                Console.WriteLine($"Skipping {nameof(default_windows_root_depends_on_drive)}. Reason: platform-specific test.");
                return;
            }

            var root = MakeFilesystem(currentDirectory).GetRoot();
            Assert.Equal(expectedRoot, root);
        }

        private IFileSystem MakeFilesystem(string currentDirectory)
        {
            return new MockFileSystem(new Dictionary<string, MockFileData>(),
                currentDirectory: currentDirectory);
        }

        private static readonly bool RunningOnWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }
}
