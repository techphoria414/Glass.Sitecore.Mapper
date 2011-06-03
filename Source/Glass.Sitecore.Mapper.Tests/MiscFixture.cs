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
using Sitecore.Data;
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.FieldTypes;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Data.Fields;
using Sitecore.SecurityModel;

namespace Glass.Sitecore.Mapper.Tests
{
    [TestFixture]
    public class MiscFixture
    {


        SitecoreService _sitecore;
        Context _context;
        Database _db;
        Item _test1;
        Item _test2;
        Item _test3;

        [SetUp]
        public void Setup()
        {
            AttributeConfigurationLoader loader = new AttributeConfigurationLoader(
               new string[] { "Glass.Sitecore.Mapper.Tests.MiscFixtureNS, Glass.Sitecore.Mapper.Tests" }
               );

            _context = new Context(loader, new AbstractSitecoreDataHandler[] {});
            global::Sitecore.Context.Site = global::Sitecore.Configuration.Factory.GetSite("website");

            _sitecore = new SitecoreService("master");
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            _test1 = _db.GetItem("/sitecore/content/Glass/Test1");
            _test2 = _db.GetItem("/sitecore/content/Glass/Test2");
            _test3 = _db.GetItem("/sitecore/content/Glass/Test1/Test3");

        }

        #region Item Test1
        // Item: /sitecore/content/Glass/Test1
        [Test]
        public void GetItem_Test1()
        {

            //Assign 
            MiscFixtureNS.BasicTemplate test = null;

            //Act
            test = _sitecore.GetItem<MiscFixtureNS.BasicTemplate>("/sitecore/content/Glass/Test1");

            //Assert

            #region SitecoreId
            
            Assert.AreEqual(_test1.ID.Guid, test.Id);

            #endregion

            #region Fields
            #region SimpleTypes

            Assert.IsTrue(test.Checkbox);

            Assert.AreEqual(new DateTime(2011, 05, 01), test.Date);

            Assert.AreEqual(new DateTime(2011, 06, 01, 06, 30, 00), test.DateTime);


            var file=new File(){
                    Id = new Guid("{368A358E-5835-458B-AFE6-BA5F80334F5A}"),
                    Src ="/~/media/Files/SimpleTextFile.ashx"
                };
            Assert.AreEqual(file.Id, test.File.Id);
            Assert.AreEqual(file.Src, test.File.Src);

            var image = new Image()
            {
                Alt = "Alternate Text  Test",
                Border = "",
                Class = "",
                Height = 540,
                HSpace = 50,
                MediaId = new Guid("{223EEAE5-DF4C-4E30-95AC-17BE2F00E2CD}"),
                Src = "/~/media/Files/Kitten1.ashx",
                VSpace = 60,
                Width = 720,
            };
            Assert.AreEqual(image.Alt, test.Image.Alt);
            Assert.AreEqual(image.Border, test.Image.Border);
            Assert.AreEqual(image.Class, test.Image.Class);
            Assert.AreEqual(image.Height, test.Image.Height);
            Assert.AreEqual(image.HSpace, test.Image.HSpace);
            Assert.AreEqual(image.MediaId, test.Image.MediaId);
            Assert.AreEqual(image.Src, test.Image.Src);
            Assert.AreEqual(image.VSpace, test.Image.VSpace);
            Assert.AreEqual(image.Width, test.Image.Width);

            Assert.AreEqual(456, test.Integer);

            Assert.AreEqual(456f, test.Float);

            Assert.AreEqual(456d, test.Double);

            Assert.AreEqual(456m, test.Decimal);

            Assert.AreEqual("Multi Line Text Test", test.MultiLineText);

            Assert.AreEqual(789, test.Number);

            Assert.AreEqual("Password Test", test.Password);

            Assert.AreEqual("<p>Rich Text Test</p>", test.RichText.Trim());

            Assert.AreEqual("Single Line Text Test", test.SingleLineText);

            #endregion

            #region List Types

            Assert.AreEqual(2, test.CheckList.Count());
            Assert.AreEqual(_test1.ID.Guid, test.CheckList.First().Id);
            Assert.AreEqual(_test2.ID.Guid, test.CheckList.Last().Id);

            Assert.AreEqual(MiscFixtureNS.TestEnum.Test1, test.DropList);

            Assert.AreEqual(_test3.ID.Guid, test.GroupedDropLink.Id);

            Assert.AreEqual(MiscFixtureNS.TestEnum.Test3, test.GroupedDropList);

            Assert.AreEqual(_test1.ID.Guid, test.MultiList.First().Id);

            Assert.AreEqual(_test3.ID.Guid, test.Treelist.First().Id);

            Assert.AreEqual(_test2.ID.Guid, test.TreeListEx.First().Id);

            #endregion
            #region Link Types

            Assert.AreEqual(_test1.ID.Guid, test.DropLink.Id);

            Assert.AreEqual(_test1.ID.Guid, test.DropTree.Id);

            var link = new Link(){
                Anchor = "",
                Class = "Style Class Test",
                Target = "_blank",
                TargetId = Guid.Empty,
                Text = "Link Description Test",
                Title = "Alternate Text Test",
                Url = "http://www.google.com"
            };
            Assert.AreEqual(link.Anchor, test.GeneralLink.Anchor);
            Assert.AreEqual(link.Class, test.GeneralLink.Class);
            Assert.AreEqual(link.Target, test.GeneralLink.Target);
            Assert.AreEqual(link.TargetId, test.GeneralLink.TargetId);
            Assert.AreEqual(link.Text, test.GeneralLink.Text);
            Assert.AreEqual(link.Title, test.GeneralLink.Title);
            Assert.AreEqual(link.Url, test.GeneralLink.Url);


            #endregion

            #region Developer Types

            Assert.AreEqual("/sitecore/shell/Themes/Standard/Applications/48x48/about.png", test.Icon);

            Assert.AreEqual(TriState.No, test.TriState);

            #endregion

            #region System Types

            Console.WriteLine("Attachment not tested");

            #endregion
            #endregion

            #region SitecoreInfo

            Assert.AreEqual(_test1.Paths.ContentPath, test.ContentPath);

            Assert.AreEqual(_test1.DisplayName, test.DisplayName);

            Assert.AreEqual(_test1.Paths.FullPath, test.FullPath);

            Assert.AreEqual(_test1.Key, test.Key);

            Console.WriteLine("Not test media URL");

            Assert.AreEqual(_test1.Paths.Path, test.Path);

            Assert.AreEqual(_test1.TemplateID.Guid, test.TemplateId);

            Assert.AreEqual(_test1.TemplateName, test.TemplateName);

            Assert.AreEqual(LinkManager.GetItemUrl(_test1), test.Url);

            Assert.AreEqual(_test1.Version.Number, test.Version);

            #endregion

            #region SitecoreChildren

            Assert.AreEqual(_test1.Children.Count, test.Children.Count());
            Assert.AreEqual(_test3.ID.Guid, test.Children.First().Id);

            #endregion

            #region SitecoreParent

            Assert.AreEqual(_test1.ParentID.Guid, test.Parent.Id);

            #endregion

            #region SitecoreQuery

            //we have to use the security disabler because we are outside of an ASP.NET context
            //if you do this without the disabler the role manager throws an exception
            using (new SecurityDisabler())
            {
                Assert.AreEqual(3, test.Query.Count());
                Assert.AreEqual(_test1.ID.Guid, test.Query.First().Id);
                Assert.AreEqual(_test2.ID.Guid, test.Query.Take(2).Last().Id);
            }
            #endregion

        }

