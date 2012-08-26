using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using Glass.Sitecore.Mapper.ObjectCreation.Implementations;
using Sitecore.Data;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Tests.Domain;
using Glass.Sitecore.Mapper.Configuration;
using Sitecore.Links;

namespace Glass.Sitecore.Mapper.Tests.ObjectCreation.Implementations
{
    [TestFixture]
    public  class CacheObjectManagerLoadTestingFixture
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
                    "Glass.Sitecore.Mapper.Tests.ObjectCreation.Implementations.CacheObjectManagerLoadTestingFixtureNS, Glass.Sitecore.Mapper.Tests" }
                );

            _context = new Context(loader);

            _sitecore = new SitecoreService("master");
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _manager = new CacheObjectManager();
        }

        [Test]
        public void LoadTest_CachedVsNonCached_1000()
        {


            Type type = typeof(CacheObjectManagerLoadTestingFixtureNS.Concrete);

            //Act

            LoadTestRunner(type, 1000);


        }

        [Test]
        public void LoadTest_CachedVsNonCached_10000()
        {


            Type type = typeof(CacheObjectManagerLoadTestingFixtureNS.Concrete);

            //Act

            LoadTestRunner(type, 10000);


        }

        [Test]
        public void LoadTest_CachedVsNonCachedWithInterfaceType_1000()
        {


            Type type = typeof(CacheObjectManagerLoadTestingFixtureNS.ProxyCheck);

            //Act

            LoadTestRunner(type, 1000);


        }

        [Test]
        public void LoadTest_CachedVsNonCachedWithInterfaceType_10000()
        {


            Type type = typeof(CacheObjectManagerLoadTestingFixtureNS.ProxyCheck);

            //Act

            LoadTestRunner(type, 10000);


        }

        private void LoadTestRunner(Type type, int requests, int iterations = 10)
        {
            //Assign
            string path = "/sitecore/content/CacheManager/CacheItem1";

            var cacheManger = new CacheObjectManager();
            var standardManager = new ClassManager();

            var item = _db.GetItem(path);


            long cacheTotal = 0;
            long standardTotal = 0;
            long rawTotal = 0;
            long cacheSeconds = 0;
            long standardSeconds = 0;
            long rawSeconds = 0;

          


            for (int j = 0; j < iterations; j++)
            {
                Stopwatch timerCached = new Stopwatch();
                timerCached.Start();

                for (int i = 0; i < requests; i++)
                {
                    var result = cacheManger.CreateClass(new ClassLoadingState(_sitecore, false, false, type, item, Guid.Empty, null)) as CacheObjectManagerLoadTestingFixtureNS.Common;
                        var id = result.Id;
                        var title = result.Title;
                        var display = result.DisplayName;
                        var url = result.Url;
                }

                timerCached.Stop();
                cacheTotal += timerCached.ElapsedTicks;
                cacheSeconds += timerCached.ElapsedMilliseconds;

                Stopwatch timerStandard = new Stopwatch();
                timerStandard.Start();

                for (int i = 0; i < requests; i++)
                {
                    var result = standardManager.CreateClass(new ClassLoadingState(_sitecore, false, false, type, item, Guid.Empty, null)) as CacheObjectManagerLoadTestingFixtureNS.Common;
                    var id = result.Id;
                    var title = result.Title;
                    var display = result.DisplayName;
                    var url = result.Url;
                }

                timerStandard.Stop();
                standardTotal += timerStandard.ElapsedTicks;
                standardSeconds += timerStandard.ElapsedMilliseconds;

                Stopwatch timerRaw = new Stopwatch();
                timerRaw.Start();

                for (int i = 0; i < requests; i++)
                {
                    var result = _db.GetItem(item.ID);
                    var id = result.ID.Guid;
                    var title = result["Title"];
                    var display = result.DisplayName;
                    var url = LinkManager.GetItemUrl(item) ;
                }

                timerRaw.Stop();
                rawTotal += timerRaw.ElapsedTicks;
                rawSeconds += timerRaw.ElapsedMilliseconds;
            
            }



            Console.WriteLine("Load Test - Requests: {0} Iterations {1} ".Formatted(requests, iterations));
            Console.WriteLine("Cached Time {0}, Standard Time {1} Raw {2}".Formatted(cacheTotal / iterations, standardTotal / iterations, rawTotal /iterations));
            Console.WriteLine("Per Class: Cached Time {0}, Standard Time {1}, Raw Time {2}".Formatted(cacheTotal / requests / iterations, standardTotal / requests / iterations, rawTotal/requests/iterations));
            Console.WriteLine("Per Class: Cached Seconds {0},  Standard Seconds {1},  Raw Time {2} for {3}".Formatted(cacheSeconds, standardSeconds, rawSeconds, requests * iterations));
            


            Assert.IsTrue(cacheTotal < standardTotal);
        }
    }

    namespace CacheObjectManagerLoadTestingFixtureNS
    {
        [SitecoreClass]
        public interface ProxyCheck : Common
        {
            [SitecoreId]
            Guid Id { get; set; }

            [SitecoreField]
             string Title { get; set; }

            [SitecoreInfo(SitecoreInfoType.DisplayName)]
             string DisplayName{get;set;}

            [SitecoreInfo(SitecoreInfoType.Url)]
            string Url { get; set; }
        }

        [SitecoreClass]
        public class Concrete : Common
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }


            [SitecoreField]
            public virtual string Title { get; set; }

            [SitecoreInfo(SitecoreInfoType.DisplayName)]
            public virtual string DisplayName { get; set; }

            [SitecoreInfo(SitecoreInfoType.Url)]
            public virtual string Url { get; set; }
        }
        public interface Common
        {
            Guid Id { get; set; }
            string Title { get; set; }

            string DisplayName { get; set; }

            string Url { get; set; }
        }
    }
}
