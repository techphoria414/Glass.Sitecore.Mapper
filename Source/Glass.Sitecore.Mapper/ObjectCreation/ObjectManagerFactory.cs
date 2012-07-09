using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SitecoreConfiguration = global::Sitecore.Configuration;
using Glass.Sitecore.Mapper.ObjectCreation.Implementations;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.ObjectCreation
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectManagerFactory
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static IObjectManager Create()
        {
            return Create(GlassConfiguration.Config.ObjectManager);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IObjectManager Create(string type)
        {
            IObjectManager objectManager = null;
            if (type.Equals("Glass.Sitecore.Mapper.ObjectCreation.CacheObjectManager", StringComparison.InvariantCultureIgnoreCase))
            {
                objectManager = new CacheObjectManager();
            }
            else
            {
                objectManager = new ClassManager();
            }

            return objectManager;
        }
    }
}
