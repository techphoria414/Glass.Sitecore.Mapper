using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Razor.Web.Ui
{
    public  class StrongControl<T> : AbstractRazorControl<T> where T:class
    {

        public override T GetModel()
        {
            ISitecoreContext _context = new SitecoreContext();
            return _context.CreateClass<T>(false, false, GetDataSourceOrContext());
        }
    }
}
