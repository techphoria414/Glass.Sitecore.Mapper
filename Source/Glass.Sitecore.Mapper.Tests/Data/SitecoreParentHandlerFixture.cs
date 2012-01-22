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

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreParentHandlerFixture
    {

        SitecoreParentHandler _handler;
        string _itemPath;
        Database _db;
        ISitecoreService _service;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreParentHandler();

            _itemPath  = "/sitecore/content/Data/SitecoreParentHandler/Parent/Child";

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
            Context context = new Context(
                   new AttributeConfigurationLoader(
                       "Glass.Sitecore.Mapper.Tests.Domain,  Glass.Sitecore.Mapper.Tests"));

            _service = new SitecoreService(_db);
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
            var result = _handler.WillHandle(property, _service.InstanceContext.Datas, _service.InstanceContext.Classes);

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
            var result = _handler.WillHandle(property, _service.InstanceContext.Datas, _service.InstanceContext.Classes);

            //Assert

            Assert.IsFalse(result);
        }


        #endregion

        #region GetValue

        [Test]
        public void GetValue_LazyLoad_ReturnsProxy()
        {
            
            //Assign
            
            Item item = _db.GetItem(_itemPath);
          
            
            SitecoreProperty property = new SitecoreProperty(){
                Attribute =new  SitecoreParentAttribute(),
                Property = new  FakePropertyInfo(typeof(EmptyTemplate1))
            };

            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue( item, _service) as EmptyTemplate1;
         
            //Assert

            Assert.AreEqual(item.Parent.ID.Guid, result.Id);
            Assert.AreNotEqual(typeof(EmptyTemplate1), result.GetType());
        }

        [Test]
        public void GetValue_NotLazy_ReturnsInstance()
        {

            //Assign
            Item item = _db.GetItem(_itemPath);
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreParentAttribute() { IsLazy = false },
                Property = new FakePropertyInfo(typeof(EmptyTemplate1))
            };

            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue(item, _service) as EmptyTemplate1;
            //Assert

            Assert.AreEqual(item.Parent.ID.Guid, result.Id);
            Assert.AreEqual(typeof(EmptyTemplate1), result.GetType());
        }


        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetValue_ParentClassNotLoaded_ThrowsException()
        {

            //Assign
            Item item = _db.GetItem(_itemPath);

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreParentAttribute(),
                Property = new FakePropertyInfo(typeof(NotLoaded))

            };

            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.GetValue( item, _service) as EmptyTemplate1;

            //Assert
            //expecting an exception
        }

        #endregion


    }

  
}
