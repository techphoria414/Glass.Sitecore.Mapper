using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Dashboard
{
    public class DashboardLoader : AttributeConfigurationLoader
    {

        public static readonly Guid IdValue = new Guid("{11EBC050-B466-4B55-8D74-AAFE5FE2F965}");

        public DashboardLoader()
            : base("Glass.Sitecore.Mapper.Dashboard.Model.Data, Glass.Sitecore.Mapper")
        {
        }
        public override Guid Id
        {
            get
            {
                return IdValue;
            }
        }
    }
}
