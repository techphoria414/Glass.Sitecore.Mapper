using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Sitecore.Data.Items;
using Sitecore.Web.UI;

namespace Glass.Sitecore.Mapper.Web.Ui
{
    public class AbstractGlassUserControl : UserControl
    {
        ISitecoreContext _sitecoreContext;
        GlassHtml _glassHtml;

        public ISitecoreContext SitecoreContext
        {
            get
            {
                return _sitecoreContext ?? (_sitecoreContext = new SitecoreContext());
            }
            set
            {
                _sitecoreContext = value;
            }
        }

        protected virtual GlassHtml GlassHtml
        {
            get
            {
                return _glassHtml ?? (_glassHtml = new GlassHtml(SitecoreContext));
            }
        }

        public string DataSource
        {
            get
            {
                WebControl parent = Parent as WebControl;
                if (parent == null)
                    return string.Empty;
                return parent.DataSource;
            }
        }
        /// <summary>
        /// Returns either the item specified by the DataSource or the current context item
        /// </summary>
        public Item LayoutItem
        {
            get
            {
                if (DataSource.IsNullOrEmpty())
                    return global::Sitecore.Context.Item;
                else
                    return global::Sitecore.Context.Database.GetItem(DataSource);

            }
        }
    }
}
