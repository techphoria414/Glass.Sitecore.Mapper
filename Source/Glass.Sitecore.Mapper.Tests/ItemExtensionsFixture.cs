using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Data;
using NUnit.Framework;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Tests.Domain;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Tests.ItemExtensionsFixtureNs;
using Sitecore.SecurityModel;

namespace Glass.Sitecore.Mapper.Tests
{
    [TestFixture]
    public class ItemExtensionsFixture
    {

        Database _db;

        [SetUp]
        public void Setup()
        {
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");


            Context context = new Context(
                   new AttributeConfigurationLoader(
                       "Glass.Sitecore.Mapper.Tests.Domain,  Glass.Sitecore.Mapper.Tests",
                       "Glass.Sitecore.Mapper.Tests.ItemExtensionsFixtureNs,  Glass.Sitecore.Mapper.Tests"));




        }

        #region Method - GlassCast

        [Test]
        public void GlassCast_ReadsItemIntoClass()
        {
            //Assign
            Item item = _db.GetItem("/sitecore/content/ItemExtensions/Test");

            //Act
            Test testItem = item.GlassCast<Test>();

            //Assert
            Assert.AreEqual(new Guid("{79E29B29-890F-4F87-A091-2B8AC9462DE6}"), testItem.Id);
            Assert.AreEqual("test content", testItem.SingleLineText);
            Assert.AreEqual(new DateTime(2012, 02, 15, 02, 30, 00), testItem.DateTime);
            Assert.IsNotNull(testItem.Link);
            Assert.AreEqual("Test", testItem.Name);
            Assert.AreEqual("/sitecore/content/ItemExtensions/Test", testItem.Path);
            Assert.AreEqual(3, testItem.Children.Count());



        }
        

        #endregion

        #region Method - GlassRead

        [Test]
        public void GlassRead_ClassReadIntoItem()
        {

            Item item = _db.GetItem("/sitecore/content/ItemExtensions/Test");

            //Assign
            Test testItem = new Test();
            testItem.DateTime = new DateTime(2011, 05, 06, 15, 16, 17);
            testItem.SingleLineText = "Awesome content";
                
            //Act
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                item.GlassRead(testItem);

                //Assert

                Assert.AreEqual("Awesome content", item["SingleLineText"]);
                Assert.AreEqual("20110506T151617", item["DateTime"]);


                item.Editing.CancelEdit();


            }





        }


        #endregion
    }
    namespace ItemExtensionsFixtureNs
    {
        [SitecoreClass]
        public class Test
        {

            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreField]
            public virtual string SingleLineText { get; set; }

            [SitecoreField]
            public virtual DateTime DateTime { get; set; }

            [SitecoreField]
            public virtual EmptyTemplate1 Link { get; set; }

            [SitecoreInfo(SitecoreInfoType.FullPath)]
            public virtual string Path { get; set; }

            [SitecoreInfo(SitecoreInfoType.Name)]
            public virtual string Name { get; set; }

            [SitecoreChildren]
            public virtual IEnumerable<EmptyTemplate1> Children { get; set; }


        }
    }
}
