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
using Sitecore.Data;
using NUnit.Framework;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Data;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreQueryHandlerFixture
    {
        Database _db;
        InstanceContext _context;
        SitecoreQueryHandler _handler;
        Guid _itemId;
        Item _item;

        [SetUp]
        public void Setup()
        {
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
            _context = new InstanceContext(
                (new SitecoreClassConfig[]{
                    new SitecoreClassConfig(){
                        ClassAttribute = new SitecoreClassAttribute(),
                        Properties = new SitecoreProperty[]{
                            new SitecoreProperty(){
                                Attribute = new SitecoreIdAttribute(),
                                Property = typeof(SitecoreQueryHandlerFixtureNS.TestClass).GetProperty("Id")
                            }
                        },
                        Type = typeof(SitecoreQueryHandlerFixtureNS.TestClass),
                        DataHandlers = new AbstractSitecoreDataHandler[]{}
                    }
                }).ToDictionary(),
                new AbstractSitecoreDataHandler[] { });

            _handler = new SitecoreQueryHandler();


            _itemId = new Guid("{D22C2A23-DF8A-4EC1-AD52-AE15FE63F937}");

            _item = _db.GetItem(new ID(_itemId));
        }

        #region GetValue
        [Test]
        public void GetValue_ReturnsResults_LazyLoad()
        {

            //Assign
            string query = _item.Paths.FullPath + "/*";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreQueryAttribute(query) { IsLazy = true },
                Property = typeof(SitecoreQueryHandlerFixtureNS.TestClass).GetProperty("Results")
            };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.GetValue( _item,  _context) as IEnumerable<SitecoreQueryHandlerFixtureNS.TestClass>;

            //Assert
            Assert.AreEqual(_item.Children.Count, result.Count());
            Assert.AreNotEqual(typeof(SitecoreQueryHandlerFixtureNS.TestClass), result.First().GetType());
            Assert.IsTrue(result.First() is SitecoreQueryHandlerFixtureNS.TestClass);

        }

        [Test]
        public void GetValue_ReturnsResults_NotLazyLoad()
        {

            //Assign
            string query = _item.Paths.FullPath + "/*";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreQueryAttribute(query) { IsLazy = false },
                Property = typeof(SitecoreQueryHandlerFixtureNS.TestClass).GetProperty("Results")
            };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.GetValue( _item,  _context) as IEnumerable<SitecoreQueryHandlerFixtureNS.TestClass>;

            //Assert
            Assert.AreEqual(_item.Children.Count, result.Count());
            Assert.AreEqual(typeof(SitecoreQueryHandlerFixtureNS.TestClass), result.First().GetType());

        }

        [Test]
        public void GetValue_ReturnsSingleResult_LazyLoad()
        {

            //Assign
            string query = _item.Paths.FullPath + "/*";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreQueryAttribute(query) { IsLazy = true },
                Property = typeof(SitecoreQueryHandlerFixtureNS.TestClass).GetProperty("SingleResult")
            };
            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue( _item, _context) as SitecoreQueryHandlerFixtureNS.TestClass;

            //Assert
            Assert.AreNotEqual(typeof(SitecoreQueryHandlerFixtureNS.TestClass), result.GetType());
            Assert.IsTrue(result is SitecoreQueryHandlerFixtureNS.TestClass);

        }

        [Test]
        public void GetValue_ReturnsSingleResult_NotLazyLoad()
        {

            //Assign
            string query = _item.Paths.FullPath + "/*";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreQueryAttribute(query) { IsLazy = false },
                Property = typeof(SitecoreQueryHandlerFixtureNS.TestClass).GetProperty("SingleResult")
            };
            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.GetValue( _item,  _context) as SitecoreQueryHandlerFixtureNS.TestClass;

            //Assert

            Assert.AreEqual(typeof(SitecoreQueryHandlerFixtureNS.TestClass), result.GetType());

        }
        #endregion

        #region SetValue

        [Test]
        public void ParseQuery_ReplacesParameters()
        {
            //Assign
            SitecoreQueryHandler handler = new SitecoreQueryHandler();
            string query = "/sitecore/content/home/*[@@id='{id}']";

            //Act
            var result = handler.ParseQuery(query, _item);

            //Assert
            string expected = "/sitecore/content/home/*[@@id='" + _item.ID.ToString() + "']";
            Assert.AreEqual(expected, result);
        }

        #endregion
    }
    namespace SitecoreQueryHandlerFixtureNS
    {

        public class TestClass
        {
            public virtual Guid Id { get; set; }
            public virtual IEnumerable<TestClass> Results { get; set; }
            public virtual TestClass SingleResult { get; set; }
        }
    }
}

