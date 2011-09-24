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
        public void GetValue_ReturnsValidLink_External()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));


            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };

            _handler.ConfigureDataHandler(property);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item["GeneralLink"] = "<link text=\"TestDesc\" querystring=\"TestQuery\" linktype=\"external\" url=\"http://www.google.com\" anchor=\"TestAnchor\" title=\"TestAlt\" class=\"TestClass\" target=\"_blank\" />";

                //Act
                Link result = _handler.GetValue(item, null) as Link;

                Assert.AreEqual("TestAnchor", result.Anchor);
                Assert.AreEqual("TestClass", result.Class);
                Assert.AreEqual("TestQuery", result.Query);
                Assert.AreEqual("_blank", result.Target);
                Assert.AreEqual(Guid.Empty, result.TargetId);
                Assert.AreEqual("TestDesc", result.Text);
                Assert.AreEqual("TestAlt", result.Title);
                Assert.AreEqual(LinkType.External, result.Type);
                Assert.AreEqual("http://www.google.com", result.Url);







                item.Editing.CancelEdit();

            }
           

            //Assert
           
        }

        [Test]
        public void GetValue_ReturnsValidLink_Anchor()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));


            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };

            _handler.ConfigureDataHandler(property);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item["GeneralLink"] = "<link text=\"TestDesc\" linktype=\"anchor\" querystring=\"TestQuery\"  target=\"_blank\" url=\"http://\" anchor=\"TestAnchor\" title=\"TestAlt\" class=\"TestClass\" />";
                //Act
                Link result = _handler.GetValue(item, null) as Link;

                Assert.AreEqual("TestAnchor", result.Anchor);
                Assert.AreEqual("TestClass", result.Class);
                Assert.AreEqual("TestQuery", result.Query);
                Assert.AreEqual("_blank", result.Target);
                Assert.AreEqual(Guid.Empty, result.TargetId);
                Assert.AreEqual("TestDesc", result.Text);
                Assert.AreEqual("TestAlt", result.Title);
                Assert.AreEqual(LinkType.Anchor, result.Type);
                Assert.AreEqual("TestAnchor", result.Url);

                item.Editing.CancelEdit();

            }


            //Assert

        }

        [Test]
        public void GetValue_ReturnsValidLink_MailTo()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));


            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };

            _handler.ConfigureDataHandler(property);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item["GeneralLink"] = "<link text=\"TestDesc\"  querystring=\"TestQuery\" target=\"_blank\" linktype=\"mailto\" url=\"mailto:test@glass.lu\" anchor=\"TestAnchor\" title=\"TestAlt\" class=\"TestClass\" />";
                //Act
                Link result = _handler.GetValue(item, null) as Link;

                Assert.AreEqual("TestAnchor", result.Anchor);
                Assert.AreEqual("TestClass", result.Class);
                Assert.AreEqual("TestQuery", result.Query);
                Assert.AreEqual("_blank", result.Target);
                Assert.AreEqual(Guid.Empty, result.TargetId);
                Assert.AreEqual("TestDesc", result.Text);
                Assert.AreEqual("TestAlt", result.Title);
                Assert.AreEqual(LinkType.MailTo, result.Type);
                Assert.AreEqual("mailto:test@glass.lu", result.Url);

                item.Editing.CancelEdit();

            }


            //Assert

        }


        [Test]
        public void GetValue_ReturnsValidLink_JavaScript()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));


            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };

            _handler.ConfigureDataHandler(property);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item["GeneralLink"] = "<link querystring=\"TestQuery\" text=\"TestDesc\" target=\"_blank\" linktype=\"javascript\" url=\"javascript:JavaScript\" anchor=\"TestAnchor\" title=\"TestAlt\" class=\"TestClass\" />";
                //Act
                Link result = _handler.GetValue(item, null) as Link;

                Assert.AreEqual("TestAnchor", result.Anchor);
                Assert.AreEqual("TestClass", result.Class);
                Assert.AreEqual("TestQuery", result.Query);
                Assert.AreEqual("_blank", result.Target);
                Assert.AreEqual(Guid.Empty, result.TargetId);
                Assert.AreEqual("TestDesc", result.Text);
                Assert.AreEqual("TestAlt", result.Title);
                Assert.AreEqual(LinkType.JavaScript, result.Type);
                Assert.AreEqual("javascript:JavaScript", result.Url);

                item.Editing.CancelEdit();

            }


            //Assert

        }


        [Test]
        public void GetValue_ReturnsValidLink_BlankField()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));


            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };

            _handler.ConfigureDataHandler(property);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item["GeneralLink"] = " ";
                //Act
                Link result = _handler.GetValue(item, null) as Link;

                Assert.IsNull(result);

                item.Editing.CancelEdit();

            }


            //Assert

        }

        [Test]
        public void GetValue_ReturnsValidLink_Internal()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));


            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLinkInternal")
            };

            _handler.ConfigureDataHandler(property);

            //Act
            Link result = _handler.GetValue(item, null) as Link;

            //Assert
            //Assert.AreEqual("", result.Anchor);
            //Assert.AreEqual("Style Class Test", result.Class);
            //Assert.AreEqual("_blank", result.Target);
            //Assert.AreEqual(Guid.Empty, result.TargetId);
            //Assert.AreEqual("Link Description Test", result.Text);
            //Assert.AreEqual("Alternate Text Test", result.Title);
            Assert.AreEqual("/en/sitecore/content/Glass/Test2.aspx", result.Url);
        }

        [Test]
        public void GetValue_ReturnsValidLink_Media()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));


            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLinkMedia")
            };

            _handler.ConfigureDataHandler(property);

            //Act
            Link result = _handler.GetValue(item, null) as Link;

            //Assert
            //Assert.AreEqual("", result.Anchor);
            //Assert.AreEqual("Style Class Test", result.Class);
            //Assert.AreEqual("_blank", result.Target);
            //Assert.AreEqual(Guid.Empty, result.TargetId);
            //Assert.AreEqual("Link Description Test", result.Text);
            //Assert.AreEqual("Alternate Text Test", result.Title);
            Assert.AreEqual("~/media/Files/Kitten1.ashx", result.Url);
        }

        #endregion
        #region SetValue

        [Test]
        public void SetValue_Internal()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            string expected = "<link text=\"TestDesc\" linktype=\"internal\" url=\"/Home.aspx\" anchor=\"TestAnch\" querystring=\"TestQuery\" title=\"TestAlt\" class=\"TestClass\" target=\"_blank\" id=\"{98F907F7-CD1A-4C88-AF11-8F38A21A7FE1}\" />";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };
            _handler.ConfigureDataHandler(property);

            Link link = new Link();
            link.Text = "TestDesc";
            link.Type = LinkType.Internal;
            link.Anchor = "TestAnch";
            link.Query = "TestQuery";
            link.Title = "TestAlt";
            link.Target = "_blank";
            link.Class = "TestClass";
            link.TargetId = new Guid("{98F907F7-CD1A-4C88-AF11-8F38A21A7FE1}");

            string result = string.Empty;

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(item, link, null);
                LinkField field = new LinkField(item.Fields["GeneralLink"]);

                Assert.AreEqual( "TestAnch", field.Anchor);
                Assert.AreEqual( "TestClass",  field.Class);
                Assert.AreEqual("internal", field.LinkType);
                Assert.AreEqual("TestQuery", field.QueryString);
                Assert.AreEqual(new Guid("{98F907F7-CD1A-4C88-AF11-8F38A21A7FE1}"), field.TargetID.Guid);
                Assert.AreEqual("TestDesc", field.Text);
                Assert.AreEqual("TestAlt", field.Title);
                    
                item.Editing.CancelEdit();
            }



           

        }

        [Test]
        public void SetValue_Media()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            string expected = "<link text=\"TestDesc\" linktype=\"media\" url=\"/Files/Kitten1\" title=\"TestText\" class=\"TestClass\" target=\"_blank\" id=\"{223EEAE5-DF4C-4E30-95AC-17BE2F00E2CD}\" />";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };
            _handler.ConfigureDataHandler(property);

            Link link = new Link();
            link.Text = "TestDesc";
            link.Type = LinkType.Media;
            link.Anchor = "TestAnch";
            link.Query = "TestQuery";
            link.Title = "TestAlt";
            link.Target = "_blank";
            link.Class = "TestClass";
            link.TargetId = new Guid("{223EEAE5-DF4C-4E30-95AC-17BE2F00E2CD}");

            string result = string.Empty;

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(item, link, null);
                LinkField field = new LinkField(item.Fields["GeneralLink"]);


                //Assert
                Assert.AreEqual("TestAnch", field.Anchor);
                Assert.AreEqual("TestClass", field.Class);
                Assert.AreEqual("media", field.LinkType);
                Assert.AreEqual("TestQuery", field.QueryString);
                Assert.AreEqual(new Guid("{223EEAE5-DF4C-4E30-95AC-17BE2F00E2CD}"), field.TargetID.Guid);
                Assert.AreEqual("TestDesc", field.Text);
                Assert.AreEqual("TestAlt", field.Title);

                item.Editing.CancelEdit();
            }

        }
        [Test]
        public void SetValue_Exteral()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            string expected = "<link text=\"TestDec\" linktype=\"external\" url=\"http://www.google.com\" anchor=\"\" title=\"TestAlt\" class=\"TestClass\" target=\"_blank\" />";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };
            _handler.ConfigureDataHandler(property);

            Link link = new Link();
            link.Text = "TestDesc";
            link.Type = LinkType.External;
            link.Anchor = "TestAnch";
            link.Query = "TestQuery";
            link.Title = "TestAlt";
            link.Target = "_blank";
            link.Class = "TestClass";
            link.Url = "http://www.google.com";


            string result = string.Empty;

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(item, link, null);
                LinkField field = new LinkField(item.Fields["GeneralLink"]);


                //Assert
                Assert.AreEqual("TestAnch", field.Anchor);
                Assert.AreEqual("TestClass", field.Class);
                Assert.AreEqual("external", field.LinkType);
                Assert.AreEqual("TestQuery", field.QueryString);
                Assert.AreEqual("TestDesc", field.Text);
                Assert.AreEqual("TestAlt", field.Title);
                Assert.AreEqual("http://www.google.com", field.Url);

                item.Editing.CancelEdit();
            }


        }
        [Test]
        public void SetValue_Anchor()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            string expected = "<link text=\"TestDesc\" linktype=\"anchor\" url=\"TestAnch\" anchor=\"TestAnch\" title=\"TestAlt\" class=\"TestClass\" />";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };
            _handler.ConfigureDataHandler(property);

            Link link = new Link();
            link.Text = "TestDesc";
            link.Type = LinkType.Anchor;
            link.Anchor = "TestAnch";
            link.Query = "TestQuery";
            link.Title = "TestAlt";
            link.Target = "_blank";
            link.Class = "TestClass";
            link.Url = "TestUrl";


            string result = string.Empty;

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(item, link, null);
                LinkField field = new LinkField(item.Fields["GeneralLink"]);


                //Assert
                Assert.AreEqual("TestAnch", field.Anchor);
                Assert.AreEqual("TestClass", field.Class);
                Assert.AreEqual("anchor", field.LinkType);
                Assert.AreEqual("TestQuery", field.QueryString);
                Assert.AreEqual("TestDesc", field.Text);
                Assert.AreEqual("TestAlt", field.Title);
                Assert.AreEqual("TestAnch", field.Url);

                item.Editing.CancelEdit();
            }

        }
        [Test]
        public void SetValue_MailTo()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            string expected = "<link text=\"TestDesc\" linktype=\"mailto\" url=\"mailto:test@glass.lu\" anchor=\"\" title=\"testalt\" class=\"testclass\" />";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };
            _handler.ConfigureDataHandler(property);

            Link link = new Link();
            link.Text = "TestDesc";
            link.Type = LinkType.MailTo;
            link.Anchor = "TestAnch";
            link.Query = "TestQuery";
            link.Title = "TestAlt";
            link.Target = "_blank";
            link.Class = "TestClass";
            link.Url = "mailto:test@glass.lu";


            string result = string.Empty;

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(item, link, null);
                LinkField field = new LinkField(item.Fields["GeneralLink"]);


                //Assert
                Assert.AreEqual("TestAnch", field.Anchor);
                Assert.AreEqual("TestClass", field.Class);
                Assert.AreEqual("mailto", field.LinkType);
                Assert.AreEqual("TestQuery", field.QueryString);
                Assert.AreEqual("TestDesc", field.Text);
                Assert.AreEqual("TestAlt", field.Title);
                Assert.AreEqual("mailto:test@glass.lu", field.Url);

                item.Editing.CancelEdit();
            }

       

        }
        [Test]
        public void SetValue_JavaScript()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            string expected = "<link text=\"Test\" linktype=\"javascript\" url=\"javascript:JavaScript\" anchor=\"\" title=\"text\" class=\"class\" />";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };
            _handler.ConfigureDataHandler(property);

            Link link = new Link();
            link.Text = "TestDesc";
            link.Type = LinkType.JavaScript;
            link.Anchor = "TestAnch";
            link.Query = "TestQuery";
            link.Title = "TestAlt";
            link.Target = "_blank";
            link.Class = "TestClass";
            link.Url = "javascript:JavaScript";


            string result = string.Empty;

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(item, link, null);
                LinkField field = new LinkField(item.Fields["GeneralLink"]);


                //Assert
                Assert.AreEqual("TestAnch", field.Anchor);
                Assert.AreEqual("TestClass", field.Class);
                Assert.AreEqual("javascript", field.LinkType);
                Assert.AreEqual("TestQuery", field.QueryString);
                Assert.AreEqual("TestDesc", field.Text);
                Assert.AreEqual("TestAlt", field.Title);
                Assert.AreEqual("javascript:JavaScript", field.Url);

                item.Editing.CancelEdit();
            }

        }
        [Test]
        public void SetValue_NullValue()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            string expected = "";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };
            _handler.ConfigureDataHandler(property);

            Link link = null; 

            string result = string.Empty;

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(item, link, null);
                result = item.Fields["GeneralLink"].Value;

                item.Editing.CancelEdit();
            }

            //Assert

            Assert.AreEqual(expected, result);

        }

        [Test]
        public void SetValue_LinkNoValues()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));

            string expected = "";

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Link), "GeneralLink")
            };
            _handler.ConfigureDataHandler(property);

            Link link = new Link();

            string result = string.Empty;

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(item, link, null);
                result = item.Fields["GeneralLink"].Value;

                item.Editing.CancelEdit();
            }

            //Assert

            Assert.AreEqual(expected, result);

        }

        #endregion

    }

}
