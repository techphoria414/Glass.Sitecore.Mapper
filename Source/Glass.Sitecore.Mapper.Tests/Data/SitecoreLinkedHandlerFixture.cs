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
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.SecurityModel;
using Glass.Sitecore.Mapper.Tests.Domain;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreLinkedHandlerFixture
    {
        SitecoreLinkedHandler _handler;
        Item _target;
        Database _database;
        ISitecoreService _service;

        [SetUp]
        public void Setup()
        {
            _handler = new SitecoreLinkedHandler();
            _database = global::Sitecore.Configuration.Factory.GetDatabase("master");
            _target = _database.GetItem("/sitecore/content/Data/SitecoreLinkedHandler/Root");

            Context context = new Context(
                   new AttributeConfigurationLoader(
                       "Glass.Sitecore.Mapper.Tests.Domain,  Glass.Sitecore.Mapper.Tests"), null);

            _service = new SitecoreService(_database);

        }

        #region WillHandle

        [Test]
        public void WillHandle()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty(){
                Attribute = new SitecoreLinkedAttribute(),
                Property = new FakePropertyInfo(typeof(IEnumerable<LinkTemplate>))
            };


            //Act
            var result =_handler.WillHandle(property, null, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion
        #region GetValue

        [Test]
        public void GetValue_ReturnsClasses_All()
        {
                //Assign

            SitecoreProperty linkedProperty = new SitecoreProperty()
            {
                Attribute = new SitecoreLinkedAttribute(),
                Property = new FakePropertyInfo(typeof(IEnumerable<LinkTemplate>))
            };
            _handler.ConfigureDataHandler(linkedProperty);

            using (new SecurityDisabler())
            {
             //Act
                var result = _handler.GetValue(_target, _service) as IEnumerable<LinkTemplate>;

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Count());
              
            }

        }

        [Test]
        public void GetValue_ReturnsClasses_Referrers()
        {
            //Assign

            SitecoreProperty linkedProperty = new SitecoreProperty()
            {
                Attribute = new SitecoreLinkedAttribute()
                {
                    Option = SitecoreLinkedOptions.Referrers
                },
                Property = new FakePropertyInfo(typeof(IEnumerable<LinkTemplate>))
            };
            _handler.ConfigureDataHandler(linkedProperty);

            using (new SecurityDisabler())
            {
                //Act
                var result = _handler.GetValue(_target, _service) as IEnumerable<LinkTemplate>;

                //Assert
                Assert.IsNotNull(result);
                //the following items point at it:
                // /sitecore/content/Data/SitecoreLinkedHandler/Root/Item1
                // /sitecore/content/Data/SitecoreLinkedHandler/Root/Item2
                Assert.AreEqual(2, result.Count());
               
            }

        }

        [Test]
        public void GetValue_ReturnsClasses_References()
        {
            //Assign

            SitecoreProperty linkedProperty = new SitecoreProperty()
            {
                Attribute = new SitecoreLinkedAttribute()
                {
                    Option = SitecoreLinkedOptions.References
                },
                Property = new FakePropertyInfo(typeof(IEnumerable<LinkTemplate>))
            };
            _handler.ConfigureDataHandler(linkedProperty);

            using (new SecurityDisabler())
            {
                //Act
                var result = _handler.GetValue(_target, _service) as IEnumerable<LinkTemplate>;

                //Assert
                Assert.IsNotNull(result);
                //it only references it's template
                Assert.AreEqual(1, result.Count());

            }

        }

        #endregion
        #region SetValue
        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void SetValue_ThrowsException()
        {
            //Assert
            var target = new object();

            //Act
            _handler.SetValue(_target, target, _service);

        }

        #endregion

        //TODO need to do test on infertype

    }
  
}
