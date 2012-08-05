using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Dashboard.Model;

namespace Glass.Sitecore.Mapper.Dashboard
{
    public static class DashboardUtilities
    {
        public static GlassClassSummary GetTypeName(Type type)
        {
            var summary = new GlassClassSummary();

            if (type.IsGenericType)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(GetTypeNameLink(type));


                sb.Append("<");

                foreach (var subType in type.GetGenericArguments())
                {
                    var subName = GetTypeName(subType);
                    sb.AppendFormat("{0},", subName.Name);

                    //TODO: this needs to be improved.
                    if (Context.StaticContext.Classes.Any(x => x.Key == subType))
                    {
                        summary.Url = "/details.gls?cls={0}".Formatted(type.FullName);
                        summary.IsGlass = true;
                    }
                }

                sb.Remove(sb.Length - 1, 1);

                sb.Append(">");

                summary.Name = sb.ToString();


            }
            else
            {
                summary.Name = GetTypeNameLink(type);

                if (Context.StaticContext.Classes.Any(x => x.Key == type))
                {
                    summary.Url = "/details.gls?cls={0}".Formatted(type.FullName);
                    summary.IsGlass = true;
                }
            }
            return summary;


        }

        public static string GetTypeNameLink(Type type)
        {
            return  type.Name.Split('`')[0];
        }

        
    }
}
