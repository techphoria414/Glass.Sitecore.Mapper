using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard
{
    public class QueryResponse : AbstractResponse
    {


        public override string Content()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<h2>Query</h2>");

            sb.Append("<h3>Settings</h3>");

            sb.Append("<form action='get'>");

            sb.Append(CreateClassDropdown());

            sb.Append("</form>");

            return sb.ToString();

   

        }

        private string CreateClassDropdown()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<label for='qClass'>Class</label>");
            
            sb.Append("<select name='qClass'>");

            foreach (var cls in Context.StaticContext.Classes)
            {
                sb.AppendFormat("<option value='{0}'>{0}</option>", cls.Key.FullName);
            }

            sb.Append("</select>");

            return sb.ToString();
        }
    }
}
