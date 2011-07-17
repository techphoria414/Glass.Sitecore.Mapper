using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glass.Sitecore.Mapper.Demo.Application.Domain
{
    public class TemplateFilter : IItemFilter
    {
        public bool LoadItem(global::Sitecore.Data.Items.Item item, ISitecoreService service)
        {
            return true;
        }
    }
}