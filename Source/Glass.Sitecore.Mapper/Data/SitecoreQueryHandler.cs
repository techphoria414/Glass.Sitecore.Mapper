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
using Sitecore.Data.Items;
using System.Collections;
using Glass.Sitecore.Mapper.Data.QueryParameters;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreQueryHandler : ISitecoreDataHandler
    {

        List<ISitecoreQueryParameter> _parameters;

        public SitecoreQueryHandler(IEnumerable<ISitecoreQueryParameter> parameters)
        {

            _parameters = new List<ISitecoreQueryParameter>();
            if(parameters !=null)
                _parameters.AddRange(parameters);

            //default parameters
            _parameters.Add(new ItemDateNowParameter());
            _parameters.Add(new ItemIdParameter());
            _parameters.Add(new ItemPathParameter());

        }
        public SitecoreQueryHandler():this(null)
        {

        }

        #region ISitecoreDataHandler Members

        public bool WillHandle(Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
        {
            return property.Attribute is SitecoreQueryAttribute;
        }

        public object GetValue(object target, global::Sitecore.Data.Items.Item item, Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
        {
            SitecoreQueryAttribute attr = property.Attribute as SitecoreQueryAttribute;

            string query = ParseQuery(attr.Query, item, property);


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

        public string ParseQuery(string query, Item item, SitecoreProperty property)
        {
            StringBuilder sb = new StringBuilder(query);
            foreach (var param in _parameters)
            {
                sb.Replace("{"+param.Name+"}", param.GetValue(item, property));
            }
            return sb.ToString();
        }

        public void SetValue(object target, global::Sitecore.Data.Items.Item item, object value, Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
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
