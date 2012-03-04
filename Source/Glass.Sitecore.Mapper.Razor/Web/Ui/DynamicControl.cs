using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Dynamic;

namespace Glass.Sitecore.Mapper.Razor.Web.Ui
{
    public class DynamicControl : AbstractRazorControl<dynamic>
    {
        public override dynamic GetModel()
        {


            return new DynamicItem(GetDataSourceOrContext());
        }
    }
}
