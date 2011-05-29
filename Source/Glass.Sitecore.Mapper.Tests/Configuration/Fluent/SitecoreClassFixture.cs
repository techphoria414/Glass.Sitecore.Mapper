using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Sitecore.Mapper.Configuration.Fluent;

namespace Glass.Sitecore.Mapper.Tests.Configuration.Fluent
{
    [TestFixture]
    public class SitecoreClassFixture
    {

        [Test]
        public void Test()
        {
           

        }

    }
    namespace SitecoreClassFixtureNS
    {
        public class Test
        {
            public virtual string MyField { get; set; }
        }

    }
}
