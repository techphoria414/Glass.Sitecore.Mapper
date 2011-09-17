using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Used to pull in a Sitecore item 
    /// </summary>
    public class SitecoreItemAttribute:AbstractSitecorePropertyAttribute
    {
         /// <summary>
        /// Used to retrieve the children of an item as a specific type.
        /// </summary>
        public SitecoreItemAttribute()
        {
            IsLazy = true;
        }
        /// <summary>
        /// Indicates if children should be loaded lazily. Default value is true. If false all children will be loaded when the contain object is created.
        /// </summary>
        public bool IsLazy
        {
            get;
            set;
        }

        /// <summary>
        /// The path to the item. If both a path and ID are specified the ID will be used.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The Id of the item. 
        /// </summary>
        public string Id { get; set; }

    }
}
