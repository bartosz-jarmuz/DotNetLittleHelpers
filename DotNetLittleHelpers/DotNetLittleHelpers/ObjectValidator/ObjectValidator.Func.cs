using System;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// Extensions methods for simpler/shorter object validation
    /// </summary>
    public static partial class ObjectValidator
    {

        /// <summary>
        /// Performs the validation function, and if source is null or validation returns false, the 'on error' callback is called
        /// <para/>
        /// This is a one-liner syntactic sugar for methods such as TryParse, Convert etc
        /// </summary>
        /// <param name="source"></param>
        /// <param name="validationFunction"></param>
        /// <param name="onErrorCallback"></param>
        public static void Validate(this object source, Func<bool> validationFunction, Action onErrorCallback)
        {
            if (source == null || !validationFunction())
            {
                onErrorCallback();
            }
        }

        /// <summary>
        /// Performs the validation function, and if source is null or validation returns false, the 'errorToThrow' is thrown
        /// <para/>
        /// This is a one-liner syntactic sugar for methods such as TryParse, Convert etc
        /// </summary>
        /// <param name="source"></param>
        /// <param name="validationFunction"></param>
        /// <param name="errorToThrow"></param>
        public static void Validate(this object source, Func<bool> validationFunction, Exception errorToThrow)
        {
            if (source == null || !validationFunction())
            {
                throw errorToThrow;
            }
        }

        /// <summary>
        /// Performs the validation function, and if it returns false, the 'on error' callback is fired
        /// <para/>
        /// This is a one-liner syntactic sugar for methods such as TryParse, Convert etc
        /// </summary>
        /// <param name="validationFunction"></param>
        /// <param name="onErrorCallback"></param>
        public static void Validate(Func<bool> validationFunction, Action onErrorCallback)
        {
            if (!validationFunction())
            {
                onErrorCallback();
            }
        }

        /// <summary>
        /// Performs the validation function, and if it returns false, the 'errorToThrow' is thrown
        /// <para/>
        /// This is a one-liner syntactic sugar for methods such as TryParse, Convert etc
        /// </summary>
        /// <param name="validationFunction"></param>
        /// <param name="errorToThrow"></param>
        public static void Validate(Func<bool> validationFunction, Exception errorToThrow)
        {
            if (!validationFunction())
            {
                throw errorToThrow;
            }
        }


        
    }
}
