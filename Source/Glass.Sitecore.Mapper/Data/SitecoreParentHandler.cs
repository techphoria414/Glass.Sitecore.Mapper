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
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreParentHandler : AbstractSitecoreDataHandler
    {

        public bool IsLazy { get; private set; }
        public bool InferType { get; private set; }
     

        public override bool WillHandle(Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, IEnumerable<AbstractSitecoreDataHandler> datas, Dictionary<Type, SitecoreClassConfig> classes)
        {
            return property.Attribute is SitecoreParentAttribute;
        }

        public override object GetValue(global::Sitecore.Data.Items.Item item, ISitecoreService service)
        {
            return service.CreateClass(this.IsLazy, this.InferType, Property.PropertyType, item.Parent);
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

        public override void ConfigureDataHandler(SitecoreProperty scProperty)
        {
            SitecoreParentAttribute attr = scProperty.Attribute as SitecoreParentAttribute;
            this.IsLazy = attr.IsLazy;
            this.InferType = attr.InferType;

            base.ConfigureDataHandler(scProperty);
        }
    }
}
