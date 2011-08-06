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
using Sitecore.SecurityModel;

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
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.ContentPath)
            };

            //Act
            var result = _handler.WillHandle(property, null, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region GetValue

        [Test]
        public void GetValue_ContentPath()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.ContentPath)
            };
            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.GetValue( _item, null);

            //Assert
            Assert.AreEqual(_item.Paths.ContentPath, result as string);
        }

        [Test]
        public void GetValue_DisplayName()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.DisplayName)
            };
            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.GetValue( _item, null);

            //Assert
            Assert.AreEqual(_item.DisplayName, result as string);
        }

        [Test]
        public void GetValue_FullPath()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.FullPath)
            };
            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.GetValue( _item,  null);

            //Assert
            Assert.AreEqual(_item.Paths.FullPath, result as string);
        }

        [Test]
        public void GetValue_Key()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Key)
            };
            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.GetValue( _item,  null);

            //Assert
            Assert.AreEqual(_item.Key, result as string);
        }

        [Test]
        public void GetValue_MediaUrl()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.MediaUrl)
            };

            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue( _item,  null);

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
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Path)
            };
            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.GetValue( _item,  null);

            //Assert
            Assert.AreEqual(_item.Paths.Path, result as string);
        }

        [Test]
        public void GetValue_TemplateId()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.TemplateId)
            };

            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue( _item, null);

            //Assert
            Assert.AreEqual(_item.TemplateID.Guid, (Guid)result);
        }

        [Test]
        public void GetValue_TemplateName()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.TemplateName)
            };
            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.GetValue(_item,  null);

            //Assert
            Assert.AreEqual(_item.TemplateName, result as string);
        }

        [Test]
        public void GetValue_Url()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Url)
            };

            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue( _item,  null);

            //Assert
            var value = LinkManager.GetItemUrl(_item);
            Assert.AreEqual(value, result as string);
        }

        [Test]
        public void GetValue_Version()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Version)
            };
            _handler.ConfigureDataHandler(property);

            //Act
            var result = _handler.GetValue( _item,  null);

            //Assert
            Assert.AreEqual(_item.Version.Number, (int)result);
        }

        [Test]
        public void GetValue_Name()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Name)
            };
            _handler.ConfigureDataHandler(property);


            //Act
            var result = _handler.GetValue(_item, null);

            //Assert
            Assert.AreEqual(_item.Name, result as string);
        }
        #endregion

        #region SetValue

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_ContentPath()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.ContentPath)
            };
            _handler.ConfigureDataHandler(property);

            //Act
            _handler.SetValue( _item, "",  null);

        }

        [Test]
        public void SetValue_DisplayName()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.DisplayName)
            };

            _handler.ConfigureDataHandler(property);

            
            //Act
            using (new SecurityDisabler())
            {
                _item.Editing.BeginEdit();

                _handler.SetValue(_item, "NewDisplayName", null);


                //Assert
                Assert.AreEqual("NewDisplayName", _item.DisplayName);

                _item.Editing.EndEdit();


            }


        }

        [Test]
        public void SetValue_Name()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Name)
            };

            _handler.ConfigureDataHandler(property);


            //Act
            using (new SecurityDisabler())
            {
                _item.Editing.BeginEdit();

                _handler.SetValue(_item, "NewItemName", null);


                //Assert
                Assert.AreEqual("NewItemName", _item.Name);

                _item.Editing.EndEdit();

                string newPath = "/sitecore/content/newitemname";

                Assert.AreEqual(newPath, _item.Paths.FullPath.ToLower());

                _item.Editing.BeginEdit();
                _item.Name="Glass";
                _item.Editing.EndEdit();


            }


        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_FullPath()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.FullPath)
            };
            _handler.ConfigureDataHandler(property);
            //Act
            _handler.SetValue( _item, "",  null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_Key()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Key)
            };
            _handler.ConfigureDataHandler(property);
            //Act
            _handler.SetValue( _item, "",  null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_MediaUrl()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.MediaUrl)
            };
            _handler.ConfigureDataHandler(property);
            //Act
            _handler.SetValue( _item, "",  null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_Path()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Path)
            };
            _handler.ConfigureDataHandler(property);
            //Act
            _handler.SetValue( _item, "",  null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_TemplateId()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.TemplateId)
            };
            _handler.ConfigureDataHandler(property);
            //Act
            _handler.SetValue( _item, "",  null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_TemplateName()
        {
            //Assign
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.TemplateName)
            };
            _handler.ConfigureDataHandler(property);
            //Act
            _handler.SetValue( _item, "",  null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_Url()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Url)
            };
            _handler.ConfigureDataHandler(property);
            //Act
            _handler.SetValue( _item, "", null);

        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetValue_Version()
        {
            //Assign
            SitecoreProperty property= new SitecoreProperty()
            {
                Attribute = new SitecoreInfoAttribute(SitecoreInfoType.Version)
            };
            _handler.ConfigureDataHandler(property);
            //Act
            _handler.SetValue( _item, "",  null);

        }

        #endregion
    }
}
