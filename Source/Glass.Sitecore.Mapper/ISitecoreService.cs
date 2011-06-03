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
using System.Collections;

namespace Glass.Sitecore.Mapper
{
    public interface ISitecoreService
    {

        InstanceContext InstanceContext { get; }

        /// <summary>
        /// Query Sitecore for a set of items. Concrete classes are created
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Sitecore items as concrete classes of the specified type</returns>
        IEnumerable<T> Query<T>(string query) where T: class;
        
        /// <summary>
        /// Query Sitecore for a set of items. Proxy classes are created.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Sitecore items as proxy classes of the specified type</returns>
        IEnumerable<T> Query<T>(string query, bool isLazy) where T : class;

        /// <summary>
        /// Query Sitecore for a single item. 
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Sitecore item as the specified type</returns>
        T QuerySingle<T>(string query) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="path">The path to the Sitecore item</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T>(string path) where T : class;
        
        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="path">The path to the Sitecore item</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T>(string path, Language language) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="path">The path to the Sitecore item</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T>(string path, Language language, global::Sitecore.Data.Version version) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T>(Guid id) where T : class;
        
        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T>(Guid id, Language language) where T : class;
        
        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T>(Guid id, Language language, global::Sitecore.Data.Version version) where T : class;


        /// <summary>
        /// Saves a class back to Sitecore. 
        /// </summary>
        /// <typeparam name="T">The type being saved. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="item">The class to save</param>
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
        
        /// <summary>
        /// Delete an item from Sitecore
        /// </summary>
        /// <typeparam name="T">The type being deleted. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="item">The class to delete</param>
        void Delete<T>(T item) where T : class;


        T CreateClass<T>(bool isLazy, Item item) where T : class;
        object CreateClass(bool isLazy, Type type, Item item);
        IEnumerable CreateClasses(bool isLazy, Type type, Func<IEnumerable<Item>> getItems);

    }
}
