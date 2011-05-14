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
using Sitecore.Data.Items;
using System.Collections;

namespace Glass.Sitecore.Persistence.Data
{
    public class SitecoreQueryHandler : ISitecoreDataHandler
    {

        #region ISitecoreDataHandler Members

        public bool WillHandle(Glass.Sitecore.Persistence.Configuration.SitecoreProperty property, InstanceContext context)
        {
            return property.Attribute is SitecoreQueryAttribute;
        }

        public object GetValue(object target, global::Sitecore.Data.Items.Item item, Glass.Sitecore.Persistence.Configuration.SitecoreProperty property, InstanceContext context)
        {
            SitecoreQueryAttribute attr = property.Attribute as SitecoreQueryAttribute;

            string query = ParseQuery(attr.Query);


            if (property.Property.PropertyType.IsGenericType)
            {
                Type outerType = Utility.GetGenericOuter(property.Property.PropertyType);

                if (typeof(IEnumerable<>) == outerType)
                {
                    Type genericType = Utility.GetGenericArgument(property.Property.PropertyType);

                    Func<IEnumerable<Item>> getItems = new Func<IEnumerable<Item>>(() =>
                    {
                        return item.Database.SelectItems(query);
                    });

                    return context.CreateClasses(attr.IsLazy, genericType, getItems);
                }
                else throw new NotSupportedException("Generic type not supported {0}".Formatted(outerType.FullName));
            }
            else
            {
                var result = item.Database.SelectSingleItem(query);
                return context.CreateClass(attr.IsLazy, property.Property.PropertyType, result);
            }

        }

        private string ParseQuery(string query)
        {
            return query;
        }

        public void SetValue(object target, global::Sitecore.Data.Items.Item item, object value, Glass.Sitecore.Persistence.Configuration.SitecoreProperty property, InstanceContext context)
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
