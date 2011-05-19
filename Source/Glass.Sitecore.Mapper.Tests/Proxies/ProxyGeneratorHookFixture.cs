using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Sitecore.Persistence.Proxies;
using System.Reflection;

namespace Glass.Sitecore.Persistence.Tests.Proxies
{
    [TestFixture]
    public class ProxyGeneratorHookFixture
    {
        ProxyClassGeneratorHook _hook;

        [SetUp]
        public void Setup()
        {
            _hook = new ProxyClassGeneratorHook();
        }

        #region ShouldInterceptMethod

        [Test]
        public void ShouldInterceptMethod_PropertyPassed_ReturnsTrue(){

            //Assign
            Type type = typeof(ProxyGeneratorHookFixtureNS.TestClass);
            MethodInfo info = type.GetProperty("Property").GetAccessors().First();

            //Act
            var result = _hook.ShouldInterceptMethod(type, info);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldInterceptMethod_MethodPassed_ReturnsFalse()
        {

            //Assign
            Type type = typeof(ProxyGeneratorHookFixtureNS.TestClass);
            MethodInfo info = type.GetMethod("Method");

            //Act
            var result = _hook.ShouldInterceptMethod(type, info);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion
    }
    
    namespace ProxyGeneratorHookFixtureNS{
        public class TestClass
        {
            public string Property { get; set; }
            public void Method() { }
        }
    }
}
