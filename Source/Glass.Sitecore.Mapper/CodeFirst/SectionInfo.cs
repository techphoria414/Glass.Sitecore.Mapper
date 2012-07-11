using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;

namespace Glass.Sitecore.Mapper.CodeFirst
{
    public class SectionInfo
    {
        public SectionInfo(string name, ID sectionId, ID templateId)
        {
            Name = name;
            SectionId = sectionId;
            TemplateId = templateId;
          
        }
        public string Name { get; set; }
        public ID SectionId { get; set; }
        public ID TemplateId { get; set; }
    }
}
