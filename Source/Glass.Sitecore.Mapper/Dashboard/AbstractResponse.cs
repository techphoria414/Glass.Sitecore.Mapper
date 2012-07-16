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
            sb.AppendFormat("<title>Glass {0}</title>", Title);
            sb.Append("</head>");
            sb.Append("<body>");
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
            sb.Append(Content());
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

        public string CSS()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Glass.Sitecore.Mapper.Dashboard.Dashboard.css");
            

        }

	}
}
