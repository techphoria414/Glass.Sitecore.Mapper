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
    public class SitecoreFieldDateTimeHandlerFixture
    {
        SitecoreFieldDateTimeHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldDateTimeHandler();
        }

        #region GetFieldValue

        [Test]
        public void GetFieldValue_ValidIsoDate_ReturnsCorrectDateTime()
        {
            //Assign
            string date = "20100908T060504";

            //Act
            var result = (DateTime)_handler.GetFieldValue(date,   null, null);

            //Assert
            Assert.AreEqual(2010, result.Year);
            Assert.AreEqual(09, result.Month);
            Assert.AreEqual(08, result.Day);
            Assert.AreEqual(06, result.Hour);
            Assert.AreEqual(05, result.Minute);
            Assert.AreEqual(04, result.Second);
        }

        [Test]
        public void GetFieldValue_EmptyIsoDate_ReturnsDateTimeMin()
        {
            //Assign
            string date = "";

            //Act
            var result = (DateTime)_handler.GetFieldValue(date,   null, null);

            //Assert
            Assert.AreEqual(DateTime.MinValue, result);
        }

        #endregion

        #region SetFieldValue

        [Test]
        public void SetFieldValue_DateTime_CorrectlySets()
        {
            //Assign
            DateTime date = new DateTime(2010, 09, 08, 06, 05, 04);
         

            //Act
            var result = _handler.SetFieldValue( date,  null);

            //Assert
            Assert.AreEqual("20100908T060504", result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]            
        public void SetFieldValue_NonDateTime_ThrowsException()
        {
            //Assign
            object date = null;
            

            //Act
            var result = _handler.SetFieldValue(date, null);

          
        }


        #endregion

    }
}
