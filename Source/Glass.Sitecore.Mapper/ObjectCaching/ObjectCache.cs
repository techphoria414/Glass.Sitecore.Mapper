using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Sitecore.Diagnostics;
using System.Threading;
using Sitecore.Data.Items;
using SitecoreConfiguration = Sitecore.Configuration;
using System.Linq;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.ObjectCaching
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ObjectCache : IObjectCache
    {
        #region Private Properties
        private const string BaseKey = "GLASS";
        #endregion

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public static List<CacheListInformation> CacheItemList = new List<CacheListInformation>();

        /// <summary>
        /// 
        /// </summary>
        public static ReaderWriterLockSlim CacheItemListLock = new ReaderWriterLockSlim();

        /// <summary>
        /// this is used for th
        /// </summary>
        protected static readonly TimeSpan Timeout;

        #endregion

        #region Constructors/Destructors
        /// <summary>
        /// Initializes the <see cref="ObjectCache"/> class.
        /// </summary>
        static ObjectCache()
        {
            Timeout = TimeSpan.Parse(GlassConfiguration.Config.ReadWriteLockTimeout);
        }
        #endregion

        #region Public Virtual Methods
        /// <summary>
        /// Gets the item key.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// a cache key for that represents an item
        /// </returns>
        public virtual object GetItemKey(Item item)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}_", BaseKey);
            sb.AppendFormat("{0}_", item.ID);
            sb.AppendFormat("{0}_", item.Language.Name);
            sb.AppendFormat("{0}_", item.Database.Name);

            //don't add a _ as this is the last item in the key
            sb.AppendFormat("{0}", item.Version.Number);

            return sb.ToString();
        }

        /// <summary>
        /// Copies the left.
        /// </summary>
        /// <param name="leftObject">The left object.</param>
        /// <param name="rightObject">The right object.</param>
        /// <returns></returns>
        protected virtual bool CopyLeft(object leftObject, object rightObject)
        {
            var returnbool = true;
            if (leftObject.GetType() == rightObject.GetType())
            {
                PropertyInfo[] properties = leftObject.GetType().GetProperties();
                //loop though all the properties
                foreach (PropertyInfo property in properties)
                {
                    if (property.CanWrite)
                    {
                        try
                        {
                            object leftValue = property.GetValue(leftObject, null);
                            object rightValue = property.GetValue(rightObject, null);

                            //set the properties values for the right to the left object
                            property.SetValue(leftObject, rightValue, null);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(String.Format("Glass.Sitecore.Mapper.ObjectCaching.ObjectCache.CopyLeft Could not update property: {0}", property.Name), ex);
                            returnbool = false;
                        }
                    }
                }
            }

            return returnbool;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <param name="reason"></param>
        public virtual void RemovedCallback(object key, ICacheableObject o, string reason)
        {
            CacheListInformation ci = null;
            //grab the CacheListInformation object
            if (CacheItemListLock.TryEnterReadLock(Timeout))
            {
                try
                {
                    ci = CacheItemList.SingleOrDefault(x => x.TemplateID == o.TemplateID);
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Glass.Sitecore.Mapper.ObjectCaching.ObjectCache: Error Removing {0} form cache", key), ex);
                }
                finally
                {
                    CacheItemListLock.ExitReadLock();
                }
            }

            if (ci != null)
            {
                if (ci.ListLock.TryEnterWriteLock(Timeout))
                {
                    try
                    {
                        //find the id that needs to be removed
                        var guid = ci.Ids.SingleOrDefault(x => x == o.ItemID);

                        //remove it
                        ci.Ids.Remove(guid);

                        ClearRelatedCache(ci);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("Glass.Sitecore.Mapper.ObjectCaching.ObjectCache: Error Removing {0} form cache", key), ex);
                    }
                    finally
                    {
                        ci.ListLock.ExitWriteLock();
                    }
                }
            }
        }
        #endregion

        #region Public Abstract Methods
        /// <summary>
        /// Gets the object from cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public abstract object GetObjectFromCache(Item item);
        /// <summary>
        /// Gets the object from cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public abstract object GetObjectFromCache(object key);
        /// <summary>
        /// Saves the object to cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public abstract bool SaveObjectToCache(Item item, ICacheableObject o);
        /// <summary>
        /// Saves the object to cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public abstract bool SaveObjectToCache(object key, ICacheableObject o);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public abstract bool SaveObjectToCache(object key, object o);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public abstract object DeleteCache(object key);

        /// <summary>
        /// Pubishes the event.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public abstract bool PubishEvent(Item i);
        /// <summary>
        /// Compares the keys.
        /// </summary>
        /// <param name="leftKey">The left key.</param>
        /// <param name="rightKey">The right key.</param>
        /// <returns></returns>
        public abstract bool CompareKeys(object leftKey, object rightKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ci"></param>
        public abstract void ClearRelatedCache(CacheListInformation ci);

        #endregion

    }
}
