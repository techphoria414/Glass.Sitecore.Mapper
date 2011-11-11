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
    public class SitecoreFieldClassHandlerFixture
    {
        SitecoreFieldClassHandler _handler;
        ISitecoreService _service;
        Database _db;
        Guid _itemId;


        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldClassHandler();

            var loadedClassIdProperty = new SitecoreProperty()
            {
                Attribute = new SitecoreIdAttribute(),
                Property = typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass).GetProperty("Id")
            };



            var baseTypeIdProperty = new SitecoreProperty()
            {
                Attribute = new SitecoreIdAttribute(),
                Property = typeof(SitecoreFieldClassHandlerFixtureNS.BaseType).GetProperty("Id")
            };


            var context = new InstanceContext(
                (new SitecoreClassConfig[]{
                    new SitecoreClassConfig(){
                         ClassAttribute = new SitecoreClassAttribute(),
                         Properties = new SitecoreProperty[]{
                            loadedClassIdProperty
                         },
                         Type = typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass),
                         DataHandlers = new AbstractSitecoreDataHandler[]{},
                         IdProperty = loadedClassIdProperty
                    },
                    new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(),
                       Properties = new SitecoreProperty[]{
                            baseTypeIdProperty
                       },
                       IdProperty= baseTypeIdProperty,
                       Type = typeof(SitecoreFieldClassHandlerFixtureNS.BaseType),
                       DataHandlers = new AbstractSitecoreDataHandler []{
                        new SitecoreIdDataHandler(){
                               Property = typeof(SitecoreFieldClassHandlerFixtureNS.BaseType).GetProperty("Id")
                        }
                       }
                   },
                   new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(){
                           
                       },
                       Properties = new SitecoreProperty[]{
                            new SitecoreProperty(){
                                Attribute = new SitecoreIdAttribute(),
                                Property = typeof(SitecoreFieldClassHandlerFixtureNS.TypeOne).GetProperty("Id")
                            }
                       },
                       Type = typeof(SitecoreFieldClassHandlerFixtureNS.TypeOne),
                       DataHandlers = new AbstractSitecoreDataHandler []{
                        new SitecoreIdDataHandler(){
                               Property = typeof(SitecoreFieldClassHandlerFixtureNS.TypeOne).GetProperty("Id")
                        }
                       },
                       TemplateId = new Guid("{1D0EE1F5-21E0-4C5B-8095-EDE2AF3D3300}")
                   },
                    
                }).ToDictionary(),
                new AbstractSitecoreDataHandler[] { }
                );

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _service = new SitecoreService(_db, context);

            // /sitecore/content/Glass/Test2
            _itemId = new Guid("{8A317CBA-81D4-4F9E-9953-64C4084AECCA}");

        }


        #region ConfigureHandler
        [Test]
        public void ConfigureHandler_SetIsLazy_True()
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute();
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = attr,
                Property = new FakePropertyInfo(typeof(string))//this can be anything                
            };

            //Act
            _handler.ConfigureDataHandler(property);

            //Assert
            Assert.IsTrue(_handler.IsLazy);
        }

        [Test]
        public void ConfigureHandler_SetIsLazy_False()
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute();
            attr.Setting = SitecoreFieldSettings.DontLoadLazily;
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = attr,
                Property = new FakePropertyInfo(typeof(string))//this can be anything                
            };

            //Act
            _handler.ConfigureDataHandler(property);

            //Assert
            Assert.IsFalse(_handler.IsLazy);
        }
        [Test]
        public void ConfigureHandler_SetIsLazy_True_InferType_False()
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute();
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = attr,
                Property = new FakePropertyInfo(typeof(string))//this can be anything                
            };

            //Act
            _handler.ConfigureDataHandler(property);

            //Assert
            Assert.IsTrue(_handler.IsLazy);
            Assert.IsFalse(_handler.InferType);
        }
        [Test]
        public void ConfigureHandler_SetIsLazy_True_InferType_True()
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute();
            attr.Setting = SitecoreFieldSettings.InferType;
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = attr,
                Property = new FakePropertyInfo(typeof(string))//this can be anything                
            };

            //Act
            _handler.ConfigureDataHandler(property);

            //Assert
            Assert.IsTrue(_handler.IsLazy);
            Assert.IsTrue(_handler.InferType);
        }
        [Test]
        public void ConfigureHandler_SetIsLazy_False_InferType_True()
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute();
            attr.Setting = SitecoreFieldSettings.DontLoadLazily | SitecoreFieldSettings.InferType;
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = attr,
                Property = new FakePropertyInfo(typeof(string))//this can be anything                
            };

            //Act
            _handler.ConfigureDataHandler(property);

            //Assert
            Assert.IsFalse(_handler.IsLazy);
            Assert.IsTrue(_handler.InferType);
        }

        #endregion


        #region WillHandle

        [Test]
        public void WillHandle_LoadedClass_ReturnsTrue()
        {

            //Assign
            SitecoreProperty property = new SitecoreProperty();
            property.Attribute = new SitecoreFieldAttribute();
            property.Property = new FakePropertyInfo(typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass));

            //Act
            var result = _handler.WillHandle(property, _service.InstanceContext.Datas, _service.InstanceContext.Classes);

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
            var result = _handler.WillHandle(property, _service.InstanceContext.Datas, _service.InstanceContext.Classes);

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
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            SitecoreProperty property = new SitecoreProperty()
                 {
                     Attribute = new SitecoreFieldAttribute(),
                     Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                 };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.GetFieldValue(
                _itemId.ToString(),                
                item,                
                _service);

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
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            SitecoreProperty property = new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.GetFieldValue(
                _itemId.ToString(),
                item,
                _service);

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
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            SitecoreProperty property = new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.GetFieldValue(
                _itemId.ToString(),
                item,
                _service) as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

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
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            SitecoreProperty property = new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                };

            _handler.ConfigureDataHandler(property);
            
            //Act
            var result = _handler.GetFieldValue(
                _itemId.ToString(),
                item,
                _service) as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

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
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            SitecoreProperty property = new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                };
            _handler.ConfigureDataHandler(property);
            
            //Act
            var result = _handler.GetFieldValue(
                path,
                item,
                _service);

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
            SitecoreFieldClassHandlerFixtureNS.ParentClass parent = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.ParentClass();
            SitecoreProperty property = new SitecoreProperty()
                {
                    Attribute = new SitecoreFieldAttribute(),
                    Property = typeof(SitecoreFieldClassHandlerFixtureNS.ParentClass).GetProperty("Child")
                };
            _handler.ConfigureDataHandler(property);
            
            //Act
            var result = _handler.GetFieldValue(
                path,
                item,               
                _service);

            parent.Child = result as SitecoreFieldClassHandlerFixtureNS.LoadedClass;

            //Assert
            Assert.IsNull(parent.Child);
        }

        [Test]
        public void GetFieldValue_IsLazyTrue_ProxyIsReturned()
        {
            //need to test that if SitecoreFieldAttribute.IsLazy is true return a proxy
        }
        [Test]
        public void GetFieldValue_IsLazyFalse_ReturnsConcrete()
        {
            //need to test that if SitecoreFieldAttribute.IsLazy is flase return a concrete class
        }



        #endregion

        #region SetFieldValue

        [Test]
        public void SetFieldValue_ValidClass_ReturnsGuid()
        {
            //Assign
            SitecoreFieldClassHandlerFixtureNS.LoadedClass target = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.LoadedClass();
            target.Id = _itemId;
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(SitecoreFieldClassHandlerFixtureNS.LoadedClass))
            };
            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.SetFieldValue(target, _service);

            //Assert
            Assert.AreEqual(_itemId, new Guid(result));

        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void SetFieldValue_NotLoadedClass_ThrowsException()
        {
            //Assign
            SitecoreFieldClassHandlerFixtureNS.NotLoadedClass target = new Glass.Sitecore.Mapper.Tests.Data.SitecoreFieldClassHandlerFixtureNS.NotLoadedClass();
            target.Id = _itemId;

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(SitecoreFieldClassHandlerFixtureNS.NotLoadedClass))
            };

            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.SetFieldValue(target, _service);

            //Assert
            Assert.AreEqual(_itemId, new Guid(result));

        }


        #endregion

        //TODO: Test infer type with lazy and not lazy

        [Test]
        public void InferType_NotLazy()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            _handler.Property = new FakePropertyInfo(typeof(BaseItem));
            _handler.IsLazy = false;
            _handler.InferType = true;
            //Act
            var result = _handler.GetFieldValue(
                _itemId.ToString(),
                item,
                _service) as SitecoreFieldClassHandlerFixtureNS.BaseType ;

           
            //Assert

            Assert.IsNotNull(result);
            Assert.IsTrue(result is SitecoreFieldClassHandlerFixtureNS.TypeOne);
            Assert.AreEqual(_itemId, result.Id);




        }

        [Test]
        public void InferType_IsLazy()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            _handler.Property = new FakePropertyInfo(typeof(SitecoreFieldClassHandlerFixtureNS.BaseType));
            _handler.IsLazy = false;
            _handler.InferType = true;
            //Act
            var result = _handler.GetFieldValue(
                _itemId.ToString(),
                item,
                _service) as SitecoreFieldClassHandlerFixtureNS.BaseType;


            //Assert

            Assert.IsNotNull(result);
            Assert.IsTrue(result is SitecoreFieldClassHandlerFixtureNS.TypeOne);
            Assert.AreEqual(_itemId, result.Id);
        }
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
        public class NotLoadedClass
        {
            public virtual Guid Id { get; set; }
        }

        [SitecoreClass]
        public class BaseType
        {

            [SitecoreId]
            public virtual Guid Id { get; set; }
        }

        [SitecoreClass(TemplateId = "{1D0EE1F5-21E0-4C5B-8095-EDE2AF3D3300}")]
        public class TypeOne : BaseType
        {

        }
    }
}
