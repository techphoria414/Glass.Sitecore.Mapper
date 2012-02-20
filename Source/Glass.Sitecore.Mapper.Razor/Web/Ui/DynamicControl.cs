using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Dynamic;

namespace Glass.Sitecore.Mapper.Razor.Web.Ui
{
    public class DynamicControl : AbstractRazorControl<dynamic>
    {

        protected override void DoRender(System.Web.UI.HtmlTextWriter output)
        {
            Model = new DynamicItem(global::Sitecore.Context.Item);

            base.DoRender(output);
        }

    }
}
