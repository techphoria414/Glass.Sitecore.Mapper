using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.ObjectCaching
{
    /// <summary>
    /// 
    /// </summary>
    public class GlassCachingDictionary
    {
        private static Dictionary<Guid, CacheListInformation> TemplateCacheItemDictionary = new Dictionary<Guid, CacheListInformation>();
        private static Dictionary<Type, CacheListInformation> TypeCacheItemDictionary = new Dictionary<Type, CacheListInformation>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static CacheListInformation Get(Type type)
        {
            var returnCacheListInformation = default(CacheListInformation);
            if (TypeCacheItemDictionary.ContainsKey(type))
            {
                returnCacheListInformation = TypeCacheItemDictionary[type];
            }

            return returnCacheListInformation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static CacheListInformation Get(Guid template)
        {
            var returnCacheListInformation =  default(CacheListInformation);
            if (TemplateCacheItemDictionary.ContainsKey(template))
            {
                returnCacheListInformation = TemplateCacheItemDictionary[template];
            }

            return returnCacheListInformation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cacheListInformation"></param>
        public static void Add(Type type, CacheListInformation cacheListInformation)
        {
            TypeCacheItemDictionary[type] = cacheListInformation;
            TemplateCacheItemDictionary[cacheListInformation.TemplateID] = cacheListInformation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="cacheListInformation"></param>
        public static void Add(Guid template, CacheListInformation cacheListInformation)
        {
            TemplateCacheItemDictionary[template] = cacheListInformation;
            TypeCacheItemDictionary[cacheListInformation.Type] = cacheListInformation;
        }

    }
}
