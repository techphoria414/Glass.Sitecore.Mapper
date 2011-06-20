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

namespace Glass.Sitecore.Mapper
{
    public class SitecoreService : ISitecoreService
    {
        public InstanceContext InstanceContext { get; private set;}
        
        

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
            return CreateClass<T>(false, item);
        }

        public T GetItem<T>(string path)  where T: class
        {
            Item item = _database.GetItem(path);
            return CreateClass<T>(false, item);
        }
        public T GetItem<T>(string path, Language language) where T:class
        {
            Item item = _database.GetItem(path, language);
            return CreateClass<T>(false, item);
        }
        public T GetItem<T>(string path, Language language, global::Sitecore.Data.Version version) where T : class
        {
            Item item = _database.GetItem(path, language, version);
            return CreateClass<T>(false, item);
        }

        public T GetItem<T>(Guid id)  where T: class
        {
            Item item = _database.GetItem(new  ID(id));
            return CreateClass<T>(false, item);
        }
        public T GetItem<T>(Guid id, Language language) where T : class
        {
            Item item = _database.GetItem(new ID(id), language);
            return CreateClass<T>(false, item);
        }
        public T GetItem<T>(Guid id, Language language, global::Sitecore.Data.Version version) where T : class
        {
            Item item = _database.GetItem(new ID(id), language, version);
            return CreateClass<T>(false, item);
        }

        public void Save<T>(T target)  where T: class
        {
            Guid guid = InstanceContext.GetClassId(typeof(T), target);
            Item item = _database.GetItem(new ID(guid));
         
            item.Editing.BeginEdit();
            WriteToItem<T>(target, item);            
            item.Editing.EndEdit();
            _linkDb.UpdateReferences(item);
            
        }

        public T Create<T, K>(K parent, string name)
            where T : class
            where K : class
        {
            return Create<T, K>(parent, name, null);
        }

        public T Create<T, K>(K parent, string name, T data)  where T: class where K: class
        {

            //check that the data is not null and if it has an ID check that it is empty
            if (data != null)
            {
                try
                {
                    Guid id = InstanceContext.GetClassId(typeof(T), data);
                    if (id != Guid.Empty) throw new MapperException("You are trying to create an item on a class that doesn't have an empty ID value");
                }
                catch (SitecoreIdException ex)
                {
                    //we can swallow this exception for now
                    //should look to do this beeter
                }

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

            string templateSt = scClass.ClassAttribute.TemplateId;
            string branchSt = scClass.ClassAttribute.BranchId;
            
            Guid templateId = Guid.Empty;
            Guid branchId = Guid.Empty;

            Item item = null;

            if (!templateSt.IsNullOrEmpty() && templateSt.GuidTryParse(out templateId))
            {
                item = pItem.Add(name, new TemplateID(new ID(templateId)));
            }
            else if (!branchSt.IsNullOrEmpty() && branchSt.GuidTryParse(out branchId))
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
            return CreateClass<T>(false, item);

        }

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

        public T CreateClass<T>(bool isLazy, Item item) where T:class
        {
            return (T) CreateClass(isLazy, typeof(T), item);
        }

        public object CreateClass(bool isLazy, Type type, Item item)
        {
            if (item == null) return null;
            if (isLazy || type.IsInterface)
            {
                SitecoreClassConfig config = InstanceContext.GetSitecoreClass(type);
                return ProxyGenerator.CreateProxy(config, this, item);
            }
            else
            {
                if (item == null) return null;

                //get the class information
                var scClass = InstanceContext.GetSitecoreClass(type);
                object t = scClass.Type.Assembly.CreateInstance(scClass.Type.FullName);

                foreach (var handler in scClass.DataHandlers)
                {
                    handler.SetProperty(t, item, this);
                }

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
            return Utility.CreateGenericType(typeof(Enumerable<>), new Type[] { type }, getItems, this, isLazy) as IEnumerable;
        }
        #endregion

        #region Private Methods

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
