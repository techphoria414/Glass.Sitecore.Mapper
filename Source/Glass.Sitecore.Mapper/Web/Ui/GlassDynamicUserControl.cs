using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Dynamic;
using Sitecore.Web.UI;
using System.Web.UI;
using Sitecore.Web.UI.WebControls;

namespace Glass.Sitecore.Mapper.Web.Ui
{
    public class GlassDynamicUserControl : AbstractGlassUserControl
    {

        /// <summary>
        /// Model to render on the sublayout
        /// </summary>
        public dynamic Model { get; private set; }


        protected override void OnLoad(EventArgs e)
        {
            Model = new DynamicItem(LayoutItem);
            base.OnLoad(e);
        }
    }
}
