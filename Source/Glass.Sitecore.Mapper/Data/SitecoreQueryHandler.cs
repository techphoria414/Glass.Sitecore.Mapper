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
    public class SitecoreQueryHandler : AbstractSitecoreDataHandler
    {

        List<ISitecoreQueryParameter> _parameters;

        public bool IsLazy { get; set; }

        public bool IsRelative { get; set; }

        public string Query { get; set; }

        public bool InferType { get; set; }

        public SitecoreQueryHandler(IEnumerable<ISitecoreQueryParameter> parameters)
        {

            _parameters = new List<ISitecoreQueryParameter>();
            if(parameters !=null)
                _parameters.AddRange(parameters);

            //default parameters
            _parameters.Add(new ItemDateNowParameter());
            _parameters.Add(new ItemIdParameter());
            _parameters.Add(new ItemPathParameter());
            _parameters.Add(new ItemIdNoBracketsParameter());

        }
        public SitecoreQueryHandler():this(null)
        {

        }

        public override bool WillHandle(Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, IEnumerable<AbstractSitecoreDataHandler> datas, Dictionary<Type, SitecoreClassConfig> classes)
        {
            return property.Attribute is SitecoreQueryAttribute;
        }

        public override object GetValue(global::Sitecore.Data.Items.Item item, ISitecoreService service)
        {

            string query = ParseQuery(Query, item);

            if (Property.PropertyType.IsGenericType)
            {
                Type outerType = Utility.GetGenericOuter(Property.PropertyType);

                if (typeof(IEnumerable<>) == outerType)
                {
                    Type genericType = Utility.GetGenericArgument(Property.PropertyType);
                    
                    Func<IEnumerable<Item>> getItems = null;
                    if (IsRelative)
                    {
                        getItems = new Func<IEnumerable<Item>>(() =>
                        {
                            return item.Axes.SelectItems(query);
                        });
                    }
                    else
                    {
                        getItems = new Func<IEnumerable<Item>>(() =>
                        {
                            return item.Database.SelectItems(query);
                        });
                    }

                    return service.CreateClasses(IsLazy, InferType, genericType, getItems);
                }
                else throw new NotSupportedException("Generic type not supported {0}. Must be IEnumerable<>.".Formatted(outerType.FullName));
            }
            else
            {
                Item result = null;
                if (IsRelative)
                {
                    result = item.Axes.SelectSingleItem(query);
                }
                else
                {
                    result = item.Database.SelectSingleItem(query);
                }
                return service.CreateClass(IsLazy, Property.PropertyType, result);
            }

        }

        public string ParseQuery(string query, Item item)
        {
            StringBuilder sb = new StringBuilder(query);
            foreach (var param in _parameters)
            {
                sb.Replace("{"+param.Name+"}", param.GetValue(item));
            }
            return sb.ToString();
        }

        public override void SetValue(global::Sitecore.Data.Items.Item item, object value, ISitecoreService service)
        {
            throw new NotImplementedException();
        }

        public override bool CanSetValue
        {
            get
            {
                return false;
            }
        }

        internal override void ConfigureDataHandler(SitecoreProperty scProperty)
        {
            SitecoreQueryAttribute attr = scProperty.Attribute as SitecoreQueryAttribute;
            IsLazy = attr.IsLazy;
            IsRelative = attr.IsRelative;
            Query = attr.Query;
            InferType = attr.InferType;

            base.ConfigureDataHandler(scProperty);
        }

    }
}
