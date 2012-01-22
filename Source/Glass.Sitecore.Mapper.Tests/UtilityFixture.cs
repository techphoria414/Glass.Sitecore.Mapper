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
using System.Collections.Specialized;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Tests
{
    [TestFixture]
    public class UtilityFixture
    {
        #region ConvertAttributes

        [Test]
        public void ConvertAttributes_ConvertsListIntoCorrectText()
        {
            //Assign
            NameValueCollection attrs = new NameValueCollection();
            attrs.Add("title", "some title");
            attrs.Add("class", "classy");

            //Act
            string result = Utility.ConvertAttributes(attrs);

            //Assert
            Assert.AreEqual("title='some title' class='classy' ", result);
            
        }
        [Test]
        public void ConvertAttributes_ConvertsListIntoCorrectTextWithNullValue()
        {
            //Assign
            NameValueCollection attrs = new NameValueCollection();
            attrs.Add("title", "some title");
            attrs.Add("class", null);

            //Act
            string result = Utility.ConvertAttributes(attrs);

            //Assert
            Assert.AreEqual("title='some title' class='' ", result);
        }

        [Test]
        public void ConvertAttributes_EmptyCollectionReturnsEmptyString()
        {
            //Assign
            NameValueCollection attrs = new NameValueCollection();

            //Act
            var result = Utility.ConvertAttributes(attrs);

            //Assert

            Assert.AreEqual("", result);
        }

        [Test]
        public void ConvertAttributes_NullCollectionReturnsEmptyString()
        {
            //Assign
            NameValueCollection attrs = null;

            //Act
            var result = Utility.ConvertAttributes(attrs);

            //Assert

            Assert.AreEqual("", result);
        }
        #endregion
        #region CreateUrlOptions

    [Test]
        public void CreateUrlOptions_NoValueSetReturnsNull()
        {
            //Assign
            SitecoreInfoUrlOptions options = SitecoreInfoUrlOptions.Default;

            //Act
            var result = Utility.CreateUrlOptions(options);

            //Asset
            Assert.IsNotNull(result);
        }
    [Test]
        public void CreateUrlOptions_CombinedValues()
        {
            //Assign
            SitecoreInfoUrlOptions options = SitecoreInfoUrlOptions.AlwaysIncludeServerUrl| SitecoreInfoUrlOptions.EncodeNames;

            //Act
            var result = Utility.CreateUrlOptions(options);

            //Asset
            Assert.IsTrue(result.AlwaysIncludeServerUrl);
            Assert.IsTrue(result.EncodeNames);
        }

        #endregion
    }
}
