using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Web.UI;
using System.Reflection;
using System.ComponentModel;
using Glass.Sitecore.Mapper.Razor.Web.Ui;

namespace Glass.Sitecore.Mapper.Razor
{
    public class ContextRazorView : WebControl
    {

        public string TypeName
        {
            get;
            set;
        }

        public string AssemblyName { get; set; }


        protected override void CreateChildControls()
        {

            Type type =  Type.GetType(TypeName);

            Type razorControlType = typeof(AbstractRazorControl<>);
            Type finalControlType = razorControlType.MakeGenericType(type);

            WebControl finalControl =Activator.CreateInstance(finalControlType) as WebControl;



            ISitecoreContext _context = new SitecoreContext();
            var model = _context.GetCurrentItem(type);

            TypeDescriptor.GetProperties(finalControlType).Find("Model", false).SetValue(finalControl, model);

            this.Controls.Add(finalControl);

            base.CreateChildControls();
        }

        protected override void DoRender(System.Web.UI.HtmlTextWriter output)
        {
           //nothing happens here :-)
        }
    }
}
