using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper;
using System.Web.UI;
using System.IO;

namespace Glass.Sitecore.Mapper.Razor.Web.Ui
{
    public class TemplateBase<T>:RazorEngine.Templating.TemplateBase<TemplateModel<T>>
    {
        GlassHtml _html;
        public TemplateBase()
        {
            _html = new GlassHtml(global::Sitecore.Context.Database);
        }
      
        public T Model
        {
            get
            {
                return base.Model.Model;
            }
            set
            {
                base.Model.Model = value;
            }
        }

        public GlassHtml Glass { get { return _html; } }

        public string RenderHolder(string key)
        {
            key = key.ToLower();
            var placeHolder = base.Model.Control.Controls.Cast<Control>()
                .Where(x => x is global::Sitecore.Web.UI.WebControls.Placeholder)
                .Cast<global::Sitecore.Web.UI.WebControls.Placeholder>()
                .FirstOrDefault(x => x.Key == key);

            if (placeHolder == null)
                return "No placeholder with key: {0}".Formatted(key);
            else
            {
                StringBuilder sb = new StringBuilder();
                placeHolder.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                return sb.ToString();
            }
        }
    }
}
