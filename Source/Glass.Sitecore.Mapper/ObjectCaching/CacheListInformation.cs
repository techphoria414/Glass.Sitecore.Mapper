using System;
using System.Collections.Generic;
using System.Threading;

namespace Glass.Sitecore.Mapper.ObjectCaching
{
    /// <summary>
    /// This hold information about the items in the cache of the Type and TemplateID
    /// </summary>
    public class CacheListInformation
    {
        private readonly Dictionary<string, List<string>> _relatedCacheKeys = new Dictionary<string, List<string>>();
        private readonly List<Guid> _ids = new List<Guid>();
        private readonly List<CachedObjectInformation> _cachedObjectInformationList = new List<CachedObjectInformation>();

        /// <summary>
        /// a list of the ID in the cache
        /// </summary>
        public IList<CachedObjectInformation> CachedObjectInformationList
        {
            get
            {
                return _cachedObjectInformationList;
            }
        }

        /// <summary>
        /// a list of the ID in the cache
        /// </summary>
        public IList<Guid> Ids
        {
            get
            {
                return _ids;
            }
        }


        /// <summary>
        /// A list of related keys so we know what other thing in cache need to be cleared if something changes to this cache list
        /// </summary>
        public Dictionary<string, List<string>> RelatedCacheKeys
        {
            get
            {
                return _relatedCacheKeys;
            }
        }

        /// <summary>
        /// The type of object that is saved in the cache
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; set; }

        /// <summary>
        /// The template ID of the corresponding  item
        /// </summary>
        /// <value>
        /// The template ID.
        /// </value>
        public Guid TemplateID { get; set; }


        /// <summary>
        /// An instance read write lock so that different caches can all read and write
        /// at the same time and don't bock each other
        /// </summary>
        public ReaderWriterLockSlim ListLock = new ReaderWriterLockSlim();
    }
}
