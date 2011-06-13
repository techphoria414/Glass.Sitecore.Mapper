/*
   Copyright 2011 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Glass.Sitecore.Mapper.Configuration.Fluent
{
    public class SitecoreParent<T> : AbstractSitecoreAttributeBuilder<T>
    {

        Configuration.Attributes.SitecoreParentAttribute _attr;

        public SitecoreParent(Expression<Func<T, object>> ex)
            : base(ex)
        {
            _attr = new Configuration.Attributes.SitecoreParentAttribute();
        }

        public SitecoreParent<T> IsNotLazy()
        {
            _attr.IsLazy = false;
            return this;
        }


        internal override Glass.Sitecore.Mapper.Configuration.Attributes.AbstractSitecorePropertyAttribute Attribute
        {
            get { return _attr; }
        }

    }
}
