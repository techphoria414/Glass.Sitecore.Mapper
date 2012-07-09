using System;
using System.Xml;
using Sitecore.Xml;
using Sitecore.Configuration;

namespace Glass.Sitecore.Mapper.Configuration
{
    /// <summary>
    /// Maybe at some point configuration for glass could be in the CMS?
    /// </summary>
    public class GlassConfiguration
    {
        private string _readWriteLockTimeout = "0:0:30";

        /// <summary>
        /// Gets or sets the read write lock timeout.
        /// </summary>
        /// <value>
        /// The read write lock timeout.
        /// </value>
        public string ReadWriteLockTimeout
        {
            get
            {
                return _readWriteLockTimeout;
            }
            set
            {
                _readWriteLockTimeout = value;
            }
        }

        private string _objectCache = "Glass.Sitecore.Mapper.ObjectCaching.Implementations.HttpRuntimeCache";

        /// <summary>
        /// 
        /// </summary>
        public string ObjectCache
        {
            get
            {
                return _objectCache;
            }
            set
            {
                _objectCache = value;
            }
        }

        private string _objectManager = "Glass.Sitecore.Mapper.ObjectCreation.ClassManager";

        /// <summary>
        /// 
        /// </summary>
        public string ObjectManager
        {
            get
            {
                return _objectManager;
            }
            set
            {
                _objectManager = value;
            }
        }


        private string _cacheSize = "100MB";

        /// <summary>
        /// 
        /// </summary>
        public string CacheSize
        {
            get
            {
                return _cacheSize;
            }
            set
            {
                _cacheSize = value;
            }
        }

        private string _cacheItemPriority = "Normal";

        /// <summary>
        /// 
        /// </summary>
        public string CacheItemPriority
        {
            get
            {
                return _cacheItemPriority;
            }
            set
            {
                _cacheItemPriority = value;
            }
        }

        private string _cacheItemLifeTime = "2:0:0";

        /// <summary>
        /// 
        /// </summary>
        public string CacheItemLifeTime
        {
            get
            {
                return _cacheItemLifeTime;
            }
            set
            {
                _cacheItemLifeTime = value;
            }
        }

        private string _cacheItemSlidingExpiration = "True";

        /// <summary>
        /// Gets or sets the cache item sliding expiration.
        /// </summary>
        /// <value>
        /// The cache item sliding expiration.
        /// </value>
        public string CacheItemSlidingExpiration
        {
            get
            {
                return _cacheItemSlidingExpiration;
            }
            set
            {
                _cacheItemSlidingExpiration = value;
            }
        }

        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public static GlassConfiguration Config { get; set; }

        /// <summary>
        /// Initializes the <see cref="GlassConfiguration"/> class.
        /// </summary>
        static GlassConfiguration()
        {
            Config = ReadConfiguration(Factory.GetConfigNodes("Glass/ObjectManager"));
        }

        /// <summary>
        /// Reads the configuration.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        public static GlassConfiguration ReadConfiguration(XmlNodeList config)
        {
            var glassConfiguration = new GlassConfiguration();
            foreach (XmlNode objectManager in config)
            {
                if (objectManager.Name.Equals("ObjectManager", StringComparison.OrdinalIgnoreCase))
                {
                    glassConfiguration.ObjectManager = XmlUtil.GetAttribute("type", objectManager);
                    var cacheConfig = XmlUtil.GetAttribute("ObjectCache", objectManager);

                    if (!String.IsNullOrEmpty(cacheConfig))
                    {
                        glassConfiguration.ObjectCache = XmlUtil.GetAttribute("type", objectManager.SelectSingleNode(cacheConfig));
                        var objectCacheSettings = objectManager.SelectSingleNode(string.Format("{0}/settings", cacheConfig));
                        if (objectCacheSettings != null)
                            foreach (XmlNode objectCacheSetting in objectCacheSettings)
                            {
                                var name = XmlUtil.GetAttribute("name", objectCacheSetting);
                                if (name.Equals("CacheSize", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    glassConfiguration.CacheSize = objectCacheSetting.InnerText;
                                }
                                else if (name.Equals("CacheItemPriority", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    glassConfiguration.CacheItemPriority = objectCacheSetting.InnerText;
                                }
                                else if (name.Equals("CacheItemLifeTime", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    glassConfiguration.CacheItemLifeTime = objectCacheSetting.InnerText;
                                }
                                else if (name.Equals("CacheItemSlidingExpiration", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    glassConfiguration.CacheItemSlidingExpiration = objectCacheSetting.InnerText;
                                }
                                else if (name.Equals("ReadWriteLockTimeout", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    glassConfiguration.ReadWriteLockTimeout = objectCacheSetting.InnerText;
                                }
                            }
                    }
                }
            }
            return glassConfiguration;
        }
    }
}
