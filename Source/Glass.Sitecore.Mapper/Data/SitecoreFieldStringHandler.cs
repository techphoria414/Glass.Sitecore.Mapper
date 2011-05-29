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
using Sitecore.Web.UI.WebControls;
using Glass.Sitecore.Mapper.Configuration;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreFieldStringHandler: AbstractSitecoreField
    {

        public override object GetValue(object parent, global::Sitecore.Data.Items.Item item, Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
        {
            SitecoreFieldAttribute attr = property.Attribute as SitecoreFieldAttribute;

            string fieldName = GetFieldName(property);

            if (item.Fields[fieldName] != null && item.Fields[fieldName].Type.StartsWith("Rich Text") && attr.Setting != SitecoreFieldSettings.RichTextRaw)
            {
                FieldRenderer renderer = new FieldRenderer();
                renderer.Item = item;
                renderer.FieldName = fieldName;
                renderer.Parameters = "";
                return renderer.Render();
            }
            else return item[fieldName];
        }



        public override void SetValue(object parent, Item item, object value, SitecoreProperty property, InstanceContext context)
        {
            SitecoreFieldAttribute attr = property.Attribute as SitecoreFieldAttribute;
            
            string fieldName = GetFieldName(property);

            if (item.Fields[fieldName] != null && item.Fields[fieldName].Type.StartsWith("Rich Text") && attr.Setting != SitecoreFieldSettings.RichTextRaw)
            {
                throw new NotSupportedException("It is not possible to save data from a rich text field when the data isn't raw."
                    + "Set the SitecoreFieldAttribute setting property to SitecoreFieldSettings.RichTextRaw for property {0} on type {1}".Formatted(property.Property.Name, property.Property.ReflectedType.FullName));
            }
            else
            {
                string fieldValue = SetFieldValue(value, property, context);
                item[fieldName] = fieldValue;
            }
        }

        public override string SetFieldValue(object value, SitecoreProperty property, InstanceContext context)
        {
            return value.ToString();
        }
        public override object GetFieldValue(string fieldValue, object parent, Item item, SitecoreProperty property, InstanceContext context)
        {
            throw new NotImplementedException();
        }
        public override Type TypeHandled
        {
            get { return typeof(System.String); }
        }
    }
}
