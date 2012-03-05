﻿using System;
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
        [Test]
        public void RenderImage_RendersImageWithAttributes()
        {
            GlassHtml html = new GlassHtml("master");

            //Assign
            FieldTypes.Image img = new FieldTypes.Image();
            img.Alt = "Some alt test";
            img.Src = "/cats.jpg";
            img.Class = "classy";

            NameValueCollection attrs = new NameValueCollection();
            attrs.Add("style", "allStyle");
            
            //Act
            var result = html.RenderImage(img, attrs);

            //Assert
            Assert.AreEqual("<img src='/cats.jpg' style='allStyle' class='classy' alt='Some alt test' />", result);

        }

        #endregion

        #region RenderLink

        [Test]
        public void RenderLink_RendersAValidaLink()
        {

            //Assign
            GlassHtml html = new GlassHtml("master");

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
            var result = html.RenderLink(link, attrs);

            //Assert
            Assert.AreEqual("<a href='/yourpage#landSighted' title='You should click here' target='xMarksTheSpot' class='classy' style='got some' class='classy' target='xMarksTheSpot' title='You should click here' >Click here</a>", result);


        }
        #endregion

    }
}
