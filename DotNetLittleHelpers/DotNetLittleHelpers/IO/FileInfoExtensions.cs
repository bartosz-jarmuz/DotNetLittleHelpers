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
        /// Verifies whether the file exists at the moment of checking.<br/>
        /// The .Exists property is a snapshot which is valid as of when the FileInfo was created. <br/>
        /// If the file was deleted later on, that snapshot is not updated (until Refresh() is called
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool ExistsAtTheMoment(this FileInfo info)
        {
            info.Refresh();
            return File.Exists(info.FullName);
        }
    }
}
