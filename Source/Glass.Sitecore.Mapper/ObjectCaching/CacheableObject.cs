using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper.ObjectCaching
{
    /// <summary>
    /// This object holds information about the the object that we are adding a cache
    /// </summary>
    [Serializable]
    public class CacheableObject : CachedObjectInformation, ICacheableObject
    {
        /// <summary>
        /// 
        /// </summary>
        public object CachedObject { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="objectForCaching"></param>
        public CacheableObject(Item item, object objectForCaching, object key):base(item, objectForCaching.GetType())
        {
            CachedObject = objectForCaching;
            Key = key;
        }

        public CachedObjectInformation ToCachedObjectInformation()
        {
            var cachedObjectInformation = new CachedObjectInformation();

            cachedObjectInformation.Database = this.Database;
            cachedObjectInformation.ItemID = this.ItemID;
            cachedObjectInformation.Key = this.Key;
            cachedObjectInformation.Language = this.Language;
            cachedObjectInformation.RevisionID = this.RevisionID;
            cachedObjectInformation.TargetType = this.TargetType;
            cachedObjectInformation.TemplateID = this.TemplateID;
            cachedObjectInformation.Type = this.Type;
            cachedObjectInformation.Version = this.Version;

            return cachedObjectInformation;
        }
    }
}
