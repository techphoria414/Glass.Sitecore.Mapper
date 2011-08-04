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
using System.Reflection;
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using System.Collections;
using Glass.Sitecore.Mapper.Proxies;

namespace Glass.Sitecore.Mapper
{
    public class InstanceContext : ICloneable
    {
        public Hashtable ClassesByType { get; private set; }
        public Hashtable ClassesById { get; set; }
        public Dictionary<Type, SitecoreClassConfig> Classes { get; private set; }
        public IEnumerable<AbstractSitecoreDataHandler> Datas { get; private set; }

        public InstanceContext(Dictionary<Type, SitecoreClassConfig> classes, IEnumerable<AbstractSitecoreDataHandler> datas)
        {
            //This needs reworking
            Classes = classes;
            ClassesByType = new Hashtable(classes);
            ClassesById = new Hashtable();
            foreach (var record in classes)
            {
                if (record.Value.TemplateId != Guid.Empty)
                    ClassesById.Add(record.Value.TemplateId, record.Value);
            }

            Datas = datas;
        }


        /// <summary>
        /// Can we move this so that it happens when the instance context is created
        /// 
        /// </summary>
        /// <param name="property"></param>
        public AbstractSitecoreDataHandler GetDataHandler(SitecoreProperty property)
        {
            AbstractSitecoreDataHandler handler = Datas.FirstOrDefault(x => x.WillHandle(property, Datas, Classes));

            if (handler == null)
                throw new NotSupportedException("No data handler for: \n\r Class: {0} \n\r Member: {1} \n\r Attribute: {2}"
                    .Formatted(
                        property.Property.ReflectedType.Name,
                        property.Property.Name,
                        property.Attribute.GetType().Name
                    ));

            var newHandler = handler.Clone() as AbstractSitecoreDataHandler;
            newHandler.ConfigureDataHandler(property);

            return newHandler;
        }



        public Guid GetClassId(Type type, object target)
        {
            var scClass = GetSitecoreClass(type);
            var attribute = scClass.Properties.FirstOrDefault(x => x.Attribute is SitecoreIdAttribute);

            if (attribute == null)
                throw new SitecoreIdException("The type {0} does not contain a property with the Glass.Sitecore.Mapper.Configuration.Attributes.SitecoreIdAttribute".Formatted(type.FullName));

            Guid guid = (Guid)attribute.Property.GetValue(target, null);
            return guid;
        }


        public SitecoreClassConfig GetSitecoreClass(Type type)
        {
            if (!ClassesByType.ContainsKey(type) || ClassesByType[type] == null)
                throw new MapperException("Type {0} has not been loaded".Formatted(type.FullName));

            return ClassesByType[type].CastTo<SitecoreClassConfig>();
        }

        public SitecoreClassConfig GetSitecoreClass(Guid templateId)
        {
            string id = templateId.ToString();
            //would it be quicker to have a second dictionary that recorded classes by their template ID?
            if (!ClassesById.ContainsKey(templateId) || ClassesById[templateId] != null)
            {
                return ClassesById[templateId].CastTo<SitecoreClassConfig>();
            }
            else return null;
        }


        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
