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
            #region TestClass1

            var tc1Property = new SitecoreProperty()
            {
                Attribute = new SitecoreIdAttribute(),
                Property = typeof(InstanceContextFixtureNS.TestClass).GetProperty("Id")
            };
            var tc1HandlerId = new SitecoreIdDataHandler();
            tc1HandlerId.ConfigureDataHandler(tc1Property);

            #endregion
            #region TestClass4

            var tc4Property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = typeof(InstanceContextFixtureNS.TestClass4).GetProperty("SingleLineText")
            };
            var tc4HandlerString = new SitecoreFieldStringHandler();
            tc4HandlerString.ConfigureDataHandler(tc4Property);

            #endregion


            var context = new InstanceContext((new SitecoreClassConfig[]{
                new SitecoreClassConfig(){
                    ClassAttribute = new SitecoreClassAttribute(),
                    Properties = new SitecoreProperty[]{
                        tc1Property
                    },
                    Type = typeof(InstanceContextFixtureNS.TestClass),
                    DataHandlers = new AbstractSitecoreDataHandler[]{
                        tc1HandlerId
                    },
                    IdProperty = tc1Property
                },
                new SitecoreClassConfig(){
                    ClassAttribute = new SitecoreClassAttribute(),
                    Properties = new SitecoreProperty[]{},
                    Type = typeof(InstanceContextFixtureNS.TestClass2),
                    DataHandlers = new AbstractSitecoreDataHandler[]{}
                },
                new SitecoreClassConfig(){
                    ClassAttribute = new SitecoreClassAttribute(),
                    Properties = new SitecoreProperty[]{
                       tc4Property
                    },
                    Type = typeof(InstanceContextFixtureNS.TestClass4),
                    DataHandlers = new AbstractSitecoreDataHandler[]{tc4HandlerString}
                }
            
            }).ToDictionary(), new AbstractSitecoreDataHandler[] { });

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _service = new SitecoreService(_db, context);

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
            config = _service.InstanceContext.GetSitecoreClass(typeof(InstanceContextFixtureNS.TestClass3));

            //Assert
            //no asserts, exception should be thrown


        }

        #endregion

     
    }

    namespace InstanceContextFixtureNS
    {

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
        public class TestClass3
        {
        }
        [SitecoreClass]
        public class TestClass4
        {
            [SitecoreField]
            public virtual string SingleLineText { get; set; }
        }


    }
}
