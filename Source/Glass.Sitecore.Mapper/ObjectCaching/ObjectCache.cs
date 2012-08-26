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

   

        public static Tuple<Guid, string, Type> CreateKey(Item item, Type type)
        {
            var revisionId = new Guid(item[CachedObjectInformation.RevisionFieldName]);

            return new Tuple<Guid, string, Type>(revisionId, item.Database.Name, type);
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
      
        public bool Add(Item item,Type type, object o){
            if(Contains(item, type))
                return false;
            else {
                var key = CreateKey(item, type);
                return AddInternal(key, new CachedObjectInformation(item, type, o));
            }
        }

        public bool Remove(Item item, Type type)
        {
            if (Contains(item, type))
            {
                var key = CreateKey(item, type);
                return RemoveInternal(key);
            }
            else
            {
                return false;
            }

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
        protected abstract CachedObjectInformation GetInternal(Tuple<Guid, string, Type> key);
       
        /// <summary>
        /// Saves the object to cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        protected abstract bool AddInternal(Tuple<Guid, string, Type> key, CachedObjectInformation info);

        /// <summary>
        /// Removes the oject formt he cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract bool RemoveInternal(Tuple<Guid, string, Type> key);

        protected abstract bool ContainsInternal(Tuple<Guid, string, Type> key);    

        #endregion

    }
}
