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
    public class SitecoreIdDataHandlerFixture
    {
        SitecoreIdDataHandler _handler;
        Guid _itemId;
        Database _db;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreIdDataHandler();
            _itemId = new Guid("{8A317CBA-81D4-4F9E-9953-64C4084AECCA}");
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
        }

        #region WillHandle

        [Test]
        public void WillHandle_ReturnsTrue()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreIdAttribute(),
                 Property = new FakePropertyInfo(typeof(Guid))
            };

            //Act
            var result = _handler.WillHandle(property, null, null);

            //Assert
            Assert.IsTrue(result);

        }
        [Test]
        public void WillHandle_ReturnsFalse_WrongAttribute()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Guid))
            };

            //Act
            var result = _handler.WillHandle(property, null, null);

            //Assert
            Assert.IsFalse(result);

        }

        [Test]
        public void WillHandle_ReturnsFalse_WrongType()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreIdAttribute(),
                Property = new FakePropertyInfo(typeof(String))
            };

            //Act
            var result = _handler.WillHandle(property, null, null);

            //Assert
            Assert.IsFalse(result);

        }
        #endregion

        #region GetValue

        [Test]
        public void GetValue_ReturnsItemId()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            //Act
            var result = _handler.GetValue(null, item, null, null);

            //Assert
            Assert.AreEqual(_itemId, result);
        }

        #endregion
        #region SetValue

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_ThrowsException()
        {
            //Act
            _handler.SetValue(null, null, _itemId, null, null);
        }

        #endregion
    }
}
