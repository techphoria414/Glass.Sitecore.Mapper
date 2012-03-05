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
    /// <summary>
    /// Used to pull in a Sitecore item 
    /// </summary>
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
        /// <summary>
        /// The path to the item. If both a path and ID are specified the ID will be used.
        /// </summary>
        public SitecoreItem<T> Path(string path)
        {
            _att.Path = path;
            return this;

        }
        /// <summary>
        /// The Id of the item. 
        /// </summary>
        public SitecoreItem<T> Id(Guid id)
        {
            _att.Id = id.ToString();
            return this;
        }
        /// <summary>
        /// Indicates that the item should not be loaded lazily. If set the item will be loaded when the containing object is created.
        /// </summary>
        public SitecoreItem<T> IsNotLazy()
        {
            _att.IsLazy = false;
            return this;
        }

    }
}
