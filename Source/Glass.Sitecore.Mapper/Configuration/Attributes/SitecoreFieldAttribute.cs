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
using Glass.Sitecore.Mapper.Data;

namespace Glass.Sitecore.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Used to populate the property with data from a Sitecore field
    /// </summary>
    public class SitecoreFieldAttribute: AbstractSitecorePropertyAttribute
    {
        /// <summary>
        /// Indicates that the property should pull data from a Sitecore field.
        /// </summary>
        public SitecoreFieldAttribute()
        {
            Setting = SitecoreFieldSettings.Default;
        }
        /// <summary>
        /// Indicates that the property should pull data from a Sitecore field.
        /// </summary>
        /// <param name="fieldName">The name of the field  to use if it is different to the property name</param>
        public SitecoreFieldAttribute(string fieldName):this()
        {
            FieldName = fieldName;
        }
        /// <summary>
        /// The name of the field  to use if it is different to the property name
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Options to override the behaviour of certain fields.
        /// </summary>
        public SitecoreFieldSettings Setting { get; set; }

        /// <summary>
        /// This is the sub handler used and set by the IEnumerableHandler
        /// </summary>
        internal AbstractSitecoreField EnumSubHandler { get; set; }

        /// <summary>
        /// When true the field will not be save back to Sitecore 
        /// </summary>
        public bool ReadOnly { get; set; }
    }
}
