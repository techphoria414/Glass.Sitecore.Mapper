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
using Glass.Sitecore.Mapper.Tests.Domain;

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
            Context context = new Context(
                  new AttributeConfigurationLoader(
                      "Glass.Sitecore.Mapper.Tests.Domain,  Glass.Sitecore.Mapper.Tests"), null);

            _service = new SitecoreService(_db);

            _handler = new SitecoreQueryHandler();

            _item = _db.GetItem("/sitecore/content/Data/SitecoreQueryHandler");
        }

        #region GetValue
        [Test]
        public void GetValue_ReturnsResults_LazyLoad()
        {

            //Assign
            string query = "/sitecore/content/Data/SitecoreQueryHandler/*";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreQueryAttribute(query) { IsLazy = true },
                Property =  new FakePropertyInfo(typeof(IEnumerable<EmptyTemplate1>))
            };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.GetValue(_item, _service) as IEnumerable<EmptyTemplate1>;

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.AreNotEqual(typeof(EmptyTemplate1), result.First().GetType());
            Assert.IsTrue(result.First() is EmptyTemplate1);

        }

        [Test]
        public void GetValue_ReturnsResults_NotLazyLoad()
        {

            //Assign
            string query = "/sitecore/content/Data/SitecoreQueryHandler/*";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreQueryAttribute(query) { IsLazy = false },
                Property = new FakePropertyInfo(typeof(IEnumerable<EmptyTemplate1>))

            };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.GetValue(_item, _service) as IEnumerable<EmptyTemplate1>;

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(typeof(EmptyTemplate1), result.First().GetType());

        }

        [Test]
        public void GetValue_ReturnsSingleResult_LazyLoad()
        {

            //Assign
            string query = "/sitecore/content/Data/SitecoreQueryHandler/*";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreQueryAttribute(query) { IsLazy = true },
                Property = new FakePropertyInfo(typeof(EmptyTemplate1))
            };
            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue(_item, _service) as EmptyTemplate1;

            //Assert
            Assert.AreNotEqual(typeof(EmptyTemplate1), result.GetType());
            Assert.IsTrue(result is EmptyTemplate1);

        }

        [Test]
        public void GetValue_ReturnsSingleResult_NotLazyLoad()
        {

            //Assign
            string query = "/sitecore/content/Data/SitecoreQueryHandler/*";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreQueryAttribute(query) { IsLazy = false },
                Property = new FakePropertyInfo(typeof(EmptyTemplate1))

            };
            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.GetValue(_item, _service) as EmptyTemplate1;

            //Assert

            Assert.AreEqual(typeof(EmptyTemplate1), result.GetType());

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
            _handler.Property = new FakePropertyInfo(typeof(IEnumerable<EmptyTemplate1>));
            _handler.Query = "/sitecore/content/Data/SitecoreQueryHandler/*";


            //Act
            var results = _handler.GetValue(_item, _service) as IEnumerable<EmptyTemplate1>;

            //Assert
            Assert.AreEqual(3, results.Count());

            Assert.AreEqual(3, results.Where(x => x is EmptyTemplate1).Count());
            Assert.AreEqual(1, results.Where(x => x is EmptyTemplate2).Count());
        }
    }


}


