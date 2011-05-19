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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Data;
using Sitecore.Data;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Proxies;
using Castle.Core.Interceptor;
using Moq;

namespace Glass.Sitecore.Mapper.Tests.Proxies
{
    [TestFixture]
    public class ProxyClassInterceptorFixture
    {
        InstanceContext _context;
        Database _db;
        Guid _itemId;

        [SetUp]
        public void Setup(){
            _context = new InstanceContext(
               (new SitecoreClassConfig[]{
                   new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(),
                       Properties = new SitecoreProperty[]{},
                       Type = typeof(ProxyClassInterceptorFixtureNS.TestClass)
                   }
               }).ToDictionary(), new ISitecoreDataHandler[]{});

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

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
                _context,
                item);

            invocation.Setup(x=>x.Method).Returns(typeof(ProxyClassInterceptorFixtureNS.TestClass).GetMethod("TestCall"));
            invocation.SetupSet(x=>x.ReturnValue).Callback((object value)=>{
                result = value.ToString();
            });
            //Act
            interceptor.Intercept(invocation.Object);
            
            //Assert

            Assert.AreEqual("true", result);
        }

    }

    namespace ProxyClassInterceptorFixtureNS{
        
        public class TestClass{
            public virtual string TestCall()
            {
                return "true";
            }
        }
    }
}
