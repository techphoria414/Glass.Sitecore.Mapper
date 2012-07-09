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
        public static IObjectCache Create()
        {
            return Create(GlassConfiguration.Config.ObjectCache);
        }

        public static IObjectCache Create(string type)
        {
            IObjectCache objectCache;
            if (type.Equals("Glass.Sitecore.Mapper.ObjectCaching.Implementations.SitecoreCache", StringComparison.InvariantCultureIgnoreCase))
            {
                objectCache = new SitecoreCache();
            }
            else
            {
                objectCache = new HttpRuntimeCache();
            }

            return objectCache;
        }
    }
}
