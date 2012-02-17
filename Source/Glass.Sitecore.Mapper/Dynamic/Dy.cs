using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Dynamic
{
    public class Dy
    {
        public static Func<dynamic, bool> Fc(Func<dynamic, bool> expression)
        {
            return expression;
        }

       
        public static Func<dynamic, dynamic> Fc(Func<dynamic, dynamic> expression)
        {
            return expression;
        }

        public static Func<dynamic, T> Fc<T>(Func<dynamic, T> expression)
        {
            return expression;
        }

    }
}
