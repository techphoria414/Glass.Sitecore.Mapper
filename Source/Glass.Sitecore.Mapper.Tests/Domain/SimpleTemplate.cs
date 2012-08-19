using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Tests.Domain
{
    [SitecoreClass]
    public class SimpleTemplate
    {
        [SitecoreField]
        public virtual string SingleLineText { get; set; }

        [SitecoreField(Setting=SitecoreFieldSettings.RichTextRaw)]
        public virtual string RichText { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<SimpleTemplate> Children { get; set; }

        [SitecoreQuery("./*", IsRelative = true)]
        public virtual IEnumerable<SimpleTemplate> ChildrenByQuery { get; set; }
    }
}

