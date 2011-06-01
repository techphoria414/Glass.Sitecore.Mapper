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

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldBooleanHandlerFixture
    {

        SitecoreFieldBooleanHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldBooleanHandler();
        }

        #region GetFieldValue

        [Test]
        public void GetFieldValue_FieldWith1_ReturnsTrue()
        {
            //Assign
            string fieldValue = "1";

            //Act
            var result = _handler.GetFieldValue(fieldValue,   null, null);

            //Assert
            Assert.IsTrue((bool)result);
        }

        [Test]
        public void GetFieldValue_FieldIsEmpty_ReturnsFalse()
        {
            //Assign
            string fieldValue = string.Empty;

            //Act
            var result = _handler.GetFieldValue(fieldValue,  null,  null);

            //Assert
            Assert.IsFalse((bool)result);
        }
        [Test]
        public void GetFieldValue_FieldWith0_ReturnsFalse()
        {
            //Assign
            string fieldValue = "0";

            //Act
            var result = _handler.GetFieldValue(fieldValue, null, null);
            //Assert
            Assert.IsFalse((bool)result);
        }

        #endregion
        #region SetFieldValue

        [Test]
        public void SetFieldValue_TrueValue_Returns1()
        {
            //Assign
            bool value = true;
            SitecoreProperty property = new SitecoreProperty()
            {
                Property = new FakePropertyInfo(typeof(Boolean))
            };

            //Act
            var result = _handler.SetFieldValue(value,  null);

            //Assert
            Assert.AreEqual("1", result);
        }
        [Test]
        public void SetFieldValue_TrueValue_Returns0()
        {
            //Assign
            bool value = false;
            SitecoreProperty property = new SitecoreProperty()
            {
                Property = new FakePropertyInfo(typeof(Boolean))
            };

            //Act
            var result = _handler.SetFieldValue( value,  null);

            //Assert
            Assert.AreEqual("0", result);
        }

        #endregion
    }
}
