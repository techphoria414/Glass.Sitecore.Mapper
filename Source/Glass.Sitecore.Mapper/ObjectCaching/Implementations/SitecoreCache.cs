using System;
using System.Collections.Generic;
using Sitecore.Caching;
using Sitecore.Diagnostics;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.ObjectCaching.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class SitecoreCache : ObjectCache
    {
        #region Private Properties
        private readonly Cache sitecoreCache;

        private static CachePriority cacheItemPriority;
        private static bool settingsRead;
        private static DateTime cacheItemLifeTime;
        private static TimeSpan cacheItemSlidingExpiration;
        #endregion

        #region Public Properties

        //Nothing here :)

        #endregion

        #region Constructors/Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreCache"/> class.
        /// </summary>
        public SitecoreCache()
        {
           sitecoreCache = Cache.GetNamedInstance(
                            "Glass.ObjectManager.ObjectCaching.SitecoreCache",
                            global::Sitecore.StringUtil.ParseSizeString(GlassConfiguration.Config.CacheItemSlidingExpiration));
            

           //i don't want to use a static constructor as this may not need to be run
           if (!settingsRead)
           {

               cacheItemPriority = (CachePriority)Enum.Parse(typeof(CachePriority), GlassConfiguration.Config.CacheItemPriority);

               if (GlassConfiguration.Config.CacheItemLifeTime.Equals("NoAbsoluteExpiration", StringComparison.InvariantCultureIgnoreCase))
               {
                   cacheItemLifeTime = System.Web.Caching.Cache.NoAbsoluteExpiration;
               }
               else
               {
                   var cacheItemLifeTimeTimeSpan = TimeSpan.Parse(GlassConfiguration.Config.CacheItemLifeTime);
                   cacheItemLifeTime = DateTime.Now.Add(cacheItemLifeTimeTimeSpan);
               }


               if (!bool.Parse(GlassConfiguration.Config.CacheItemSlidingExpiration))
               {
                   cacheItemSlidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
               }

               //set settingRead to true so we don't try to get the 
               settingsRead = true;
           }

           sitecoreCache.DefaultPriority = cacheItemPriority;
        }

        #endregion

        #region Public Override Methods

        /// <summary>
        /// This is not available when using the Sitecore cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <param name="reason"></param>
        public override void RemovedCallback(object key, ICacheableObject o, string reason)
        {
            throw new NotImplementedException();
        }

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
            return sitecoreCache.GetValue(cacheKey);
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
        /// Saves the object to cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public override bool SaveObjectToCache(object key, ICacheableObject o)
        {
            var returnBool = true;
            long cacheSize;
            try
            {
                var mem = new System.IO.MemoryStream();
                var binFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binFormatter.Serialize(mem, o);
                cacheSize = mem.Length + 500; // increase just in case
            }
            catch (Exception ex)
            {
                cacheSize = 1500; // default size we have made it bigger than normal just in case

                Log.Warn(string.Format("Cache - Size Serialize: '{0}'", key), ex, this);
                
                // if the object is a collection then we need to take into account the number of items it contains
                Type tType = o.GetType();

                // the type is a collection
                if (typeof(System.Collections.ICollection).IsAssignableFrom(tType)
                    || typeof(ICollection<>).IsAssignableFrom(tType))
                {
                    // we want to try and see if the item is a collection
                    try
                    {
                        // set the data as ICollection so we can get the data
                        var iEnum = (o as System.Collections.ICollection);

                        // make sure it casts correctly
                        if (iEnum != null)
                        {
                            // we need to set this as it will cause issues
                            long fakeCacheSize = cacheSize;
                            cacheSize = iEnum.Count * fakeCacheSize;
                        }
                    }
                    catch (Exception exer)
                    {
                        cacheSize = 5000; // at least set it bigger just in case

                        // do we display the logs
                        Log.Warn(string.Format("Cache - Collection Count: '{0}'", key), exer, this);
                    }
                }
            }

            try
            {
                var cacheEntry = sitecoreCache.Add(key.ToString(),o, cacheSize, cacheItemSlidingExpiration);
                
                cacheEntry.AbsoluteExpiration = cacheItemLifeTime;
                cacheEntry.EntryRemoved += CacheEntryEntryRemoved;

            }
            catch
            {
                returnBool = false;
            }

            return returnBool;
        }

        /// <summary>
        /// Caches the entry entry removed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The instance containing the event data.</param>
        public void CacheEntryEntryRemoved(object sender, EntryRemovedEventArgs e)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Pubishes the event.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public override bool PubishEvent(global::Sitecore.Data.Items.Item i)
        {
            throw new NotImplementedException();
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
        /// 
        /// </summary>
        /// <param name="ci"></param>
        public override void ClearRelatedCache(CacheListInformation ci)
        {
            throw new NotImplementedException();
        }

        public override bool SaveObjectToCache(object key, object o)
        {
            throw new NotImplementedException();
        }

        public override object DeleteCache(object key)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
