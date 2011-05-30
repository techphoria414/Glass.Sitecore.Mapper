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
using Glass.Sitecore.Mapper.FieldTypes;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreFieldTriStateHandler :AbstractSitecoreField
    {
        public override object GetFieldValue(string fieldValue, object parent, Item item, InstanceContext context)
        {
            switch (fieldValue)
            {
                case "1":
                    return TriState.Yes;
                case "0":
                    return TriState.No;
                default:
                    return TriState.Default;
            }
        }

        public override string SetFieldValue( object value, InstanceContext context)
        {
            if (value is TriState)
            {
                TriState state = (TriState)value;
                switch (state)
                {
                    case TriState.Default:
                        return "";
                    case TriState.No:
                        return "0";
                    case TriState.Yes:
                        return "1";
                    default:
                        return "";
                }
            }
            else throw new MapperException("Value is not of type {0}".Formatted(typeof(TriState).FullName));
        }

        public override Type TypeHandled
        {
            get { return typeof(FieldTypes.TriState); }
        }
    }
}
