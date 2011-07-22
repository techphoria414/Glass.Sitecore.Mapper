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
using Glass.Sitecore.Mapper.FieldTypes;
using Sitecore.Data.Fields;
using Sitecore.SecurityModel;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldFileHandlerFixture
    {
        SitecoreFieldFileHandler _handler;
        Database _db;
        Guid _itemId;

        [SetUp]
        public void Setup()
        {
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
            _itemId = new Guid("{BD193B3A-D3CA-49B4-BF7A-2A61ED77F19D}");
            _handler = new SitecoreFieldFileHandler();
        }

        #region GetValue

        [Test]
        public void GetValue_ReturnsValidFile()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(File), "File")
            };

            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue(item, null) as File;

            //Assert
            Assert.AreEqual("/~/media/Files/SimpleTextFile.ashx", result.Src);
            Assert.AreEqual(new Guid("{368A358E-5835-458B-AFE6-BA5F80334F5A}"), result.Id);

        }

        #endregion

        #region SetValue

        [Test]
        public void SetValue_UpdatesWithNewFile()
        {
            //Assign
            Guid id = new Guid("{B89EA3C6-C947-44AF-9AEF-7EF89CEB0A4B}");
            Item item = _db.GetItem(new ID(_itemId));

            File file = new File();
            file.Id = id;

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(File), "File")
            };
            _handler.ConfigureDataHandler(property);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(item, file, null);

                //Assert
                FileField field = new FileField(item.Fields["File"]);
                Assert.AreEqual(id, field.MediaID.Guid);
                Assert.AreEqual("/~/media/Files/SimpleTextFile2.ashx", field.Src);

                item.Editing.CancelEdit();
            }

        }

        [Test]
        public void SetValue_EmptyGuid_BlanksField()
        {
            //Assign
            Guid id = Guid.Empty;
            Item item = _db.GetItem(new ID(_itemId));

            File file = new File();
            file.Id = id;

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(File), "File")
            };

            _handler.ConfigureDataHandler(property);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue( item, file, null);

                //Assert
                FileField field = new FileField(item.Fields["File"]);
                Assert.AreEqual(null, field.MediaItem);
                Assert.AreEqual(string.Empty, field.Src);

                item.Editing.CancelEdit();
            }

        }

        [Test]
        public void SetValue_NullFile_BlanksField()
        {
            //Assign
            Guid id = Guid.Empty;
            Item item = _db.GetItem(new ID(_itemId));

            File file = null;

            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(File), "File")
            };

            _handler.ConfigureDataHandler(property);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(item, file, null);

                //Assert
                FileField field = new FileField(item.Fields["File"]);
                Assert.AreEqual(null, field.MediaItem);
                Assert.AreEqual(string.Empty, field.Src);

                item.Editing.CancelEdit();
            }

        }

        #endregion
    }
}
