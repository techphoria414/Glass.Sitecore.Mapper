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
using Sitecore.Data.Items;
using Glass.Sitecore.Persistence.Configuration;
using Glass.Sitecore.Persistence.Configuration.Attributes;

namespace Glass.Sitecore.Persistence.Data
{
    public interface ISitecoreDataHandler
    {
        bool WillHandle(SitecoreProperty property, InstanceContext context);

        /// <summary>
        /// Reads an item from the Sitecore item and returns it as the requested return type
        /// </summary>
        /// <param name="target">The object that the value will be set on</param>
        /// <param name="item">The Sitecore item to work on</param>
        /// <param name="property">Information about the clas property on the target being set</param>
        /// <param name="context">The context used to load the target object</param>
        /// <returns></returns>
        object GetValue(object target, Item item, SitecoreProperty property, InstanceContext context);

        /// <summary>
        /// Sets a value on the Sitecore item
        /// </summary>
        /// <param name="target">The object that the value will be read from</param>
        /// <param name="item">The Sitecore item to write the value to</param>
        /// <param name="value">The value to be set</param>
        /// <param name="property">Information about the class property on the target being read</param>        
        /// <param name="context">The context used to load the target object</param>
        void SetValue(object target, Item item, object value, SitecoreProperty property, InstanceContext context);

        /// <summary>
        /// Indicates the data handler can be used to set a value on a Sitecore item.
        /// </summary>
        bool CanSetValue { get; }
    }
}
