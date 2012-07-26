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

            string header = ManifestView.GetFile("Html.HtmlHeader.htm");
            string footer = ManifestView.GetFile("Html.HtmlFooter.htm");
            string content = ManifestView.GetFile(_file);

            response.Write(header);
            response.Write(content);
            response.Write(footer);
        }
    }
}
