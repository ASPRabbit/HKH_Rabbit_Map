using ServiceStack.Redis;

namespace HKH_Rabbit_Map.DataCollection.Utility
{
    public class RedisHelper
    {
        /// <summary>
        /// 创建PooledRedisClientManager并返回
        /// </summary>
        /// <returns>PooledRedisClientManager</returns>
        public static PooledRedisClientManager CreateRedisPool()
        {
            var rwHosts = new string[] { "127.0.0.1:6379" };
            var rHosts = new string[] { };

            var redisPool = new PooledRedisClientManager(rwHosts, rHosts, new RedisClientManagerConfig()
            {
                MaxReadPoolSize = 4,
                MaxWritePoolSize = 10,
                AutoStart = true
            });
            return redisPool;
        }
    }
}
