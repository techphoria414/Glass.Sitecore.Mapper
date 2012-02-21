using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Razor.Model
{
    [SitecoreClass(TemplateId="{9B162562-C999-45BF-B688-1C65D0EBCAAD}")]
    public class GlassBehindRazor
    {
        [SitecoreInfo(Configuration.SitecoreInfoType.Name)]
        public string Name { get; set; }

        [SitecoreField("Name")]
        public string View { get; set; }

        [SitecoreField]
        public string Type { get; set; }

        [SitecoreField]
        public string Assembly { get; set; }
    }
}
