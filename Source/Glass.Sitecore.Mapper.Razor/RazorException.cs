using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Razor
{
    public class RazorException: ApplicationException
    {
        public RazorException(string message)
            : base(message)
        {
        }
        public RazorException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}
