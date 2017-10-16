using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using ConvertHelper = WeihanLi.Common.Helpers.ConvertHelper;

namespace PowerControlDemo.Helper
{
    public class RedisHelper
    {
        private static readonly string redisConn;
        private static ConnectionMultiplexer connection;
        private static readonly int dataBaseIndex = 0;
        private static IDatabase db = null;
        private static object asyncState = new object();

        static RedisHelper()
        {
            redisConn = System.Configuration.ConfigurationManager.AppSettings["redisConf"];
            connection = ConnectionMultiplexer.Connect(redisConn);
            db = connection.GetDatabase(dataBaseIndex, asyncState);
        }

        #region Cache

        #region Exists

        public static bool Exists(string key, CommandFlags flags = CommandFlags.None)
        {
            return db.KeyExists(key, flags);
        }

        public static async Task<bool> ExistsAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            return await db.KeyExistsAsync(key, flags);
        }

        #endregion Exists

        #region Get

        public static string Get(string key, CommandFlags flags = CommandFlags.None)
        {
            return db.StringGet(key, flags);
        }

        public static async Task<string> GetAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            return await db.StringGetAsync(key, flags);
        }

        public static T Get<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            return ConvertHelper.JsonToObject<T>(Get(key, flags));
        }

        public static async Task<T> GetAsync<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            return ConvertHelper.JsonToObject<T>(await GetAsync(key, flags));
        }

        #endregion Get

        #region Set

        public static bool Set<T>(string key, T value) =>
            Set(key, value, null);

        public static bool Set<T>(string key, T value, TimeSpan? expiration) =>
            Set(key, ConvertHelper.ObjectToJson(value), expiration);

        public static bool Set(string key, string value) =>
            Set(key, value, null);

        public static bool Set(string key, string value, TimeSpan? expiration) =>
            Set(key, value, expiration, When.Always);

        public static bool Set(string key, string value, TimeSpan? expiration, When when) =>
            Set(key, value, expiration, when, CommandFlags.None);

        public static bool Set(string key, string value, TimeSpan? expiration, When when, CommandFlags flags) =>
            db.StringSet(key, value, expiration ?? TimeSpan.FromDays(7), when, flags);

        public static Task<bool> SetAsync<T>(string key, T value) =>
            SetAsync(key, value, null);

        public static Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration) =>
            SetAsync(key, ConvertHelper.ObjectToJson(value), expiration);

        public static Task<bool> SetAsync(string key, RedisValue value) =>
            SetAsync(key, value, null);

        public static Task<bool> SetAsync(string key, RedisValue value, TimeSpan? expiration) =>
            SetAsync(key, value, expiration, When.Always);

        public static Task<bool> SetAsync(string key, RedisValue value, TimeSpan? expiration, When when) =>
            SetAsync(key, value, expiration, when, CommandFlags.None);

        public static async Task<bool> SetAsync(string key, RedisValue value, TimeSpan? expiration, When when, CommandFlags flags) =>
            await db.StringSetAsync(key, value, expiration ?? TimeSpan.FromDays(7), when, flags);

        #endregion Set

        #region Increment

        public static long Increment(string key) =>
            Increment(key, 1);

        public static long Increment(string key, long value) =>
            Increment(key, value, CommandFlags.None);

        public static long Increment(string key, long value, CommandFlags flags) =>
            db.StringIncrement(key, value, flags);

        public static Task<long> IncrementAsync(string key) =>
            IncrementAsync(key, 1);

        public static Task<long> IncrementAsync(string key, long value) =>
            IncrementAsync(key, value, CommandFlags.None);

        public static Task<long> IncrementAsync(string key, long value, CommandFlags flags) =>
            db.StringIncrementAsync(key, value, flags);

        #endregion Increment

        #region Decrement

        public static long Decrement(string key) =>
            Decrement(key, 1);

        public static long Decrement(string key, long value) =>
            Decrement(key, value, CommandFlags.None);

        public static long Decrement(string key, long value, CommandFlags flags) =>
            db.StringDecrement(key, value, flags);

        public static Task<long> DecrementAsync(string key) =>
            DecrementAsync(key, 1);

        public static Task<long> DecrementAsync(string key, long value) =>
            DecrementAsync(key, value, CommandFlags.None);

        public static Task<long> DecrementAsync(string key, long value, CommandFlags flags) =>
            db.StringDecrementAsync(key, value, flags);

        #endregion Decrement

        #region Remove

        public static bool Remove(string key) =>
            Remove(key, CommandFlags.None);

        public static bool Remove(string key, CommandFlags flags) =>
            db.KeyDelete(key, flags);

        public static bool RemoveSafely(string key) =>
            RemoveSafely(key, CommandFlags.None);

        public static bool RemoveSafely(string key, CommandFlags flags)
        {
            if (Exists(key))
            {
                return Remove(key, flags);
            }
            return true;
        }

        public static Task<bool> RemoveAsync(string key) =>
            RemoveAsync(key, CommandFlags.None);

        public static Task<bool> RemoveAsync(string key, CommandFlags flags) =>
            db.KeyDeleteAsync(key, flags);

        #endregion Remove

        #endregion Cache
    }
}