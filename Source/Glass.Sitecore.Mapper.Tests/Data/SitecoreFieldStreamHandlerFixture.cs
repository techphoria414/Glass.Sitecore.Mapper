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
using Sitecore.Resources.Media;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Data;
using Sitecore.Data;
using System.IO;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Sitecore.SecurityModel;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldStreamHandlerFixture
    {


        SitecoreFieldStreamHandler _handler;
        Database _db;
        Guid _itemId;

        [SetUp]
        public void Setup()
        {
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");
            _itemId = new Guid("{8A317CBA-81D4-4F9E-9953-64C4084AECCA}");
            _handler = new SitecoreFieldStreamHandler();
        }

        #region GetValue

        [Test]
        public void GetValue()
        {
            //Assign
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Stream), "Attachment")
            };

            _handler.ConfigureDataHandler(property);
            //Act
            Stream stream = _handler.GetValue(null, item, null) as Stream;

            //Assert
            Assert.IsNull(stream);

        }

        #endregion
        #region SetValue

        [Test]
        public void SetValue()
        {

            //Assign
            Item item = _db.GetItem(new ID(_itemId));
            SitecoreProperty property = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(),
                Property = new FakePropertyInfo(typeof(Stream), "Attachment")
            };

            _handler.ConfigureDataHandler(property);

            string testString = "Hello World" + DateTime.Now.ToString();
            MemoryStream stream = new MemoryStream();
            byte[] data = UTF8Encoding.UTF8.GetBytes(testString);
            stream.Write(data, 0, data.Length);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();

                //Act
                _handler.SetValue(null, item, stream, null);




                //Assert



                Stream result = item.Fields["Attachment"].GetBlobStream();
                Assert.AreEqual(data.Length, result.Length);

                byte[] dataOut = new byte[result.Length];
                result.Read(dataOut, 0, dataOut.Length);
                string text = UTF8Encoding.UTF8.GetString(dataOut);

                Assert.AreEqual(testString, text);

                //tidy up
                item.Editing.CancelEdit();


            }


        }

        #endregion
    }
}
