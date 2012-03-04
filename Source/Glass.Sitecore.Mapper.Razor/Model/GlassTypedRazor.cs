using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Razor.Model
{
    [SitecoreClass(TemplateId = "{7B10C01D-B0DF-4626-BE34-F48E38828FB7}")]
    public class GlassTypedRazor
    {
        [SitecoreInfo(Configuration.SitecoreInfoType.Name)]
        public string Name { get; set; }

        [SitecoreField("Name")]
        public string View { get; set; }

        [SitecoreField]
        public string Code { get; set; }

        [SitecoreField]
        public string Type { get; set; }

        [SitecoreField]
        public string Assembly { get; set; }
       

    }
}
