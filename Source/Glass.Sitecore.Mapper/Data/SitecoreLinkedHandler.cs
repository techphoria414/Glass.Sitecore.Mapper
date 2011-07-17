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
    public class SitecoreLinkedHandler : AbstractSitecoreDataHandler
    {
        public  bool IsLazy { get; set; }
        public bool InferType { get; set; }

        #region ISitecoreDataHandler Members

        public override bool WillHandle(Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, IEnumerable<AbstractSitecoreDataHandler> datas, Dictionary<Type, SitecoreClassConfig> classes)
        {
            if (!property.Property.PropertyType.IsGenericType) return false;

            Type type = Utility.GetGenericOuter(property.Property.PropertyType);
            return property.Attribute is SitecoreLinkedAttribute && typeof(IEnumerable<>) == type;
        }

        public override object GetValue(global::Sitecore.Data.Items.Item item,  ISitecoreService service)
        {
                int numChildren = item.Children.Count;
                Type genericType = Utility.GetGenericArgument(Property.PropertyType);


                Func<IEnumerable<Item>> getItems = new Func<IEnumerable<Item>>(() =>
                {
                    var itemLinks = item.Links.GetAllLinks();
                    return itemLinks.Select(x => x.GetTargetItem());
                });

                return service.CreateClasses(IsLazy, InferType,  genericType, getItems);
        }

        public override void SetValue(global::Sitecore.Data.Items.Item item, object value, ISitecoreService service)
        {
            throw new NotImplementedException();
        }

        public override bool CanSetValue{
            get
            {
                return false;
            }
        }


        internal override void ConfigureDataHandler(SitecoreProperty scProperty)
        {
            SitecoreLinkedAttribute attr = scProperty.Attribute as SitecoreLinkedAttribute;
            IsLazy = attr.IsLazy;
            InferType = attr.InferType;

            base.ConfigureDataHandler(scProperty);
        }


        #endregion
    }
}
