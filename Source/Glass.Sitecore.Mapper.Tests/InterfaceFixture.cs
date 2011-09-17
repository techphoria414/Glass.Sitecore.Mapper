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
using Glass.Sitecore.Mapper.Tests.InterfaceFixtureNS;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.SecurityModel;

namespace Glass.Sitecore.Mapper.Tests
{
    /// <summary>
    /// This fixture test the frameworks ability to handle interfaces.
    /// </summary>
    [TestFixture]
    public class InterfaceFixture
    {
        Context _context;
        SitecoreService _sitecore;
        Database _db;

        Item _test1;
        Item _test2;
        Item _test3;

        [SetUp]
        public void Setup()
        {
            _context = new Context(
                new AttributeConfigurationLoader(
                    "Glass.Sitecore.Mapper.Tests.InterfaceFixtureNS, Glass.Sitecore.Mapper.Tests"), null);

            global::Sitecore.Context.Site = global::Sitecore.Configuration.Factory.GetSite("website");

            _sitecore = new SitecoreService("master");

            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _test1 = _db.GetItem("/sitecore/content/Glass/Test1");
            _test2 = _db.GetItem("/sitecore/content/Glass/Test2");
            _test3 = _db.GetItem("/sitecore/content/Glass/Test1/Test3");
        }

        [Test]
        public void GetItem_ReturnsProxyClassOfInterface()
        {
            //Assign
            TestInterface inter = null;

            //Act 
            inter = _sitecore.GetItem<TestInterface>("/sitecore/content/Glass/Test1");

            //Assert
            Assert.IsNotNull(inter);
            Assert.AreEqual(_test1.ID.Guid, inter.Id);
        }

        [Test]
        public void Interface_DropLinkField()
        {
            //Assign
            TestInterface inter = null;

            //Act 
            inter = _sitecore.GetItem<TestInterface>("/sitecore/content/Glass/Test1");

            //Assert
            Assert.IsNotNull(inter);

            Assert.AreEqual(_test1.ID.Guid, inter.DropLink.Id);
        }
        [Test]
        public void Interface_MultiList()
        {
            //Assign
            TestInterface inter = null;

            //Act 
            inter = _sitecore.GetItem<TestInterface>("/sitecore/content/Glass/Test1");

            //Assert
            Assert.IsNotNull(inter);

            Assert.AreEqual(_test1.ID.Guid, inter.MultiList.First().Id);
        }

        [Test]
        public void Interface_Children()
        {
            //Assign
            TestInterface inter = null;

            //Act 
            inter = _sitecore.GetItem<TestInterface>("/sitecore/content/Glass/Test1");

            //Assert
            Assert.IsNotNull(inter);

            Assert.AreEqual(_test1.Children.Count, inter.Children.Count());
            Assert.AreEqual(_test3.ID.Guid, inter.Children.First().Id);

        }

        [Test]
        public void Interface_Parent()
        {
            //Assign
            TestInterface inter = null;

            //Act 
            inter = _sitecore.GetItem<TestInterface>("/sitecore/content/Glass/Test1");

            //Assert
            Assert.IsNotNull(inter);

            Assert.AreEqual(_test1.ParentID.Guid, inter.Parent.Id);
        }

        [Test]
        public void Interface_Query()
        {
            //Assign
            TestInterface inter = null;

            //Act 
            inter = _sitecore.GetItem<TestInterface>("/sitecore/content/Glass/Test1");

            //Assert
            Assert.IsNotNull(inter);

            using (new SecurityDisabler())
            {
                Assert.AreEqual(5, inter.Query.Count());
                Assert.AreEqual(_test1.ID.Guid, inter.Query.First().Id);
                Assert.AreEqual(_test2.ID.Guid, inter.Query.Take(2).Last().Id);
            }
        }

        [Test]
        public void Interface_TreeList()
        {
            //Assign
            TestInterface inter = null;

            //Act 
            inter = _sitecore.GetItem<TestInterface>("/sitecore/content/Glass/Test1");

            //Assert
            Assert.IsNotNull(inter);

            Assert.AreEqual(_test3.ID.Guid, inter.Treelist.First().Id);
        }

        [Test]
        public void Interface_TreeListEx()
        {
            //Assign
            TestInterface inter = null;

            //Act 
            inter = _sitecore.GetItem<TestInterface>("/sitecore/content/Glass/Test1");

            //Assert
            Assert.IsNotNull(inter);

            Assert.AreEqual(_test2.ID.Guid, inter.TreeListEx.First().Id);
        }

        [Test]
        public void Interface_BasicReads()
        {
            //Assign
            TestInterface inter = null;

            //Act 
            inter = _sitecore.GetItem<TestInterface>("/sitecore/content/Glass/Test1");

            //Assert
            Assert.IsNotNull(inter);

            Assert.AreEqual("Multi Line Text Test", inter.MultiLineText);

            Assert.AreEqual(789, inter.Number);
        }


        [Test]
        public void Interface_SetValues()
        {
            //Assign
            TestInterface inter = null;

            //Act 
            inter = _sitecore.GetItem<TestInterface>("/sitecore/content/Glass/Test2");

            inter.MultiLineText = "Test MultiLineText";
            inter.Number = 986;

            using (new SecurityDisabler())
            {
                _sitecore.Save<TestInterface>(inter);
            }

            //Assert
            Assert.IsNotNull(inter);

            Item result = _db.GetItem(_test2.ID);

            Assert.AreEqual("Test MultiLineText", result["MultiLineText"]);
            Assert.AreEqual("986", result["Number"]);
        }

        

    }

    namespace InterfaceFixtureNS
    {
        [SitecoreClass]
        public interface TestInterface
        {
            [SitecoreId]
            Guid Id { get; set; }

            [SitecoreField]
            TestSubInterface DropLink { get; set; }

            [SitecoreField]
            IEnumerable<TestSubInterface> MultiList { get; set; }

            [SitecoreChildren]
            IEnumerable<TestSubInterface> Children { get; set; }

            [SitecoreParent]
            TestSubInterface Parent { get; set; }

            [SitecoreQuery("/sitecore/content/Glass/*[@@TemplateName='BasicTemplate']")]
            IEnumerable<TestSubInterface> Query { get; set; }

            [SitecoreField]
            IEnumerable<TestSubInterface> Treelist { get; set; }
           
            [SitecoreField]
            IEnumerable<TestSubInterface> TreeListEx { get; set; }

            [SitecoreField]
            string MultiLineText { get; set; }

            [SitecoreField]
            int Number { get; set; }

        }
        [SitecoreClass]
        public interface TestSubInterface
        {
            [SitecoreId]
            Guid Id { get; set; }
        }

    }
}
