using System.Collections.Generic;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// Compares version strings
    /// </summary>
    public class VersionStringComparer : IComparer<string>
    {
        /// <summary>
        /// Returns -1 if first version is larger, 1 if version is smaller and 0 if they are equal.
        /// It orders from newest to oldest version
        /// </summary>
        /// <param name="firstOne"></param>
        /// <param name="secondOne"></param>
        /// <returns></returns>
        public int Compare(string firstOne, string secondOne)
        {
            if (firstOne.IsNewerVersionThan(secondOne))
            {
                return 1;
            }
            else if (firstOne == secondOne)
            {
                return 0;
            }
            else
            {
                return -1;
            }

        }
    }

}