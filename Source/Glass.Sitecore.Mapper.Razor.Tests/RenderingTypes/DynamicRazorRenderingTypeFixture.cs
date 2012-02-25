using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Razor.RenderingTypes;
using NUnit.Framework;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Glass.Sitecore.Mapper.Razor.Tests.RenderingTypes
{
    [TestFixture]
    public class DynamicRazorRenderingTypeFixture
    {
        DynamicRazorRenderingType _renderingType;

        [SetUp]
        public void Setup()
        {
            _renderingType = new DynamicRazorRenderingType();


        }

        #region Method - GetControl

        [Test]
        public void GetControl_CorrectlySetsView()
        {
            //Assign
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Name", "myView");

            //Act
            var result = _renderingType.GetControl(parameters, true);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result as WebControl);
            Assert.IsNotNull(result as IRazorControl);
            Assert.AreEqual("myView", ((IRazorControl)result).View);
        }

        [Test]
        [ExpectedException(typeof(RazorException))]
        public void GetControl_NoNameParameter_ThrowsException()
        {
            //Assign
            NameValueCollection parameters = new NameValueCollection();

            //Act
            var result = _renderingType.GetControl(parameters, true);

            //Assert
            //exception should occur
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetControl_NameParameterIsNullOrEmpty_ThrowsException()
        {
            //Assign
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Name", string.Empty);

            //Act
            var result = _renderingType.GetControl(parameters, true);

            //Assert
            //exception should occur
        }

        #endregion

    }
}
