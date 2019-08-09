using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetLittleHelpers
{
    /// <summary>
    /// Simple retry mechanism
    /// </summary>
    // ReSharper disable once CommentTypo
    // ReSharper disable once IdentifierTypo - Retrier is a very nice word for something that retries!
    public static class Retrier
    {
        #region Synchronous methods

        #region Retry Void methods
        /// <summary>
        ///     Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <returns></returns>
        public static void Retry(Action action, TimeSpan retryInterval, int maxAttemptCount = 3, decimal retryIntervalMultiplier = 1, params Type[] exceptionTypeWhitelist)
        {
            Retry(action, retryInterval, maxAttemptCount, retryIntervalMultiplier, exceptionTypeWhitelist, null);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <param name="exceptionTypeBlacklist">Never retry on these exceptions</param>
        /// <returns></returns>
        public static void Retry(Action action, TimeSpan retryInterval, int maxAttemptCount, decimal retryIntervalMultiplier, IEnumerable<Type> exceptionTypeWhitelist, IEnumerable<Type> exceptionTypeBlacklist)
        {
            List<TimeSpan> intervals = BuildIntervals(retryInterval, maxAttemptCount, retryIntervalMultiplier);
            var filterFunction = BuildFilters(exceptionTypeWhitelist, exceptionTypeBlacklist);
            Retry(action, filterFunction, intervals);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="isRetryAllowed">Function that evaluates the exception and specifies if its allowed or not</param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <returns></returns>
        public static void Retry(Action action, Func<Exception, bool> isRetryAllowed, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            Retry(action, isRetryAllowed, BuildIntervals(retryInterval, maxAttemptCount, 1));
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="isRetryAllowed">Function that evaluates the exception and specifies if its allowed or not</param>
        /// <param name="retryIntervals"></param>
        /// <returns></returns>
        public static void Retry(Action action, Func<Exception, bool> isRetryAllowed, IEnumerable<TimeSpan> retryIntervals)
        {
            var exceptions = new List<Exception>();
            bool firstAttemptDone = false;
            foreach (TimeSpan retryInterval in retryIntervals)
            {
                try
                {
                    if (firstAttemptDone)
                    {
                        Thread.Sleep(retryInterval);
                    }

                    firstAttemptDone = true;
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    if (!isRetryAllowed(ex))
                    {
                        throw;
                    }

                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
        #endregion Retry Void methods

        #region Retry Generic Type methods

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="isRetryAllowed">Function that evaluates the exception and specifies if its allowed or not</param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <returns></returns>
        public static T Retry<T>(Func<T> action, Func<Exception, bool> isRetryAllowed, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            return Retry(action, isRetryAllowed, BuildIntervals(retryInterval, maxAttemptCount, 1));
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <returns></returns>
        public static T Retry<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount = 3, decimal retryIntervalMultiplier = 1, params Type[] exceptionTypeWhitelist)
        {
            return  Retry(action, retryInterval, maxAttemptCount, retryIntervalMultiplier, exceptionTypeWhitelist, null);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <param name="exceptionTypeBlacklist">Never retry on these exceptions</param>
        /// <returns></returns>
        public static T Retry<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount, decimal retryIntervalMultiplier, IEnumerable<Type> exceptionTypeWhitelist, IEnumerable<Type> exceptionTypeBlacklist)
        {
            List<TimeSpan> intervals = BuildIntervals(retryInterval, maxAttemptCount, retryIntervalMultiplier);
            var filterFunction = BuildFilters(exceptionTypeWhitelist, exceptionTypeBlacklist);
            return Retry(action, filterFunction, intervals);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="isRetryAllowed">Function that evaluates the exception and specifies if its allowed or not</param>
        /// <param name="retryIntervals"></param>
        /// <returns></returns>
        public static T Retry<T>(Func<T> action, Func<Exception, bool> isRetryAllowed, IEnumerable<TimeSpan> retryIntervals)
        {
            var exceptions = new List<Exception>();
            bool firstAttemptDone = false;
            foreach (TimeSpan retryInterval in retryIntervals)
            {
                try
                {
                    if (firstAttemptDone)
                    {
                        Thread.Sleep(retryInterval);
                    }

                    firstAttemptDone = true;
                    return action();
                }
                catch (Exception ex)
                {
                    if (!isRetryAllowed(ex))
                    {
                        throw;
                    }

                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }

        #endregion Retry Generic Type methods

        #endregion Synchronous methods

        #region Async methods

        #region Retry Void methods
        /// <summary>
        ///     Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <returns></returns>
        public static async Task RetryAsync(Action action, TimeSpan retryInterval, int maxAttemptCount = 3, decimal retryIntervalMultiplier = 1, params Type[] exceptionTypeWhitelist)
        {
            await RetryAsync(action, retryInterval, maxAttemptCount, retryIntervalMultiplier, exceptionTypeWhitelist, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <param name="exceptionTypeBlacklist">Never retry on these exceptions</param>
        /// <returns></returns>
        public static async Task RetryAsync(Action action, TimeSpan retryInterval, int maxAttemptCount, decimal retryIntervalMultiplier, IEnumerable<Type> exceptionTypeWhitelist, IEnumerable<Type> exceptionTypeBlacklist)
        {
            List<TimeSpan> intervals = BuildIntervals(retryInterval, maxAttemptCount, retryIntervalMultiplier);
            var filterFunction = BuildFilters(exceptionTypeWhitelist, exceptionTypeBlacklist);
            await RetryAsync(action, filterFunction, intervals).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="isRetryAllowed">Function that evaluates the exception and specifies if its allowed or not</param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <returns></returns>
        public static async Task RetryAsync(Action action, Func<Exception, bool> isRetryAllowed, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            await RetryAsync(action, isRetryAllowed, BuildIntervals(retryInterval, maxAttemptCount, 1)).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="isRetryAllowed">Function that evaluates the exception and specifies if its allowed or not</param>
        /// <param name="retryIntervals"></param>
        /// <returns></returns>
        public static async Task RetryAsync(Action action, Func<Exception, bool> isRetryAllowed, IEnumerable<TimeSpan> retryIntervals)
        {
            await RetryTaskAsync<object>(() => {
                action();
                return Task.FromResult(default(object));
            }, isRetryAllowed, retryIntervals).ConfigureAwait(false);
        }
        #endregion Retry Void methods

        #region Retry Generic methods
        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="isRetryAllowed">Function that evaluates the exception and specifies if its allowed or not</param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <returns></returns>
        public static async Task<T> RetryAsync<T>(Func<T> action, Func<Exception, bool> isRetryAllowed, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            return await RetryAsync(action, isRetryAllowed, BuildIntervals(retryInterval, maxAttemptCount, 1)).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <returns></returns>
        public static async Task<T> RetryAsync<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount = 3, decimal retryIntervalMultiplier = 1, params Type[] exceptionTypeWhitelist)
        {
            return await RetryAsync(action, retryInterval, maxAttemptCount, retryIntervalMultiplier, exceptionTypeWhitelist, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <param name="exceptionTypeBlacklist">Never retry on these exceptions</param>
        /// <returns></returns>
        public static async Task<T> RetryAsync<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount, decimal retryIntervalMultiplier, IEnumerable<Type> exceptionTypeWhitelist, IEnumerable<Type> exceptionTypeBlacklist)
        {
            List<TimeSpan> intervals = BuildIntervals(retryInterval, maxAttemptCount, retryIntervalMultiplier);
            var filterFunction = BuildFilters(exceptionTypeWhitelist, exceptionTypeBlacklist);
            return await RetryAsync(action, filterFunction, intervals).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="isRetryAllowed">Function that evaluates the exception and specifies if its allowed or not</param>
        /// <param name="retryIntervals"></param>
        /// <returns></returns>
        public static async Task<T> RetryAsync<T>(Func<T> action, Func<Exception, bool> isRetryAllowed, IEnumerable<TimeSpan> retryIntervals)
        {
            var exceptions = new List<Exception>();
            bool firstAttemptDone = false;
            foreach (TimeSpan retryInterval in retryIntervals)
            {
                try
                {
                    if (firstAttemptDone)
                    {
                        await Task.Delay(retryInterval).ConfigureAwait(false);
                    }

                    firstAttemptDone = true;
                    return action();
                }
                catch (Exception ex)
                {
                    if (!isRetryAllowed(ex))
                    {
                        throw;
                    }

                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
        #endregion Retry Generic methods

        #region Retry Task methods
        /// <summary>
        ///     Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <returns></returns>
        public static async Task RetryTaskAsync(Func<Task> action, TimeSpan retryInterval, int maxAttemptCount = 3, decimal retryIntervalMultiplier = 1, params Type[] exceptionTypeWhitelist)
        {
            await RetryTaskAsync(action, retryInterval, maxAttemptCount, retryIntervalMultiplier, exceptionTypeWhitelist, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <param name="exceptionTypeBlacklist">Never retry on these exceptions</param>
        /// <returns></returns>
        public static async Task RetryTaskAsync(Func<Task> action, TimeSpan retryInterval, int maxAttemptCount, decimal retryIntervalMultiplier, IEnumerable<Type> exceptionTypeWhitelist, IEnumerable<Type> exceptionTypeBlacklist)
        {
            await RetryTaskAsync<object>(async () => {
                await action();
                return Task.FromResult(default(object));
            }, retryInterval, maxAttemptCount, retryIntervalMultiplier, exceptionTypeWhitelist, exceptionTypeBlacklist).ConfigureAwait(false);
        }

        #endregion Retry Task methods

        #region Retry Task<T> methods
        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <returns></returns>
        public static async Task<T> RetryTaskAsync<T>(Func<Task<T>> action, TimeSpan retryInterval, int maxAttemptCount = 3, decimal retryIntervalMultiplier = 1, params Type[] exceptionTypeWhitelist)
        {
            return await RetryTaskAsync(action, retryInterval, maxAttemptCount, retryIntervalMultiplier, exceptionTypeWhitelist, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <param name="action"></param>
        /// <param name="isRetryAllowed">Function that evaluates the exception and specifies if its allowed or not</param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <returns></returns>
        public static async Task<T> RetryTaskAsync<T>(Func<Task<T>> action, Func<Exception, bool> isRetryAllowed, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            return await RetryTaskAsync(action, isRetryAllowed, BuildIntervals(retryInterval, maxAttemptCount, 1)).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval"></param>
        /// <param name="maxAttemptCount"></param>
        /// <param name="retryIntervalMultiplier">Increase the delay between attempts by the specified multiplier on each retry</param>
        /// <param name="exceptionTypeWhitelist">Only retry on these exceptions</param>
        /// <param name="exceptionTypeBlacklist">Never retry on these exceptions</param>
        /// <returns></returns>
        public static async Task<T> RetryTaskAsync<T>(Func<Task<T>> action, TimeSpan retryInterval, int maxAttemptCount, decimal retryIntervalMultiplier, IEnumerable<Type> exceptionTypeWhitelist, IEnumerable<Type> exceptionTypeBlacklist)
        {
            List<TimeSpan> intervals = BuildIntervals(retryInterval, maxAttemptCount, retryIntervalMultiplier);
            var filterFunction = BuildFilters(exceptionTypeWhitelist, exceptionTypeBlacklist);
            return await RetryTaskAsync(action, filterFunction, intervals).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs the action trying several times and awaiting between the calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="isRetryAllowed">Function that evaluates the exception and specifies if its allowed or not</param>
        /// <param name="retryIntervals"></param>
        /// <returns></returns>
        public static async Task<T> RetryTaskAsync<T>(Func<Task<T>> action, Func<Exception, bool> isRetryAllowed, IEnumerable<TimeSpan> retryIntervals)
        {
            var exceptions = new List<Exception>();
            bool firstAttemptDone = false;
            foreach (TimeSpan retryInterval in retryIntervals)
            {
                try
                {
                    if (firstAttemptDone)
                    {
                        await Task.Delay(retryInterval).ConfigureAwait(false);
                    }

                    firstAttemptDone = true;
                    return await action().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    if (!isRetryAllowed(ex))
                    {
                        throw;
                    }

                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
        #endregion Retry Task<T> methods

        #endregion Async methods

        #region Helper methods
        private static Func<Exception, bool> BuildFilters(IEnumerable<Type> whitelist, IEnumerable<Type> blacklist)
        {

            Func<Exception, bool> func = (Exception ex) =>
            {
                if (whitelist != null)
                {
                    foreach (Type type in whitelist)
                    {
                        if (ex.GetType() != type)
                        {
                            return false;
                        }
                    }
                }
                if (blacklist != null)
                {
                    foreach (Type type in blacklist)
                    {
                        if (ex.GetType() == type)
                        {
                            return false;
                        }
                    }
                }

                return true;
            };

            return func;
        }

        private static List<TimeSpan> BuildIntervals(TimeSpan retryInterval, int maxAttemptCount, decimal retryIntervalMultiplier)
        {
            List<TimeSpan> intervals = new List<TimeSpan>();
            var initialInterval = retryInterval;
            for (int i = 0; i < maxAttemptCount; i++)
            {
                intervals.Add(initialInterval);
                initialInterval = TimeSpan.FromTicks((long)(initialInterval.Ticks * retryIntervalMultiplier));
            }

            return intervals;
        }

        #endregion Helper methods 
    }
}
