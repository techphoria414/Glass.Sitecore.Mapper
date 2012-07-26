using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard.Model
{
    public class GlassItem
    {
        public bool IsLazy { get; set; }

        public string Path { get; set; }

        public string Id { get; set; }

        public string GetType { get; set; }

        public string Name { get; set; }
    }
}
