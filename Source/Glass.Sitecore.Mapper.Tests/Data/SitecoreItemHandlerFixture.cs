using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Data;
using NUnit.Framework;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreItemHandlerFixture
    {
        SitecoreItemHandler _handler;
        ISitecoreService _service;
        
        [SetUp]
        public void Setup()
        {

            _handler = new SitecoreItemHandler();
            
            var db = global::Sitecore.Configuration.Factory.GetDatabase("master");


            var testTypeIdProperty = new SitecoreProperty()
            {
                Attribute = new SitecoreIdAttribute(),
                Property = typeof(SitecoreItemHandlerFixtureNS.TestClass).GetProperty("Id")
            };

            var context = new InstanceContext(
                 (new SitecoreClassConfig[]{
                     new SitecoreClassConfig(){
                       ClassAttribute = new SitecoreClassAttribute(),
                       Properties = new SitecoreProperty[]{
                            testTypeIdProperty
                       },
                       IdProperty= testTypeIdProperty,
                       Type = typeof(SitecoreItemHandlerFixtureNS.TestClass),
                       DataHandlers = new AbstractSitecoreDataHandler []{
                        new SitecoreIdDataHandler(){
                               Property = typeof(SitecoreItemHandlerFixtureNS.TestClass).GetProperty("Id")
                        },
                        new SitecoreInfoHandler(){
                               Property = typeof(SitecoreItemHandlerFixtureNS.TestClass).GetProperty("Path"),
                               InfoType = SitecoreInfoType.Path
                        },
                       }
                   }
                 }).ToDictionary(),
                new AbstractSitecoreDataHandler[] { });

            _service = new SitecoreService(db, context);

        }

        [Test]
        public void ConfigureDataHandler_CorrectlyConfiguresWithID()
        {
            //Assign
            SitecoreItemAttribute attr = new SitecoreItemAttribute();
            attr.Id = "{51C00CB9-E82F-4445-8B3A-F2E9A29B2876}";
            attr.IsLazy = true;
            SitecoreProperty prop = new SitecoreProperty();
            prop.Attribute = attr;

            //Act
            _handler.ConfigureDataHandler(prop );

            //Assert
            Assert.AreEqual(new Guid(attr.Id), _handler.Id);
            Assert.AreEqual(attr.IsLazy, _handler.IsLazy);
            Assert.AreEqual(prop, _handler.Property);
            Assert.IsTrue(_handler.Path.IsNullOrEmpty());

        }

        [Test]
        public void ConfigureDataHandler_CorrectlyConfiguresWithPath()
        {
            //Assign
            SitecoreItemAttribute attr = new SitecoreItemAttribute();
            attr.Path = "/stecore/content";
            attr.IsLazy = true;
            SitecoreProperty prop = new SitecoreProperty();
            prop.Attribute = attr;

            //Act
            _handler.ConfigureDataHandler(prop);

            //Assert
            Assert.AreEqual(Guid.Empty, _handler.Id);
            Assert.AreEqual(attr.IsLazy, _handler.IsLazy);
            Assert.AreEqual(prop, _handler.Property);
            Assert.AreEqual(attr.Path, _handler.Path);

        }

        [Test]
        public void GetValue_GetsClassUsingId()
        {
             //Assign
            SitecoreItemAttribute attr = new SitecoreItemAttribute();
            attr.Id = "{1E7A2641-E27E-4346-ACDE-839480927CF6}";
            attr.IsLazy = true;
            SitecoreProperty prop = new SitecoreProperty();
            prop.Attribute = attr;
            prop.Property = new FakePropertyInfo(typeof(SitecoreItemHandlerFixtureNS.TestClass));
            _handler.ConfigureDataHandler(prop);

            //Act
            SitecoreItemHandlerFixtureNS.TestClass result = _handler.GetValue(null, _service) as SitecoreItemHandlerFixtureNS.TestClass;


            //Assert
            Assert.AreEqual(new Guid(attr.Id), result.Id);
        }

        [Test]
        public void GetValue_GetsClassUsingPath()
        {
            //Assign
            SitecoreItemAttribute attr = new SitecoreItemAttribute();
            attr.Path = "/sitecore/content/Glass/SitecoreItemHandler";
            attr.IsLazy = true;
            SitecoreProperty prop = new SitecoreProperty();
            prop.Attribute = attr;
            prop.Property = new FakePropertyInfo(typeof(SitecoreItemHandlerFixtureNS.TestClass));


            _handler.ConfigureDataHandler(prop);

            //Act
            SitecoreItemHandlerFixtureNS.TestClass result = _handler.GetValue(null, _service) as SitecoreItemHandlerFixtureNS.TestClass;


            //Assert
            Assert.AreEqual(attr.Path, result.Path);
        }


    

    }

    namespace SitecoreItemHandlerFixtureNS
    {
        [SitecoreClass]
        public class TestClass
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreInfo(SitecoreInfoType.Path)]
            public virtual string Path { get; set; }
        }
    
    }
}
