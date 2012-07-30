using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Dashboard.Model.Data
{
    [SitecoreClass]
    public class TemplateData
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }
    }

}
