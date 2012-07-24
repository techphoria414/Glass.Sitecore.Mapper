using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Glass.Sitecore.Mapper.Dashboard
{
    public abstract class AbstractController
    {
        public HttpContext Context { get; set; }
        public InstanceContext GlassContext { get; set; }
    }
}
