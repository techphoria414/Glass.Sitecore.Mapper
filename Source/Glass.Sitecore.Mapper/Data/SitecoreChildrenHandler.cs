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
using Glass.Sitecore.Mapper.Proxies;
using System.Collections;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreChildrenHandler : ISitecoreDataHandler
    {
        #region ISitecoreDataHandler Members

        public bool WillHandle(Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, IEnumerable<ISitecoreDataHandler> datas, Dictionary<Type, SitecoreClassConfig> classes)
        {
            if (!property.Property.PropertyType.IsGenericType) return false;

            Type type = Utility.GetGenericOuter(property.Property.PropertyType);
            return property.Attribute is SitecoreChildrenAttribute && typeof(IEnumerable<>) == type;
        }

        public object GetValue(object target, global::Sitecore.Data.Items.Item item, Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
        {
                int numChildren = item.Children.Count;
                Type genericType = Utility.GetGenericArgument(property.Property.PropertyType);

                SitecoreChildrenAttribute attr = property.Attribute as SitecoreChildrenAttribute;

                Func<IEnumerable<Item>> getItems = new Func<IEnumerable<Item>>(() =>
                {
                    return item.Children.Cast<Item>();
                });

                return context.CreateClasses(attr.IsLazy, genericType, getItems);
        }

        public void SetValue(object target, global::Sitecore.Data.Items.Item item, object value, Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
        {
            throw new NotImplementedException();
        }

        public bool CanSetValue(SitecoreProperty property)
        {
            return false; 
        }

        #endregion
    }
}
