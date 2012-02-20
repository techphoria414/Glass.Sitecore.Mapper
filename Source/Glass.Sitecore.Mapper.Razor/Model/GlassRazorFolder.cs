using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Razor.Model
{
    [SitecoreClass]
    public class GlassRazorFolder
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }
    }
}
