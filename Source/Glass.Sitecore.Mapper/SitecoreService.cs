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
using Sitecore.Data;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Configuration;
using Sitecore.Globalization;
using Glass.Sitecore.Mapper.Proxies;
using System.Collections;
using Sitecore.Links;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Reflection;

namespace Glass.Sitecore.Mapper
{
    public class SitecoreService : ISitecoreService
    {
        public InstanceContext InstanceContext { get; private set;}
        
        /// <summary>
        /// The database used by the Sitecore service, used by some internal processed.
        /// </summary>
        public Database Database{
            get
            {
                return _database;
            }
        }

        Database _database;
        LinkDatabase _linkDb;

        public SitecoreService(string database):this(global::Sitecore.Configuration.Factory.GetDatabase(database))
        {
           
        }
        public SitecoreService(Database database)
        {
            InstanceContext = Context.GetContext();
            _linkDb = global::Sitecore.Configuration.Factory.GetLinkDatabase();
            _database = database;
        }
        
        #if DEBUG

        /// <summary>
        /// Used by the test assembly only!!
        /// </summary>
        /// <param name="database"></param>
        /// <param name="context"></param>
        internal SitecoreService(Database database, InstanceContext context){
            InstanceContext = context;
            _database = database;
        
        }

        #endif

        #region ISitecoreService Members

        public IEnumerable<T> Query<T>(string query) where T: class
        {
            return Query<T>(query, false);
        }
        public IEnumerable<T> Query<T>( string query,bool isLazy) where T : class
        {
            Item[] items = _database.SelectItems(query);
            return CreateClasses(isLazy, typeof(T), () => { return items; }) as IEnumerable<T>;
        }


        public T QuerySingle<T>(string query)  where T: class
        {            
            Item item = _database.SelectSingleItem(query);            
            return CreateClass<T>(false, false,  item);
        }

        public T GetItem<T>(string path)  where T: class
        {
            Item item = _database.GetItem(path);
            return CreateClass<T>(false, false, item);
        }
        public T GetItem<T>(string path, Language language) where T:class
        {
            Item item = _database.GetItem(path, language);
            return CreateClass<T>(false, false, item);
        }
        public T GetItem<T>(string path, Language language, global::Sitecore.Data.Version version) where T : class
        {
            Item item = _database.GetItem(path, language, version);
            return CreateClass<T>(false, false, item);
        }

        public T GetItem<T>(Guid id)  where T: class
        {
            Item item = _database.GetItem(new  ID(id));
            return CreateClass<T>(false, false, item);
        }
        public T GetItem<T>(Guid id, Language language) where T : class
        {
            Item item = _database.GetItem(new ID(id), language);
            return CreateClass<T>(false, false, item);
        }
        public T GetItem<T>(Guid id, Language language, global::Sitecore.Data.Version version) where T : class
        {
            Item item = _database.GetItem(new ID(id), language, version);
            return CreateClass<T>(false, false, item);
        }

        public void Save<T>(T target)  where T: class
        {
            Item item = GetItemFromSitecore<T>(target);
         
            item.Editing.BeginEdit();
            WriteToItem<T>(target, item);            
            item.Editing.EndEdit();
            _linkDb.UpdateReferences(item);
            
        }

        public T AddVersion<T>(T target) where T:class
        {
            Item item = GetItemFromSitecore<T>(target);

            Item newVersion = item.Versions.AddVersion();

            return CreateClass<T>(false, false, newVersion);

        }

        /// <summary>
        /// Retrieves an item based on the passed in class based on the version and language properties on the class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        private Item GetItemFromSitecore<T>(T target)
        {
            Guid guid = InstanceContext.GetClassId(typeof(T), target);

            SitecoreClassConfig config = InstanceContext.GetSitecoreClass(typeof(T));
            Language language = null;
            int versionNumber = -1;

            if (config.LanguageProperty != null)
                language = config.LanguageProperty.Property.GetValue(target, null) as Language;
            if (config.VersionProperty != null)
            {
                versionNumber = (int)config.VersionProperty.Property.GetValue(target, null);

            }
            if (language != null && versionNumber > 0)
            {
                return _database.GetItem(new ID(guid), language, new global::Sitecore.Data.Version(versionNumber));
            }
            else if (language != null)
            {
                return _database.GetItem(new ID(guid), language);
            }
            else
            {
                return _database.GetItem(new ID(guid));
            }

            

            



        }

