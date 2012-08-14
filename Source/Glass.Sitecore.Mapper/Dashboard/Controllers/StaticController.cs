using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Dashboard.Web;

namespace Glass.Sitecore.Mapper.Dashboard.Controllers
{
    public class StaticController : AbstractController
    {
        public AbstractView Index(string file){

            string contentType = string.Empty;
            switch (file.Split('.').Last().ToLower())
            {
                case "js":
                    contentType = "text/javascript";
                    break;
                case "css":
                    contentType = "text/css";
                    break;
                case "gif":
                    contentType = "image/gif";
                    break;
            }
           return new ManifestView(file, contentType);
        }
    }
}
