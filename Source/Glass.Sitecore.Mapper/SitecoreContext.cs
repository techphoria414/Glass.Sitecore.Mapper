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
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper
{
    public class SitecoreContext : SitecoreService, ISitecoreContext
    {
        public SitecoreContext() :base(global::Sitecore.Context.Database)
        {

        }
        #region ISitecoreContext Members

        public T GetCurrentItem<T>() where T:class
        {
            return base.GetItem<T>(global::Sitecore.Context.Item.ID.Guid);
        }


        public T GetHomeItem<T>() where T : class
        {

            return base.GetItem<T>(global::Sitecore.Context.Site.RootPath);
        }

        public IEnumerable<T> QueryRelative<T>(string query, bool isLazy, bool inferType) where T : class
        {
            Item item = global::Sitecore.Context.Item;
            var results = item.Axes.SelectItems(query);
            return base.CreateClasses(isLazy, inferType, typeof(T), () => { return results; }) as IEnumerable<T>;

        }

        public T QuerySingleRelative<T>(string query, bool isLazy, bool inferType) where T : class
        {
            Item item = global::Sitecore.Context.Item;
            var results = item.Axes.SelectSingleItem(query);
            return base.CreateClass<T>(isLazy, inferType, item);
        }
        
        #endregion
    }
}
