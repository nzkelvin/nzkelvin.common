using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nzkelvin.Common.Cache
{
    public interface ICache
    {
        void Add<T>(string key, T value);
        T Get<T>(string key);
    }

    public static class Extensions
    {
        private static string TRANSACTION_LOCK_PREFIX = "cacheItemTransactionLock_";

        //To use this method. You make a new method on the fly and use the method immediately.
        /*
         var getRules = _cacheService.Retrieve<string, ApplicationValidationRules>(key => key, key => GetApplicationValidationRules())(CACHE_KEY_PREFIX + "AppValidationRules");
         */
        public static Func<TArg, TResult> Retrieve<TArg, TResult>(this ICache cacheService,
                                                                    Func<TArg, string> createCacheKey,
                                                                    Func<TArg, TResult> getItem)
        {
            return arg =>
            {
                var key = createCacheKey(arg);
                var lockKey = TRANSACTION_LOCK_PREFIX + key;

                var value = cacheService.Get<TResult>(key);
                if (value == null)
                {
                    var lockObj = GetTransactionLockObject(lockKey, cacheService); // Garenteed fast
                    lock (lockObj)
                    {
                        if (value == null)
                        {
                            value = getItem(arg); // Potentially can be very slow.
                            cacheService.Add<TResult>(key, value);
                        }
                    }
                }

                return value;
            };
        }

        private static Object GetTransactionLockObject(string lockObjKey, ICache cacheService)
        {
            var lockObj = cacheService.Get<Object>(lockObjKey);
            if (lockObj != null)
                return lockObj;

            lockObj = new Object();
            cacheService.Add(lockObjKey, lockObj);
            return lockObj;
        }
    }
}
