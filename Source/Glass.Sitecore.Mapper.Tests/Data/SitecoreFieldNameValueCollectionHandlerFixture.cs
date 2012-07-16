using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Sitecore.Mapper.Data;
using System.Collections.Specialized;

namespace Glass.Sitecore.Mapper.Tests.Data
{
    [TestFixture]
    public class SitecoreFieldNameValueCollectionHandlerFixture
    {


        #region GetFieldValue

        [Test]
        public void  GetFieldValue_EmtpyField_ReturnsNameValueCollectionNoItems(){
            //Assign
            SitecoreFieldNameValueCollectionHandler handler = new SitecoreFieldNameValueCollectionHandler();
            string input = string.Empty;

            //Act
            var result = handler.GetFieldValue(input, null, null) as NameValueCollection;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void GetFieldValue_FieldWithValues_ReturnsNameValueCollectionNoItems()
        {
            //Assign
            SitecoreFieldNameValueCollectionHandler handler = new SitecoreFieldNameValueCollectionHandler();
            string input = "Test=value%26&Mike=ted";

            //Act
            var result = handler.GetFieldValue(input, null, null) as NameValueCollection;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Test", result.Keys[0]);
            Assert.AreEqual("ted", result["Mike"]);
            Assert.AreEqual("value&", result["Test"]);


        }

        #endregion

        #region SetFieldValue

        [Test]
        public void SetFieldValue()
        {
            //Assign
            SitecoreFieldNameValueCollectionHandler handler = new SitecoreFieldNameValueCollectionHandler();
            
            NameValueCollection values = new NameValueCollection();
            values["Test"] = "value&";
            values["Mike"] = "ted";

            string expected = "Test=value%26&Mike=ted";

            //Act
            var result = handler.SetFieldValue(values, null);

            //Assert

            Assert.AreEqual(expected, result);

        }

        #endregion

    }
}
