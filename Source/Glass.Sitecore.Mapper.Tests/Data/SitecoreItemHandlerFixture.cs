using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Data;
using NUnit.Framework;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Tests.Domain;

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


            Context context = new Context(
                new AttributeConfigurationLoader(                   
                    "Glass.Sitecore.Mapper.Tests.Domain,  Glass.Sitecore.Mapper.Tests"));

            _service = new SitecoreService(db);

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
            attr.Id = "{66E62701-3FF2-492D-81A4-BD3E55428837}";
            attr.IsLazy = true;
            SitecoreProperty prop = new SitecoreProperty();
            prop.Attribute = attr;
            prop.Property = new FakePropertyInfo(typeof(EmptyTemplate1));
            _handler.ConfigureDataHandler(prop);

            //Act
            EmptyTemplate1 result = _handler.GetValue(null, _service) as EmptyTemplate1;


            //Assert
            Assert.AreEqual(new Guid(attr.Id), result.Id);
        }

        [Test]
        public void GetValue_GetsClassUsingPath()
        {
            //Assign
            SitecoreItemAttribute attr = new SitecoreItemAttribute();
            attr.Path = "/sitecore/content/Data/SitecoreItemHandler/Item1";
            attr.IsLazy = true;
            SitecoreProperty prop = new SitecoreProperty();
            prop.Attribute = attr;
            prop.Property = new FakePropertyInfo(typeof(EmptyTemplate1));


            _handler.ConfigureDataHandler(prop);

            //Act
            EmptyTemplate1 result = _handler.GetValue(null, _service) as EmptyTemplate1;


            //Assert
            Assert.AreEqual(attr.Path, result.Path);
        }


    

    }

   
}
