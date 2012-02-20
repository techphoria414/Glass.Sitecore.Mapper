using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Sitecore.Mapper.Razor.Web.Ui;

namespace Glass.Sitecore.Mapper.Razor.Web.Ui
{
    public class TemplateModel<T>
    {
        public AbstractRazorControl<T> Control
        {
            get;
            set;
        }
        public T Model { get; set; }
    }
}