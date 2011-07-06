using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Demo.Application.Domain
{
    [SitecoreClass]
    public class PageEditorDemo
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }

        [SitecoreField]
        public virtual Glass.Sitecore.Mapper.FieldTypes.Image Image { get; set; }

        [SitecoreField]
        public virtual string Body { get; set; }

        [SitecoreField]
        public virtual PageEditorDemoSub AnotherItem { get; set; }
    }

    [SitecoreClass]
    public class PageEditorDemoSub
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }

        [SitecoreField]
        public virtual string Content { get; set; }
    }
}