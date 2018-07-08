using System;

namespace DotNetLittleHelpers
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Extensions methods for simpler/shorter object validation
    /// </summary>
    public static partial class ObjectValidator
    {
        /// <summary>
        /// Throws an ArgumentNullException if the argument is null
        /// </summary>
        /// <param name="source"></param>
        /// <param name="argumentName"></param>
        /// <param name="methodName"></param>
        public static void CheckArgumentNull(this object source, string argumentName, [CallerMemberName] string methodName = "")
        {
            if (source == null)
            {
                throw new ArgumentNullException(argumentName, $"Null parameter passed to method [{methodName}].");
            }
        }

        
    }
}
