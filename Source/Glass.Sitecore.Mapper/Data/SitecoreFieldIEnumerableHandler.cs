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
using Glass.Sitecore.Mapper.Configuration;
using System.Reflection;
using System.Collections;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreFieldIEnumerableHandler : AbstractSitecoreField
    {
        protected AbstractSitecoreField EnumSubHandler { get; set; }

        public override object GetFieldValue(string fieldValue, object parent, Item item, InstanceContext context)
        {
            Type type = Property.PropertyType;
            //Get generic type
            Type pType = Utility.GetGenericArgument(type);

            if (EnumSubHandler == null) EnumSubHandler = GetSubHandler(pType, context);
            
            //The enumerator only works with piped lists
            IEnumerable<string> parts = fieldValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            //replace any pipe encoding with an actual pipe
            parts = parts.Select(x => x.Replace(Settings.PipeEncoding, "|")).ToArray();

                      

            IEnumerable<object> items = parts.Select(x => EnumSubHandler.GetFieldValue(x, parent, item, context)).ToArray();
            var list = Utility.CreateGenericType(typeof(List<>), new Type[] { pType }) ;
            Utility.CallAddMethod(items, list);

            return list;
            


        }

        public override string SetFieldValue(object value, InstanceContext context)
        {
            Type pType = Utility.GetGenericArgument(Property.PropertyType);


            if (EnumSubHandler == null)
                EnumSubHandler = GetSubHandler(pType, context);

            IEnumerable list = value as IEnumerable;

            List<string> sList = new List<string>();
                       

            foreach (object obj in list)
            {
                string result = EnumSubHandler.SetFieldValue(obj, context);
                if (!result.IsNullOrEmpty())
                    sList.Add(result);
            }

            StringBuilder sb = new StringBuilder();
            sList.ForEach(x => sb.AppendFormat("{0}|", x.Replace("|", Settings.PipeEncoding)));
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public override bool WillHandle(Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, IEnumerable<AbstractSitecoreDataHandler> datas, Dictionary<Type, SitecoreClassConfig> classes)
        {

            if(!(property.Attribute is SitecoreFieldAttribute)) return false;

            Type type = property.Property.PropertyType;

            if (!type.IsGenericType) return false;


            if (type.GetGenericTypeDefinition() != typeof(IEnumerable<>))
                return false;
            
            return true;
            
        }
      

        public override Type TypeHandled
        {
            get { return typeof(object); }
        }

        private AbstractSitecoreField GetSubHandler(Type type, InstanceContext context)
        {
            SitecoreProperty fakeProp = new SitecoreProperty()
            {
                Attribute = new SitecoreFieldAttribute(){
                    FieldName = FieldName,
                    ReadOnly = ReadOnly,
                    Setting = Setting
                },
                Property = new FakePropertyInfo(type)
                
            };

            var handler = context.Datas.FirstOrDefault(x => x.WillHandle(fakeProp, context.Datas, context.Classes)) as AbstractSitecoreField;
            if (handler == null) throw new NotSupportedException("No handler to support field type {0}".Formatted(type.FullName));

            handler.ConfigureDataHandler(fakeProp);
            return handler;


        }
    }
}
