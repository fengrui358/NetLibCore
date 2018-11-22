// Description: C# Extension Methods Library to enhances the .NET Framework by adding hundreds of new methods. It drastically increases developers productivity and code readability. Support C# and VB.NET
// Website & Documentation: https://github.com/zzzprojects/Z.ExtensionMethods
// Forum: https://github.com/zzzprojects/Z.ExtensionMethods/issues
// License: https://github.com/zzzprojects/Z.ExtensionMethods/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright ?ZZZ Projects Inc. 2014 - 2016. All rights reserved.

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace FrHello.NetLib.Core.Compression
{
    public static partial class BaseCompressionExtensions
    {
        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFileName">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName)
        {
            CreateZipFile(@this, destinationArchiveFileName, CompressionLevel.Fastest, true,
                GlobalCompressionOptions.DefaultEncoding);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory, uses the specified compression level, and optionally includes the base directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFileName">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to
        ///     emphasize speed or compression effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from
        ///     sourceDirectoryName at the root of the archive; false to include only the contents of the
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName,
            CompressionLevel compressionLevel, bool includeBaseDirectory)
        {
            CreateZipFile(@this, destinationArchiveFileName, CompressionLevel.Fastest, includeBaseDirectory,
                GlobalCompressionOptions.DefaultEncoding);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified directory, uses the specified
        ///     compression level and character encoding for entry names, and optionally includes the base directory.
        /// </summary>
        /// <param name="this">
        ///     The path to the directory to be archived, specified as a relative or absolute path. A relative path
        ///     is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="destinationArchiveFileName">
        ///     The path of the archive to be created, specified as a relative or absolute
        ///     path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to emphasize speed or compression
        ///     effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from sourceDirectoryName at the root of the
        ///     archive; false to include only the contents of the directory.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in this archive. Specify a
        ///     value for this parameter only when an encoding is required for interoperability with zip archive tools and
        ///     libraries that do not support UTF-8 encoding for entry names.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName,
            CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
        {
            if (entryNameEncoding == null)
            {
                if (GlobalCompressionOptions.DefaultEncoding != null)
                {
                    entryNameEncoding = GlobalCompressionOptions.DefaultEncoding;
                }
                else
                {
                    throw new ArgumentNullException(nameof(entryNameEncoding));
                }
            }

            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFileName, compressionLevel, includeBaseDirectory,
                entryNameEncoding);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFile">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile)
        {
            CreateZipFile(@this, destinationArchiveFile.FullName, CompressionLevel.Fastest, true,
                GlobalCompressionOptions.DefaultEncoding);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory, uses the specified compression level, and optionally includes the base directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFile">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to
        ///     emphasize speed or compression effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from
        ///     sourceDirectoryName at the root of the archive; false to include only the contents of the
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile,
            CompressionLevel compressionLevel, bool includeBaseDirectory)
        {
            CreateZipFile(@this, destinationArchiveFile.FullName, CompressionLevel.Fastest, includeBaseDirectory,
                GlobalCompressionOptions.DefaultEncoding);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory, uses the specified compression level and character encoding for entry names, and
        ///     optionally includes the base directory.
        /// </summary>
        /// <param name="this">
        ///     The path to the directory to be archived, specified as a relative or
        ///     absolute path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="destinationArchiveFile">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to
        ///     emphasize speed or compression effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from
        ///     sourceDirectoryName at the root of the archive; false to include only the contents of the
        ///     directory.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in
        ///     this archive. Specify a value for this parameter only when an encoding is required for
        ///     interoperability with zip archive tools and libraries that do not support UTF-8 encoding for
        ///     entry names.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile,
            CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
        {
            CreateZipFile(@this, destinationArchiveFile.FullName, CompressionLevel.Fastest, includeBaseDirectory,
                entryNameEncoding);
        }
    }
}