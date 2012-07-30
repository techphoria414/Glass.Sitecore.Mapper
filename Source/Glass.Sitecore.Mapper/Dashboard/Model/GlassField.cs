using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard.Model
{
    public class GlassField
    {
        public string Name { get; set; }

        public GlassClassSummary Type { get; set; }

        public string FieldId { get; set; }

        public bool ReadOnly { get; set; }

        public bool CodeFirst { get; set; }

        public string SectionName { get; set; }

        public string FieldType { get; set; }

        public string FieldTitle { get; set; }

        public string FieldSource { get; set; }

        public string FieldName { get; set; }
    }
}
