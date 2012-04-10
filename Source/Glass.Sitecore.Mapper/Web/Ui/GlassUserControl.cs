using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Linq.Expressions;

namespace Glass.Sitecore.Mapper.Web.Ui
{
    public class GlassUserControl<T> : AbstractGlassUserControl where T:class
    {
        protected override void OnLoad(EventArgs e)
        {
            Model = SitecoreContext.CreateClass<T>(false, false, LayoutItem);
            base.OnLoad(e);
        }
        
        public T Model { get; set; }

        public string Editable(Expression<Func<T, object>> field)
        {

            if (field == null) throw new NullReferenceException("No field set");

            if (Model == null) throw new NullReferenceException("No model set");

            try
            {
                var result = GlassHtml.Editable<T>(field, Model);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

       
    }
}
