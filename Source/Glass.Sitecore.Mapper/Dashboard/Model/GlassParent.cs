using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard.Model
{
    public class GlassParent
    {
        public bool IsLazy { get; set; }

        public bool InferType { get; set; }

        public string GetType { get; set; }

        public string Name { get; set; }
    }
}
