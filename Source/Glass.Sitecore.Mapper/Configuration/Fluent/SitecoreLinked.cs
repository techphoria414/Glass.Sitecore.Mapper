using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Glass.Sitecore.Mapper.Configuration.Fluent
{
    /// <summary>
    /// Indicates that the property should pull from the item links list.
    /// </summary>
    public class SitecoreLinked<T> : AbstractSitecoreAttributeBuilder<T>
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
        /// <summary>
        /// Indicates if linked items should not be  loaded lazily. Default value is true. If set linked items will be loaded when the contain object is created.
        /// </summary>
        public SitecoreLinked<T> IsNotLazy()
        {
            _att.IsLazy = false;
            return this;
        }
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        public SitecoreLinked<T> InferType()
        {
            _att.InferType = false;
            return this;
        }
        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        public SitecoreLinked<T> Option(SitecoreLinkedOptions option)
        {
            _att.Option = option;
            return this;
        }

    }
}
