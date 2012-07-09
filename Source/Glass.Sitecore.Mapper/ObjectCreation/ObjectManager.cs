using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Proxies;
using System.Reflection;

namespace Glass.Sitecore.Mapper.ObjectCreation
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ObjectManager : IObjectManager
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectManager"/> class.
        /// </summary>
        public ObjectManager()
        {
        }

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
        public abstract object CreateClass(ISitecoreService service, bool isLazy, bool inferType, Type type, Item item, params object[] constructorParameters);

        /// <summary>
        /// Creates the object.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="type">The type.</param>
        /// <param name="item">The item.</param>
        /// <param name="constructorParameters">The constructor parameters.</param>
        /// <returns></returns>
        protected object CreateObject(ISitecoreService service, bool isLazy, bool inferType, Type type, Item item, params object[] constructorParameters)
        {
            //check there is an item to create a class from
            if (item == null) return null;
            //check that there are some constructor arguments 

            SitecoreClassConfig config = null;

            if (!inferType)
            {
                //this retrieves the class based on return type
                config = Context.StaticContext.GetSitecoreClass(type);
            }
            else
            {
                //this retrieves the class by inferring the type from the template ID
                //if ths return type can not be found then the system will try to create a type 
                //base on the return type
                config = Context.StaticContext.GetSitecoreClass(item.TemplateID.Guid, type);
                if (config == null) config = Context.StaticContext.GetSitecoreClass(type);

            }

            //if the class should be lazy loaded or is an interface then load using a proxy
            if (isLazy || type.IsInterface)
            {
                return ProxyGenerator.CreateProxy(config, service, item, inferType);
            }
            else
            {

                ConstructorInfo constructor = config.Type.GetConstructor(constructorParameters == null || constructorParameters.Count() == 0 ? Type.EmptyTypes : constructorParameters.Select(x => x.GetType()).ToArray());

                if (constructor == null) throw new MapperException("No constructor for class {0} with parameters {1}".Formatted(config.Type.FullName, string.Join(",", constructorParameters.Select(x => x.GetType().FullName).ToArray())));

                Delegate conMethod = config.CreateObjectMethods[constructor];
                object t = conMethod.DynamicInvoke(constructorParameters);
                ReadFromItem(service, t, item, config);
                return t;
            }
        }

        /// <summary>
        /// Reads from item.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="target">The target.</param>
        /// <param name="item">The item.</param>
        /// <param name="config">The config.</param>
        public void ReadFromItem(ISitecoreService service, object target, Item item, SitecoreClassConfig config)
        {
            foreach (var handler in config.DataHandlers)
            {
                handler.SetProperty(target, item, service);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="o"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract bool AddRelatedCache(string key, string group, object o, Type type);
    }
}