        /// <summary>
        /// Creates a new Sitecore item. 
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the SitecoreClassAttribute or fluent equivalent</typeparam>
        /// <typeparam name="K">The type of the parent item</typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the SitecoreIdAttribute or fluent equivalent</param>
        /// <param name="newItem">New item to create, must have the attribute SitecoreInfoAttribute of type SitecoreInfoType.Name or the fluent equivalent</param>
        /// <returns></returns>
        public T Create<T, K>(K parent, T newItem)
            where T : class
            where K : class
        {
            try
            {
                Guid id = InstanceContext.GetClassId(typeof(T), newItem);
                if (id != Guid.Empty) throw new MapperException("You are trying to create an item on a class that doesn't have an empty ID value");
            }
            catch (SitecoreIdException ex)
            {
                //we can swallow this exception for now
                //should look to do this beeter
            }


            Guid parentId = Guid.Empty;
            try
            {
                parentId = InstanceContext.GetClassId(typeof(K), parent);
            }
            catch (SitecoreIdException ex)
            {
                throw new MapperException("Failed to get parent ID", ex);
            }


            if (parentId == Guid.Empty)
                throw new MapperException("Guid for parent is empty");

            Item pItem = GetItemFromSitecore<K>(parent);
            if (pItem == null)
                throw new MapperException("Could not find parent item with ID {0}".Formatted(parentId));

            SitecoreClassConfig scClass = InstanceContext.GetSitecoreClass(typeof(T));

            var nameProperty = scClass.Properties.Where(x => x.Attribute is SitecoreInfoAttribute)
                .Cast<SitecoreProperty>().FirstOrDefault(x => x.Attribute.CastTo<SitecoreInfoAttribute>().Type == SitecoreInfoType.Name);

            if (nameProperty == null)
                throw new MapperException("Type {0} does not have a property with SitecoreInfoType.Name".Formatted(typeof(T).FullName));
            
            string name = string.Empty;
            
            try
            {
                 name = nameProperty.Property.GetValue(newItem, null).ToString();
            }
            catch
            {
                throw new MapperException("Failed to get item name");
            }

            if (name.IsNullOrEmpty())
                throw new MapperException("New class has no name");


            Guid templateId = scClass.TemplateId;
            Guid branchId = scClass.BranchId;

            Item item = null;

            if (templateId != Guid.Empty)
            {
                item = pItem.Add(name, new TemplateID(new ID(templateId)));
            }
            else if (branchId != Guid.Empty)
            {
                item = pItem.Add(name, new BranchId(new ID(branchId)));
            }
            else
            {
                throw new MapperException("Type {0} does not have a Template ID or Branch ID".Formatted(typeof(T).FullName));
            }

            if (item == null) throw new MapperException("Failed to create item");
           
            //write new data to the item

            item.Editing.BeginEdit();
            WriteToItem<T>(newItem, item);
            item.Editing.EndEdit();

            //then read it back
            ReadFromItem(newItem, item, scClass);
            return newItem;
         //   return CreateClass<T>(false, false, item);

        }

       


     

        #region OBSOLETE

        [Obsolete("Use Create<T,K>(K parent, T newItem)")]
        public T Create<T, K>(K parent, string name)
            where T : class
            where K : class
        {
            return Create<T, K>(parent, name, null);
        }


        [Obsolete("Use Create<T,K>(K parent, T newItem)")]
        public T Create<T, K>(K parent, string name, T data)  where T: class where K: class
        {

            //check that the data is not null and if it has an ID check that it is empty
            if (data != null)
            {
               

            }
            
            Guid guid = Guid.Empty;
            try
            {
                 guid = InstanceContext.GetClassId(typeof(K), parent);
            }
            catch (SitecoreIdException ex)
            {
                throw new MapperException("Failed to get parent ID", ex);
            }


            if (guid == Guid.Empty) 
                throw new MapperException("Guid for parent is empty");

            Item pItem = _database.GetItem(new ID(guid));
            if (pItem == null)
                throw new MapperException("Could not find parent item");

            SitecoreClassConfig scClass = InstanceContext.GetSitecoreClass(typeof(T));
            
            Guid templateId = scClass.TemplateId;
            Guid branchId = scClass.BranchId;

            Item item = null;

            if (templateId != Guid.Empty)
            {
                item = pItem.Add(name, new TemplateID(new ID(templateId)));
            }
            else if (branchId != Guid.Empty)
            {
                item = pItem.Add(name, new BranchId(new ID(branchId)));
            }
            else
            {
                throw new MapperException("Type {0} does not have a Template ID or Branch ID".Formatted(typeof(T).FullName));
            }



            if (item == null)
                throw new MapperException("Failed to create child with name {0} and parent {1}".Formatted(name, item.Paths.FullPath));

            //if we have data save it to the item
            if (data != null)
            {
                item.Editing.BeginEdit();
                WriteToItem<T>(data, item);
                item.Editing.EndEdit();
            }
            return CreateClass<T>(false, false, item);

        }

