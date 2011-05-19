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
using Glass.Sitecore.Persistence.Configuration;
using Sitecore.Data.Items;
using System.Reflection;
using Glass.Sitecore.Persistence.Data;
using Glass.Sitecore.Persistence.Configuration.Attributes;
using System.Collections;
using Glass.Sitecore.Persistence.Proxies;

namespace Glass.Sitecore.Persistence
{
    public class InstanceContext : ICloneable
    {

        public IEnumerable<SitecoreClassConfig> Classes { get; private set; }
        public IEnumerable<ISitecoreDataHandler> Datas { get; private set; }

        public InstanceContext(IEnumerable<SitecoreClassConfig> classes, IEnumerable<ISitecoreDataHandler> datas)
        {
            Classes = classes;
            Datas = LoadDataHandlers(datas);
        }


        private IEnumerable<ISitecoreDataHandler> LoadDataHandlers(IEnumerable<ISitecoreDataHandler> handlers)
        {
            if (handlers == null) handlers = new List<ISitecoreDataHandler>();
            List<ISitecoreDataHandler> _handlers = new List<ISitecoreDataHandler>(handlers);
            
            //load default handlers
            _handlers.AddRange( new List<ISitecoreDataHandler>(){
                new SitecoreChildrenHandler(),
                new SitecoreFieldBooleanHandler(),
                new SitecoreFieldClassHandler(),
                new SitecoreFieldDateTimeHandler(),
                new SitecoreFieldDecimalHandler(),
                new SitecoreFieldDoubleHandler(),
                new SitecoreFieldEnumHandler(),
                new SitecoreFieldFileHandler(),
                new SitecoreFieldFloatHandler(),
                new SitecoreFieldGuidHandler(),
                new SitecoreFieldIEnumerableHandler(),
                new SitecoreFieldImageHandler(),
                new SitecoreFieldIntegerHandler(),
                new SitecoreFieldLinkHandler(),
                new SitecoreFieldStreamHandler(),
                new SitecoreFieldStringHandler(),
                new SitecoreFieldTriStateHandler(),
                new SitecoreIdDataHandler(),
                new SitecoreInfoHandler(),
                new SitecoreParentHandler(),
                new SitecoreQueryHandler()
            });

            return _handlers;

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
            if (isLazy)
            {
                return Utility.CreateGenericType(typeof(LazyEnumerable<>), new Type[] { type }, getItems, this) as IEnumerable;
            }
            else
            {
                IList list = Utility.CreateGenericType(typeof(List<>), new Type[] { type }) as IList;

                foreach (Item item in getItems.Invoke().Where(x=>x != null))
                {
                    var result = this.MakeClass(item, type);
                    list.Add(result);
                }

                return list;
            }
        }

        public object CreateClass(bool isLazy, Type type, Item item)
        {
            if (item == null) return null;
            if (isLazy)
            {
                return ProxyGenerator.CreateProxy(type, this, item);
            }
            else
            {
                return this.MakeClass(item, type);
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


        public object MakeClass(Item item, Type type){
            if (item == null) return null;

            //get the class information
            var scClass = GetSitecoreClass(type);
            object t = scClass.Type.Assembly.CreateInstance(scClass.Type.FullName);

            foreach (var property in scClass.Properties)
            {
                SetProperty(t, property, item); 
            }
                        
            return t;
        }
        public void SaveClass<T>(T target, Item item)
        {
            var scClass = GetSitecoreClass(typeof(T));


            foreach (var property in scClass.Properties)
            {
                if (property.DataHandler.CanSetValue)
                {
                    PropertyInfo info = property.Property;
                    object value = info.GetValue(target, null);
                    property.DataHandler.SetValue(target, item, value, property, this);
                }
            }
            
        }
        public Guid GetClassId<T>(T target) where T : class
        {
            return GetClassId(target.GetType(), target);
        }
        public Guid GetClassId(Type type, object target){
            var scClass = GetSitecoreClass(type);
            var attribute = scClass.Properties.FirstOrDefault(x => x.Attribute is SitecoreIdAttribute);

            if (attribute == null) 
                throw new SitecoreIdException("The type {0} does not contain a property with the Glass.Sitecore.Persistence.Configuration.Attributes.SitecoreIdAttribute".Formatted(type.FullName));

            Guid guid =  (Guid) attribute.Property.GetValue(target, null);
            return guid;
        }

        public SitecoreClassConfig GetSitecoreClass(Type type)
        {
            SitecoreClassConfig scClass = Classes.FirstOrDefault(x => x.Type == type);
            if (scClass == null)
                throw new PersistenceException("Type {0} has not been loaded".Formatted(type.FullName));

            return scClass;
        }

        private void SetProperty(object target, SitecoreProperty property, Item item)
        {
            object value = null;

            SetDataHandler(property);

            value = property.DataHandler.GetValue(target, item, property, this);

            property.Property.SetValue(target, value, null);
        }

        private void SetDataHandler(SitecoreProperty property)
        {
            if (property.DataHandler == null)
                property.DataHandler = Datas.FirstOrDefault(x => x.WillHandle(property, this));

            if (property.DataHandler == null) 
                throw new NotSupportedException("No data handler for: \n\r Class: {0} \n\r Member: {1} \n\r Attribute: {2}"
                    .Formatted(
                        property.Property.ReflectedType.Name,
                        property.Property.Name,
                        property.Attribute.GetType().Name
                    ));
        }
     
       


        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
