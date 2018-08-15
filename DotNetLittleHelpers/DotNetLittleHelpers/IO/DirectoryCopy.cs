using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// Handles copying of directories
    /// </summary>
    public static class DirectoryCopy
    {
        /// <summary>
        /// Copies the directory and all its content into the other path. The folder structure will be created if does not exist<para/>
        /// Any existing file will be overwritten
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);
            Copy(diSource, diTarget);
        }

        /// <summary>
        /// Copies the directory and all its content into the other path. The folder structure will be created if does not exist<para/>
        /// Any existing file will be overwritten
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Copy(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                Copy(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
