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
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Data
{
    public abstract class AbstractSitecoreField : ISitecoreDataHandler
    {



        public abstract object GetFieldValue(string fieldValue, object parent, Item item, SitecoreProperty property, InstanceContext context);

       
        public abstract string SetFieldValue(Type returnType, object value, InstanceContext context);

        #region ISitecoreDataHandler Members

        public virtual void SetValue(object parent, Item item, object value, SitecoreProperty property, InstanceContext context)
        {

            string fieldName = GetFieldName(property); 

            string fieldValue = SetFieldValue(property.Property.PropertyType, value, context);
            item[fieldName] = fieldValue;
        }

        public virtual object GetValue(object parent, Item item, SitecoreProperty property, InstanceContext context)
        {

            string fieldName = GetFieldName(property);
            string fieldValue = item[fieldName];
            return GetFieldValue(fieldValue, parent, item,property, context);
        }

        public virtual bool WillHandle(Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
        {
            return property.Attribute is SitecoreFieldAttribute && property.Property.PropertyType == TypeHandled;

        }

        public bool CanSetValue
        {
            get { return true; }
        }

        #endregion

        public abstract Type TypeHandled { get; }

        protected string GetFieldName(SitecoreProperty property)
        {

            SitecoreFieldAttribute attr = property.Attribute as SitecoreFieldAttribute;
            if (attr != null && !attr.FieldName.IsNullOrEmpty()) return attr.FieldName;
            else return property.Property.Name;
        }


     

    }
}
