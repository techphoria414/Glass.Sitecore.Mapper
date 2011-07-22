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
    public class SitecoreFieldIEnumerableHandlerFixture
    {
        SitecoreFieldIEnumerableHandler _handler;
        ISitecoreService  _service;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreFieldIEnumerableHandler();
            var context = new InstanceContext(
                new Dictionary<Type, SitecoreClassConfig>(),
                new AbstractSitecoreDataHandler[] { new SitecoreFieldIntegerHandler() });
            _service = new SitecoreService("master");
        }

        #region GetFieldValue
        [Test]
        public void GetFieldValue_ReturnsArrayOfIntegers()
        {
            //Assign
            string value = "45|535|22|";
            SitecoreProperty property = new SitecoreProperty()
                    {
                        Property = new FakePropertyInfo(typeof(IEnumerable<int>)),
                        Attribute = new SitecoreFieldAttribute()
                    };
            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetFieldValue(value, null,  _service);

            //Assert
            var list = result as IEnumerable<int>;
            Assert.AreEqual(3, list.Count());
            Assert.AreEqual(45, list.First());
            Assert.AreEqual(535, list.Take(2).Last());
            Assert.AreEqual(22, list.Last());

        }
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetFieldValue_NoHandler_ThrowsException()
        {
            //Assign
            string value = "45|535|22|";
            SitecoreProperty property = new SitecoreProperty()
                       {
                           Property = typeof(SitecoreFieldIEnumerableHandlerFixtureNS.TestClass).GetProperty("Test"),
                           Attribute = new SitecoreFieldAttribute()
                       };
            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetFieldValue(value, null, _service);

            //Assert
            //Exception thrown

        }

        #endregion
        #region SetFieldValue

        [Test]
        public void SetFieldValue_ReturnsPipeString()
        {
            //Assign
            IEnumerable<int> list = new int[] { 44, 535, 22 };

            SitecoreProperty property = new SitecoreProperty()
                {
                    Property = typeof(SitecoreFieldIEnumerableHandlerFixtureNS.TestClass).GetProperty("IntList"),
                    Attribute = new SitecoreFieldAttribute()
                };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.SetFieldValue(
                list,
                
                _service);

            //Assert
            Assert.AreEqual("44|535|22", result);
        }

        [Test]
        public void SetFieldValue_HandlesNull()
        {
            //Assign
            IEnumerable<int> list = null;

            SitecoreProperty property = new SitecoreProperty()
            {
                Property = typeof(SitecoreFieldIEnumerableHandlerFixtureNS.TestClass).GetProperty("IntList"),
                Attribute = new SitecoreFieldAttribute()
            };

            _handler.ConfigureDataHandler(property);
            //Act
            var result = _handler.SetFieldValue(
                list,

                _service);

            //Assert
            Assert.AreEqual("", result);
        }

        #endregion



    }
    namespace SitecoreFieldIEnumerableHandlerFixtureNS
    {
        public class TestClass
        {
            public IEnumerable<TestType> Test { get; set; }
            public IEnumerable<int> IntList { get; set; }
        }
        public class TestType { }
    }

}
