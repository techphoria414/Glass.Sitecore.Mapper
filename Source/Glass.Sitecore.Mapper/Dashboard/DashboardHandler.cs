using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Glass.Sitecore.Mapper.Dashboard
{
    public class DashboardHandler : IHttpHandler
    {



        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            AbstractResponse response = null;

            if (context.Request.QueryString["class"].IsNotNullOrEmpty())
            {
                response = new ClassDetailsResponse();
            }
            else
            {
                response = new ClassListResponse();
            }


            response.Request = context.Request;

            context.Response.Write(response.ToString());

        }

     



    }
}
