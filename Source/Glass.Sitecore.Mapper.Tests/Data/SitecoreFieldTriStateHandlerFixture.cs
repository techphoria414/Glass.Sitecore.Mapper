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
using Glass.Sitecore.Mapper.FieldTypes;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldTriStateHandlerFixture
    {
        SitecoreFieldTriStateHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldTriStateHandler();
        }

        #region GetFieldValue

        [Test]
        public void GetFieldValue_EmptyString_ReturnsDefault()
        {
            //Assign
            string value = "";

            //Act
            var result = _handler.GetFieldValue(value, null, null, null);

            //Assert
            Assert.AreEqual(TriState.Default, result);
        }
        [Test]
        public void GetFieldValue_StringWithOne_ReturnsYes()
        {
            //Assign
            string value = "1";

            //Act
            var result = _handler.GetFieldValue(value, null, null, null);

            //Assert
            Assert.AreEqual(TriState.Yes, result);
        }
        [Test]
        public void GetFieldValue_StringWithZero_ReturnsNo()
        {
            //Assign
            string value = "0";

            //Act
            var result = _handler.GetFieldValue(value, null, null,  null);

            //Assert
            Assert.AreEqual(TriState.No, result);
        }
        [Test]
        public void GetFieldValue_RandomString_ReturnsDefault()
        {
            //Assign
            string value = "adfgadga";

            //Act
            var result = _handler.GetFieldValue(value, null, null,  null);

            //Assert
            Assert.AreEqual(TriState.Default, result);
        }
        #endregion

        #region SetFieldValue
        [Test]
        public void SetFieldValue_StateDefault_ReturnsEmptyString()
        {
            //Assign
            TriState value = TriState.Default;
           
            //Act
            var result = _handler.SetFieldValue(value,  null);

            //Assert
            Assert.AreEqual("", result);
        }
        [Test]
        public void SetFieldValue_StateNo_ReturnsStringZero()
        {
            //Assign
            TriState value = TriState.No;
            

            //Act
            var result = _handler.SetFieldValue( value,  null);

            //Assert
            Assert.AreEqual("0", result);
        }
        [Test]
        public void SetFieldValue_StateYes_ReturnsStringOne()
        {
            //Assign
            TriState value = TriState.Yes;
           
            //Act
            var result = _handler.SetFieldValue( value,  null);

            //Assert
            Assert.AreEqual("1", result);
        }
        #endregion

    }
}
