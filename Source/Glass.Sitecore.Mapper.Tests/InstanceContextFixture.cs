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
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.SecurityModel;
using Glass.Sitecore.Mapper.Proxies;
using Glass.Sitecore.Mapper.Tests.Domain;
using Glass.Sitecore.Mapper.Tests.InstanceContextFixtureNS;

namespace Glass.Sitecore.Mapper.Tests
{
    [TestFixture]
    public class InstanceContextFixture
    {

        ISitecoreService _service;
        Database _db;
        Guid _itemId;
        Guid _itemId2;

        [SetUp]
        public void Setup()
        {
            Context context = new Context(
                new AttributeConfigurationLoader(
                    "Glass.Sitecore.Mapper.Tests.InstanceContextFixtureNS,  Glass.Sitecore.Mapper.Tests",
                    "Glass.Sitecore.Mapper.Tests.Domain,  Glass.Sitecore.Mapper.Tests"));


            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _service = new SitecoreService(_db);

            _itemId = new Guid("{8A317CBA-81D4-4F9E-9953-64C4084AECCA}");
            _itemId2 = new Guid("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}");

        }

        #region CreateClass

        [Test]
        public void CreateClass_NullItem_ReturnsNull()
        {
            //Assign
            Item item = null;

            //Act 
            var result = _service.CreateClass<InstanceContextFixtureNS.TestClass>(false, false, item);

            //Assert 
            Assert.IsNull(result);

        }
        [Test]
        public void CreateClass_ValidItem_CreatesClass()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            //Act
            var result = _service.CreateClass<InstanceContextFixtureNS.TestClass>(false, false, item);