        #endregion

        #region Item Test 
        [Test]
        public void SetItem_Test2()
        {
            //Assign 
            //clear all fields
            using (new SecurityDisabler())
            {
                _test2.BeginEdit();
                foreach (Field field in _test2.Fields)
                {
                    field.Value = "";
                }
                _test2["GroupedDropList"] = "Test2";
                _test2["DropList"] = "Test2";
                _test2.EndEdit();
            }
            
            MiscFixtureNS.BasicTemplate test = 
                _sitecore.GetItem<MiscFixtureNS.BasicTemplate>("/sitecore/content/Glass/Test2");
            
            //Simple Types

            test.Checkbox = true;
            test.Date = new DateTime(2011, 02, 28);
            test.DateTime = new DateTime(2011, 03, 04, 15, 23, 12);
            test.File = new File() { Id = new Guid("{B89EA3C6-C947-44AF-9AEF-7EF89CEB0A4B}") };
            test.Image = new Image()
            {
                Alt="Test Alt",
                Border = "Test Border",
                Class="Test Class",
                Height=487,
                HSpace=52,
                MediaId = new Guid("{0CF0A6D0-8A2B-479B-AD8F-14938135174A}"),
                VSpace= 32,
                Width = 26
            };
            test.Integer = 659;
            test.Float = 458.7f;
            test.Double = 789.5d;
            test.Decimal = 986.4m;
            test.MultiLineText = "Test MultiLineText";
            test.Number = 986;
            test.Password = "test password";
            test.RichText = "test Rich Text";
            test.SingleLineText = "test single line text";

            //List Types
            test.CheckList = new MiscFixtureNS.SubClass[]{
                new MiscFixtureNS.SubClass(){Id = _test1.ID.Guid},
                new MiscFixtureNS.SubClass(){Id = _test2.ID.Guid},
            };
            test.DropList = MiscFixtureNS.TestEnum.Test3;
            test.GroupedDropLink = new MiscFixtureNS.SubClass() { Id = _test3.ID.Guid };
            test.GroupedDropList = MiscFixtureNS.TestEnum.Test3;
            test.MultiList = new MiscFixtureNS.SubClass[]{
                new MiscFixtureNS.SubClass(){Id = _test1.ID.Guid},
                new MiscFixtureNS.SubClass(){Id = _test2.ID.Guid},
            };
            test.Treelist = new MiscFixtureNS.SubClass[]{
                new MiscFixtureNS.SubClass(){Id = _test1.ID.Guid},
                new MiscFixtureNS.SubClass(){Id = _test2.ID.Guid},
            };
            test.TreeListEx = new MiscFixtureNS.SubClass[]{
                new MiscFixtureNS.SubClass(){Id = _test1.ID.Guid},
                new MiscFixtureNS.SubClass(){Id = _test2.ID.Guid},
            };

            //Link Types 
            test.DropLink = new MiscFixtureNS.SubClass() { Id = _test3.ID.Guid };
            test.DropTree = new MiscFixtureNS.SubClass() { Id = _test3.ID.Guid };
            test.GeneralLink = new Link(){
                Anchor="test anchor",
                Class="test class",
                Target="test target",
                Text="test text",
                Title="test title",
                Url="test url"
            };

            //Developer Types
            test.Icon = "test icon";
            test.TriState = TriState.Yes;


            //Act
            using (new SecurityDisabler())
            {
                _sitecore.Save<MiscFixtureNS.BasicTemplate>(test);
            }
            
            //Assert

            //Simple Types
            Item result = _db.GetItem(_test2.ID);
            Assert.AreEqual("1", result["Checkbox"]);
            Assert.AreEqual("20110228T000000", result["Date"]);
            Assert.AreEqual("20110304T152312", result["DateTime"]);
          
            var file = new FileField(result.Fields["File"]);
            Assert.AreEqual(new Guid("{B89EA3C6-C947-44AF-9AEF-7EF89CEB0A4B}"), file.MediaID.Guid);
            Assert.AreEqual("/~/media/Files/SimpleTextFile2.ashx", file.Src);
           
            var image = new ImageField(result.Fields["Image"]);
            Assert.AreEqual("Test Alt", image.Alt);
            Assert.AreEqual("Test Border", image.Border);
            Assert.AreEqual("Test Class", image.Class);
            Assert.AreEqual("487", image.Height);
            Assert.AreEqual("52", image.HSpace);
            Assert.AreEqual(new Guid("{0CF0A6D0-8A2B-479B-AD8F-14938135174A}"), image.MediaID.Guid);
            Assert.AreEqual("/~/media/Files/Kitten2.ashx", image.Src);
            Assert.AreEqual("32", image.VSpace);
            Assert.AreEqual("26", image.Width);

            Assert.AreEqual("659", result["Integer"]);
            Assert.AreEqual("458.7", result["Float"]);
            Assert.AreEqual("789.5", result["Double"]);
            Assert.AreEqual("986.4", result["Decimal"]);
            Assert.AreEqual("Test MultiLineText", result["MultiLineText"]);
            Assert.AreEqual("986", result["Number"]);
            Assert.AreEqual("test password", result["Password"]);
            Assert.AreEqual("test Rich Text", result["RichText"]);
            Assert.AreEqual("test single line text", result["SingleLineText"]);
            
            //List Types

            Assert.AreEqual("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}|{8A317CBA-81D4-4F9E-9953-64C4084AECCA}", result["CheckList"].ToUpper());
            Assert.AreEqual("Test3", result["DropList"]);
            Assert.AreEqual("{C28E80AF-26E5-4004-BF99-F63FA8772D39}", result["GroupedDropLink"].ToUpper());
            Assert.AreEqual("Test3", result["GroupedDropList"]);
            Assert.AreEqual("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}|{8A317CBA-81D4-4F9E-9953-64C4084AECCA}", result["MultiList"].ToUpper());
            Assert.AreEqual("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}|{8A317CBA-81D4-4F9E-9953-64C4084AECCA}", result["Treelist"].ToUpper());
            Assert.AreEqual("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}|{8A317CBA-81D4-4F9E-9953-64C4084AECCA}", result["TreeListEx"].ToUpper());

            //Linked Types
            Assert.AreEqual("{C28E80AF-26E5-4004-BF99-F63FA8772D39}", result["DropLink"].ToUpper());
            Assert.AreEqual("{C28E80AF-26E5-4004-BF99-F63FA8772D39}", result["DropTree"].ToUpper());
            LinkField link = new LinkField(result.Fields["GeneralLink"]);
            Assert.AreEqual("test anchor", link.Anchor);
            Assert.AreEqual("test class", link.Class);
            Assert.AreEqual("test target", link.Target);
            Assert.AreEqual("test text", link.Text);
            Assert.AreEqual("test title", link.Title);
            Assert.AreEqual("test url", link.Url);

            //Developer Type

            Assert.AreEqual("test icon", result["Icon"]);
            Assert.AreEqual("1", result["TriState"]);

        }
        #endregion
    }

