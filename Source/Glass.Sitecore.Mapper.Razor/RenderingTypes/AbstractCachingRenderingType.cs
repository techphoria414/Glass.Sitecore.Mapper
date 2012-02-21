using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Web.UI;

namespace Glass.Sitecore.Mapper.Razor.RenderingTypes
{
    public abstract class AbstractCachingRenderingType : RenderingType
    {

        private readonly object _key = new object();
        private readonly object _typeKey = new object();

        protected  static  Dictionary<string, Type> LoadedTypes { get; private set; }

        public AbstractCachingRenderingType()
        {
            if (LoadedTypes == null)
            {
                lock (_key)
                {
                    if (LoadedTypes == null)
                    {
                        LoadedTypes = new Dictionary<string, Type>();
                    }
                }
            }
        }

        public Type GetControlType(string typeName, Func<string, Type> typeLoader)
        {
            Type finalType = null;

            if (LoadedTypes.ContainsKey(typeName))
                finalType = LoadedTypes[typeName];
            else
            {
                finalType = typeLoader(typeName);
                if (finalType == null) throw new NullReferenceException("Could not find type {0} for Razor view.".Formatted(typeName));

                //we added to the collection making sure no one else added it before
                if (!LoadedTypes.ContainsKey(typeName))
                {
                    lock (_typeKey)
                    {
                        if (!LoadedTypes.ContainsKey(typeName))
                        {
                            LoadedTypes.Add(typeName, finalType);
                        }
                    }
                }
            }

            return finalType;
        }
    }
}
