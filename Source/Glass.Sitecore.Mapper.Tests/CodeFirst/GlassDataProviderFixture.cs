using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Sitecore.Mapper.CodeFirst;

namespace Glass.Sitecore.Mapper.Tests.CodeFirst
{
    [TestFixture]
    public class GlassDataProviderFixture
    {
        [Test]
        public void GetTemplates_CreatesTemplate()
        {
            GlassDataProvider _provider = new GlassDataProvider("master");
            var result = _provider.GetTemplates(null);


            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("templateName", result.First().Name);
            Assert.AreEqual("sectionName", result.First().GetSections().First().Name);
            Assert.AreEqual("fieldName", result.First().GetSections().First().GetFields().First().Name);

        }
    }
}
