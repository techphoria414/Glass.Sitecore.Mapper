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
    public class LazyEnumerable<T> : IEnumerable<T> where T:class
    {
        Func<IEnumerable<Item>> _getItems;
        InstanceContext _context;
        
        
        IList<T> _proxyList = null; 

        bool _loaded = false;

        public LazyEnumerable(Func<IEnumerable<Item>> getItems, InstanceContext context)
        {
            _getItems = getItems;
            _context = context;
        }

        private void LoadProxies()
        {

            Type type = typeof(T);
            _proxyList = Utility.CreateGenericType(typeof(List<>), new Type[] { type }) as IList<T>;

            foreach (Item item in _getItems.Invoke().Where(x=>x != null))
            {
                var result = ProxyGenerator.CreateProxy(type, _context, item) as T;
                _proxyList.Add(result);
            }
            _loaded = true;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            if (!_loaded) LoadProxies();
            return _proxyList.GetEnumerator();
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
