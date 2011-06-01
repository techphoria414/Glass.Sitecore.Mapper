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
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.FieldTypes;
using Sitecore.Data;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.SecurityModel;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldLinkHandlerFixture
    {
        SitecoreFieldLinkHandler _handler;
        Database _db;
        Guid _itemId;

        [SetUp]
        public void Setup()
        {
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
            _itemId = new Guid("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}");
            _handler = new SitecoreFieldLinkHandler();
        }

        #region GetValue

        [Test]
        public void GetValue_ReturnsValidLink()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));


            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };

            _handler.ConfigureDataHandler(property);

            //Act
            Link result = _handler.GetValue(item, null) as Link;

            //Assert
            Assert.AreEqual("", result.Anchor);
            Assert.AreEqual("Style Class Test", result.Class);
            Assert.AreEqual("_blank", result.Target);
            Assert.AreEqual(Guid.Empty, result.TargetId);
            Assert.AreEqual("Link Description Test", result.Text);
            Assert.AreEqual("Alternate Text Test", result.Title);
            Assert.AreEqual("http://www.google.com", result.Url);
        }

        #endregion
        #region SetValue

        [Test]
        public void SetValue_SetsLink()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));
            Guid targetId = new Guid("{D22C2A23-DF8A-4EC1-AD52-AE15FE63F937}");


            Link link = new Link();
            link.Anchor = "new anchor";
            link.Class = "new class";
            link.Target = "new target";
            link.TargetId = targetId;
            link.Text = "new text";
            link.Title = "new title";
            link.Url = "new url";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };

            _handler.ConfigureDataHandler(property);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue( item, link, null);

                //Assert
                LinkField field = new LinkField(item.Fields["GeneralLink"]);
                Assert.AreEqual(link.Anchor, field.Anchor);
                Assert.AreEqual(link.Class, field.Class);
                Assert.AreEqual(link.Target, field.Target);
                Assert.AreEqual(link.Text, field.Text);
                Assert.AreEqual(link.Title, field.Title);
                Assert.AreEqual(targetId, field.TargetItem.ID.Guid);

                item.Editing.CancelEdit();
            }
        }

        [Test]
        public void SetValue_GuidEmptyTarget()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));
            Guid targetId = Guid.Empty;

            Link link = new Link();
            link.Anchor = "new anchor";
            link.Class = "new class";
            link.Target = "new target";
            link.TargetId = targetId;
            link.Text = "new text";
            link.Title = "new title";
            link.Url = "new url";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };
            _handler.ConfigureDataHandler(property);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue( item, link, null);

                //Assert
                LinkField field = new LinkField(item.Fields["GeneralLink"]);
                Assert.AreEqual(link.Anchor, field.Anchor);
                Assert.AreEqual(link.Class, field.Class);
                Assert.AreEqual(link.Target, field.Target);
                Assert.AreEqual(link.Text, field.Text);
                Assert.AreEqual(link.Title, field.Title);
                Assert.AreEqual(null, field.TargetItem);

                item.Editing.CancelEdit();
            }

        }

        #endregion

    }

}
