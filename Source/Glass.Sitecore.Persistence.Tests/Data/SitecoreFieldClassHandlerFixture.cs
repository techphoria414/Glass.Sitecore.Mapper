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
using Glass.Sitecore.Persistence.Data;
using Glass.Sitecore.Persistence.Configuration;
using Glass.Sitecore.Persistence.Configuration.Attributes;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Persistence.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldClassHandlerFixture
    {
        SitecoreFieldClassHandler _handler;
        InstanceContext _context;
        Database _db;
        Guid _itemId;


        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldClassHandler();
            _context = new InstanceContext(
                new SitecoreClassConfig[]{
                    new SitecoreClassConfig(){
                         ClassAttribute = new SitecoreClassAttribute(),
                         Properties = new SitecoreProperty[]{
                            new SitecoreProperty(){
                                Attribute =new SitecoreIdAttribute(),
                                DataHandler = new SitecoreIdDataHandler(),
                                Property = typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass).GetProperty("Id")
                            }
                         },
                         Type = typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass)
                    },
                    
                },
                new ISitecoreDataHandler[] { }
                );

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _itemId = new Guid("{8A317CBA-81D4-4F9E-9953-64C4084AECCA}");

        }

        #region WillHandle

        [Test]
        public void WillHandle_LoadedClass_ReturnsTrue()
        {

            //Assign
            SitecoreProperty property = new SitecoreProperty();
            property.Attribute = new SitecoreFieldAttribute();
            property.Property = new FakePropertyInfo(typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass));

            //Act
            var result = _handler.WillHandle(property, _context);

            //Assert
            Assert.IsTrue(result);

        }
        [Test]
        public void WillHandle_NotLoadedClass_ReturnsFalse()
        {

            //Assign
            SitecoreProperty property = new SitecoreProperty();
            property.Property = new FakePropertyInfo(typeof(SitecoreFieldClassHandlerFixtureNS.NotLoadedClass));

            //Act
            var result = _handler.WillHandle(property, _context);

            //Assert
            Assert.IsFalse(result);

        }


        #endregion

        #region GetFieldValue

        [Test]
        public void GetFieldValue_GuidId_CreatesProxyClass_UsingSetOnProperty()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Persistence.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            //Act
            var result = _handler.GetFieldValue(
                _itemId.ToString(),
                parent,
                item,
                new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                }, 
                _context);

            parent.Child = result as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

            //Assert
            Assert.AreNotEqual(typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass), parent.Child.GetType());

            parent.Child.CallMe = "test";
        }

        [Test]
        public void GetFieldValue_GuidId_CreatesProxyClass_UsingGetOnProperty()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Persistence.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            //Act
            var result = _handler.GetFieldValue(
                _itemId.ToString(),
                parent,
                item,
                new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                },
                _context);

            parent.Child = result as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

            //Assert
            Assert.AreNotEqual(typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass), parent.Child.GetType());

            var callMe = parent.Child.CallMe;
        }

        [Test]
        public void GetFieldValue_GuidId_SetOnProxyUpdatesActual()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Persistence.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            //Act
            var result = _handler.GetFieldValue(
                _itemId.ToString(),
                parent,
                item,
                new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                },
                _context) as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

            parent.Child = result;
            result.CallMe = "Some value";

            //Assert

            var test = parent.Child.CallMe;

            Assert.AreEqual(result.CallMe, test);
        }
        [Test]
        public void GetFieldValue_GuidId_SetOnActualUpdatesProxy()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Persistence.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            //Act
            var result = _handler.GetFieldValue(
                _itemId.ToString(),
                parent,
                item,
                new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                },
                _context) as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

            parent.Child = result;
            parent.Child.CallMe = "Some value";

            //Assert

            var test = result.CallMe;

            Assert.AreEqual(result.CallMe, test);
        }

        [Test]
        public void GetFieldValue_Path_CreatesProxyClass()
        {
            //Assign
            string path = "/sitecore/content/Glass/Test1";

            Item item = _db.GetItem(new ID(_itemId));
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Persistence.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            //Act
            var result = _handler.GetFieldValue(
                path,
                parent,
                item,
                new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                },
                _context);

            parent.Child = result as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

            //Assert
            Assert.AreNotEqual(typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass), parent.Child.GetType());

            parent.Child.CallMe = "test";
        }
        [Test]
        public void GetFieldValue_InvalidData_ReturnsNull()
        {
            //Assign
            string path = "agfaegfaeg";

            Item item = _db.GetItem(new ID(_itemId));
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Persistence.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            //Act
            var result = _handler.GetFieldValue(
                path,
                parent,
                item,
                new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                },
                _context);

            parent.Child = result as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

            //Assert
            Assert.IsNull(parent.Child);
        }

     

        #endregion

        #region SetFieldValue

        [Test]
        public void SetFieldValue_ValidClass_ReturnsGuid()
        {
            //Assign
            SitecoreFieldClassHandlerFixtureNS.LoadedClass target= new Glass.Sitecore.Persistence.Tests.Data.SitecoreFieldClassHandlerFixtureNS.LoadedClass();
            target.Id = _itemId;

            //Act
            var result = _handler.SetFieldValue(typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass), target, _context);

            //Assert
            Assert.AreEqual(_itemId, new Guid(result));

        }

        [Test]
        [ExpectedException(typeof(PersistenceException))]
        public void SetFieldValue_NotLoadedClass_ThrowsException()
        {
            //Assign
            SitecoreFieldClassHandlerFixtureNS.NotLoadedClass target = new Glass.Sitecore.Persistence.Tests.Data.SitecoreFieldClassHandlerFixtureNS.NotLoadedClass();
            target.Id = _itemId;

            //Act
            var result = _handler.SetFieldValue(typeof(SitecoreFieldClassHandlerFixtureNS.NotLoadedClass), target, _context);

            //Assert
            Assert.AreEqual(_itemId, new Guid(result));

        }


        #endregion
    }

    namespace SitecoreFieldClassHandlerFixtureNS
    {
        public class ParentClass
        {
            public LoadedClass Child { get; set; }
        }
        public class LoadedClass
        {
            public virtual Guid Id { get; set; }
            public virtual string CallMe { get; set; }
        }
        public class NotLoadedClass {
            public virtual Guid Id { get; set; }
        }
    }
}
