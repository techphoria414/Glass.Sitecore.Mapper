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
using Glass.Sitecore.Mapper.Tests.Domain;
using Sitecore.Data.Managers;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldClassHandlerFixture
    {
        SitecoreFieldClassHandler _handler;
        ISitecoreService _service;
        Database _db;
        string _item1Path;
        string _item2Path;


        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldClassHandler();

            Context context = new Context(
                new AttributeConfigurationLoader(
                    "Glass.Sitecore.Mapper.Tests.Data.SitecoreChildrenHandlerFixture,  Glass.Sitecore.Mapper.Tests",
                    "Glass.Sitecore.Mapper.Tests.Domain,  Glass.Sitecore.Mapper.Tests"));

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
            _service = new SitecoreService(_db);

            _item1Path = "/sitecore/content/Data/SitecoreFieldClassHandler/Item1";
            _item2Path = "/sitecore/content/Data/SitecoreFieldClassHandler/Item2";

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
            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(RootClass).GetProperty("LazyLoaded"));

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
            property.Property = new FakePropertyInfo(typeof(NotLoadedClass));

            //Act
            var result = _handler.WillHandle(property, _service.InstanceContext.Datas, _service.InstanceContext.Classes);

            //Assert
            Assert.IsFalse(result);

        }


        #endregion

        #region GetFieldValue

        [Test]
        public void GetFieldValue_LazyLoads()
        {
            //Assign
            Item item = _db.GetItem(_item1Path);
            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(RootClass).GetProperty("LazyLoaded"));

            _handler.ConfigureDataHandler(property);
            RootClass root = new RootClass();


            //Act
            var result = _handler.GetFieldValue(
                item.ID.ToString(),                
                item,                
                _service);

            root.LazyLoaded = result as EmptyTemplate1;

            //Assert
            Assert.IsNotNull(root.LazyLoaded);
            //should not be equal because of proxy class
            Assert.AreNotEqual(typeof(EmptyTemplate1), root.LazyLoaded.GetType());
                        
        }
     
        [Test]
        public void GetFieldValue_InvalidData_ReturnsNull()
        {
            //Assign
            //Assign
            Item item = _db.GetItem(_item1Path);
            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(RootClass).GetProperty("LazyLoaded"));

            _handler.ConfigureDataHandler(property);
            RootClass root = new RootClass();
            
            //Act
            var result = _handler.GetFieldValue(
                "random value",
                item,               
                _service);

            root.LazyLoaded = result as EmptyTemplate1;

            //Assert
            Assert.IsNull(root.LazyLoaded);
        }

       
        [Test]
        public void GetFieldValue_IsLazyFalse_ReturnsConcrete()
        {
            //Assign
            Item item = _db.GetItem(_item1Path);
            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(RootClass).GetProperty("NotLazyLoaded"));

            _handler.ConfigureDataHandler(property);
            RootClass root = new RootClass();


            //Act
            var result = _handler.GetFieldValue(
                item.ID.ToString(),
                item,
                _service);

            root.LazyLoaded = result as EmptyTemplate1;

            //Assert
            Assert.IsNotNull(root.LazyLoaded);
            //should not be equal because of proxy class
            Assert.AreEqual(typeof(EmptyTemplate1), root.LazyLoaded.GetType());
        }

        [Test]
        public void GetFieldValue_IsLazyFalse_ReturnsConcrete_LanguageSpecific()
        {
            //Assign
            Item item = _db.GetItem(_item1Path, LanguageManager.GetLanguage("af-ZA"));
            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(RootClass).GetProperty("NotLazyLoaded"));

            _handler.ConfigureDataHandler(property);
            RootClass root = new RootClass();


            //Act
            var result = _handler.GetFieldValue(
                item.ID.ToString(),
                item,
                _service);

            root.LazyLoaded = result as EmptyTemplate1;

            //Assert
            Assert.IsNotNull(root.LazyLoaded);
            //should not be equal because of proxy class
            Assert.AreEqual(typeof(EmptyTemplate1), root.LazyLoaded.GetType());
            Assert.AreEqual(LanguageManager.GetLanguage("af-ZA"), root.LazyLoaded.Language);
        }

        [Test]
        public void GetFieldValue_InferType_NotLazy_ReturnsConcrete()
        {
            //Assign
            Item item = _db.GetItem(_item2Path);
            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(RootClass).GetProperty("NotLazyLoadedInferred"));

            _handler.ConfigureDataHandler(property);
            RootClass root = new RootClass();


            //Act
            var result = _handler.GetFieldValue(
                item.ID.ToString(),
                item,
                _service);

            root.NotLazyLoadedInferred = result as EmptyTemplate1;

            //Assert
            Assert.IsNotNull(root.NotLazyLoadedInferred);
            Assert.AreNotEqual(typeof(EmptyTemplate1), root.NotLazyLoadedInferred.GetType());
            Assert.AreEqual(typeof(EmptyTemplate2), root.NotLazyLoadedInferred.GetType());
            //the type is inferred so should be EmptyTemplate2
            Assert.IsTrue(root.NotLazyLoadedInferred is EmptyTemplate2);
        }

        [Test]
        public void GetFieldValue_InferType_Lazy_ReturnsConcrete()
        {
            //Assign
            Item item = _db.GetItem(_item2Path);
            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(RootClass).GetProperty("LazyLoadedInferred"));

            _handler.ConfigureDataHandler(property);
            RootClass root = new RootClass();


            //Act
            var result = _handler.GetFieldValue(
                item.ID.ToString(),
                item,
                _service);

            root.LazyLoadedInferred = result as EmptyTemplate1;

            //Assert
            Assert.IsNotNull(root.LazyLoadedInferred);
            Assert.AreNotEqual(typeof(EmptyTemplate1), root.LazyLoadedInferred.GetType());
            Assert.AreNotEqual(typeof(EmptyTemplate2), root.LazyLoadedInferred.GetType());
            //the type is inferred so should be EmptyTemplate2
            Assert.IsTrue(root.LazyLoadedInferred is EmptyTemplate2);
        }

        #endregion

        #region SetFieldValue

        [Test]
        public void SetFieldValue_ValidClass_ReturnsGuid()
        {
            //Assign
            EmptyTemplate1 target = new EmptyTemplate1();
            target.Id = Guid.NewGuid();
            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(RootClass).GetProperty("LazyLoaded"));

            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.SetFieldValue(target, _service);

            //Assert
            Assert.AreEqual(target.Id, new Guid(result));

        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void SetFieldValue_NotLoadedClass_ThrowsException()
        {
            //Assign
            NotLoadedClass target = new NotLoadedClass();
            target.Id = Guid.NewGuid();

            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(RootClass).GetProperty("NotLoaded"));

            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.SetFieldValue(target, _service);

            //Assert
            Assert.AreEqual(target.Id, new Guid(result));

        }


        #endregion

        #region CLASSES

        [SitecoreClass]
        public class RootClass
        {
            [SitecoreField]
            public virtual EmptyTemplate1 LazyLoaded { get; set; }

            [SitecoreField(Setting= SitecoreFieldSettings.DontLoadLazily)]
            public virtual EmptyTemplate1 NotLazyLoaded { get; set; }

            [SitecoreField(Setting = SitecoreFieldSettings.DontLoadLazily | SitecoreFieldSettings.InferType)]
            public virtual EmptyTemplate1 NotLazyLoadedInferred { get; set; }

            [SitecoreField(Setting = SitecoreFieldSettings.InferType)]
            public virtual EmptyTemplate1 LazyLoadedInferred { get; set; }

            [SitecoreField]
            public virtual NotLoadedClass NotLoaded { get; set; }
        }

        public class NotLoadedClass
        {
            public virtual Guid Id { get; set; }
        }

        #endregion
    }

}
