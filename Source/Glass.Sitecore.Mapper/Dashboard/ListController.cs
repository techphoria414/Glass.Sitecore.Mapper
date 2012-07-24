using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dashboard
{
    public class ListController : AbstractController
    {

        public void Index(string some)
        {
            Context.Response.Write("Fuck yeh"+some);
            
        }
    }
}
