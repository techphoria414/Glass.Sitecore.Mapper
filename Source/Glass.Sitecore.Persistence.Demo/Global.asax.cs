using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Glass.Sitecore.Persistence.Configuration.Attributes;

namespace Glass.Sitecore.Persistence.Demo
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AttributeConfigurationLoader loader = new AttributeConfigurationLoader(
               new string[] { "Glass.Sitecore.Persistence.Demo.Application.Domain, Glass.Sitecore.Persistence.Demo" }
               );

            Persistence.Context  context = new Context(loader, null);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}