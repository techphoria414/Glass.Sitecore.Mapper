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

namespace Glass.Sitecore.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Used to populate the property with data from a Sitecore field
    /// </summary>
    public class SitecoreFieldAttribute: AbstractSitecorePropertyAttribute
    {
        public SitecoreFieldAttribute()
        {
            IsLazy = true;
        }
        public SitecoreFieldAttribute(string fieldName)
        {
            IsLazy = true;
            FieldName = fieldName;
        }
        public string FieldName { get; set; }
        
        /// <summary>
        /// Used on properties that load other classes loaded by Glass Sitecore Mapper. Indicates that the class should be 
        /// loaded lazyily. Be default this is true.
        /// </summary>
        public bool IsLazy { get; set; }

        /// <summary>
        /// Where a field data is manipulated before being returned setting this to true will return the fields raw value.
        /// For example if the property returns string and the data is coming from a Rich Text field it stops the contents
        /// going through the renderField pipeline. False by default.
        /// </summary>
        public bool ReturnRaw { get; set; }
    }
}
