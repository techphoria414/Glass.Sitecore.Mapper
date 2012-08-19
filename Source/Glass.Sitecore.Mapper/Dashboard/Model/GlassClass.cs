using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard.Model
{
    public class GlassClass
    {
        public IEnumerable<GlassClassSummary> Inherited { get; set; }
        public IEnumerable<GlassInfo> Info { get; set; }

        public string Name { get; set; }

        public IEnumerable<GlassClassSummary> Derived { get; set; }

        public IEnumerable<GlassQuery> Queries { get; set; }

        public IEnumerable<GlassChildren> Children { get; set; }

        public IEnumerable<GlassParent> Parents { get; set; }

        public IEnumerable<GlassLinked> Links { get; set; }

        public IEnumerable<GlassItem> Items { get; set; }

        public IEnumerable<GlassField> Fields { get; set; }

        public string Id { get; set; }

        public bool CodeFirst { get; set; }



        public GlassItemCheck Template { get; set; }

        public GlassItemCheck Branch { get; set; }
    }
}
