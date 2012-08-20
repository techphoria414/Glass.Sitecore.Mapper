using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard.Model
{
    public class GlassQuery
    {

        public string Name { get; set; }


        public bool InferType { get; set; }

        public bool IsLazy { get; set; }

        public bool IsRelative { get; set; }

        public string Query { get; set; }

        public bool UseQueryContext { get; set; }

        public GlassClassSummary Type { get; set; }
    }
}
