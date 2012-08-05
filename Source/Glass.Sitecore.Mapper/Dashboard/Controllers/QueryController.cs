using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Dashboard.Web;
using Glass.Sitecore.Mapper.Dashboard.Model.Data;

namespace Glass.Sitecore.Mapper.Dashboard.Controllers
{
    public class QueryController : AbstractController
    {
        public AbstractView Index()
        {
            return new HtmlView("Html.Query.htm");
        }

        public AbstractView Paths(string term)
        {
            IEnumerable<PathQuery> result = new PathQuery[0];

            term = term.ToLower();

            if (term.StartsWith("/sitecore"))
            {
                ISitecoreService service = new SitecoreService("master");

                if (term.EndsWith("/"))
                {
                    result = service.Query<PathQuery>(term + "*");
                }
                else
                {
                    var parts = term.Split('/');

                    var query = parts.Take(parts.Length - 1).Aggregate((x, y) => "{0}/{1}".Formatted(x, y));

                    result = service.Query<PathQuery>(query + "/*");

                    result = result.Where(x => x.value.ToLower().StartsWith(term));
                        
                }
            }

            return new JsonView(result);
        }

        public AbstractView Query(string cls, string query)
        {
            if (cls.IsNullOrEmpty() || query.IsNullOrEmpty()) return null;

            var types = GlassContext.Classes.Where(x => x.Key.FullName == cls);
            if (types.Any()) {
                var type = types.First();
                SitecoreService service = new SitecoreService("master");
                var item = service.Database.GetItem(query);
                var result = service.CreateClass(false, false, type.Key, item);

                return new JsonView(result);

            
            }
            else return null;



        }
    }
}
