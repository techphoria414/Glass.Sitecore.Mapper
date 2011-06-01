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

        public Dictionary<Type, SitecoreClassConfig> Classes { get; private set; }
        public IEnumerable<AbstractSitecoreDataHandler> Datas { get; private set; }

        public InstanceContext(Dictionary<Type, SitecoreClassConfig> classes, IEnumerable<AbstractSitecoreDataHandler> datas)
        {
           
            Classes = classes;
            Datas = datas;
        }




        /// <summary>
        /// Creates an enumerable of the specified type
        /// </summary>
        /// <param name="isLazy"></param>
        /// <param name="type"></param>
        /// <param name="getItems"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable CreateClasses(bool isLazy, Type type, Func<IEnumerable<Item>> getItems)
        {
            return Utility.CreateGenericType(typeof(Enumerable<>), new Type[] { type }, getItems, this, isLazy) as IEnumerable;
        }

        public object CreateClass(bool isLazy, Type type, Item item)
        {
            if (item == null) return null;
            if (isLazy || type.IsInterface)
            {
                return ProxyGenerator.CreateProxy(type, this, item);
            }
            else
            {
                if (item == null) return null;

                //get the class information
                var scClass = GetSitecoreClass(type);
                object t = scClass.Type.Assembly.CreateInstance(scClass.Type.FullName);

                foreach (var handler in scClass.DataHandlers)
                {
                    handler.SetProperty(t, item, this);
                }

                return t;
            }
        }

        public IEnumerable<T> CreateClasses<T>(bool isLazy, IEnumerable<Item> items) where T : class
        {
            Func<IEnumerable<Item>> getItems = new Func<IEnumerable<Item>>(() =>
            {
                return items;
            });
            return CreateClasses(isLazy, typeof(T), getItems) as IEnumerable<T>;
        }
      
        public T CreateClass<T>(bool isLazy, Item item) where T : class
        {
            return CreateClass(isLazy, typeof(T),  item) as T;
        }
       
        public void SaveClass<T>(T target, Item item)
        {
            var scClass = GetSitecoreClass(typeof(T));


            foreach (var handler in scClass.DataHandlers)
            {
               
                if (handler.CanSetValue)
                {
                    handler.ReadProperty(target, item, this);
                }
            }
            
        }
        public Guid GetClassId<T>(T target) where T : class
        {
            return GetClassId(typeof(T), target);
        }
        public Guid GetClassId(Type type, object target){
            var scClass = GetSitecoreClass(type);
            var attribute = scClass.Properties.FirstOrDefault(x => x.Attribute is SitecoreIdAttribute);

            if (attribute == null) 
                throw new SitecoreIdException("The type {0} does not contain a property with the Glass.Sitecore.Mapper.Configuration.Attributes.SitecoreIdAttribute".Formatted(type.FullName));

            Guid guid =  (Guid) attribute.Property.GetValue(target, null);
            return guid;
        }


        public SitecoreClassConfig GetSitecoreClass(Type type)
        {
            
           
            if (!Classes.ContainsKey(type) || Classes[type] == null)
                throw new MapperException("Type {0} has not been loaded".Formatted(type.FullName));

            return Classes[type];
        }


        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
