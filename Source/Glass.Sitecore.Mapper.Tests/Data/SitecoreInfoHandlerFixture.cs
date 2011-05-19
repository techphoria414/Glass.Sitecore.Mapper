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
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Configuration;
using Sitecore.Links;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreInfoHandlerFixture
    {
        InstanceContext _context;
        Database _db;
        Item _item;

        SitecoreInfoHandler _handler;

        [SetUp]
        public void Setup()
        {
             _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
             _item = _db.GetItem(new ID(new Guid("{D22C2A23-DF8A-4EC1-AD52-AE15FE63F937}")));
             _handler = new SitecoreInfoHandler();
        }

        #region  WillHandler

        [Test]
        public void WillHandler_ReturnsTrue()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.ContentPath)
            };
               

            //Act
            var result = _handler.WillHandle(prop, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion 

        #region GetValue

        [Test]
        public void GetValue_ContentPath()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.ContentPath)
            };


            //Act
            var result = _handler.GetValue(null, _item, prop, null);

            //Assert
            Assert.AreEqual(_item.Paths.ContentPath, result as string);
        }

        [Test]
        public void GetValue_DisplayName()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.DisplayName)
            };


            //Act
            var result = _handler.GetValue(null, _item, prop, null);

            //Assert
            Assert.AreEqual(_item.DisplayName, result as string);
        }

        [Test]
        public void GetValue_FullPath()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.FullPath)
            };


            //Act
            var result = _handler.GetValue(null, _item, prop, null);

            //Assert
            Assert.AreEqual(_item.Paths.FullPath, result as string);
        }

        [Test]
        public void GetValue_Key()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Key)
            };


            //Act
            var result = _handler.GetValue(null, _item, prop, null);

            //Assert
            Assert.AreEqual(_item.Key, result as string);
        }

        [Test]
        public void GetValue_MediaUrl()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.MediaUrl)
            };


            //Act
            var result = _handler.GetValue(null, _item, prop, null);

            //Assert
            //It think this test is probably wrong
            global::Sitecore.Data.Items.MediaItem media = new global::Sitecore.Data.Items.MediaItem(_item);
            string value = global::Sitecore.Resources.Media.MediaManager.GetMediaUrl(media);

            Assert.AreEqual(value, result as string);
        }

        [Test]
        public void GetValue_Path()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Path)
            };


            //Act
            var result = _handler.GetValue(null, _item, prop, null);

            //Assert
            Assert.AreEqual(_item.Paths.Path, result as string);
        }

        [Test]
        public void GetValue_TemplateId()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.TemplateId)
            };


            //Act
            var result = _handler.GetValue(null, _item, prop, null);

            //Assert
            Assert.AreEqual(_item.TemplateID.Guid, (Guid)result );
        }

        [Test]
        public void GetValue_TemplateName()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.TemplateName)
            };


            //Act
            var result = _handler.GetValue(null, _item, prop, null);

            //Assert
            Assert.AreEqual(_item.TemplateName, result as string);
        }

        [Test]
        public void GetValue_Url()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Url)
            };


            //Act
            var result = _handler.GetValue(null, _item, prop, null);

            //Assert
            var value =  LinkManager.GetItemUrl(_item);
            Assert.AreEqual(value, result as string);
        }

        [Test]
        public void GetValue_Version()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Version)
            };


            //Act
            var result = _handler.GetValue(null, _item, prop, null);

            //Assert
            Assert.AreEqual(_item.Version.Number, (int)result);
        }
        #endregion

        #region SetValue
       
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_ContentPath()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.ContentPath)
            };
            //Act
            _handler.SetValue(null, _item, "", prop, null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_DisplayName()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.DisplayName)
            };
            //Act
            _handler.SetValue(null, _item, "", prop, null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_FullPath()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.FullPath)
            };
            //Act
            _handler.SetValue(null, _item, "", prop, null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_Key()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Key)
            };
            //Act
            _handler.SetValue(null, _item, "", prop, null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_MediaUrl()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.MediaUrl)
            };
            //Act
            _handler.SetValue(null, _item, "", prop, null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_Path()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Path)
            };
            //Act
            _handler.SetValue(null, _item, "", prop, null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_TemplateId()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.TemplateId)
            };
            //Act
            _handler.SetValue(null, _item, "", prop, null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_TemplateName()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.TemplateName)
            };
            //Act
            _handler.SetValue(null, _item, "", prop, null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_Url()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Url)
            };
            //Act
            _handler.SetValue(null, _item, "", prop, null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_Version()
        {
            //Assign
            SitecoreProperty prop = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Version)
            };
            //Act
            _handler.SetValue(null, _item, "", prop, null);

        }

        #endregion
    }
}
