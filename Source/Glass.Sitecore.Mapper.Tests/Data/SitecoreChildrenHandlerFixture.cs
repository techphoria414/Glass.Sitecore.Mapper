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
using Glass.Sitecore.Mapper.Tests.Domain;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreChildrenHandlerFixture
    {
        ISitecoreService _service;
        Database _db;
        string _itemPath = "";

        SitecoreChildrenHandler _handler;

        [SetUp]
        public void Setup()
        {
            Context context = new Context(
                new AttributeConfigurationLoader(
                    "Glass.Sitecore.Mapper.Tests.Data.SitecoreChildrenHandlerFixture,  Glass.Sitecore.Mapper.Tests",
                    "Glass.Sitecore.Mapper.Tests.Domain,  Glass.Sitecore.Mapper.Tests"), null);

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _service = new SitecoreService(_db);
            _itemPath = "/sitecore/content/Data/SitecoreChildrenHandler/Root";

            _handler = new SitecoreChildrenHandler();
        }

        #region GetValue

        [Test]
        public void GetValue_ReturnsChildren_UsingLazy()
        {
            //Assign
            Item item = _db.GetItem(_itemPath);

            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(ChildrenRoot).GetProperty("Children"));
            _handler.ConfigureDataHandler(property);
                
            //Act
            var result = _handler.GetValue(item, _service) as Enumerable<EmptyTemplate1>;
            ChildrenRoot assignTest = new ChildrenRoot();
            assignTest.Children = result;

            //Assert
            Assert.AreEqual(item.Children.Count, result.Count());
            Assert.AreEqual(result, assignTest.Children);
            //if classes are being loaded lazy they should be of the proxy type and not the concrete type
            Assert.AreNotEqual(typeof(EmptyTemplate1), result.First().GetType());
            //but the proxy inherits from the concrete
            Assert.IsTrue(result.First() is EmptyTemplate1);

        }

        [Test]
        public void GetValue_ReturnsChildren_NotLazy()
        {
            //Assign
            Item item = _db.GetItem(_itemPath);

            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(ChildrenRoot).GetProperty("Children"));
            property.Attribute.CastTo<SitecoreChildrenAttribute>().IsLazy = false;

            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue(item, _service) as IEnumerable<EmptyTemplate1>;
            ChildrenRoot assignTest = new ChildrenRoot();
            assignTest.Children = result;
            //Assert
            Assert.AreEqual(item.Children.Count, result.Count());
            Assert.AreEqual(result, assignTest.Children);

            //if not lazy loaded then the type loaded should be the concrete type
            Assert.AreEqual(typeof(EmptyTemplate1), result.First().GetType());
            Assert.IsTrue(result.First() is EmptyTemplate1);

        }

        [Test]
        public void InferringType_ReturnsClasses()
        {
            //Assign
            Item item = _db.GetItem(_itemPath);

            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(ChildrenRoot).GetProperty("Children"));
            property.Attribute.CastTo<SitecoreChildrenAttribute>().IsLazy = true;
            property.Attribute.CastTo<SitecoreChildrenAttribute>().InferType = true;

            _handler.ConfigureDataHandler(property);

            //Act
            var results = _handler.GetValue(item, _service) as IEnumerable<EmptyTemplate1>;

            //Assert
            Assert.AreEqual(item.Children.Count, results.Count());


            //two of the item should be of type EmptyTemplate2

            Assert.AreEqual(2, results.Count(x => x is EmptyTemplate2));





        }

        #endregion

        #region WillHandle

        [Test]
        public void WillHandle_ReturnsTrue()
        {
            //Assign
            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(ChildrenRoot).GetProperty("Children"));

            //Act
            var result = _handler.WillHandle(property, null, null);


            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void WillHandle_ReturnsFalse()
        {
            //Assign
            SitecoreProperty property = AttributeConfigurationLoader.GetProperty(typeof(ChildrenRoot).GetProperty("List"));


            //Act
            var result = _handler.WillHandle(property, null, null);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

       

        [SitecoreClass]
        public class ChildrenRoot : EmptyTemplate1
        {
            [SitecoreChildren]
            public virtual IEnumerable<EmptyTemplate1> Children { get; set; }
            [SitecoreQuery]
            public virtual IEnumerable<EmptyTemplate1> List { get; set; }
           
        }
    }

    }
