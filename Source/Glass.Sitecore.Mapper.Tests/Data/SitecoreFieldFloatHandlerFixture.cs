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

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldFloatHandlerFixture
    {
        SitecoreFieldFloatHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldFloatHandler();
        }

        #region GetFieldValue

        [Test]
        public void GetFieldValue_ValidFloat_ReturnsValue()
        {
            //Assign
            string value = "10.11";

            //Act
            var result = _handler.GetFieldValue(value, null, null, null, null);

            //Assert
            Assert.AreEqual(result, 10.11f);

        }

        [Test]
        public void GetFieldValue_EmptyField_ReturnsZero()
        {
            //Assign
            string value = "";

            //Act
            var result = _handler.GetFieldValue(value, null, null, null, null);

            //Assert
            Assert.AreEqual(result, 0.00f);

        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetFieldValue_InvalidFloat_ThrowsException()
        {
            //Assign
            string value = "help";

            //Act
            var result = _handler.GetFieldValue(value, null, null, null, null);

            //Assert
            //exception should be shown
        }

        #endregion

        #region SetFieldValue

        [Test]
        public void SetFieldValue_Float_WritesCorrectly()
        {
            //Assign
            float value = 10.11f;

            //Act
            var result = _handler.SetFieldValue(typeof(float), value, null);

            //Assert
            Assert.AreEqual("10.11", result);

        }

        #endregion
    }
}
