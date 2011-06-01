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
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreFieldDateTimeHandler : AbstractSitecoreField
    {
        public override object GetFieldValue(string fieldValue, Item item, InstanceContext context)
        {
            return global::Sitecore.DateUtil.IsoDateToDateTime(fieldValue);
        }

        public override string SetFieldValue(object value, InstanceContext context)
        {
            DateTime date = (DateTime)value;
            return global::Sitecore.DateUtil.ToIsoDate(date);
        }

        public override Type TypeHandled
        {
            get { return typeof(DateTime); }
        }
    }
}
