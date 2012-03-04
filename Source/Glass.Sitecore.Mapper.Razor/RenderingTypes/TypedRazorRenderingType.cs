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
    public class TypedRazorRenderingType : AbstractCachingRenderingType
    {

        Func<string, Type> _typeLoader = typeName =>
        {
            var strongGeneric = typeof(StrongControl<>);
            var modelType = Type.GetType(typeName);
            return strongGeneric.MakeGenericType(modelType);
        };

      


        public override Control GetControl(NameValueCollection parameters, bool assert)
        {
            string view = parameters["Name"];
            string type = parameters["Type"];
            string assembly = parameters["assembly"];

            string typeName = "{0}, {1}".Formatted(type, assembly);

            var viewType = GetControlType(typeName, _typeLoader);

            IRazorControl control = global::Sitecore.Reflection.ReflectionUtil.CreateObject(viewType) as IRazorControl;
            control.View = view;
            return control as System.Web.UI.WebControls.WebControl;
        }
    }

}
