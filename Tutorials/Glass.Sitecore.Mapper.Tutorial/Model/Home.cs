using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Tutorial.Model
{
    [SitecoreClass]
    public class Home
    {
        [SitecoreField]
        public virtual string Title { get; set; }

        [SitecoreField]
        public virtual string Body { get; set; }

        [SitecoreField("__Created")]
        public virtual DateTime Date { get; set; }
    }
}
