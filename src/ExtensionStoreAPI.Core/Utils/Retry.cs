namespace ExtensionStoreAPI.Core.Utils;

public static class Retry
    {
        /// <summary>
        /// Perform the given action and retry it if necessary
        /// </summary>
        /// <param name="action">The action to perform; a method to call</param>
        /// <param name="retryInterval">The TimeSpan to wait before trying again</param>
        /// <param name="maxAttemptCount">max number of times to try the action before returning an exception</param>
        public static void Do(Action action, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            _ = Do<object?>(() =>
            {
                action();
                return null;
            }, retryInterval, maxAttemptCount);
        }

        private static T Do<T>(Func<T> func, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            var exceptions = new List<Exception>();
            var attempted = 0;
            
            while (attempted < maxAttemptCount)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    return func();
                }
                catch (Exception ex)
                {
                    attempted++;
                    exceptions.Add(ex);
                }
            }
            throw new AggregateException(exceptions);
        }
    }