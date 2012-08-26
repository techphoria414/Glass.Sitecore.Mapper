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
            var result = _manager.CreateClass(new ClassLoadingState(_sitecore, false, false, type, item, Guid.Empty, null)) as SimpleTemplate;


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
            var result1 = _manager.CreateClass(new ClassLoadingState(_sitecore, false, false, type, item,Guid.Empty, null)) as SimpleTemplate;
            var result2 = _manager.CreateClass(new ClassLoadingState(_sitecore, false, false, type, item, Guid.Empty, null)) as SimpleTemplate;


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
            var result1 = _manager.CreateClass(new ClassLoadingState(_sitecore, false, false, type, item, Guid.Empty, null)) as SimpleTemplate;
            var result2 = _manager.CreateClass(new ClassLoadingState(_sitecore, false, false, type, item, Guid.Empty, null)) as SimpleTemplate;


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
            var result = _manager.CreateClass(new ClassLoadingState(_sitecore, false, false, type, item, Guid.Empty, null)) as SimpleTemplate;

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
            var result = _manager.CreateClass(new ClassLoadingState(_sitecore, false, false, type, item, Guid.Empty, null)) as SimpleTemplate;

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
            var result = _manager.CreateClass(new ClassLoadingState(_sitecore, false, false, type, item, Guid.Empty, null)) as SimpleTemplate;

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

      
        
        #endregion

        #region MISC - LoadTesting

      

        #endregion
    }

    namespace CacheObjectManagerFixtureNS
    {
        
    }
}
