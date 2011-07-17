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

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreChildrenHandlerFixture
    {
        ISitecoreService _service;
        Database _db;
        Guid _itemId;

        SitecoreChildrenHandler _handler;

        [SetUp]
        public void Setup()
        {
            var context = new InstanceContext(
              (new SitecoreClassConfig[]{
                   new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(),
                       Properties = new SitecoreProperty[]{},
                       Type = typeof(SitecoreChildrenHandlerFixtureNS.SubClass),
                       DataHandlers = new AbstractSitecoreDataHandler []{}
                   },
                   new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(),
                       Properties = new SitecoreProperty[]{
                            new SitecoreProperty(){
                                Attribute = new SitecoreIdAttribute(),
                                Property = typeof(SitecoreChildrenHandlerFixtureNS.BaseType).GetProperty("Id")
                            }
                       },
                       Type = typeof(SitecoreChildrenHandlerFixtureNS.BaseType),
                       DataHandlers = new AbstractSitecoreDataHandler []{
                        new SitecoreIdDataHandler(){
                               Property = typeof(SitecoreChildrenHandlerFixtureNS.BaseType).GetProperty("Id")
                        }
                       }
                   },
                   new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(){
                           
                       },
                       Properties = new SitecoreProperty[]{
                            new SitecoreProperty(){
                                Attribute = new SitecoreIdAttribute(),
                                Property = typeof(SitecoreChildrenHandlerFixtureNS.TypeOne).GetProperty("Id")
                            }
                       },
                       Type = typeof(SitecoreChildrenHandlerFixtureNS.TypeOne),
                       DataHandlers = new AbstractSitecoreDataHandler []{
                        new SitecoreIdDataHandler(){
                               Property = typeof(SitecoreChildrenHandlerFixtureNS.TypeOne).GetProperty("Id")
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
                                Property = typeof(SitecoreChildrenHandlerFixtureNS.TypeTwo).GetProperty("Id")
                            }
                       },
                       Type = typeof(SitecoreChildrenHandlerFixtureNS.TypeTwo),
                       DataHandlers = new AbstractSitecoreDataHandler []{
                        new SitecoreIdDataHandler(){
                               Property = typeof(SitecoreChildrenHandlerFixtureNS.TypeTwo).GetProperty("Id")
                        }
                       },
                       TemplateId = new Guid("{3902F503-7DC7-48B2-9FD8-1EB878CEBA93}")
                   }

               }).ToDictionary(), new AbstractSitecoreDataHandler[] { });



            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _service = new SitecoreService(_db, context);

            _itemId = new Guid("{D22C2A23-DF8A-4EC1-AD52-AE15FE63F937}");
            _handler = new SitecoreChildrenHandler();
        }

        #region GetValue

        [Test]
        public void GetValue_ReturnsChildren_UsingLazy()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreChildrenAttribute(),
                Property = typeof(SitecoreChildrenHandlerFixtureNS.TestClass).GetProperty("Children")
            };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.GetValue(item, _service) as Enumerable<SitecoreChildrenHandlerFixtureNS.SubClass>;
            SitecoreChildrenHandlerFixtureNS.TestClass assignTest = new Glass.Sitecore.Mapper.Tests.Data.SitecoreChildrenHandlerFixtureNS.TestClass();
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

            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue(item, _service) as IEnumerable<SitecoreChildrenHandlerFixtureNS.SubClass>;
            SitecoreChildrenHandlerFixtureNS.TestClass assignTest = new Glass.Sitecore.Mapper.Tests.Data.SitecoreChildrenHandlerFixtureNS.TestClass();
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
            var result = _handler.WillHandle(property, null, null);


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
            var result = _handler.WillHandle(property, null, null);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

        [Test]
        public void InferringType_ReturnsClasses()
        {
            //Assign
            _handler.InferType = true;
            _handler.IsLazy = true;
            _handler.Property = new FakePropertyInfo(typeof(IEnumerable<SitecoreChildrenHandlerFixtureNS.BaseType>));

            Item home = _db.GetItem("{98F907F7-CD1A-4C88-AF11-8F38A21A7FE1}");

            //Act
            var results = _handler.GetValue(home, _service) as IEnumerable<SitecoreChildrenHandlerFixtureNS.BaseType>;

            //Assert
            Assert.AreEqual(home.Children.Count, results.Count());
            
            Guid typeOneTemp = new Guid("{5B684B69-F532-4BB2-8A98-02AFCDE4BB84}");
            Guid typeTwoTemp = new Guid("{3902F503-7DC7-48B2-9FD8-1EB878CEBA93}");
            foreach(Item child in home.Children){
                var itemClass = results.FirstOrDefault(x=>x.Id == child.ID.Guid);

                Assert.IsNotNull(itemClass, "Failed to load item");

                if(child.TemplateID.Guid == typeOneTemp){
                       Assert.IsTrue(itemClass is SitecoreChildrenHandlerFixtureNS.TypeOne);
                }
                else if(child.TemplateID.Guid == typeTwoTemp){
                       Assert.IsTrue(itemClass is SitecoreChildrenHandlerFixtureNS.TypeTwo);
                }
                else{
                    Assert.IsTrue(itemClass is SitecoreChildrenHandlerFixtureNS.BaseType);
                }

            }




        }
    }

    namespace SitecoreChildrenHandlerFixtureNS
    {
        public class TestClass
        {
            public virtual IEnumerable<SubClass> Children { get; set; }
            public virtual IList<SubClass> List { get; set; }
        }
        public class SubClass { }

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
