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

        Hashtable _table = new Hashtable();


        protected override CachedObjectInformation GetInternal(Tuple<Guid, string, Type> key)
        {
            return _table[key] as CachedObjectInformation;
        }

        protected override bool AddInternal(Tuple<Guid, string, Type> key, CachedObjectInformation info)
        {
            _table.Add(key, info);
            return true;
        }

        protected override bool RemoveInternal(Tuple<Guid, string, Type> key)
        {
             _table.Remove(key);
             return true;
        }

        protected override bool ContainsInternal(Tuple<Guid, string, Type> key)
        {
            return _table.ContainsKey(key);
        }
    }
}
