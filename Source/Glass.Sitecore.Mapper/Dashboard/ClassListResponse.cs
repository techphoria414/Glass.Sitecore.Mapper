using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Glass.Sitecore.Mapper.Dashboard
{
    public class ClassListResponse : AbstractResponse
    {
        public override string Content()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<h2>Class List</h2>");

            sb.Append("<ul>");
            foreach (var cls in Context.StaticContext.Classes.OrderBy(x=>x.Key.FullName))
            {
                sb.Append("<li>");

                sb.AppendFormat("<a href='?class={0}'>{1}</a>", HttpUtility.UrlEncode(cls.Key.FullName),  cls.Key.FullName);

                sb.Append("</li>");
            }
            sb.Append("</ul>");

            return sb.ToString();

        }
    }
}