            //Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void CreateClass_ValidItem_CreatesClassAndCorrectId()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            //Act
            var result = _service.CreateClass<InstanceContextFixtureNS.TestClass>(false, false, item);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_itemId, result.Id);
        }

        #endregion

        #region GetClassId

        [Test]
        [ExpectedException(typeof(SitecoreIdException))]
        public void GetClassId_NoIdProperty_ThrowsException()
        {
            //Assign
            InstanceContextFixtureNS.TestClass2 testClass = new Glass.Sitecore.Mapper.Tests.InstanceContextFixtureNS.TestClass2();

            //Act
            var result = _service.InstanceContext.GetClassId(typeof(InstanceContextFixtureNS.TestClass2),testClass);

            //Assert
            //no assertion, expections should be thrown
        }
        [Test]
        public void GetClassId_HasId_IdIsReturned()
        {
            //Assign
            InstanceContextFixtureNS.TestClass testClass = new Glass.Sitecore.Mapper.Tests.InstanceContextFixtureNS.TestClass();
            testClass.Id = _itemId;

            //Act 
            var result = _service.InstanceContext.GetClassId(typeof(InstanceContextFixtureNS.TestClass), testClass);

            //Assert
            Assert.AreEqual(_itemId, result);
        }

        #endregion

        #region GetSitecoreClass

        [Test]
        public void GetSitecoreClass_LoadedClass_ReturnsClassConfig()
        {
            //Assign
            SitecoreClassConfig config = null;

            //Act
            config = _service.InstanceContext.GetSitecoreClass(typeof(InstanceContextFixtureNS.TestClass2));

            //Assert
            Assert.IsNotNull(config);
            Assert.AreEqual(typeof(InstanceContextFixtureNS.TestClass2), config.Type);

        }
        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetSitecoreClass_NotLoadedClass_ThrowsException()
        {
            //Assign
            SitecoreClassConfig config = null;

            //Act
            config = _service.InstanceContext.GetSitecoreClass(typeof(Domain.NotLoaded));

            //Assert
            //no asserts, exception should be thrown


        }

        #endregion

        #region InferredType Test

        [Test]
        public void GetsInferredType_InfersTypeCorrectly()
        {
            //Assign
            Item item = _db.GetItem("/sitecore/content/InstanceContext/Item1");

            //Act
            EmptyTemplate1 temp1 = _service.CreateClass<EmptyTemplate1>(false, true, item);
            
            //Assert

            Assert.IsNotNull(temp1);
            Assert.IsTrue(temp1 is EmptyTemplate2);


        }

        [Test]
        public void GetsInferredType_InfersTypeCorrectly_WithTwoDifferentClassesDefinedToSupportTheTemplate()
        {
            //Assign
            Item item = _db.GetItem("/sitecore/content/InstanceContext/Item1");

            //Act
            EmptyTemplate1 temp1 = _service.CreateClass<EmptyTemplate1>(false, true, item);
            InferredTypeBase temp2 = _service.CreateClass<InferredTypeBase>(false, true, item);

            //Assert

            Assert.IsNotNull(temp1);
            Assert.IsTrue(temp1 is EmptyTemplate2);
            Assert.IsNotNull(temp2);
            Assert.IsTrue(temp2 is InferredTypeSub);


        }
        #endregion

        #region Method - GetDataHandler

        [Test]
        public void GetDataHandler_HandlerFromStandardList()
        {
            //Assign
            InstanceContext context = new InstanceContext(new Dictionary<Type,SitecoreClassConfig>(), Utility.GetDefaultDataHanlders());
            SitecoreProperty property = new SitecoreProperty();
            property.Attribute = new SitecoreFieldAttribute();
            property.Property = typeof(InstanceContextFixtureNS.GetDataHandlerClass).GetProperty("IntProperty");

            //Act
            var result = context.GetDataHandler(property);

            //Assert
            Assert.IsTrue(result is SitecoreFieldIntegerHandler);
        }

        [Test]
        public void GetDataHandler_HandlerFromDataHandler()
        {
            //Assign
            InstanceContext context = new InstanceContext(new Dictionary<Type, SitecoreClassConfig>(), Utility.GetDefaultDataHanlders());
            SitecoreProperty property = new SitecoreProperty();
            property.Attribute = new SitecoreFieldAttribute();
            property.Property = typeof(InstanceContextFixtureNS.GetDataHandlerClass).GetProperty("IntProperty");
            property.Attribute.DataHandler = typeof(InstanceContextFixtureNS.CustomDataHandler);

            //Act
            var result = context.GetDataHandler(property);

            //Assert
            Assert.IsTrue(result is InstanceContextFixtureNS.CustomDataHandler);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetDataHandler_HandlerFromDataHandlerNotCorrectClass_ThrowsException()
        {
            //Assign
            InstanceContext context = new InstanceContext(new Dictionary<Type, SitecoreClassConfig>(), Utility.GetDefaultDataHanlders());
            SitecoreProperty property = new SitecoreProperty();
            property.Attribute = new SitecoreFieldAttribute();
            property.Property = typeof(InstanceContextFixtureNS.GetDataHandlerClass).GetProperty("IntProperty");
            property.Attribute.DataHandler = typeof(InstanceContextFixtureNS.GetDataHandlerClass);

            //Act
            var result = context.GetDataHandler(property);

            //Assert
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetDataHandler_HandlerFromDataHandlerCorrectClassButErrorWhenIfnstantiated_ThrowsException()
        {
            //Assign
            InstanceContext context = new InstanceContext(new Dictionary<Type, SitecoreClassConfig>(), Utility.GetDefaultDataHanlders());
            SitecoreProperty property = new SitecoreProperty();
            property.Attribute = new SitecoreFieldAttribute();
            property.Property = typeof(InstanceContextFixtureNS.GetDataHandlerClass).GetProperty("IntProperty");
            property.Attribute.DataHandler = typeof(InstanceContextFixtureNS.GetDataHandlerClassErroring);

            //Act
            var result = context.GetDataHandler(property);

            //Assert
        }


        #endregion

    }

    namespace InstanceContextFixtureNS
    {

        [SitecoreClass]
        public class InferredTypeBase
        {
        }
        [SitecoreClass(TemplateId = "{4AE4FCCE-F176-405F-9FFB-CF3AFC23F403}")]
        public class InferredTypeSub : InferredTypeBase
        {
        }


        [SitecoreClass]
        public class TestClass
        {

            [SitecoreId]
            public Guid Id { get; set; }
        }

        [SitecoreClass]
        public class TestClass2
        {
        }
       
        [SitecoreClass]
        public class TestClass4
        {
            [SitecoreField]
            public virtual string SingleLineText { get; set; }
        }

        public class GetDataHandlerClass
        {
            public virtual int IntProperty { get; set; }
        }

        public class GetDataHandlerClassErroring
        {
            public GetDataHandlerClassErroring(){
                throw new ApplicationException();
            }
            public virtual int IntProperty { get; set; }
        }

        public class CustomDataHandler : AbstractSitecoreDataHandler
        {
            public override bool WillHandle(SitecoreProperty property, IEnumerable<AbstractSitecoreDataHandler> datas, Dictionary<Type, SitecoreClassConfig> classes)
            {
                throw new NotImplementedException();
            }

            public override object GetValue(Item item, ISitecoreService service)
            {
                throw new NotImplementedException();
            }

            public override void SetValue(Item item, object value, ISitecoreService service)
            {
                throw new NotImplementedException();
            }

            public override bool CanSetValue
            {
                get { throw new NotImplementedException(); }
            }
        }


    }
}
