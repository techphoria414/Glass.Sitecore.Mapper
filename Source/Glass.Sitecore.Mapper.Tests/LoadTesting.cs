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
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Data;
using Sitecore.Data;
using System.Timers;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Tests.LoadTestingNS;
using System.Diagnostics;

namespace Glass.Sitecore.Mapper.Tests
{

    /*
     * This is a speed test to see the comparisons between direct Sitecore items and the Mapper
     * 
     */


    [TestFixture]
    public class LoadTesting
    {
        SitecoreService _sitecore;
        Context _context;
        Database _db;
        string itemPath = "/sitecore/content/Glass/Test1";

        [SetUp]
        public void Setup()
        {

            AttributeConfigurationLoader loader = new AttributeConfigurationLoader(
                new string[] { "Glass.Sitecore.Mapper.Tests.LoadTestingNS, Glass.Sitecore.Mapper.Tests" }
                );

            _context = new Context(loader, new ISitecoreDataHandler[] { new SitecoreIdDataHandler() });

            _sitecore = new SitecoreService("master");
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
        }


        [Test]
        public void ReadingAStringFromAField()
        {
            //cache the item first
            Item preCache = _db.GetItem(itemPath);

            int tests = 10;
            int loops = 1000;

            long rawAverage = 0;
            long classAverage = 0;

            for (int j = 0; j < tests; j++)
            {

                Stopwatch watch1 = new Stopwatch();
                watch1.Start();

                for (int i = 0; i < loops; i++)
                {
                    Item item = _db.GetItem(itemPath);
                    string value = item["SingleLineText"];
                }

                watch1.Stop();

                Stopwatch watch2 = new Stopwatch();
                watch2.Start();

                for (int i = 0; i < loops; i++)
                {
                    LoadClass item = _sitecore.GetItem<LoadClass>(itemPath);
                    string value = item.SingleLineText;

                }

                watch2.Stop();

                rawAverage += watch1.ElapsedMilliseconds;
                classAverage += watch2.ElapsedMilliseconds;

                Console.WriteLine("{2} Raw Sitecore Item: Number of loops {0} Total Time: {1}".Formatted(loops, watch1.ElapsedMilliseconds, j));
                Console.WriteLine("{2} Glass Mapper Item: Number of loops {0} Total Time: {1}".Formatted(loops, watch2.ElapsedMilliseconds, j));

            }

            Console.WriteLine("Raw Sitecore Item Average: {0}".Formatted(rawAverage / tests));
            Console.WriteLine("Glass Mapper Item Average: {0}".Formatted(classAverage / tests));
        }

    }

    namespace LoadTestingNS
    {
        [SitecoreClass]
        public class LoadClass
        {
            [SitecoreField]
            public virtual string SingleLineText { get; set; }
        }

    }
}
