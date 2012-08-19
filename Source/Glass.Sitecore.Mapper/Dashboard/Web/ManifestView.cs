using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Glass.Sitecore.Mapper.Dashboard.Web
{
    public class ManifestView : AbstractView
    {
        string _file;
        string _contentType;

        public ManifestView(string file)
            : this(file, "text/html")
        {
        }

        public ManifestView(string file, string contentType)
        {
            _file = file;
            _contentType = contentType;
        }

        public override void Response(System.Web.HttpResponse response)
        {
            Stream content = GetFile(_file);           
            response.ContentType = _contentType;

            Utility.WriteToResponse(response, content);
        }

        public  static Stream GetFile(string file){
            string name = "Glass.Sitecore.Mapper.Dashboard." + file;
            return Utility.GetResource(name);
        }
    }
}
