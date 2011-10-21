using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Glass.Sitecore.Mapper.Configuration.Fluent
{
    public class SitecoreItem<T> : AbstractSitecoreAttributeBuilder<T>
    {
        Configuration.Attributes.SitecoreItemAttribute _att;

        public SitecoreItem(Expression<Func<T, object>> ex)
            : base(ex)
        {
            _att = new Configuration.Attributes.SitecoreItemAttribute(); 
        }

        internal override Attributes.AbstractSitecorePropertyAttribute Attribute
        {
            get { return _att; }
        }

        public SitecoreItem<T> Path(string path)
        {
            _att.Path = path;
            return this;

        }
        public SitecoreItem<T> Id(Guid id)
        {
            _att.Id = id.ToString();
            return this;
        }
        public SitecoreItem<T> IsNotLazy()
        {
            _att.IsLazy = false;
            return this;
        }

    }
}
