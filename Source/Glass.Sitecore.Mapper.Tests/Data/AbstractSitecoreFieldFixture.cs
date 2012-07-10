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
    public class AbstractSitecoreFieldFixture
    {
        AbstractSitecoreFieldFixtureNS.TestClass testClass;

        [SetUp]
        public void Setup()
        {
            testClass = new Glass.Sitecore.Mapper.Tests.Data.AbstractSitecoreFieldFixtureNS.TestClass();

        }

        [Test]
        public void CanSetValue_FieldIsWritable_ReturnsTrue()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty(){
                Property = new FakePropertyInfo(typeof(string)),
                 Attribute = new SitecoreFieldAttribute(){
                        
                }
            };
            testClass.ConfigureDataHandler(property);
            //Act
            var result = testClass.CanSetValue;
               
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanSetValue_FieldIsReadOnly_ReturnsFalse()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Property = new FakePropertyInfo(typeof(string)),
                Attribute = new SitecoreFieldAttribute()
                {
                    ReadOnly = true
                }
            };

            testClass.ConfigureDataHandler(property);


            //Act
            var result = testClass.CanSetValue;

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void SetValue_FieldDoesntExist()
        {
            //Assign
            


            //Act

            //Assert
        }
    }

    namespace AbstractSitecoreFieldFixtureNS
    {
        public class TestClass : AbstractSitecoreField
        {
            public object SetValue { get; set; }

            public override object GetFieldValue(string fieldValue, global::Sitecore.Data.Items.Item item , ISitecoreService service)
            {
                return SetValue;
            }

            public override string SetFieldValue(object value, ISitecoreService service)
            {
                throw new NotImplementedException();
            }

            public override Type TypeHandled
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
