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
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreIdDataHandler : ISitecoreDataHandler
    {

        public bool WillHandle(Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, IEnumerable<ISitecoreDataHandler> datas, Dictionary<Type, SitecoreClassConfig> classes)
        {
            return property.Attribute is SitecoreIdAttribute && property.Property.PropertyType == typeof(Guid);
        }

        public object GetValue(object container, global::Sitecore.Data.Items.Item item, SitecoreProperty property, InstanceContext context)
        {
            return item.ID.Guid;
        }

        public void SetValue(object container, global::Sitecore.Data.Items.Item item, object value, SitecoreProperty property, InstanceContext context)
        {
            throw new NotSupportedException("It isn't possible to write an ID field");
        }

        public bool CanSetValue
        {
            get { return false; }
        }
    }
}
