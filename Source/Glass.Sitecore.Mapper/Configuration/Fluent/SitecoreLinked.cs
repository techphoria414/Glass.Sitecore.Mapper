using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Glass.Sitecore.Mapper.Configuration.Fluent
{
    class SitecoreLinked<T> : AbstractSitecoreAttributeBuilder<T>
    {
        Configuration.Attributes.SitecoreLinkedAttribute _att;

        public SitecoreLinked(Expression<Func<T, object>> ex)
            : base(ex)
        {
            _att = new Configuration.Attributes.SitecoreLinkedAttribute(); 
        }

        internal override Attributes.AbstractSitecorePropertyAttribute Attribute
        {
            get { return _att; }
        }

        public SitecoreLinked<T> IsNotLazy()
        {
            _att.IsLazy = false;
            return this;
        }
        public SitecoreLinked<T> InferType()
        {
            _att.InferType = false;
            return this;
        }

        public SitecoreLinked<T> Option(SitecoreLinkedOptions option)
        {
            _att.Option = option;
            return this;
        }

    }
}
