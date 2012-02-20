using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RazorEngine;
using System.Reflection;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using Sitecore.Web.UI;
using System.Web.UI;

namespace Glass.Sitecore.Mapper.Razor.Web.Ui
{
    public abstract class AbstractRazorControl<T> : WebControl, IRazorControl, global::Sitecore.Layouts.IExpandable
    {

        public IEnumerable<string> Placeholders
        {
            get;
            set;
        }

        public string View
        {
            get;
            set;
        }
        public string AssemblyName { get; set; }

        public T Model
        {
            get;
            set;
        }

        public NameValueCollection Form
        {
            get
            {
                return this.Context.Request.Form;
            }
        }

    
        
        protected override void DoRender(HtmlTextWriter output)
        {
        
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(View);
            TextReader reader = new StringReader(new StreamReader(stream).ReadToEnd());
            //global::RazorEngine.Razor.de.Namespaces.Add("Glass.Sitecore.Mapper.FieldTypes");

            if (Model == null) throw new NullReferenceException("The model for the razor view is empty");

            try
            {
                using (new global::Sitecore.SecurityModel.SecurityDisabler())
                {

                    var rzControl = this;

                    TemplateModel<T> tModel = new TemplateModel<T>();
                    tModel.Control = this;
                    tModel.Model = Model;

                  

                    string content = global::RazorEngine.Razor.Parse<TemplateModel<T>>(reader.ReadToEnd(), tModel);

                    output.Write(content);
                }
               // base.RenderContents(writer);
            }
            catch (RazorEngine.Templating.TemplateCompilationException ex)
            {
                throw new Exception(ex.Errors.First().ErrorText, ex);
            }
        }

        public void Expand()
        {
            if (Placeholders != null)
            {
                foreach (var placeHolderName in Placeholders)
                {
                    global::Sitecore.Web.UI.WebControls.Placeholder holder = new global::Sitecore.Web.UI.WebControls.Placeholder();
                    holder.Key = placeHolderName.ToLower();
                    this.Controls.Add(holder);
                }
            }

            this.Controls.Cast<Control>().Where(x => x is global::Sitecore.Layouts.IExpandable)
                .Cast<global::Sitecore.Layouts.IExpandable>().ToList().ForEach(x => x.Expand());
        }
    }
   
}
