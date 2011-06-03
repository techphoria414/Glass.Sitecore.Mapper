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
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreFieldEnumHandler:AbstractSitecoreField
    {
        public override object GetFieldValue(string fieldValue, global::Sitecore.Data.Items.Item item, ISitecoreService service)
        {
            Type enumType = Property.PropertyType;

            int intValue;
            if (int.TryParse(fieldValue, out intValue))
            {
                return Enum.ToObject(enumType, 0);
            }
            else
            {
                if (Enum.IsDefined(enumType, fieldValue))
                    return Enum.Parse(enumType, fieldValue, true);
                else
                    throw new MapperException("Can not convert value {0} to enum type {1}".Formatted(fieldValue, enumType.FullName));
            }

        }

        public override string SetFieldValue(object value,  ISitecoreService service)
        {
            return Enum.GetName(Property.PropertyType, value);
        }

        public override bool WillHandle(Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, IEnumerable<AbstractSitecoreDataHandler> datas, Dictionary<Type, SitecoreClassConfig> classes)
        {
            return property.Property.PropertyType.IsEnum && property.Property.PropertyType != typeof(FieldTypes.TriState);
            
        }
        public override Type TypeHandled
        {
            get { throw new NotImplementedException(); }
        }
    }
}