    namespace MiscFixtureNS
    {
        [SitecoreClass]
        public class BasicTemplate
        {
            #region SitecoreId

            [SitecoreId]
            public virtual Guid Id { get; set; }

            #endregion

            #region Fields
            #region Simple Types

            [SitecoreField]
            public virtual bool Checkbox { get; set; }
            [SitecoreField]
            public virtual DateTime Date { get; set; }
            [SitecoreField]
            public virtual DateTime DateTime { get; set; }
            [SitecoreField]
            public virtual File File { get; set; }
            [SitecoreField]
            public virtual Image Image { get; set; }
            [SitecoreField]
            public virtual int Integer { get; set; }
            [SitecoreField]
            public virtual float Float { get; set; }
            [SitecoreField]
            public virtual double Double { get; set; }
            [SitecoreField]
            public virtual decimal Decimal { get; set; }

            [SitecoreField]
            public virtual string MultiLineText { get; set; }
            [SitecoreField]
            public virtual int Number { get; set; }
            [SitecoreField]
            public virtual string Password { get; set; }
            [SitecoreField(Setting=SitecoreFieldSettings.RichTextRaw)]
            public virtual string RichText { get; set; }
            [SitecoreField]
            public virtual string SingleLineText { get; set; }

            #endregion

            #region List Types

