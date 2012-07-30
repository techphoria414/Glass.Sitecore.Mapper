using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard.Model
{
    public class GlassChildren
    {
        public bool IsLazy { get; set; }

        public bool InferType { get; set; }

        public GlassClassSummary Type { get; set; }

        public string Name { get; set; }
    }
}
