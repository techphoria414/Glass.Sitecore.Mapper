using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Razor.Web.Ui
{
    public class StrongControl<T> : AbstractRazorControl<T> where T:class
    {
        protected override void DoRender(System.Web.UI.HtmlTextWriter output) 
        {
            ISitecoreContext _context = new SitecoreContext();

            Model = _context.GetCurrentItem<T>();

            base.DoRender(output);
        }

    }
}
