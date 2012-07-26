using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard
{
    public static class DashboardUtilities
    {
        public static string GetTypeName(Type type)
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

                return sb.ToString();

            }

            return GetTypeNameLink(type);

        }

        public static string GetTypeNameLink(Type type)
        {
            string name = type.Name.Split('`')[0];
            if (Context.StaticContext.Classes.Any(x => x.Key == type))
            {
                return "<a href='?class={0}'>{1}</a>".Formatted(type.FullName, name);
            }
            return name;
        }
    }
}
