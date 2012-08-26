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
using System.Collections;
using Glass.Sitecore.Mapper.Proxies;

namespace Glass.Sitecore.Mapper
{
    public class Enumerable<T> : IEnumerable<T> where T:class
    {
        Func<IEnumerable<Item>> _getItems;
        ISitecoreService _service;
        
        
        IList<T> _itemList = null; 

        bool _loaded = false;
        bool _isLazy = true;
        bool _inferType = false;
        Guid _ownerId;

        public Enumerable(Func<IEnumerable<Item>> getItems, ISitecoreService service, bool isLazy, bool inferType, Guid ownerId)
        {
            _getItems = getItems;
            _service = service;
            _isLazy = isLazy;
            _inferType = inferType;
            _ownerId = ownerId;
        }

        private void LoadItems()
        {

            Type type = typeof(T);
            _itemList = Utility.CreateGenericType(typeof(List<>), new Type[] { type }) as IList<T>;

            if (_getItems == null) throw new NullReferenceException("No function to return items");

            var items = _getItems.Invoke();

            if (items != null)
            {
                foreach (Item item in items.Where(x => x != null))
                {

                    var result = _service.CreateClass<T>(new ClassLoadingState(_service, _isLazy, _inferType, typeof(T), item, _ownerId, null));
                    _itemList.Add(result);
                }
            }

            _loaded = true;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            if (!_loaded) LoadItems();
            return _itemList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
