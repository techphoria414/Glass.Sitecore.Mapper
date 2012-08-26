using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Data;
using Sitecore.Data;
using Glass.Sitecore.Mapper.ObjectCreation.Implementations;
using Glass.Sitecore.Mapper.Tests.Domain;
using Sitecore.SecurityModel;
using Glass.Sitecore.Mapper.ObjectCreation;
using System.Timers;
using System.Diagnostics;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper.Tests.ObjectCreation.Implementations
{
    [TestFixture]
    public class CacheObjectManagerFixture
    {

        SitecoreService _sitecore;
        Context _context;
        Database _db;
        CacheObjectManager _manager;

        [SetUp]
        public void Setup()
        {

            AttributeConfigurationLoader loader = new AttributeConfigurationLoader(
                new string[] { 
                    "Glass.Sitecore.Mapper.Tests.Domain, Glass.Sitecore.Mapper.Tests",                    
                    "Glass.Sitecore.Mapper.Tests.ObjectCreation.Implementations.CacheObjectManagerFixtureNS, Glass.Sitecore.Mapper.Tests" }
                );

            _context = new Context(loader);

            _sitecore = new SitecoreService("master");
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _manager = new CacheObjectManager();
        }

        #region Method - CreateClass

        [Test]
        public void CreateClass_ReturnsProxyClass_ValueMatchesSitecore()
        {
            //Assign
            string path = "/sitecore/content/CacheManager/CacheItem1";

            var item = _db.GetItem(path);
            Type type = typeof(SimpleTemplate); 

            //Act
            var result = _manager.CreateClass(_sitecore, false, false, type, item, null) as SimpleTemplate;


            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item["SingleLineText"], result.SingleLineText);
            Assert.AreEqual(item["RichText"], result.RichText);
        }

        [Test]
        public void CreateClass_ReturnsTwoProxyClass_AreOfDifferentReferences()
        {
            //Assign
            string path = "/sitecore/content/CacheManager/CacheItem1";

            var item = _db.GetItem(path);
            Type type = typeof(SimpleTemplate);

            //Act
            var result1 = _manager.CreateClass(_sitecore, false, false, type, item, null) as SimpleTemplate;
            var result2 = _manager.CreateClass(_sitecore, false, false, type, item, null) as SimpleTemplate;


            //Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual(item["SingleLineText"], result1.SingleLineText);
            Assert.AreEqual(item["RichText"], result1.RichText);

            Assert.IsNotNull(result2);
            Assert.AreEqual(item["SingleLineText"], result2.SingleLineText);
            Assert.AreEqual(item["RichText"], result2.RichText);

            Assert.AreNotEqual(result1, result2);
        }

        [Test]
        public void CreateClass_ReturnsTwoProxyClassWriteToFirst_DoesNotAffectSecondProxyObject()
        {
            //Assign
            string path = "/sitecore/content/CacheManager/CacheItem1";

            var item = _db.GetItem(path);
            Type type = typeof(SimpleTemplate);

            //Act
            var result1 = _manager.CreateClass(_sitecore, false, false, type, item, null) as SimpleTemplate;
            var result2 = _manager.CreateClass(_sitecore, false, false, type, item, null) as SimpleTemplate;


            //Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual(item["SingleLineText"], result1.SingleLineText);
            Assert.AreEqual(item["RichText"], result1.RichText);

            result1.SingleLineText = "Some random change";

            Assert.IsNotNull(result2);
            Assert.AreEqual(item["SingleLineText"], result2.SingleLineText);
            Assert.AreEqual(item["RichText"], result2.RichText);

            Assert.AreEqual("Some random change", result1.SingleLineText);
            
            Assert.AreNotEqual(result1, result2);
        }


        [Test]
        public void CreateClass_ReturnsProxyClassAndSaveValue_SavesValueBackToSitecore()
        {
            //Assign
            string path = "/sitecore/content/CacheManager/CacheItem1";

            var item = _db.GetItem(path);
            Type type = typeof(SimpleTemplate);

            //Act
            var result = _manager.CreateClass(_sitecore, false, false, type, item, null) as SimpleTemplate;

            Assert.IsNotNull(result);
            Assert.AreEqual(item["SingleLineText"], result.SingleLineText);
            Assert.AreEqual(item["RichText"], result.RichText);

            result.SingleLineText = "SomeRandomUpdate";

            //Assert
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                _sitecore.WriteToItem(result, item);

                Assert.AreEqual(item["SingleLineText"], "SomeRandomUpdate");

                item.Editing.CancelEdit();
            }
        }


        [Test]
        public void CreateClass_ReturnsProxyClass_ChildrenReturned()
        {
            //Assign
            string path = "/sitecore/content/CacheManager/CacheItem1";

            var item = _db.GetItem(path);
            Type type = typeof(SimpleTemplate);

            //Act
            var result = _manager.CreateClass(_sitecore, false, false, type, item, null) as SimpleTemplate;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item["SingleLineText"], result.SingleLineText);
            Assert.AreEqual(item["RichText"], result.RichText);

            Assert.AreEqual(item.Children.Count, result.Children.Count());
            Assert.AreEqual(item.Children.Count, result.ChildrenByQuery.Count());
        }

        [Test]
        public void CreateClass_ReturnsProxyClassGetChildren_ChildrenReturned()
        {
            //Assign
            string path = "/sitecore/content/CacheManager/CacheItem1";

            var item = _db.GetItem(path);
            Type type = typeof(SimpleTemplate);

            //Act
            var result = _manager.CreateClass(_sitecore, false, false, type, item, null) as SimpleTemplate;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item["SingleLineText"], result.SingleLineText);
            Assert.AreEqual(item["RichText"], result.RichText);

            Assert.AreEqual(item.Children.Count, result.Children.Count());
            Assert.AreEqual(item.Children.Count, result.ChildrenByQuery.Count());

            var child1 = result.Children.First();
            var child2 = result.ChildrenByQuery.First();

            Assert.AreEqual(child1.SingleLineText, child2.SingleLineText);

        }

        static Item testItem;
        [Test]
        public void CreateClass_ReturnsProxyClassGetChildren_ChildrenReturned()
        {
            //Assign
            string path = "/sitecore/content/CacheManager/CacheItem1";

            testItem = _db.GetItem(path);


            private void PublishItem(Sitecore.Data.Items.Item item)
{
&nbsp; // The publishOptions determine the source and target database,
&nbsp; // the publish mode and language, and the publish date
&nbsp; Sitecore.Publishing.PublishOptions publishOptions =
&nbsp;&nbsp;&nbsp; new Sitecore.Publishing.PublishOptions(item.Database,
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Database.GetDatabase("web"),
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Sitecore.Publishing.PublishMode.SingleItem,
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; item.Language,
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; System.DateTime.Now);&nbsp; // Create a publisher with the publishoptions
&nbsp; Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);

&nbsp; // Choose where to publish from
&nbsp; publisher.Options.RootItem = item;

&nbsp; // Publish children as well?
&nbsp; publisher.Options.Deep = true;

&nbsp; // Do the publish!
&nbsp; publisher.Publish();
}

            Assert.AreEqual(testItem["SingleLineText"], testItem["SingleLineText"]);

        }

        #endregion

        #region MISC - LoadTesting

      

        #endregion
    }

    namespace CacheObjectManagerFixtureNS
    {
        
    }
}
