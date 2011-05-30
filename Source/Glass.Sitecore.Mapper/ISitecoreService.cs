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
using Sitecore.Globalization;

namespace Glass.Sitecore.Mapper
{
    public interface ISitecoreService
    {
        IEnumerable<T> Query<T>(string query) where T: class;
        IEnumerable<T> Query<T>(string query, bool isLazy) where T : class;
        T QuerySingle<T>(string query) where T : class;

        T GetItem<T>(string path) where T : class;
        T GetItem<T>(string path, Language language) where T : class;
        T GetItem<T>(string path, Language language, global::Sitecore.Data.Version version) where T : class;
        T GetItem<T>(Guid id) where T : class;
        T GetItem<T>(Guid id, Language language) where T : class;
        T GetItem<T>(Guid id, Language language, global::Sitecore.Data.Version version) where T : class;


        
        void Save<T>(T item) where T : class;
        
        /// <summary>
        /// Create a blank Sitecore item
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <typeparam name="K">The type of the parent</typeparam>
        /// <param name="parent">The parent item. This item must have been load by Glass Sitecore Mapper</param>
        /// <param name="name">The name of the item</param>
        /// <returns></returns>
        T Create<T, K>(K parent, string name)
            where T : class
            where K : class;

        /// <summary>
        /// Create an item with pre-populated data
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <typeparam name="K">The type of the parent</typeparam>
        /// <param name="parent">The parent item. This item must have been load by Glass Sitecore Mapper</param>
        /// <param name="name">The name of the item</param>
        /// <param name="data">The data to pre-populate the item with</param>
        /// <returns></returns>
        T Create<T, K>(K parent, string name, T data)
            where T : class
            where K : class;
        
        void Delete<T>(T item) where T : class;
    }
}
