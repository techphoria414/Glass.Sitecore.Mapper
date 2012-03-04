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
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper.Razor.Web.Ui
{
    public abstract class AbstractRazorControl<T> : WebControl, IRazorControl, global::Sitecore.Layouts.IExpandable
    {
        private readonly object _key = new object();
        private readonly object _viewKey = new object();

        protected static Dictionary<string, string> ViewCache { get; private set; }

        Func<string, string> _viewLoader = viewPath =>
        {
            var fullPath = System.Web.HttpContext.Current.Server.MapPath(viewPath);
            //TODO: more error catching
            return  File.ReadAllText(fullPath);

        };

        public AbstractRazorControl()
        {
            if (ViewCache == null)
            {
                lock (_key)
                {
                    if (ViewCache == null)
                    {
                        ViewCache = new Dictionary<string, string>();
                    }
                }
            }
        }

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

        public T Model
        {
            get;
            private set;
        }

        public NameValueCollection Form
        {
            get
            {
                return this.Context.Request.Form;
            }
        }

        /// <summary>
        /// Put your logic to create your model here
        /// </summary>
        /// <returns></returns>
        public abstract T GetModel();

        /// <summary>
        /// Returns either the data source item or if no data source is specified the context item
        /// </summary>
        /// <returns></returns>
        protected Item GetDataSourceOrContext()
        {
            return this.DataSource.IsNullOrEmpty() ? global::Sitecore.Context.Item :
                global::Sitecore.Context.Database.GetItem(this.DataSource);
        }
    
        
        protected override void DoRender(HtmlTextWriter output)
        {

            Model = GetModel();

            var viewContents = GetRazorView(View, _viewLoader);

            try
            {
                    TemplateModel<T> tModel = new TemplateModel<T>();
                    tModel.Control = this;
                    tModel.Model = Model;

                    string content = global::RazorEngine.Razor.Parse<TemplateModel<T>>(viewContents, tModel);

                    output.Write(content);
            }
            catch (RazorEngine.Templating.TemplateCompilationException ex)
            {
                throw new Exception(ex.Errors.First().ErrorText, ex);
            }
        }


        public string GetRazorView(string viewPath, Func<string, string> viewLoader)
        {
            string finalview = null;

            if (ViewCache.ContainsKey(viewPath))
                finalview = ViewCache[viewPath];
            else
            {
                finalview = viewLoader(viewPath);
                if (finalview == null) throw new NullReferenceException("Could not find file {0}.".Formatted(viewPath));

                //we added to the collection making sure no one else added it before
                if (!ViewCache.ContainsKey(viewPath))
                {
                    lock (_viewKey)
                    {
                        if (!ViewCache.ContainsKey(viewPath))
                        {
                            ViewCache.Add(viewPath, finalview);
                        }
                    }
                }
            }

            return finalview;
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
