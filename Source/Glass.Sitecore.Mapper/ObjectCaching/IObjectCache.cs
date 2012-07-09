using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper.ObjectCaching
{
    /// <summary>
    /// 
    /// </summary>
    public interface IObjectCache
    {
        /// <summary>
        /// Removeds the callback.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="o">The o.</param>
        /// <param name="reason">The reason.</param>
        void RemovedCallback(object key, ICacheableObject o, string reason);

        /// <summary>
        /// Gets the object from cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        object GetObjectFromCache(Item item);


        /// <summary>
        /// Gets the object from cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        object GetObjectFromCache(object key);

        /// <summary>
        /// Saves the object to cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        bool SaveObjectToCache(global::Sitecore.Data.Items.Item item, ICacheableObject o);

        /// <summary>
        /// Saves the object to cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        bool SaveObjectToCache(object key, ICacheableObject o);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        bool SaveObjectToCache(object key, object o);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object DeleteCache(object key);

        /// <summary>
        /// Gets the item key.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        object GetItemKey(Item item);

        /// <summary>
        /// Pubishes the event.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        bool PubishEvent(Item i);

        /// <summary>
        /// Compares the keys.
        /// </summary>
        /// <param name="leftKey">The left key.</param>
        /// <param name="rightKey">The right key.</param>
        /// <returns></returns>
        bool CompareKeys(object leftKey, object rightKey);
        
    }
}
