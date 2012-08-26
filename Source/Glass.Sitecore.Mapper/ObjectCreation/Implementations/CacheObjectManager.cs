using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.ObjectCaching;
using Diagnostics = global::Sitecore.Diagnostics;
using System.Threading;
using Sitecore.Diagnostics;
using SitecoreConfiguration = global::Sitecore.Configuration;
using Castle.DynamicProxy;
using Glass.Sitecore.Mapper.Proxies;

namespace Glass.Sitecore.Mapper.ObjectCreation.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheObjectManager : ObjectManager, IObjectManager
    {
        #region Private Properties
        private readonly ObjectCache _objectCache;
        private static readonly TimeSpan Timeout;
        #endregion

        public static ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();
 

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
        public CacheObjectManager(ObjectCache objectCache)
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
        public override object CreateClass(ClassLoadingState state)
        {
            object returnObject = null;


            //hopefully we can return the object from the cache
            returnObject = _objectCache.Get(state.Item, state.Type);

            //if we can't
            if (returnObject == null)
            {


                //get the object 
                // can I move this in to the SaveToCache call?? ***************** Aaron look here!


                if (Lock.TryEnterWriteLock(Timeout))
                {
                    try
                    {
                        if (_objectCache.Contains(state.Item, state.Type))
                            //check that someone else hasn't just added it
                            returnObject = _objectCache.Get(state.Item, state.Type);

                        //do a double check encase for some reason the returned object is null
                        if (returnObject == null)
                        {

                            //remove any existing item
                            _objectCache.Remove(state.Item, state.Type);
                            //create the new one
                            returnObject = CreateObject(state);
                            //and save it to the cache
                            _objectCache.Add(state.Item, state.Type, returnObject,state.Related);
                        }
                    }
                    finally
                    {
                        //if all else fails just create a normal object
                        if(returnObject == null)
                            returnObject = CreateObject(state);
                        Lock.ExitWriteLock();
                    }
                }
            }

            

            return ObjectCaching.Proxy.CacheProxyGenerator.CreateProxy(returnObject);
        }


        #endregion


    }
}
