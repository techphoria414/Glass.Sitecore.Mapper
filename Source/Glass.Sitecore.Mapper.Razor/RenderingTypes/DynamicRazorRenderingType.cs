using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Specialized;
using Sitecore.Web.UI;
using System.Web.UI;
using System.IO;
using RazorEngine;
using System.Web.UI.WebControls;
using Glass.Sitecore.Mapper.Razor.Web.Ui;

namespace Glass.Sitecore.Mapper.Razor.RenderingTypes
{
    public class DynamicRazorRenderingType : RenderingType
    {
        public override Control GetControl(NameValueCollection parameters, bool assert)
        {
            if(!parameters.AllKeys.Any(x=>x=="Name"))
                throw new RazorException("Parameters does not contain parameter 'Name'");
            
            
            string view = parameters["Name"];

            if(view.IsNullOrEmpty())
                throw new NullReferenceException("Parameter 'Name' is null or empty");

            IRazorControl control = global::Sitecore.Reflection.ReflectionUtil.CreateObject(typeof(DynamicControl)) as IRazorControl;
            control.View = view;
            
            return control as System.Web.UI.WebControls.WebControl;

        }
    }

}
