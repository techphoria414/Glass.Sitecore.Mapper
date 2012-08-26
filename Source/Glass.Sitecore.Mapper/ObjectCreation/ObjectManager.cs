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
        public abstract object CreateClass(ClassLoadingState state);

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
        protected object CreateObject(ClassLoadingState state)
        {
            //check there is an item to create a class from
            if (state.Item == null) return null;
            //check that there are some constructor arguments 

            SitecoreClassConfig config = null;

            if (!state.InferType)
            {
                //this retrieves the class based on return type
                config = Context.StaticContext.GetSitecoreClass(state.Type);
            }
            else
            {
                //this retrieves the class by inferring the type from the template ID
                //if ths return type can not be found then the system will try to create a type 
                //base on the return type
                config = Context.StaticContext.GetSitecoreClass(state.Item.TemplateID.Guid, state.Type);
                if (config == null) config = Context.StaticContext.GetSitecoreClass(state.Type);

            }

            //if the class should be lazy loaded or is an interface then load using a proxy
            if (state.IsLazy || state.Type.IsInterface)
            {
                return ProxyGenerator.CreateProxy(config, state.Service, state.Item, state.InferType);
            }
            else
            {

                ConstructorInfo constructor = config.Type.GetConstructor(state.ConstructorParameters == null || state.ConstructorParameters.Count() == 0 ? Type.EmptyTypes : state.ConstructorParameters.Select(x => x.GetType()).ToArray());

                if (constructor == null) throw new MapperException("No constructor for class {0} with parameters {1}".Formatted(config.Type.FullName, string.Join(",", state.ConstructorParameters.Select(x => x.GetType().FullName).ToArray())));

                Delegate conMethod = config.CreateObjectMethods[constructor];
                object t = conMethod.DynamicInvoke(state.ConstructorParameters);
                ReadFromItem(state.Service, t, state.Item, config);
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


       
    }
}
