﻿using System.IO;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// Extension methods for DirectoryInfo class
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Verifies whether the file exists at the moment
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool Exists(this DirectoryInfo info)
        {
            return Directory.Exists(info.FullName);
        }

        /// <summary>
        /// Appends a file name to the directory path, thus returning a new FileInfo
        /// <para></para>
        /// This DOES NOT create/copy files and DOES NOT verify whether anything exists
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileInfo CreateFileInfo(this DirectoryInfo directoryInfo, string fileName)
        {
            directoryInfo.ThrowIfNull("directoryInfo");
            directoryInfo.ThrowIfNull("fileName");
            return new FileInfo(Path.Combine(directoryInfo.FullName, fileName));
        }

        /// <summary>
        /// Appends a file name to the directory path, thus returning a new DirectoryInfo
        /// <para></para>
        /// This DOES NOT create/copy directories and DOES NOT verify whether anything exists
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="subDirectoryName"></param>
        /// <returns></returns>
        public static DirectoryInfo CreateSubDirectoryInfo(this DirectoryInfo directoryInfo, string subDirectoryName)
        {
            directoryInfo.ThrowIfNull("directoryInfo");
            directoryInfo.ThrowIfNull("subDirectoryName");
            return new DirectoryInfo(Path.Combine(directoryInfo.FullName, subDirectoryName));
        }
    }
    
}