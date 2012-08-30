using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.ObjectCaching
{
    public struct CacheKey : IEquatable<CacheKey>
    {
        public CacheKey(Guid revisionId, string database, Type type):this()
        {
            RevisionId = revisionId;
            Database = database;
            Type = type;
        }
        public Guid RevisionId { get; private set; }
        public string Database { get; private set; }
        public Type Type { get; private set; }

        public override string ToString()
        {
            return "{0},{1},{2}".Formatted(RevisionId, Database, Type);
        }

        public bool Equals(CacheKey other)
        {
            return other.RevisionId == this.RevisionId && other.Database == this.Database && other.Type == this.Type;
        }
    }
}
