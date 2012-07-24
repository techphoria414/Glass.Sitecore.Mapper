using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Glass.Sitecore.Mapper.Dashboard
{
	public abstract class AbstractResponse
	{
        public HttpRequest Request { get; set; }

        public string Title{
            get;set;
        }
        public string Header()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<link rel='stylesheet' href='http://current.bootstrapcdn.com/bootstrap-v204/css/bootstrap-combined.min.css' />");
            sb.AppendFormat("<title>Glass {0}</title>", Title);
            sb.AppendFormat("<style>{0}</style>", GetCSS());
            sb.Append("</head>");
            sb.Append("<body>");
            return sb.ToString();
        }

        public string NavBar()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='navbar navbar-fixed-top'><div class='navbar-inner'><div class='container'>");
            sb.Append("<ul class='nav'>");

            string page = Request.Path;

            sb.AppendFormat("<li><a href='{0}'>Class List</a></li>", page);
            sb.AppendFormat("<li><a href=?query'>Query Sitecore</a></li>", page);

            sb.Append("</ul>");
            sb.Append("</div></div></div>");

            return sb.ToString();

        }

        public string Footer()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        public abstract string Content();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Header());
            sb.Append(NavBar());
            sb.Append("<div class='container'>");
            sb.Append("<div class='row'>");
            sb.Append(Content());
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append(Footer());

            return sb.ToString();
        }

        public string GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(GetTypeNameLink(type));
                

                sb.Append("&lt;");

                foreach (var subType in type.GetGenericArguments())
                {
                    var subName = GetTypeName(subType);
                    sb.AppendFormat("{0},", subName);
                }
                
                sb.Remove(sb.Length - 1, 1);

                sb.Append("&gt;");

                return  sb.ToString();

            }

            return GetTypeNameLink(type);
          
        }

        public string GetTypeNameLink(Type type)
        {
            string name = type.Name.Split('`')[0];
            if (Context.StaticContext.Classes.Any(x=>x.Key == type))
            {
                return "<a href='?class={0}'>{1}</a>".Formatted(type.FullName, name);
            }
            return name;
        }

        public string GetCSS()
        {
            return Utility.GetResource("Glass.Sitecore.Mapper.Dashboard.Dashboard.css");
        }
	}
}
