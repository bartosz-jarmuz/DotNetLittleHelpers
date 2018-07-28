using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// Extension methods for FileInfo class
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Verifies whether the file exists at the moment
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool Exists(this FileInfo info)
        {
       
            return File.Exists(info.FullName);
        }
    }
}
