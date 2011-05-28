using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Glass.Sitecore.Mapper.Tutorial.Model;

namespace Glass.Sitecore.Mapper.Tutorial.Layouts
{
    public partial class MainLayout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ISitecoreContext context = new SitecoreContext();

            Home item = context.GetCurrentItem<Home>();

            title.Text = item.Title;
            body.Text = item.Body;
            date.Text = item.Date.ToString("dd MMM yyyy");

        }
    }
}
