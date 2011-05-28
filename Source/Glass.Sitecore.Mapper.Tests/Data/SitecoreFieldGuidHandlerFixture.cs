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
    public class SitecoreFieldGuidHandlerFixture
    {
        SitecoreFieldGuidHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldGuidHandler();
        }

        #region GetFieldValue

        [Test]
        public void GetFieldValue_ValidString_ReturnsGuid()
        {
            //Assign
            string value = "{FC1D0AFD-71CC-47e2-84B3-7F1A2973248B}";

            //Act
            var result = _handler.GetFieldValue(value, null, null, null, null);

            //Assert
            Assert.AreEqual(new Guid("{FC1D0AFD-71CC-47e2-84B3-7F1A2973248B}"), result);
        }
        [Test]
        public void GetFieldValue_EmptyString_ReturnsEmptyGuid()
        {
            //Assign
            string value = "";

            //Act
            var result = _handler.GetFieldValue(value, null, null, null, null);

            //Assert
            Assert.AreEqual(Guid.Empty, result);
        }
        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetFieldValue_InvalidString_ThrowsException()
        {
            //Assign
            string value = "aefeagfea";

            //Act
            var result = _handler.GetFieldValue(value, null, null, null, null);

            //Assert
            //Exception thrown
        }

        #endregion

        #region SetFieldValue

        [Test]
        public void SetFieldValue_GuidReturnsString()
        {
            //Assign
            Guid guid = new Guid("{FC1D0AFD-71CC-47e2-84B3-7F1A2973248B}");
            SitecoreProperty property = new SitecoreProperty()
            {
                Property = new FakePropertyInfo(typeof(Guid))
            };
            //Act
            var result = _handler.SetFieldValue( guid, property, null);

            //Assert
            Assert.AreEqual("{FC1D0AFD-71CC-47e2-84B3-7F1A2973248B}".ToLower(), result);
        }

        #endregion
    }
}
