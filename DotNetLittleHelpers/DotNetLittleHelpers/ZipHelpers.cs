using System;
using System.Diagnostics;
using System.IO;

namespace DotNetLittleHelpers
{
    
    /// <summary>
    /// Helps with compressed object handling
    /// </summary>
    public static class ZipHelpers
    {
        private const int ZipLeadBytes = 0x04034b50;
        private const ushort GzipLeadBytes = 0x8b1f;

        /// <summary>
        /// Determines whether stream is a zip
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns><c>true</c> if [is zip compressed data] [the specified stream]; otherwise, <c>false</c>.</returns>
        public static bool IsZipCompressedData(Stream stream)
        {
            return IsCompressedData(stream, ZipLeadBytes);

        }

        /// <summary>
        /// Determines whether stream is a gzip
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns><c>true</c> if [is g zip compressed data] [the specified stream]; otherwise, <c>false</c>.</returns>
        public static bool IsGZipCompressedData(Stream stream)
        {
            return IsCompressedData(stream, GzipLeadBytes);
        }



        private static bool IsCompressedData(Stream stream, params int[] bytesOptions)
        {
            stream.Seek(0, 0);

            try
            {
                byte[] bytes = new byte[4];
                stream.Read(bytes, 0, 4);
                foreach (int bytesOption in bytesOptions)
                {
                    if (BitConverter.ToInt32(bytes, 0) == bytesOption)
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                stream.Seek(0, 0);  // set the stream back to the begining
            }
        }

    }
}