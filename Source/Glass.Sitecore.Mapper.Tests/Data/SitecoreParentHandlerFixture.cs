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
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreParentHandlerFixture
    {

        SitecoreParentHandler _handler;
        Guid _itemId;
        Database _db;
        InstanceContext _context;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreParentHandler();

            _itemId = new Guid("{8A317CBA-81D4-4F9E-9953-64C4084AECCA}");
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
            _context = new InstanceContext(
                (new SitecoreClassConfig[]{
                    new SitecoreClassConfig(){
                        ClassAttribute = new SitecoreClassAttribute(),
                        Properties = new SitecoreProperty[]{
                            new SitecoreProperty(){
                                Attribute = new SitecoreIdAttribute(),
                                DataHandler = new SitecoreIdDataHandler(),
                                Property = typeof(SitecoreParentHandlerFixtureNS.ChildClass).GetProperty("Id")
                            }
                        },
                        Type = typeof(SitecoreParentHandlerFixtureNS.ChildClass)
                    }
                }).ToDictionary(),
                new AbstractSitecoreDataHandler[] { });
                
        }
     
        #region WillHandle

        [Test]
        public void WillHandle_HandlesParentAttribute_ReturnsTrue()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreParentAttribute()
            };

            //Act
            var result = _handler.WillHandle(property, _context.Datas, _context.Classes);

            //Assert

            Assert.IsTrue(result);
        }

        [Test]
        public void WillHandle_RejectNonParentAttribute_ReturnsFalse()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreIdAttribute()
            };

            //Act
            var result = _handler.WillHandle(property, _context.Datas, _context.Classes);

            //Assert

            Assert.IsFalse(result);
        }


        #endregion

        #region GetValue

        [Test]
        public void GetValue_LazyLoad_ReturnsProxy()
        {
            
            //Assign
            SitecoreParentHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreParentHandlerFixtureNS.ParentClass();
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreProperty property = new SitecoreProperty(){
                Attribute =new  SitecoreParentAttribute(),
                DataHandler = _handler,
                Property = typeof(SitecoreParentHandlerFixtureNS.ParentClass).GetProperty("Child")
            };

            //Act
            var result = _handler.GetValue(parent, item, property, _context) as SitecoreParentHandlerFixtureNS.ChildClass;
            parent.Child = result;
            //Assert

            Assert.AreNotEqual(typeof(SitecoreParentHandlerFixtureNS.ChildClass), parent.Child.GetType());
            Assert.IsTrue(parent.Child is SitecoreParentHandlerFixtureNS.ChildClass);
        }

        [Test]
        public void GetValue_NotLazy_ReturnsInstance()
        {

            //Assign
            SitecoreParentHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreParentHandlerFixtureNS.ParentClass();
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreParentAttribute() { IsLazy = false },
                DataHandler = _handler,
                Property = typeof(SitecoreParentHandlerFixtureNS.ParentClass).GetProperty("Child")
            };

            //Act
            var result = _handler.GetValue(parent, item, property, _context) as SitecoreParentHandlerFixtureNS.ChildClass;
            parent.Child = result;
            //Assert

            Assert.AreEqual(typeof(SitecoreParentHandlerFixtureNS.ChildClass), parent.Child.GetType());
        }


        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetValue_ParentClassNotLoaded_ThrowsException()
        {

            //Assign
            SitecoreParentHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreParentHandlerFixtureNS.ParentClass();
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreParentAttribute(),
                DataHandler = _handler,
                Property = typeof(SitecoreParentHandlerFixtureNS.ParentClass).GetProperty("NotLoaded")
            };

            //Act
            var result = _handler.GetValue(parent, item, property, _context) as SitecoreParentHandlerFixtureNS.ChildClassNotLoaded;
            parent.NotLoaded = result;

            parent.NotLoaded.CallMe = "";
            //Assert
            //expecting an exception
        }

        #endregion


    }

    namespace SitecoreParentHandlerFixtureNS
    {
        public class ParentClass
        {
            public ChildClass Child{get;set;}
            public ChildClassNotLoaded NotLoaded { get; set; }
        }
        public class ChildClass{
            public virtual Guid Id { get; set; }
            public virtual string CallMe{get;set;}
        }
        public class ChildClassNotLoaded
        {
            public virtual string CallMe { get; set; }

        }
        
    }
}
