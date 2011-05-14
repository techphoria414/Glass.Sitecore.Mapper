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
using Glass.Sitecore.Persistence.Configuration.Attributes;
using Glass.Sitecore.Persistence.Configuration;
using Glass.Sitecore.Persistence.Data;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Persistence.Tests.Data
{
    [TestFixture]
    public class SitecoreChildrenHandlerFixture
    {
        InstanceContext _context;
        Database _db;
        Guid _itemId;

        SitecoreChildrenHandler _handler;

        [SetUp]
        public void Setup()
        {
            _context = new InstanceContext(
              new SitecoreClassConfig[]{
                   new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(),
                       Properties = new SitecoreProperty[]{},
                       Type = typeof(SitecoreChildrenHandlerFixtureNS.SubClass)
                   }
               }, new ISitecoreDataHandler[] { });

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _itemId = new Guid("{D22C2A23-DF8A-4EC1-AD52-AE15FE63F937}");
            _handler = new SitecoreChildrenHandler();
        }

        #region GetValue
        
        [Test]
        public  void GetValue_ReturnsChildren_UsingLazy()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            SitecoreProperty property = new SitecoreProperty(){
                Attribute = new SitecoreChildrenAttribute(),
                Property = typeof(SitecoreChildrenHandlerFixtureNS.TestClass).GetProperty("Children")
            };

            //Act
            var result = _handler.GetValue(null, item, property, _context) as LazyEnumerable<SitecoreChildrenHandlerFixtureNS.SubClass>;
            SitecoreChildrenHandlerFixtureNS.TestClass assignTest = new Glass.Sitecore.Persistence.Tests.Data.SitecoreChildrenHandlerFixtureNS.TestClass();
            assignTest.Children = result;
            //Assert
            Assert.AreEqual(item.Children.Count, result.Count());
            Assert.AreEqual(result, assignTest.Children);
            Assert.AreNotEqual(typeof(SitecoreChildrenHandlerFixtureNS.SubClass), result.First().GetType());
            Assert.IsTrue(result.First() is SitecoreChildrenHandlerFixtureNS.SubClass);

        }

        [Test]
        public void GetValue_ReturnsChildren_NotLazy()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreChildrenAttribute() { IsLazy = false },
                Property = typeof(SitecoreChildrenHandlerFixtureNS.TestClass).GetProperty("Children")
            };

            //Act
            var result = _handler.GetValue(null, item, property, _context) as IEnumerable<SitecoreChildrenHandlerFixtureNS.SubClass>;
            SitecoreChildrenHandlerFixtureNS.TestClass assignTest = new Glass.Sitecore.Persistence.Tests.Data.SitecoreChildrenHandlerFixtureNS.TestClass();
            assignTest.Children = result;
            //Assert
            Assert.AreEqual(item.Children.Count, result.Count());
            Assert.AreEqual(result, assignTest.Children);

            Assert.AreEqual(typeof(SitecoreChildrenHandlerFixtureNS.SubClass), result.First().GetType());
            Assert.IsTrue(result.First() is SitecoreChildrenHandlerFixtureNS.SubClass);

        }

        #endregion

        #region WillHandle

        [Test]
        public void WillHandle_ReturnsTrue()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreChildrenAttribute(),
                Property = typeof(SitecoreChildrenHandlerFixtureNS.TestClass).GetProperty("Children")
            };

            //Act
            var result = _handler.WillHandle(property, null);


            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void WillHandle_ReturnsFalse()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreChildrenAttribute(),
                Property = typeof(SitecoreChildrenHandlerFixtureNS.TestClass).GetProperty("List")
            };

            //Act
            var result = _handler.WillHandle(property, null);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion
    }

    namespace SitecoreChildrenHandlerFixtureNS
    {
        public class TestClass
        {
            public virtual IEnumerable<SubClass> Children { get; set; }
            public virtual IList<SubClass> List { get; set; }
        }
        public class SubClass { }
    }
}
