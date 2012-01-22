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

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldEnumHandlerFixture
    {
        SitecoreFieldEnumHandler _handler;
        SitecoreProperty _property;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldEnumHandler();
            _property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = typeof(TestClass).GetProperty("Enum")
            };
        }

        #region WillHandle

        [Test]
        public void WillHandler_ReturnsTrue()
        {
            //Assign

            //Act

            var result = _handler.WillHandle(_property, null, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region GetFieldValue

        [Test]
        public void GetFieldValue_ConvertsToEnum()
        {
            //Assign
            string fieldValue = "Oranges";
            _handler.ConfigureDataHandler(_property);

            //Act
            var result = (TestEnum)_handler.GetFieldValue(fieldValue,  null, null);

            //Assert
            Assert.AreEqual(TestEnum.Oranges, result);

        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetFieldValue_ValueNoInEnum_ThrowsException()
        {
            //Assign
            string fieldValue = "RandomFruit";
            _handler.ConfigureDataHandler(_property);

            //Act
            var result = (TestEnum)_handler.GetFieldValue(fieldValue,  null, null);

            //Assert


        }
        #endregion

        #region SetFieldValue

        [Test]
        public void SetFieldValue_ReturnsEnumValue()
        {
            //Assign
            TestEnum value = TestEnum.Berry;
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(TestEnum))
            };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.SetFieldValue(value, null);

            //Assert
            Assert.AreEqual("Berry", result);
        }


        #endregion

        #region CLASSES

        public enum TestEnum
        {
            Oranges,
            Apples,
            Berry
        }
        public class TestClass
        {
            public TestEnum Enum { get; set; }
        }

        #endregion
    }
   
}
