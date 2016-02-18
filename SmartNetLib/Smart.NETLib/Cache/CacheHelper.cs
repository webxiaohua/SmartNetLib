using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Collections;

namespace Smart.NETLib
{
    /// <summary>
    /// @Author:webxiaohua
    /// @QQ:1584004295
    /// @Date:2016-01-22
    /// @Desc:文件读写操作类
    /// </summary>
    public class CacheHelper<T>
    {
        #region  全局变量
        private static CacheHelper<T> _instance = null;
        private static readonly object _instanceLock = new object();
        #endregion

        #region 局部变量
        private int Minutes = 60;
        private int Hour = 60 * 60;
        private int Day = 60 * 60 * 24;
        private HttpContext context = HttpContext.Current; //当前管道
        #endregion

        #region 构造函数
        private CacheHelper()
        {

        }
        #endregion

        #region  属性
        /// <summary>         
        ///根据key获取value     
        /// </summary>         
        /// <value></value>      
        public T this[string key]
        {
            get { return (T)HttpRuntime.Cache[key]; }
        }
        #endregion

        #region 公开函数
        /// <summary>
        /// 检查是否包含key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get(string key)
        {
            return (T)HttpRuntime.Cache.Get(key);
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static CacheHelper<T> GetInstance()
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new CacheHelper<T>();
                    }
                }
            }
            return _instance;
        }
        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, T value) {
            Add(key, value, Minutes * 20);
        }

        /// <summary>         
        /// 插入缓存        
        /// </summary>         
        /// <param name="key"> key</param>         
        /// <param name="value">value</param>         
        /// <param name="cacheDurationInSeconds">过期时间单位秒</param>         
        public void Add(string key, T value, int cacheDurationInSeconds)
        {
            Add(key, value, cacheDurationInSeconds, CacheItemPriority.Default);
        }

        /// <summary>         
        /// 插入缓存.         
        /// </summary>         
        /// <param name="key">key</param>         
        /// <param name="value">value</param>         
        /// <param name="cacheDurationInSeconds">过期时间单位秒</param>         
        /// <param name="priority">缓存项属性</param>         
        public void Add(string key, T value, int cacheDurationInSeconds, CacheItemPriority priority)
        {
            HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddSeconds(cacheDurationInSeconds), System.Web.Caching.Cache.NoSlidingExpiration, priority, null);
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key) {
            HttpRuntime.Cache.Remove(key);
        }
        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public void RemoveAll() {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }
            foreach (string key in al)
            {
                cache.Remove(key);
            }
        }
        /// <summary>
        /// 删除包含关键字的缓存
        /// </summary>
        /// <param name="removeExpression"> string  关键字</param>
        public void RemoveAll(Func<string, bool> removeExpression) {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            var allKeyList = GetAllKeys();
            var delKeyList = allKeyList.Where(removeExpression).ToList();
            foreach (var key in delKeyList)
            {
                Remove(key);
            }
        }

        public IEnumerable<string> GetAllKeys() {
            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                yield return CacheEnum.Key.ToString();
            }
            yield break; //可以不加
        }
        #endregion
    }
}
