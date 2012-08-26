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
using Glass.Sitecore.Mapper.ObjectCaching.Implementations;
using Glass.Sitecore.Mapper.ObjectCaching;

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
        public void CreateClass_UpdateItem_HasTwoCacheEntries_FromHttpRuntimeCacheImplementation()
        {
            //Assign
            string path = "/sitecore/content/CacheManager/CacheItem1";

            var item = _db.GetItem(path);

            //Act
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                item.Fields["SingleLineText"].Value = "SomeRandomUpdate";

                item.Editing.EndEdit();
            }

            Type type = typeof(SimpleTemplate);

            
            var result = _manager.CreateClass(_sitecore, false, false, type, item, null) as SimpleTemplate;

            var revisionID1 = item.Fields["__revision"].Value;

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                item.Fields["SingleLineText"].Value = "Text 1";

                item.Editing.EndEdit();
            }
            
            item = _db.GetItem(path);
            result = _manager.CreateClass(_sitecore, false, false, type, item, null) as SimpleTemplate;

            var revisionID2 = item.Fields["__revision"].Value;

            var revision1Key = ObjectCaching.ObjectCache.GetItemDefaultKey(revisionID1);
            var revision2Key = ObjectCaching.ObjectCache.GetItemDefaultKey(revisionID2);

            var httpRuntimeCache = new HttpRuntimeCache();
            var object1 = httpRuntimeCache.GetObjectFromCache(revision1Key) as ICacheableObject;
            var object2 = httpRuntimeCache.GetObjectFromCache(revision2Key) as ICacheableObject;

            var simepleType1 = object1.CachedObject as SimpleTemplate;
            var simepleType2 = object2.CachedObject as SimpleTemplate;

            //Assert
            Assert.AreEqual(simepleType1.SingleLineText, "SomeRandomUpdate");
            Assert.AreEqual(simepleType2.SingleLineText, "Text 1");
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

        #endregion

        #region MISC - LoadTesting

        [Test]
        public void LoadTest_CachedVsNonCached()
        {

            //Assign
            string path = "/sitecore/content/CacheManager/CacheItem1";

            var cacheManger = new CacheObjectManager();
            var standardManager = new ClassManager();
            Type type = typeof(SimpleTemplate);
            var item = _db.GetItem(path);

            //Act

            long cacheTotal = 0;
            long standardTotal = 0;
            long cacheSeconds =0; 
            long standardSeconds = 0;

            for (int j = 0; j < 10; j++)
            {
                Stopwatch timerCached = new Stopwatch();
                timerCached.Start();

                for (int i = 0; i < 1000; i++)
                {
                    var result = cacheManger.CreateClass(_sitecore, false, false, type, item, null);
                }

                timerCached.Stop();
                cacheTotal += timerCached.ElapsedTicks;
                cacheSeconds += timerCached.ElapsedMilliseconds;

                Stopwatch timerStandard = new Stopwatch();
                timerStandard.Start();

                for (int i = 0; i < 1000; i++)
                {
                    var result = standardManager.CreateClass(_sitecore, false, false, type, item, null);
                }

                timerStandard.Stop();
                standardTotal += timerStandard.ElapsedTicks;
                standardSeconds += timerStandard.ElapsedMilliseconds;
            }


            Console.WriteLine("Cached Time {0}, Standard Time {1}".Formatted(cacheTotal/10, standardTotal/10));
            Console.WriteLine("Per Class: Cached Time {0}, Standard Time {1}".Formatted(cacheTotal / 10000, standardTotal / 10000));
            Console.WriteLine("Per Class Seconds: Cached Time {0}, Standard Time {1}".Formatted(cacheSeconds / 10000, standardSeconds / 10000));


            Assert.IsTrue(cacheTotal < standardTotal);
           

        }

        #endregion
    }
}
