using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Glass.Sitecore.Persistence.Demo.Application.Domain;
using System.Web.UI.HtmlControls;

namespace Glass.Sitecore.Persistence.Demo.Layouts
{
    public partial class Content : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ISitecoreContext context = new SitecoreContext();

            DemoClass item = context.GetCurrentItem<DemoClass>();
            title.Text = item.Title;
            body.Text = item.Body;

            links.DataSource = item.Links;
            links.DataBind();
        }
    }
}