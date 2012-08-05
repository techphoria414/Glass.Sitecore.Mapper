using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Dashboard.Model.Data
{
    [SitecoreClass]
    public class PathQuery
    {
        [SitecoreId]
        public Guid id { get; set; }
        
        [SitecoreInfo(SitecoreInfoType.Path)]
        public string value { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public string label { get; set; }
    }
}
