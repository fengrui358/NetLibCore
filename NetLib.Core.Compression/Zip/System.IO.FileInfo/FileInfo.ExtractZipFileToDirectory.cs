using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace FrHello.NetLib.Core.Compression
{
    public static partial class BaseCompressionExtensions
    {
        /// <summary>
        ///     Extracts all the files in the specified zip archive to a directory on the file system
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationDirectoryName">
        ///     The path to the directory in which to place the
        ///     extracted files, specified as a relative or absolute path. A relative path is interpreted as
        ///     relative to the current working directory.
        /// </param>
        public static void ExtractZipFileToDirectory(this global::System.IO.FileInfo @this,
            string destinationDirectoryName)
        {
            ExtractZipFileToDirectory(@this, destinationDirectoryName, GlobalCompressionOptions.DefaultEncoding);
        }

        /// <summary>
        ///     Extracts all the files in the specified zip archive to a directory on the file system and uses the specified
        ///     character encoding for entry names.
        /// </summary>
        /// <param name="this">The path to the archive that is to be extracted.</param>
        /// <param name="destinationDirectoryName">
        ///     The path to the directory in which to place the extracted files, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in this archive. Specify a
        ///     value for this parameter only when an encoding is required for interoperability with zip archive tools and
        ///     libraries that do not support UTF-8 encoding for entry names.
        /// </param>
        public static void ExtractZipFileToDirectory(this global::System.IO.FileInfo @this,
            string destinationDirectoryName, Encoding entryNameEncoding)
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

            ZipFile.ExtractToDirectory(@this.FullName, destinationDirectoryName, entryNameEncoding);
        }

        /// <summary>Extracts all the files in the specified zip archive to a directory on the file system.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationDirectory">Pathname of the destination directory.</param>
        public static void ExtractZipFileToDirectory(this global::System.IO.FileInfo @this,
            DirectoryInfo destinationDirectory)
        {
            ExtractZipFileToDirectory(@this, destinationDirectory.FullName, GlobalCompressionOptions.DefaultEncoding);
        }

        /// <summary>
        ///     Extracts all the files in the specified zip archive to a directory on the file system
        ///     and uses the specified character encoding for entry names.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationDirectory">Pathname of the destination directory.</param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in
        ///     this archive. Specify a value for this parameter only when an encoding is required for
        ///     interoperability with zip archive tools and libraries that do not support UTF-8 encoding for
        ///     entry names.
        /// </param>
        public static void ExtractZipFileToDirectory(this global::System.IO.FileInfo @this,
            DirectoryInfo destinationDirectory, Encoding entryNameEncoding)
        {
            ExtractZipFileToDirectory(@this, destinationDirectory.FullName, entryNameEncoding);
        }
    }
}