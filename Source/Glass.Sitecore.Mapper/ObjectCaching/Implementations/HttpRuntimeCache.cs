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

        #region Public Override Methods

        /// <summary>
        /// Gets the object from cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public override object GetObjectFromCache(global::Sitecore.Data.Items.Item item)
        {
            return GetObjectFromCache(GetItemKey(item));
        }

        /// <summary>
        /// Gets the object from cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public override object GetObjectFromCache(object key)
        {
            //the key will be a string for HttpRuntime.Cache implementation
            var cacheKey = key.ToString();
            return System.Web.HttpRuntime.Cache.Get(cacheKey);
        }

        /// <summary>
        /// Saves the object to cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public override bool SaveObjectToCache(object key, ICacheableObject o)
        {
            return SaveToCache(o, key.ToString());
        }

       
        /// <summary>
        /// Saves the object to cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public override bool SaveObjectToCache(global::Sitecore.Data.Items.Item item, ICacheableObject o)
        {
            return SaveObjectToCache(GetItemKey(item), o);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool SaveObjectToCache(object key, object o)
        {
            //the key will be a string for HttpRuntime.Cache implementation
           return SaveToCache(o, key.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override object DeleteCache(object key)
        {
            return System.Web.HttpRuntime.Cache.Remove(key.ToString());
        }

        /// <summary>
        /// Publish the event.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public override bool PubishEvent(global::Sitecore.Data.Items.Item item)
        {
            var returnBool = false;
            CacheListInformation ci = null;
            if (CacheItemListLock.TryEnterReadLock(Timeout))
            {
                try
                {
                    ci = null;//CacheItemDictionary. [item.TemplateID.Guid];
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Error running publish event fro item {0} Error {1}", item.ID, ex.Message), this);
                }
                finally
                {
                    CacheItemListLock.ExitReadLock();
                }
            }

            if (ci != null)
            {
                //take out a write lock so we block any threads while we are trying to up date it
                if (ci.ListLock.TryEnterWriteLock(Timeout))
                {
                    try
                    {
                        var leftObject = GetObjectFromCache(item);

                        //its in the cache so lets update it
                        if (leftObject != null)
                        {
                            //we want to use the default ObjectManager as we don't want to check the cache or we will get the item we are trying to replace
                            var classManager = new ClassManager();
                            var sitecoreService = new SitecoreService(item.Database);
                            var rightObject = classManager.CreateClass(sitecoreService, false, false, ci.Type, item);

                            //CopyLeft(leftObject, rightObject);
                            returnBool = true;
                        }
                        else
                        {
                            Log.Info("This item is not in the cache ", this);
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error updating item in cache on publish", ex);
                    }
                    finally
                    {
                        ci.ListLock.ExitWriteLock();
                    }
                }
            }
            else
            {
                Log.Info("This template could not be found in the cache", this);
            }
            return returnBool;
        }

        /// <summary>
        /// Compares the keys.
        /// </summary>
        /// <param name="leftKey">The left key.</param>
        /// <param name="rightKey">The right key.</param>
        /// <returns></returns>
        public override bool CompareKeys(object leftKey, object rightKey)
        {
            return leftKey.ToString().Equals(rightKey.ToString());
        }

        /// <summary>
        /// Clears the related cache.
        /// </summary>
        /// <param name="ci">The ci.</param>
        public override void ClearRelatedCache(CacheListInformation ci)
        {
            //we are adding a new item and there is no way for knowing if it needed to be any of the related caches so just drop them
            foreach (var group in ci.RelatedCacheKeys.Values)
            {
                foreach (var key in group)
                {
                    DeleteCache(key);
                }
            }
            ci.RelatedCacheKeys.Clear();
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
            base.RemovedCallback(key, o as ICacheableObject, reason.ToString());
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Deletes the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        private void DeleteCache(string key)
        {
            System.Web.HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// Saves to cache.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns></returns>
        private bool SaveToCache(object o, string cacheKey)
        {
            bool returnBool = true;
            try
            {
                System.Web.HttpRuntime.Cache.Add(
                    cacheKey,
                    o,
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

        #endregion        
    }
}
