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
using Glass.Sitecore.Persistence.Configuration.Attributes;
using Glass.Sitecore.Persistence.Proxies;

namespace Glass.Sitecore.Persistence.Data
{
    public class SitecoreParentHandler : ISitecoreDataHandler
    {

        #region ISitecoreDataHandler Members

        public bool WillHandle(Glass.Sitecore.Persistence.Configuration.SitecoreProperty property, InstanceContext context)
        {
            return property.Attribute is SitecoreParentAttribute;
        }

        public object GetValue(object parent, global::Sitecore.Data.Items.Item item, Glass.Sitecore.Persistence.Configuration.SitecoreProperty property, InstanceContext context)
        {
            SitecoreParentAttribute attr = property.Attribute as SitecoreParentAttribute;
            return context.CreateClass(attr.IsLazy, property.Property.PropertyType, item.Parent);
        }

        public void SetValue(object parent, global::Sitecore.Data.Items.Item item, object value, Glass.Sitecore.Persistence.Configuration.SitecoreProperty property, InstanceContext context)
        {
            throw new NotImplementedException();
        }

        public bool CanSetValue
        {
            get { return false; }
        }

        #endregion
    }
}
