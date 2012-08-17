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
using Glass.Sitecore.Mapper.Tests.Domain;

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
            Context context = new Context(
                new AttributeConfigurationLoader(
                    "Glass.Sitecore.Mapper.Tests.Data.SitecoreChildrenHandlerFixture,  Glass.Sitecore.Mapper.Tests",
                    "Glass.Sitecore.Mapper.Tests.Domain,  Glass.Sitecore.Mapper.Tests"));

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
        public void GetFieldValue_RemovesNullClasses()
        {
            //Assign
            string value = "{11111111-1111-1111-1111-111111111111}|{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}|{AC25A3FC-83E3-46E9-AEDA-79A1ECE3A22C}|";
            SitecoreProperty property = new SitecoreProperty()
            {
                Property = new FakePropertyInfo(typeof(IEnumerable<EmptyTemplate1>)),
                Attribute = new SitecoreFieldAttribute()
            };
            _handler.ConfigureDataHandler(property);

            var item = _service.Database.GetItem("/sitecore");

            //Act
            var result = _handler.GetFieldValue(value, item, _service);

            //Assert
            var list = result as IEnumerable<EmptyTemplate1>;
            Assert.AreEqual(2, list.Count());
            Assert.IsNotNull(list.First());
            Assert.IsNotNull(list.Last());

        }


        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetFieldValue_NoHandler_ThrowsException()
        {
            //Assign
            string value = "45|535|22|";
            SitecoreProperty property = new SitecoreProperty()
                       {
                           Property = new FakePropertyInfo(typeof(IEnumerable<TestType>)),
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
                    Property = new FakePropertyInfo(typeof(IEnumerable<int>)),
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
                Property = new FakePropertyInfo(typeof(IEnumerable<int>)),
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

        #region CLASSES

        public class TestType { }
        
        #endregion  

    }
   

}
