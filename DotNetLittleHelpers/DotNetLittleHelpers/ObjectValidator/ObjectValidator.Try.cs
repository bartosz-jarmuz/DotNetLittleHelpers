using System;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// Extensions methods for simpler/shorter object validation
    /// </summary>
    public static partial class ObjectValidator
    {
        /// <summary>
        /// Performs a validation function in a 'try' block and if source is null or an error happens, an 'on error' callback is called
        /// <para/>
        /// This is a one-liner syntactic sugar for methods such as Parse, Convert etc
        /// </summary>
        /// <param name="source"></param>
        /// <param name="validationFunction"></param>
        /// <param name="onErrorCallback"></param>
        public static void ValidateTry(this object source, Action validationFunction, Action onErrorCallback)
        {
            if (source == null)
            {
                onErrorCallback();
            }
            try
            {
                validationFunction();
            }
            catch (Exception)
            {
                onErrorCallback();
            }
        }

        /// <summary>
        /// Performs a validation function in a 'try' block and if source is null or an error happens, an 'on error' callback is called.
        /// Actual error is passed to the callback
        /// <para/>
        /// This is a one-liner syntactic sugar for methods such as Parse, Convert etc
        /// </summary>
        /// <param name="source"></param>
        /// <param name="validationFunction"></param>
        /// <param name="onErrorCallback"></param>
        public static void ValidateTry(this object source, Action validationFunction, Action<Exception> onErrorCallback)
        {
            if (source == null)
            {
                onErrorCallback(new NullReferenceException("The source object is null"));
            }
            try
            {
                validationFunction();
            }
            catch (Exception ex)
            {
                onErrorCallback(ex);
            }
        }

        /// <summary>
        /// Performs a validation function in a 'try' block and if an error happens, an 'on error' callback is called.
        /// Actual error is passed to the callback
        /// <para/>
        /// This is a one-liner syntactic sugar for methods such as Parse, Convert etc
        /// </summary>
        /// <param name="validationFunction"></param>
        /// <param name="onErrorCallback"></param>
        public static void ValidateTry( Action validationFunction, Action<Exception> onErrorCallback)
        {
            try
            {
                validationFunction();
            }
            catch (Exception ex)
            {
                onErrorCallback(ex);
            }
        }


        /// <summary>
        /// Performs a validation function in a 'try' block and if source is null or an error happens, specified error is thrown
        /// <para/>
        /// This is a one-liner syntactic sugar for methods such as Parse, Convert etc
        /// </summary>
        /// <param name="source"></param>
        /// <param name="validationFunction"></param>
        /// <param name="errorToThrow"></param>
        public static void ValidateTry(this object source, Action validationFunction, Exception errorToThrow)
        {
            if (source == null)
            {
                throw errorToThrow;
            }
            try
            {
                validationFunction();
            }
            catch (Exception)
            {
                throw errorToThrow;
            }
        }

        /// <summary>
        /// Performs a validation function in a 'try' block and if an error happens, an 'on error' callback is called
        /// <para/>
        /// This is a one-liner syntactic sugar for methods such as Parse, Convert etc
        /// </summary>
        /// <param name="validationFunction"></param>
        /// <param name="onErrorCallback"></param>
        public static void ValidateTry(Action validationFunction, Action onErrorCallback)
        {

            try
            {
                validationFunction();
            }
            catch (Exception)
            {
                onErrorCallback();
            }
        }

        /// <summary>
        /// Performs a validation function in a 'try' block and if an error happens, specified error is thrown
        /// <para/>
        /// This is a one-liner syntactic sugar for methods such as Parse, Convert etc
        /// </summary>
        /// <param name="validationFunction"></param>
        /// <param name="errorToThrow"></param>
        public static void ValidateTry(Action validationFunction, Exception errorToThrow)
        {
            try
            {
                validationFunction();
            }
            catch(Exception)
            {
                throw errorToThrow;
            }
        }

       
        
    }
}
