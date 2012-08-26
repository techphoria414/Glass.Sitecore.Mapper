using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.ObjectCreation
{
    public interface IObjectManager
    {
        /// <summary>
        /// Creates the class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="type">The type.</param>
        /// <param name="item">The item.</param>
        /// <param name="constructorParameters">The constructor parameters.</param>
        /// <returns></returns>
        object CreateClass(ISitecoreService service, bool isLazy, bool inferType, Type type, Item item, params object[] constructorParameters);
        /// <summary>
        /// Reads from item.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="target">The target.</param>
        /// <param name="item">The item.</param>
        /// <param name="config">The config.</param>
        void ReadFromItem(ISitecoreService service, object target, Item item, SitecoreClassConfig config);

      
    }
}
