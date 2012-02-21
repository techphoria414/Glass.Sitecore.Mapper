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
    public class BehindRazorRenderingType : AbstractCachingRenderingType
    {

        Func<string, Type> _typeLoader = typeName => Type.GetType(typeName);


        public override Control GetControl(NameValueCollection parameters, bool assert)
        {
            string view = parameters["Name"];
            string type = parameters["Type"];
            string assembly = parameters["Assembly"];

            string typeName = "{0}, {1}".Formatted(type, assembly);

            Type codeBehindType = GetControlType(typeName, _typeLoader);

            IRazorControl control = global::Sitecore.Reflection.ReflectionUtil.CreateObject(codeBehindType) as IRazorControl;
            control.View = view;
            return control as System.Web.UI.WebControls.WebControl;

        }
    }

}
