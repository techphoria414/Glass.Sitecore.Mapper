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
using System.Globalization;
using System.Threading;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldNullableHandlerFixture
    {
        SitecoreFieldNullableHandler<int?, SitecoreFieldIntegerHandler> _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldNullableHandler<int?, SitecoreFieldIntegerHandler>();
        }

        #region GetFieldValue
        [Test]
        public void GetFieldValue_RetrievesValidInteger()
        {
            //Assign
            string value = "45";

            //Act
            var result = _handler.GetFieldValue(value,   null, null);

            //Assert
            Assert.AreEqual(45, result);
        }

        [Test]
        public void GetFieldValue_EmptyStringReturnsNull()
        {
            //Assign
            string value = "";

            //Act
            var result = _handler.GetFieldValue(value,   null, null);

            //Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetFieldValue_InvalidIntThrowsError()
        {
            //Assign
            string value = "4fff5";

            //Act
            var result = _handler.GetFieldValue(value,   null, null);

            //Assert
            //error should have  been thrown
        }

        [Test]
        public void GetFieldValue_ValidInt_ReturnsValue_RegardlessOfCulture([Values("en-GB", "en-US", "de-DE", "da-DK", "fr-FR")]string culture)
        {
            //Assign
            string sitecoreNumericFieldValueInInvariantCultureFormat = "10,000";
            CultureInfo systemCulture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);

            //Act
            var result = _handler.GetFieldValue(sitecoreNumericFieldValueInInvariantCultureFormat, null, null);

            //Assert
            try
            {
                Assert.That(result, Is.EqualTo(10000));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = systemCulture;
            }
        }
        #endregion
        #region SetFieldValue
        [Test]
        public void SetFieldValue_ReturnsString()
        {
            //Assign
            int value = 487;

            //Act
            var result = _handler.SetFieldValue(value, null);

            //Assert
            Assert.AreEqual("487", result);
        }

        [Test]
        public void SetFieldValue_ValueIsNull_ReturnsEmptyString()
        {
            //Assign
            int? value = null;

            //Act
            var result = _handler.SetFieldValue(value, null);

            //Assert
            Assert.AreEqual(string.Empty, result);
        }
        #endregion
    }
}
