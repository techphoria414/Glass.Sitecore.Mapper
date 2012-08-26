using System;
using System.Linq;
using System.Web.Caching;
using Sitecore.Diagnostics;
using SitecoreConfiguration = Sitecore.Configuration;
using Glass.Sitecore.Mapper.ObjectCreation.Implementations;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.ObjectCaching.Implementations
{
    /// <summary>
    /// This uses System.Web.HttpRuntime.Cache as its cache, and is the most basic example, 
    /// more about this can be read at http://msdn.microsoft.com/en-us/library/system.web.caching.cache(v=vs.100).aspx
    /// 
    /// We have very little control over what it does but it is simple to use
    /// 
    /// Thing worth noting:
    ///  --> It is static and there is only one.  So I don't think using this is a good idea for large complex projects
    ///  --> The system can and will just remove your items at will but you can set priority http://msdn.microsoft.com/en-us/library/system.web.caching.cacheitempriority(v=vs.100).aspx
    /// </summary>
    public class HttpRuntimeCache : ObjectCache
    {
        #region Private Properties

        //remembers if the setting have be read.  As they are static we don't need to populate them each time we create this
        private readonly bool _settingsRead;
        private static CacheItemPriority _cacheItemPriority;
        private static DateTime _cacheItemLifeTime;
        private static TimeSpan _cacheItemSlidingExpiration;
        #endregion

        #region Public Properties

        //Nothing here :)

        #endregion

        #region Constructors/Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRuntimeCache"/> class.
        /// it gets all the setting from the config file
        /// I am comparing a sting as different caching Implementations may expect different things
        /// </summary>
        public HttpRuntimeCache()
        {
            //i don't want to use a static constructor as this may not need to be run
            if (!_settingsRead)
            {
                //get the priority that items that are added to the cache will have
                _cacheItemPriority = (CacheItemPriority)Enum.Parse(typeof(CacheItemPriority), GlassConfiguration.Config.CacheItemPriority);

                //gets the life time of the items that will be added to the cache.  
                if (GlassConfiguration.Config.CacheItemLifeTime.Equals("NoAbsoluteExpiration", StringComparison.InvariantCultureIgnoreCase))
                {
                    //the item will not expire
                    _cacheItemLifeTime = Cache.NoAbsoluteExpiration;
                }
                else
                {
                    //the item will expire at a time not + the lifetime set in the config
                    _cacheItemLifeTime = DateTime.Now.Add(TimeSpan.Parse(GlassConfiguration.Config.CacheItemLifeTime));
                }

                //will the item life extend if the object is accessed?
                if (!TimeSpan.TryParse(GlassConfiguration.Config.CacheItemSlidingExpiration, out _cacheItemSlidingExpiration))
                {
                    _cacheItemSlidingExpiration = Cache.NoSlidingExpiration;
                }

                //set settingRead to true so we don't try to get the 
                _settingsRead = true;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <param name="reason"></param>
        public void RemovedCallback(string key, object o, CacheItemRemovedReason reason)
        {
           //TODO
        }
        #endregion

        protected override CachedObjectInformation GetInternal(CacheKey key)
        {
            return System.Web.HttpRuntime.Cache.Get(key.ToString()) as CachedObjectInformation;
        }

        protected override bool AddInternal(CacheKey key, CachedObjectInformation info)
        {
            bool returnBool = true;
            try
            {
                System.Web.HttpRuntime.Cache.Add(
                    key.ToString(),
                    info,
                    null,
                    _cacheItemLifeTime,
                    _cacheItemSlidingExpiration,
                    _cacheItemPriority,
                    RemovedCallback);
            }
            catch (Exception ex)
            {
                returnBool = false;
                Log.Error(String.Format("Error saving object to HttpRuntime.Cache Exception: {0}", ex.Message), this);
            }
            return returnBool;
        }

        protected override bool RemoveInternal(CacheKey key)
        {
             System.Web.HttpRuntime.Cache.Remove(key.ToString());
             return true;
        }

        protected override bool ContainsInternal(CacheKey key)
        {
            return GetInternal(key) != null;
        }
    }
}
