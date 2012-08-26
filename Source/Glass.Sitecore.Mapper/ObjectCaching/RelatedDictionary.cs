using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Glass.Sitecore.Mapper.ObjectCaching
{
    public class RelatedDictionary
    {
        public static ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        Dictionary<Guid, List<CacheKey>> _related = new Dictionary<Guid, List<CacheKey>>();


        public void Add(Guid owner, CacheKey key)
        {
            if (owner == Guid.Empty)
                return;

            if (!_related.ContainsKey(owner))
            {
                _related.Add(owner, new List<CacheKey>());
            }

            if (Lock.TryEnterWriteLock(100))
            {
                try
                {
                    _related[owner].Add(key);
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
        }

        public List<CacheKey> FlushKeys(Guid owner)
        {
            if (owner == Guid.Empty)
                return default(List<CacheKey>);

            if (_related.ContainsKey(owner))
            {
                if (Lock.TryEnterWriteLock(100))
                {
                    var list = _related[owner];
                    try
                    {
                        //reset the internal list
                        _related[owner] = new List<CacheKey>();
                    }
                    finally
                    {
                        Lock.ExitWriteLock();
                    }
                    return list;
                }
            }

            return new List<CacheKey>();
        }
    }
}
