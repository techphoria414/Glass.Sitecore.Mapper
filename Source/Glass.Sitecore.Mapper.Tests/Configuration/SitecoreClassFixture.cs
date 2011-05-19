using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Sitecore.Persistence.Configuration;

namespace Glass.Sitecore.Persistence.Tests.Configuration
{
    [TestFixture]
    public class SitecoreClassFixture
    {
        SitecoreClass _class;
        [SetUp]
        public void Setup()
        {
            _class = new SitecoreClass();

        }

    }
}
