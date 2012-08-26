using System;
using Glass.Sitecore.Mapper.ObjectCaching.Implementations;
using SitecoreConfiguration = Sitecore.Configuration;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.ObjectCaching
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectCacheFactory
    {

         /// <summary>
         /// Creates this instance.
         /// </summary>
         /// <returns></returns>
        public static ObjectCache Create()
        {
            return Create(GlassConfiguration.Config.ObjectCache);
        }

        public static ObjectCache Create(string type)
        {
              

            ObjectCache objectCache = null;
            if (type.Equals("Glass.Sitecore.Mapper.ObjectCaching.Implementations.SitecoreCache", StringComparison.InvariantCultureIgnoreCase))
            {
               // objectCache = new SitecoreCache();
            }
            else if (type.Equals("Glass.Sitecore.Mapper.ObjectCaching.Implementations.HttpRuntimeCache", StringComparison.InvariantCultureIgnoreCase))
            {
                objectCache = new HttpRuntimeCache();
               
            }
            else
            {
                objectCache = new CacheTable();
            }

            return objectCache;
        }
    }
}
