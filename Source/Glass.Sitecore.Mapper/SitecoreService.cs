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

namespace Glass.Sitecore.Mapper
{
    public class SitecoreService : ISitecoreService
    {
        InstanceContext _context;

        Database _database;

        public SitecoreService(string database):this(global::Sitecore.Configuration.Factory.GetDatabase(database))
        {
            
        }
        public SitecoreService(Database database)
        {
            _context = Context.GetContext();
            _database = database;
        }

        #region ISitecoreService Members

        public IEnumerable<T> Query<T>(string query) where T: class
        {
            return Query<T>(query, false);
        }
        public IEnumerable<T> Query<T>( string query,bool isLazy) where T : class
        {
            Item[] items = _database.SelectItems(query);
            return _context.CreateClasses<T>(isLazy, items);
        }


        public T QuerySingle<T>(string query)  where T: class
        {
            Item item = _database.SelectSingleItem(query);
            return _context.CreateClass<T>(false, item);
        }

        public T GetItem<T>(string path)  where T: class
        {
            Item item = _database.GetItem(path);
            return _context.CreateClass<T>(false, item);
        }

        public T GetItem<T>(Guid id)  where T: class
        {
            Item item = _database.GetItem(new  ID(id));
            return _context.CreateClass<T>(false, item);
        }

        public void Save<T>(T target)  where T: class
        {
            Guid guid = _context.GetClassId(target);
            Item item = _database.GetItem(new ID(guid));
         
            item.Editing.BeginEdit();
            _context.SaveClass<T>(target, item);            
            item.Editing.EndEdit();
            
        }

        public T Create<T>(object parent, string name)  where T: class
        {
            Guid guid = Guid.Empty;
            try
            {
                 guid = _context.GetClassId(parent);
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

            SitecoreClassConfig scClass = _context.GetSitecoreClass(typeof(T));

            string templateSt = scClass.ClassAttribute.TemplateId;
            string branchSt = scClass.ClassAttribute.BranchId;
            
            Guid templateId = Guid.Empty;
            Guid branchId = Guid.Empty;

            Item item = null;

            if (templateSt.GuidTryParse(out templateId))
            {
                item = pItem.Add(name, new TemplateID(new ID(templateId)));
            }
            else if (branchSt.GuidTryParse(out branchId))
            {
                item = pItem.Add(name, new BranchId(new ID(branchId)));
            }
            else
            {
                throw new MapperException("Type {0} does not have a Template ID or Branch ID".Formatted(typeof(T).FullName));
            }


            if (item == null)
                throw new MapperException("Failed to create child with name {0} and parent {1}".Formatted(name, item.Paths.FullPath));

            return _context.CreateClass<T>(false, item);

        }

        public void Delete<T>(T item)  where T: class
        {
            Guid guid = Guid.Empty;
            try
            {
                guid = _context.GetClassId(item);
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

        #endregion
    }
}