        #endregion


        public void Delete<T>(T item)  where T: class
        {
            Guid guid = Guid.Empty;
            try
            {
                guid = InstanceContext.GetClassId(typeof(T), item);
            }
            catch (SitecoreIdException ex)
            {
                throw new MapperException("Failed to get item ID", ex);
            }

            if (guid == Guid.Empty) 
                throw new MapperException("Guid for item is empty");

            Item scItem = _database.GetItem(new ID(guid));

            if (scItem == null)
                throw new MapperException("Item not found");

            scItem.Delete();
        }

        public T CreateClass<T>(bool isLazy, bool inferType, Item item) where T : class
        {
            return (T)CreateClass(isLazy, inferType, typeof(T), item);
        }

        public object CreateClass(bool isLazy, bool inferType, Type type, Item item)
        {
            //we have to add null to the list of parameters otherwise we get a stack overflow
            return CreateClass(isLazy, inferType, type, item, null);
        }

        public object CreateClass(bool isLazy, bool inferType, Type type, Item item, params object[] constructorParameters)
        {
            //check there is an item to create a class from
            if (item == null) return null;
            //check that there are some constructor arguments 

            SitecoreClassConfig config=null;

            if (!inferType)
            {
                //this retrieves the class based on return type
                config = InstanceContext.GetSitecoreClass(type);
            }
            else
            {
                //this retrieves the class by inferring the type from the template ID
                //if ths return type can not be found then the system will try to create a type 
                //base on the return type
                config = InstanceContext.GetSitecoreClass(item.TemplateID.Guid, type);
                if (config == null) config = InstanceContext.GetSitecoreClass(type);
                
            }

            //if the class should be lazy loaded or is an interface then load using a proxy
            if (isLazy || type.IsInterface)
            {
                return ProxyGenerator.CreateProxy(config, this, item, inferType);
            }
            else
            {

                ConstructorInfo constructor = config.Type.GetConstructor(constructorParameters == null || constructorParameters.Count() == 0 ? Type.EmptyTypes : constructorParameters.Select(x=>x.GetType()).ToArray());

                if (constructor == null) throw new MapperException("No constructor for class {0} with parameters {1}".Formatted(config.Type.FullName, string.Join(",", constructorParameters.Select(x => x.GetType().FullName).ToArray())));

                Delegate conMethod = config.CreateObjectMethods[constructor];
                object t = conMethod.DynamicInvoke(constructorParameters);
                ReadFromItem(t, item, config);
                return t;
            }
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
            return CreateClasses(isLazy, false, type, getItems);                
        }
        public IEnumerable CreateClasses(bool isLazy, bool inferType, Type type, Func<IEnumerable<Item>> getItems)
        {
            return Utility.CreateGenericType(typeof(Enumerable<>), new Type[] { type }, getItems, this, isLazy, inferType) as IEnumerable;
        }

        public T CreateClass<T, K>(bool isLazy, bool inferType, Item item, K param1)
        {
            return (T)CreateClass(isLazy, inferType, typeof(T), item, param1);

        }

        public T CreateClass<T,K,L>(bool isLazy, bool inferType, Item item, K param1, L param2)
        {          
            return (T)CreateClass(isLazy, inferType, typeof(T), item, param1, param2);
        }

        public T CreateClass<T, K, L, M>(bool isLazy, bool inferType, Item item, K param1, L param2, M param3)
        {
            return (T)CreateClass(isLazy, inferType, typeof(T), item, param1, param2, param3);
        }

        public T CreateClass<T, K, L, M, N>(bool isLazy, bool inferType, Item item, K param1, L param2, M param3, N param4)
        {
            return (T)CreateClass(isLazy, inferType, typeof(T), item, param1, param2, param3, param4);            
        }

         

        #endregion

        #region Private Methods

        private void ReadFromItem(object target, Item item, SitecoreClassConfig config)
        {
            foreach (var handler in config.DataHandlers)
            {
                handler.SetProperty(target, item, this);
            }
        }

        private void WriteToItem<T>(T target, Item item)
        {
            var scClass = InstanceContext.GetSitecoreClass(typeof(T));

            foreach (var handler in scClass.DataHandlers)
            {

                if (handler.CanSetValue)
                {
                    handler.ReadProperty(target, item, this);
                }
            }
        }

 

        #endregion


       
    }
}
