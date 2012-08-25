using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.ObjectCaching;
using Diagnostics = global::Sitecore.Diagnostics;
using System.Threading;
using Sitecore.Diagnostics;
using SitecoreConfiguration = global::Sitecore.Configuration;

namespace Glass.Sitecore.Mapper.ObjectCreation.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheObjectManager : ObjectManager, IObjectManager
    {
        #region Private Properties
        private readonly IObjectCache _objectCache;
        private static readonly ReaderWriterLockSlim CacheItemListLock = new ReaderWriterLockSlim();
        private static readonly TimeSpan Timeout;
        #endregion

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public static GlassCachingDictionary GlassCachingDictionary = new GlassCachingDictionary();

        #endregion

        #region Constructors/Destructors
        /// <summary>
        /// Initializes the <see cref="CacheObjectManager"/> class.
        /// </summary>
        static CacheObjectManager()
        {
            var timeSpan = SitecoreConfiguration.Settings.GetSetting("Glass.ObjectManager.ReadWriteLockTimeout", "0:0:30");
            Timeout = TimeSpan.Parse(timeSpan);
        }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheObjectManager"/> class.
        /// </summary>
        /// <param name="objectCache">The object cache.</param>
        public CacheObjectManager(IObjectCache objectCache)
        {
            Diagnostics.Assert.IsNotNull(objectCache, "objectCache can not be null");
            _objectCache = objectCache;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheObjectManager"/> class.
        /// </summary>
        public CacheObjectManager()
        {
            _objectCache = ObjectCacheFactory.Create();
        }

        #endregion

        #region Public Override Methods
        /// <summary>
        /// Returns a .NET object mapped from a Sitecore Item
        /// </summary>
        /// <param name="service"></param>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <param name="type"></param>
        /// <param name="item"></param>
        /// <param name="constructorParameters"></param>
        /// <returns></returns>
        public override object CreateClass(ISitecoreService service, bool isLazy, bool inferType, Type type, global::Sitecore.Data.Items.Item item, params object[] constructorParameters)
        {
            var key = _objectCache.GetItemKey(item);

            //hopefully we can return the object from the cache
            var returnObject = _objectCache.GetObjectFromCache(key) as ICacheableObject;

            //if we can't
            if (returnObject == null)
            {
                //get the object 
                // can I move this in to the SaveToCache call?? ***************** Aaron look here!
                var createdObject = CreateObject(service, isLazy, inferType, type, item, constructorParameters);
                returnObject = new CacheableObject(item, createdObject, key);

                //and save it to the cache
                 SaveToCache(returnObject, type, key);
            }

            return ObjectCaching.Proxy.CacheProxyGenerator.CreateProxy(returnObject.CachedObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="o"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool AddRelatedCache(string key, string group, object o, Type type)
        {
            var returnBool = false;
            CacheListInformation ci = null;
            if (CacheItemListLock.TryEnterReadLock(Timeout))
            {
                try
                {
                    ci = GlassCachingDictionary.Get(type);
                }
                catch (Exception ex)
                {
                    Log.Error("Glass.Sitecore.Mapper.ObjectCreation.Implementations.CacheObjectManager: Error building object", ex);
                }
                finally
                {
                    CacheItemListLock.ExitReadLock();
                }
            }

            if (ci != null)
            {
                //take out a write lock here as we might be modifying the list
                if (ci.ListLock.TryEnterWriteLock(Timeout))
                {
                    try
                    {
                        var cacheKey = String.Format("{0}_{1}", group, key);
                        returnBool = true;
                        if (!ci.RelatedCacheKeys.ContainsKey(group))
                        {
                            ci.RelatedCacheKeys.Add(group, new List<string>());
                            ci.RelatedCacheKeys[group].Add(cacheKey);
                        }
                        else
                        {
                            if (ci.RelatedCacheKeys[group].FirstOrDefault(x => x == cacheKey) == null)
                            {
                                ci.RelatedCacheKeys[group].Add(cacheKey);
                            }
                        }
                        _objectCache.SaveObjectToCache(cacheKey, o);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Glass.Sitecore.Mapper.ObjectCreation.Implementations.CacheObjectManager: Error building object", ex);
                    }
                    finally
                    {
                        ci.ListLock.ExitWriteLock();
                    }
                }
            }

            return returnBool;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool ClearRelatedCache(Type type, string group)
        {
            var returnBool = false;
            CacheListInformation ci = null;
            if (CacheItemListLock.TryEnterReadLock(Timeout))
            {
                try
                {
                    ci = GlassCachingDictionary.Get(type);
                }
                catch (Exception ex)
                {
                    Log.Error("Glass.Sitecore.Mapper.ObjectCreation.Implementations.CacheObjectManager: Error building object", ex);
                }
                finally
                {
                    CacheItemListLock.ExitReadLock();
                }
            }

            if (ci != null)
            {
                //take out a write lock here as we might be modifying the list
                if (ci.ListLock.TryEnterWriteLock(Timeout))
                {
                    try
                    {
                        if (ci.RelatedCacheKeys.ContainsKey(group))
                        {
                            foreach (var cacheKey in ci.RelatedCacheKeys[group])
                            {
                                _objectCache.DeleteCache(cacheKey);
                            }
                            //just remove the group and let the Garbage Collector deal with the reset :)
                            ci.RelatedCacheKeys.Remove(group);
                        }

                        returnBool = true;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Glass.Sitecore.Mapper.ObjectCreation.Implementations.CacheObjectManager: Error building object", ex);
                    }
                    finally
                    {
                        ci.ListLock.ExitWriteLock();
                    }
                }
            }


            return returnBool;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Saves to cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="cacheableObject">The created object.</param>
        /// <param name="type">The type.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private bool SaveToCache(ICacheableObject cacheableObject, Type type, object key)
        {
            //if createdObject is null stop here as there is no point caching a null object
            if (cacheableObject == null)
                return false;

            Diagnostics.Assert.IsNotNull(cacheableObject, "we need the cacheableObject to check the cache or we will not be able to generate a key");
            Diagnostics.Assert.IsNotNull(type, "we need an type to check the cache or we will not be able to generate a key");

            var returnBool = true;
            CacheListInformation ci = GetCacheListInformation(cacheableObject.TemplateID);

            //if thi is null then there is no CacheListInformation for this type
            if (ci == null)
            {
                ci = AddedCacheListInformation(cacheableObject, type);
            }

            //only keep going if ci not not null now.  If it is not then something happened when trying to build and add it
            if (ci != null)
            {
                //take out a writer lock so that we block any other threads when we do our work
                if (ci.ListLock.TryEnterWriteLock(Timeout))
                {
                    try
                    {
                        //once the lock is release just check that the thread before hand has not just done the work we want to do
                        //this is safe as we are in a write lock
                        var i = ci.Ids.SingleOrDefault(x => x == cacheableObject.ItemID);
                        if (i == Guid.Empty)
                        {
                            _objectCache.SaveObjectToCache(key, cacheableObject);
                            ci.Ids.Add(cacheableObject.ItemID);
                            ci.CachedObjectInformationList.Add(cacheableObject.ToCachedObjectInformation());
                        }
                        else
                        {
                            _objectCache.SaveObjectToCache(key, cacheableObject);
                            ci.CachedObjectInformationList.Add(cacheableObject.ToCachedObjectInformation());
                        }
                    }
                    catch (Exception ex)
                    {
                        //log and error
                        Log.Error("Glass.Sitecore.Mapper.ObjectCreation.Implementations.CacheObjectManager: Error building object", ex);
                        returnBool = false;
                    }
                    finally
                    {
                        //before releasing the lock check to see if we have one out on the object as we could have had a time out exception
                        if (ci.ListLock.IsWriteLockHeld)
                        {
                            //release the lock so we don't get a dead lock
                            ci.ListLock.ExitWriteLock();
                        }
                    }
                }
            }
            else
            {
                returnBool = false;
            }

            return returnBool;
        }


        /// <summary>
        /// Addeds the cache list information.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private CacheListInformation AddedCacheListInformation(ICacheableObject cacheableObject, Type type)
        {
            CacheListInformation ci = null;
            if (CacheObjectManager.CacheItemListLock.TryEnterWriteLock(Timeout))
            {
                try
                {
                    //just check again now that we are in the read lock, it may have been added before the loc was taken out
                    // this is save as we are in a write lock
                    ci = GlassCachingDictionary.Get(cacheableObject.TemplateID);
                    if (ci == null)
                    {
                        ci = new CacheListInformation();
                        ci.Type = type;
                        ci.TemplateID = cacheableObject.TemplateID;
                        GlassCachingDictionary.Add(cacheableObject.TemplateID, ci);
                    }
                }
                catch (Exception ex)
                {
                    var message = "Error adding cache information for type {0}";
                    Log.Error(string.Format(message, type.ToString()), ex);
                }
                finally
                {
                    CacheItemListLock.ExitWriteLock();
                }
            }
            return ci;
        }

        /// <summary>
        /// Gets the cache list information.
        /// </summary>
        /// <param name="templateID">The template ID.</param>
        /// <returns></returns>
        private CacheListInformation GetCacheListInformation(Guid templateID)
        {
            CacheListInformation ci = null;

            if (CacheObjectManager.CacheItemListLock.TryEnterReadLock(Timeout))
            {
                try
                {
                    ci = GlassCachingDictionary.Get(templateID);
                }
                catch (Exception ex)
                {
                    Log.Error(String.Format("Glass.Sitecore.Mapper.ObjectCreation.CacheObjectManager: Error building object Error: {0}", ex.Message), this);
                }
                finally
                {
                    CacheItemListLock.ExitReadLock();
                }
            }

            return ci;
        }

        #endregion
    }
}
