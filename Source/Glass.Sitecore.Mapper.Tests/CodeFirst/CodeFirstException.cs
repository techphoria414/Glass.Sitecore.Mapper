using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Tests.CodeFirst
{
    public class CodeFirstException: Exception
    {
        public CodeFirstException(string message) : base(message) { }
    }
}
