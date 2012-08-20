using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard.Web
{
    public class HtmlView : AbstractView
    {
        string _file;
        public HtmlView(string file) {
            _file = file;
        }

        public override void Response(System.Web.HttpResponse response)
        {
            response.ContentType = "text/html";

            Utility.WriteToResponse(response, ManifestView.GetFile("Html.HtmlHeader.htm"));
            Utility.WriteToResponse(response, ManifestView.GetFile(_file));
            Utility.WriteToResponse(response, ManifestView.GetFile("Html.HtmlFooter.htm"));
        }
    }
}
