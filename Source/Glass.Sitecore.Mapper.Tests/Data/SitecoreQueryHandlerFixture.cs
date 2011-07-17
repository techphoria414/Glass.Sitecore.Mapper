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
        ISitecoreService _service;
        SitecoreQueryHandler _handler;
        Guid _itemId;
        Item _item;

        [SetUp]
        public void Setup()
        {
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
            var context = new InstanceContext(
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
                    },
                     new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(),
                       Properties = new SitecoreProperty[]{
                            new SitecoreProperty(){
                                Attribute = new SitecoreIdAttribute(),
                                Property = typeof(SitecoreQueryHandlerFixtureNS.BaseType).GetProperty("Id")
                            }
                       },
                       Type = typeof(SitecoreQueryHandlerFixtureNS.BaseType),
                       DataHandlers = new AbstractSitecoreDataHandler []{
                        new SitecoreIdDataHandler(){
                               Property = typeof(SitecoreQueryHandlerFixtureNS.BaseType).GetProperty("Id")
                        }
                       }
                   },
                   new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(){
                           
                       },
                       Properties = new SitecoreProperty[]{
                            new SitecoreProperty(){
                                Attribute = new SitecoreIdAttribute(),
                                Property = typeof(SitecoreQueryHandlerFixtureNS.TypeOne).GetProperty("Id")
                            }
                       },
                       Type = typeof(SitecoreQueryHandlerFixtureNS.TypeOne),
                       DataHandlers = new AbstractSitecoreDataHandler []{
                        new SitecoreIdDataHandler(){
                               Property = typeof(SitecoreQueryHandlerFixtureNS.TypeOne).GetProperty("Id")
                        }
                       },
                       TemplateId = new Guid("{5B684B69-F532-4BB2-8A98-02AFCDE4BB84}")
                   },
                   new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(){
                          
                       },
                       Properties = new SitecoreProperty[]{
                            new SitecoreProperty(){
                                Attribute = new SitecoreIdAttribute(),
                                Property = typeof(SitecoreQueryHandlerFixtureNS.TypeTwo).GetProperty("Id")
                            }
                       },
                       Type = typeof(SitecoreQueryHandlerFixtureNS.TypeTwo),
                       DataHandlers = new AbstractSitecoreDataHandler []{
                        new SitecoreIdDataHandler(){
                               Property = typeof(SitecoreQueryHandlerFixtureNS.TypeTwo).GetProperty("Id")
                        }
                       },
                       TemplateId = new Guid("{3902F503-7DC7-48B2-9FD8-1EB878CEBA93}")
                   }

                }).ToDictionary(),
                new AbstractSitecoreDataHandler[] { });

            _service = new SitecoreService(_db, context);

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
            var result = _handler.GetValue(_item, _service) as IEnumerable<SitecoreQueryHandlerFixtureNS.TestClass>;

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
            var result = _handler.GetValue(_item, _service) as IEnumerable<SitecoreQueryHandlerFixtureNS.TestClass>;

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
            var result = _handler.GetValue(_item, _service) as SitecoreQueryHandlerFixtureNS.TestClass;

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
            var result = _handler.GetValue(_item, _service) as SitecoreQueryHandlerFixtureNS.TestClass;

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

        [Test]
        public void InferringType_ReturnsClasses()
        {
            //Assign
            _handler.InferType = true;
            _handler.IsLazy = true;
            _handler.Property = new FakePropertyInfo(typeof(IEnumerable<SitecoreQueryHandlerFixtureNS   .BaseType>));
            _handler.Query = "/sitecore/content/home/*";

            Item home = _db.GetItem("{98F907F7-CD1A-4C88-AF11-8F38A21A7FE1}");

            //Act
            var results = _handler.GetValue(home, _service) as IEnumerable<SitecoreQueryHandlerFixtureNS.BaseType>;

            //Assert
            Assert.AreEqual(home.Children.Count, results.Count());

            Guid typeOneTemp = new Guid("{5B684B69-F532-4BB2-8A98-02AFCDE4BB84}");
            Guid typeTwoTemp = new Guid("{3902F503-7DC7-48B2-9FD8-1EB878CEBA93}");
            foreach (Item child in home.Children)
            {
                var itemClass = results.FirstOrDefault(x => x.Id == child.ID.Guid);

                Assert.IsNotNull(itemClass, "Failed to load item");

                if (child.TemplateID.Guid == typeOneTemp)
                {
                    Assert.IsTrue(itemClass is SitecoreQueryHandlerFixtureNS.TypeOne);
                }
                else if (child.TemplateID.Guid == typeTwoTemp)
                {
                    Assert.IsTrue(itemClass is SitecoreQueryHandlerFixtureNS.TypeTwo);
                }
                else
                {
                    Assert.IsTrue(itemClass is SitecoreQueryHandlerFixtureNS.BaseType);
                }

            }
        }
    }


    namespace SitecoreQueryHandlerFixtureNS
    {

        public class TestClass
        {
            public virtual Guid Id { get; set; }
            public virtual IEnumerable<TestClass> Results { get; set; }
            public virtual TestClass SingleResult { get; set; }
        }
        [SitecoreClass]
        public class BaseType
        {

            [SitecoreId]
            public virtual Guid Id { get; set; }
        }

        [SitecoreClass(TemplateId = "{5B684B69-F532-4BB2-8A98-02AFCDE4BB84}")]
        public class TypeOne : BaseType
        {

        }
        [SitecoreClass(TemplateId = "{3902F503-7DC7-48B2-9FD8-1EB878CEBA93}")]
        public class TypeTwo : BaseType
        {

        }
    }
}


