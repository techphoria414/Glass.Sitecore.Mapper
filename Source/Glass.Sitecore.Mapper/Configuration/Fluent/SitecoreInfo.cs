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
    /// Used to map item information to a class property
    /// </summary>
    public class SitecoreInfo<T>: AbstractSitecoreAttributeBuilder<T>

    {
        Configuration.Attributes.SitecoreInfoAttribute _attr;

        public SitecoreInfo(Expression<Func<T, object>> ex):base(ex){
            _attr = new Configuration.Attributes.SitecoreInfoAttribute();
        }

        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        public SitecoreInfo<T> InfoType(SitecoreInfoType type)
        {
            _attr.Type = type;
            return this;
        }
        /// <summary>
        /// UrlOptions, use in conjunction with SitecoreInfoType.Url
        /// </summary>
        public SitecoreInfo<T> UrlOptions(SitecoreInfoUrlOptions option)
        {
            _attr.UrlOptions = option;
            return this;
        }

        internal override Glass.Sitecore.Mapper.Configuration.Attributes.AbstractSitecorePropertyAttribute Attribute
        {
            get { return _attr; }
        }

    }
}
