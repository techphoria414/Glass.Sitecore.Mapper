/*
   Copyright 2011 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/
using System;
using NUnit.Framework;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Data;
using Sitecore.Data;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Proxies;
using Moq;
using Castle.DynamicProxy;

namespace Glass.Sitecore.Mapper.Tests.Proxies
{
    [TestFixture]
    public class ProxyClassInterceptorFixture
    {
        ISitecoreService _service;
        Database _db;
        Guid _itemId;

        [SetUp]
        public void Setup(){
            var context = new Context(new AttributeConfigurationLoader("Glass.Sitecore.Mapper.Tests.Proxies.ProxyClassInterceptorFixtureNS, Glass.Sitecore.Mapper.Tests"), null);

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _service = new SitecoreService(_db);

            _itemId = new Guid("{8A317CBA-81D4-4F9E-9953-64C4084AECCA}");

        }

        [Test]
        public void Intercept_CreatesAProxyThatIsReplaced()
        {
            //Assign 
            Item item = _db.GetItem(new ID(_itemId));
            Mock<IInvocation> invocation = new Mock<IInvocation>();
            string result ="";

            ProxyClassInterceptor interceptor = new ProxyClassInterceptor(
                typeof(ProxyClassInterceptorFixtureNS.TestClass),
                _service,
                item, false);

            invocation.Setup(x=>x.Method).Returns(typeof(ProxyClassInterceptorFixtureNS.TestClass).GetMethod("TestCall"));
            invocation.SetupSet(x=>x.ReturnValue).Callback((object value)=>{
                result = value.ToString();
            });
            //Act
            interceptor.Intercept(invocation.Object);
            
            //Assert

            Assert.AreEqual("true", result);
        }

        //TODO: more proxy tests

        //[Test]
        //public void GetFieldValue_GuidId_CreatesProxyClass_UsingGetOnProperty()
        //{
        //    //Assign
        //    Item item = _db.GetItem(_item1Path);
        //    SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
        //    SitecoreProperty property = new SitecoreProperty()
        //    {
        //        Attribute = new SitecoreFieldAttribute(),
        //        Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
        //    };

        //    _handler.ConfigureDataHandler(property);
        //    //Act
        //    var result = _handler.GetFieldValue(
        //        _itemId.ToString(),
        //        item,
        //        _service);

        //    parent.Child = result as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

        //    //Assert
        //    Assert.AreNotEqual(typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass), parent.Child.GetType());

        //    var callMe = parent.Child.CallMe;
        //}

        //[Test]
        //public void GetFieldValue_GuidId_SetOnProxyUpdatesActual()
        //{
        //    //Assign
        //    Item item = _db.GetItem(new ID(_itemId));
        //    SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
        //    SitecoreProperty property = new SitecoreProperty()
        //    {
        //        Attribute = new SitecoreFieldAttribute(),
        //        Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
        //    };

        //    _handler.ConfigureDataHandler(property);
        //    //Act
        //    var result = _handler.GetFieldValue(
        //        _itemId.ToString(),
        //        item,
        //        _service) as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

        //    parent.Child = result;
        //    result.CallMe = "Some value";

        //    //Assert

        //    var test = parent.Child.CallMe;

        //    Assert.AreEqual(result.CallMe, test);
        //}
        //[Test]
        //public void GetFieldValue_GuidId_SetOnActualUpdatesProxy()
        //{
        //    //Assign
        //    Item item = _db.GetItem(new ID(_itemId));
        //    SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
        //    SitecoreProperty property = new SitecoreProperty()
        //    {
        //        Attribute = new SitecoreFieldAttribute(),
        //        Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
        //    };

        //    _handler.ConfigureDataHandler(property);

        //    //Act
        //    var result = _handler.GetFieldValue(
        //        _itemId.ToString(),
        //        item,
        //        _service) as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

        //    parent.Child = result;
        //    parent.Child.CallMe = "Some value";

        //    //Assert

        //    var test = result.CallMe;

        //    Assert.AreEqual(result.CallMe, test);
        //}

    }

    namespace ProxyClassInterceptorFixtureNS{
        
        [SitecoreClass]
        public class TestClass{
            public virtual string TestCall()
            {
                return "true";
            }
        }
    }
}
