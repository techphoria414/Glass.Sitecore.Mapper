using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections.Specialized;

namespace Glass.Sitecore.Mapper.Tests
{
    [TestFixture]
    public class HtmlFixture
    {

        #region RenderImage

        public void RenderImage_RendersImageWithAttributes()
        {
            //Assign
            FieldTypes.Image img = new FieldTypes.Image();
            img.Alt = "Some alt test";
            img.Src = "/cats.jpg";
            img.Class = "classy";

            NameValueCollection attrs = new NameValueCollection();
            attrs.Add("style", "allStyle");
            
            //Act
            var result = Html.RenderImage(img, attrs);

            //Assert
            Assert.AreEqual("<img src='/cats.jpg' alt='Some alt test' class='classy' style='allStyle' />", result);

        }

        #endregion

        #region RenderLink

        [Test]
        public void RenderLink_RendersAValidaLink()
        {
            //Assign

            FieldTypes.Link link = new FieldTypes.Link();
            link.Class = "classy";
            link.Anchor = "landSighted";
            link.Target = "xMarksTheSpot";
            link.Text = "Click here";
            link.Title = "You should click here";
            link.Url = "/yourpage";

            NameValueCollection attrs = new NameValueCollection();
            attrs.Add("style", "got some");

            //Act
            var result = Html.RenderLink(link, attrs);

            //Assert
            Assert.AreEqual("<a href='/yourpage#landSighted' title='You should click here' target='xMarksTheSpot' class='classy' style='got some' >Click here</a>", result);


        }
        #endregion

    }
}
