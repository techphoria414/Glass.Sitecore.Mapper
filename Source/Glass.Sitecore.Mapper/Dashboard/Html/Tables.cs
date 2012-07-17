using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard.Html
{
    public static class Tables
    {
        public static string TableOpen(string classValue = "table table-bordered table-striped")
        {
            return "<table class='{0}'>".Formatted(classValue);
        }
    
    }
}
