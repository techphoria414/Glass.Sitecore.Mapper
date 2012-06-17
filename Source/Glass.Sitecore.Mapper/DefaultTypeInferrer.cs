using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Configuration;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper
{
    public class DefaultTypeInferrer
    {

        public virtual SitecoreClassConfig InferrerType(Item item, Type type)
        {
            var config = Context.StaticContext.GetSitecoreClass(item.TemplateID.Guid, type);
            if (config == null) config = Context.StaticContext.GetSitecoreClass(type);
            return config;
        }
    }
}
