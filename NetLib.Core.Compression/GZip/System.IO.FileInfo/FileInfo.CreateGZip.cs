﻿using System;
using System.IO;
using System.IO.Compression;

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core.Compression
{
    /// <summary>
    /// 文件压缩Gizp辅助方法
    /// </summary>
    public static partial class BaseCompressionExtensions
    {
        /// <summary>
        ///     A FileInfo extension method that creates a zip file.
        /// </summary>
        /// <param name="fileInfo">待压缩的文件</param>
        public static void CreateGZip(this FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            using (var originalFileStream = fileInfo.OpenRead())
            {
                using (var compressedFileStream = File.Create(GlobalCompressionOptions.AutoFillFileSuffix
                    ? AutoFillFileSuffix(fileInfo.FullName)
                    : fileInfo.FullName))
                {
                    using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        /// <summary>
        ///     A FileInfo extension method that creates a zip file.
        /// </summary>
        /// <param name="fileInfo">待压缩的文件</param>
        /// <param name="destination">Destination for the zip.</param>
        public static void CreateGZip(this FileInfo fileInfo, string destination)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            using (var originalFileStream = fileInfo.OpenRead())
            {
                using (var compressedFileStream = File.Create(GlobalCompressionOptions.AutoFillFileSuffix
                    ? AutoFillFileSuffix(destination)
                    : destination))
                {
                    using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        /// <summary>
        ///     A FileInfo extension method that creates a zip file.
        /// </summary>
        /// <param name="fileInfo">待压缩的文件</param>
        /// <param name="destination">Destination for the zip.</param>
        public static void CreateGZip(this FileInfo fileInfo, FileInfo destination)
        {
            CreateGZip(fileInfo, destination?.FullName);
        }

        private const string GZipSuffix = ".gzip";

        /// <summary>
        /// 自动填充文件后缀
        /// </summary>
        /// <param name="destination">目标文件路径</param>
        /// <returns>填充后缀后的文件路径</returns>
        private static string AutoFillFileSuffix(string destination)
        {
            var suffix = Path.GetExtension(destination);
            if (suffix?.ToLower() != GZipSuffix)
            {
                destination += GZipSuffix;
            }

            return destination;
        }
    }
}
