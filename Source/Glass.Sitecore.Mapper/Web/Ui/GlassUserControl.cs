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

        /// <summary>
        /// Model to render on the sublayout
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
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
