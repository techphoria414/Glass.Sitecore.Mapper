using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard.Model
{
    public class GlassItemCheck
    {
        public bool Exists { get; set; }
        public string Url { get; set; }
        public bool Checked { get; set; }

        public Guid Id { get; set; }
    }
}
