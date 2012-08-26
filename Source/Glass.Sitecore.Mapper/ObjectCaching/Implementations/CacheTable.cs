using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using System.Collections;

namespace Glass.Sitecore.Mapper.ObjectCaching
{
    public class CacheTable : ObjectCache
    {

        static volatile Hashtable _table = new Hashtable();


        protected override CachedObjectInformation GetInternal(CacheKey key)
        {
            return _table[key] as CachedObjectInformation;
        }

        protected override bool AddInternal(CacheKey key, CachedObjectInformation info)
        {
            _table.Add(key, info);
            return true;
        }

        protected override bool RemoveInternal(CacheKey key)
        {
             _table.Remove(key);
             return true;
        }

        protected override bool ContainsInternal(CacheKey key)
        {
            return _table.ContainsKey(key);
        }
    }
}
