using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Configuration.Attributes
{
    public class LoadAllAttributeConfigurationLoader : AttributeConfigurationLoader
    {
        public override IDictionary<Type, SitecoreClassConfig> LoadClasses()
        {

            return base.LoadClasses();
        }

    }
}
