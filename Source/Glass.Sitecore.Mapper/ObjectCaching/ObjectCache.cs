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
    public abstract class ObjectCache
    {
        #region Private Properties
        private const string BaseKey = "GLASS";
        #endregion

        #region Public Properties
      
        protected static readonly TimeSpan Timeout;
        static volatile RelatedDictionary _related = new RelatedDictionary();



        private static CacheKey CreateKey(Guid revisionId, string database, Type type)
        {

            return new CacheKey(revisionId, database, type);
        }

        public static CacheKey CreateKey(Item item, Type type)
        {

            return new CacheKey(item.RevisionId(), item.Database.Name, type);
        }

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

        #region Public Methods

        public object Get(Item item, Type type){
            if (Contains(item, type))
            {
                var key = CreateKey(item, type);
                return GetInternal(key).Object;
            }
            else
                return null;
        }


      
        public bool Add(Item item,Type type, object o, Guid relatedId){

            

            if(Contains(item, type))
                return false;
            else {
                var key = CreateKey(item, type);
                if (relatedId != Guid.Empty)
                {
                    _related.Add(relatedId, key);
                    _related.Add(item.TemplateID.Guid, key);
                }
                return AddInternal(key, new CachedObjectInformation(item, type, o));
            }
        }

        public bool Remove(Item item, Type type)
        {
            if (Contains(item, type))
            {
                var key = CreateKey(item, type);

                return Remove(key);
            }
            else
            {
                return false;
            }

        }
        private bool Remove(CacheKey key)
        {
                var related = _related.FlushKeys(key.RevisionId);
                
                foreach (var relate in related)
                {
                    var get = GetInternal(relate);
                    if (get != null)
                        Remove(CreateKey(get.RevisionID, get.Database, get.Type));
                }
                
                return RemoveInternal(key);
        }


        public bool Contains(Item item, Type type){
            var key = CreateKey(item, type);
            return ContainsInternal(key);
        }
        

        #endregion
        
        
        #region Public Abstract Methods
       
        /// <summary>
        /// Gets the object from cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected abstract CachedObjectInformation GetInternal(CacheKey key);
       
        /// <summary>
        /// Saves the object to cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        protected abstract bool AddInternal(CacheKey key, CachedObjectInformation info);

        /// <summary>
        /// Removes the oject formt he cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract bool RemoveInternal(CacheKey key);

        protected abstract bool ContainsInternal(CacheKey key);    

        #endregion

    }
}
