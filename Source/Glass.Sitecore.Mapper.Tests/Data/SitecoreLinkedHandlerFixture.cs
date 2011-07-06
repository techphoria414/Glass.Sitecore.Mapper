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
            _target = _database.GetItem("/sitecore/content/Glass/ItemLinksTest");

            SitecoreProperty idProperty =  new SitecoreProperty(){
                            Attribute = new SitecoreIdAttribute(),
                            Property = typeof(SitecoreLinkedHandlerFixtureNS.LinkedTestClass).GetProperty("Id")
                        };
            SitecoreIdDataHandler idHandler = new SitecoreIdDataHandler();
            idHandler.ConfigureDataHandler(idProperty);
                 

            var context = new InstanceContext(
              (new SitecoreClassConfig[]{
                   new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(),
                       Properties = new SitecoreProperty[]{
                           idProperty                       
                       },
                       Type = typeof(SitecoreLinkedHandlerFixtureNS.LinkedTestClass),
                       DataHandlers = new AbstractSitecoreDataHandler []{
                            idHandler
                       }
                   }
               }).ToDictionary(), new AbstractSitecoreDataHandler[] { });


            SitecoreProperty linkedProperty = new SitecoreProperty()
            {
                Attribute = new SitecoreLinkedAttribute(),
                Property = typeof(SitecoreLinkedHandlerFixtureNS.LinkedTestClass).GetProperty("Linked")
            };
            _handler.ConfigureDataHandler(linkedProperty);

            _service = new SitecoreService(_database, context);

        }

        #region WillHandle

        [Test]
        public void WillHandle()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty(){
                Attribute = new SitecoreLinkedAttribute(),
                Property = new FakePropertyInfo(typeof(IEnumerable<SitecoreLinkedHandlerFixtureNS.LinkedTestClass>))
            };


            //Act
            var result =_handler.WillHandle(property, null, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion
        #region GetValue

        [Test]
        public void GetValue_ReturnsClasses()
        {
                //Assert

            using (new SecurityDisabler())
            {
             //Act
                var result = _handler.GetValue(_target, _service) as IEnumerable<SitecoreLinkedHandlerFixtureNS.LinkedTestClass>;

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(_target.Links.GetAllLinks().Count(), result.Count());
                Assert.AreEqual(_target.Links.GetAllLinks().First().TargetItemID.Guid, result.First().Id);
                Assert.AreEqual(_target.Links.GetAllLinks().Last().TargetItemID.Guid, result.Last().Id);
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

    }
    namespace SitecoreLinkedHandlerFixtureNS
    {
        [SitecoreClass]
        public class LinkedTestClass
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreLinked]
            public virtual IEnumerable<LinkedTestClass> Linked { get; set; }
        }
    }
}