            [SitecoreField]
            public virtual IEnumerable<SubClass> CheckList { get; set; }
            [SitecoreField]
            public virtual TestEnum DropList { get; set; }
            [SitecoreField]
            public virtual SubClass GroupedDropLink { get; set; }
            [SitecoreField]
            public virtual TestEnum GroupedDropList { get; set; }
            [SitecoreField]
            public virtual IEnumerable<SubClass> MultiList { get; set; }
            [SitecoreField]
            public virtual IEnumerable<SubClass> Treelist { get; set; }
            [SitecoreField]
            public virtual IEnumerable<SubClass> TreeListEx { get; set; }

            #endregion

            #region Link Types

            [SitecoreField]
            public virtual SubClass DropLink { get; set; }
            [SitecoreField]
            public virtual SubClass DropTree { get; set; }
            [SitecoreField]
            public virtual Link GeneralLink { get; set; }

            #endregion

            #region Developer Types

            [SitecoreField]
            public virtual string Icon { get; set; }
            
            [SitecoreField]
            public virtual TriState TriState { get; set; }

            #endregion

            #region SystemType

            [SitecoreField]
            public virtual System.IO.Stream Attachment { get; set; }

            #endregion

            #endregion

            #region SitecoreInfo

            [SitecoreInfo(SitecoreInfoType.ContentPath)]
            public virtual string ContentPath { get; set; }
            [SitecoreInfo(SitecoreInfoType.DisplayName)]
            public virtual string DisplayName { get; set; }
            [SitecoreInfo(SitecoreInfoType.FullPath)]
            public virtual string FullPath { get; set; }
            [SitecoreInfo(SitecoreInfoType.Key)]
            public virtual string Key { get; set; }
            [SitecoreInfo(SitecoreInfoType.MediaUrl)]
            public virtual string MediaUrl { get; set; }
            [SitecoreInfo(SitecoreInfoType.Path)]
            public virtual string Path { get; set; }
            [SitecoreInfo(SitecoreInfoType.TemplateId)]
            public virtual Guid TemplateId { get; set; }
            [SitecoreInfo(SitecoreInfoType.TemplateName)]
            public virtual string TemplateName { get; set; }
            [SitecoreInfo(SitecoreInfoType.Url)]
            public virtual string Url { get; set; }
            [SitecoreInfo(SitecoreInfoType.Version)]
            public virtual int Version { get; set; }

            #endregion

            #region SitecoreChildren
            
            [SitecoreChildren]
            public virtual IEnumerable<SubClass> Children { get; set; }

            #endregion
            
            #region SitecoreParent

            [SitecoreParent]
            public virtual SubClass Parent { get; set; }

            #endregion

            #region SitecoreQuery

            [SitecoreQuery("/sitecore/content/Glass/*[@@TemplateName='BasicTemplate']")]
            public virtual IEnumerable<SubClass> Query { get; set; }

            #endregion

        }

        [SitecoreClass]
        public class SubClass{
            
            [SitecoreId]
            public virtual Guid Id{get;set;}

        }

        public enum TestEnum
        {
            Test1,
            Test2,
            Test3
        }
    }
}
