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
using Sitecore.Data;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Sitecore.SecurityModel;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldStringHandlerFixture
    {
        SitecoreFieldStringHandler _handler;
        Database _db;
        Guid _itemId;
        Item _item;

        string _richTextContent= "<p>This is a test with <a href=\"~/link.aspx?_id=98F907F7CD1A4C88AF118F38A21A7FE1&amp;_z=z\">link</a></p>";
        string _richTextOriginal = "";

        [SetUp]
        public void Setup()
        {
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
            _itemId = new Guid("{8A317CBA-81D4-4F9E-9953-64C4084AECCA}");
            _handler = new SitecoreFieldStringHandler();

            _item = _db.GetItem(new ID(_itemId));
            _richTextOriginal = _item["RichText"];

            using (new SecurityDisabler())
            {
                _item.Editing.BeginEdit();
                _item["RichText"] = _richTextContent;
                _item.Editing.EndEdit();
            }

        }

        [TearDown]
        public void TearDown()
        {
            using (new SecurityDisabler())
            {
                _item.Editing.BeginEdit();
                _item["RichText"] = _richTextOriginal;
                _item.Editing.EndEdit();
            }
        }

        #region GetValue

        [Test]
        public void GetValue_SingleLineText_GetsRawString()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(string), "SingleLineText")
            };

            //Act
            var result = _handler.GetValue(null, _item, property, null);

            //Assert
            Assert.AreEqual("test single line text", result);

        }

        [Test]
        public void GetValue_RichText_GetsModifiedString()
        {
            //Assign
            //for the render method we have to set a site context for link replacement.
            //this also required a change in the app.config for the website site config, database changed to master 
            //in express edition the website database is set to web instead of master
            global::Sitecore.Context.Site = global::Sitecore.Configuration.Factory.GetSite("website");

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(string), "RichText")
            };

            //Act
            var result = _handler.GetValue(null, _item, property, null);

            //Assert
            Assert.AreNotEqual(_richTextContent, result);
        }

        [Test]
        public void GetValue_RichText_ReturnRawTrue_ReturnsRawContent()
        {
            //Assign
            //for the render method we have to set a site context for link replacement.
            //this also required a change in the app.config for the website site config, database changed to master 
            //in express edition the website database is set to web instead of master
            global::Sitecore.Context.Site = global::Sitecore.Configuration.Factory.GetSite("website");

            Item item = _db.GetItem(new ID(_itemId));
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(){Setting = SitecoreFieldSettings.RichTextRaw},
                Property = new FakePropertyInfo(typeof(string), "RichText")
            };

            //Act
            var result = _handler.GetValue(null, item, property, null);

            //Assert
            Assert.AreEqual(_richTextContent, result);
        }

        #endregion
        #region SetFieldValue

        [Test]
        public void SetFieldValue_ReturnsSameString()
        {
            //Assign
            string value = "some test string";
            SitecoreProperty property = new SitecoreProperty()
            {
                Property = new FakePropertyInfo(typeof(string))
            };

            //Act
            var result = _handler.SetFieldValue( value, property, null);

            //Assert
            Assert.AreEqual(value, result);
        }
        #endregion

        #region SetValue

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_SetRichTextField_NoRaw_ThrowException()
        {
            //Assign
            string value = "some test string";
            SitecoreProperty property = new SitecoreProperty()
            {
                Property = new FakePropertyInfo(typeof(string), "RichText"),
                Attribute = new SitecoreFieldAttribute()
            };

            using (new SecurityDisabler())
            {
                //Act
                _item.Editing.BeginEdit();
                try
                {

                    _handler.SetValue(null, _item, value, property, null);
                }
                finally
                {
                    //Assert
                    _item.Editing.EndEdit();

                }
            }   


        }

        [Test]
        public void SetValue_SetRichTextField_Raw_SetsValue()
        {
            //Assign
            string value = "some test string";
            SitecoreProperty property = new SitecoreProperty()
            {
                Property = new FakePropertyInfo(typeof(string), "RichText"),
                Attribute = new SitecoreFieldAttribute()
                {
                    Setting  = SitecoreFieldSettings.RichTextRaw
                }
            };

            using (new SecurityDisabler())
            {
                //Act

                _item.Editing.BeginEdit();
                _handler.SetValue(null, _item, value, property, null);

                //Assert
                Assert.AreEqual(_item["RichText"], value);

                _item.Editing.EndEdit();

            }
        }


        #endregion
    }
}
