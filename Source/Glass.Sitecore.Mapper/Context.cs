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
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using System.Reflection.Emit;

namespace Glass.Sitecore.Mapper
{
    public class Context
    {
        

        #region STATICS

        /// <summary>
        /// The context created with the classes loaded. If you want a copy of this use GetContext.
        /// </summary>
        internal static InstanceContext StaticContext { get; set; }
        static readonly object _lock = new object();
        /// <summary>
        /// Indicates that the context has been loaded
        /// </summary>
        static bool _contextLoaded = false;

        #endregion

        /// <summary>
        /// The context constructor should only be called once. After the context has been created call GetContext for specific copies
        /// </summary>
        /// <param name="loader">The loader used to load classes.</param>
        public Context(params IConfigurationLoader [] loaders)
        {
            //the setup must only run if the context has not been setup
            //second attempts to setup a context should throw an error
            if (!_contextLoaded)
            {
                lock (_lock)
                {
                    if (!_contextLoaded)
                    {

                        //load all classes
                        List<SitecoreClassConfig> configs = new List<SitecoreClassConfig>();
                        List<AbstractSitecoreDataHandler> dataHandlers = new List<AbstractSitecoreDataHandler>();
                       
                        foreach(var loader in loaders){
                            configs.AddRange(loader.Load());
                            dataHandlers.AddRange(loader.DataHandlers);
                        }
                                                
                        var classes = configs.ToDictionary();

                        InstanceContext instance = new InstanceContext(classes, dataHandlers);
                        StaticContext = instance;

                        //now assign a data handler to each property
                        foreach (var cls in classes)
                        {
                            IList<AbstractSitecoreDataHandler> handlers = new List<AbstractSitecoreDataHandler>();

                            //create constructors

                            CreateConstructorDelegates(cls.Value);

                            foreach (var prop in cls.Value.Properties)
                            {

                                var handler = instance.GetDataHandler(prop);

                                //set the ID property of the class
                                //the ID property is needed later for writing and page editing, 
                                //saves time having to look it
                                if (prop.Attribute is SitecoreIdAttribute)
                                    cls.Value.IdProperty = prop;
                                else if (prop.Attribute is SitecoreInfoAttribute
                                    && prop.Attribute.CastTo<SitecoreInfoAttribute>().Type == SitecoreInfoType.Language)
                                    cls.Value.LanguageProperty = prop;
                                else if (prop.Attribute is SitecoreInfoAttribute
                                   && prop.Attribute.CastTo<SitecoreInfoAttribute>().Type == SitecoreInfoType.Version)
                                    cls.Value.VersionProperty = prop;


                                handlers.Add(handler);

                            }
                            cls.Value.DataHandlers = handlers;
                        }



                    }
                }
            }
            else
                throw new MapperException("Context already loaded");

        }

        /// <summary>
        /// The context constructor should only be called once. After the context has been created call GetContext for specific copies
        /// </summary>
        /// <param name="loader">The loader used to load classes.</param>
        /// <param name="datas">Custom data handlers.</param>
        [Obsolete("Assign AbstractDataHandlers to the IConfigurationLoader.DataHandlers property instead of using this contructor")]
        public Context(IConfigurationLoader loader, IEnumerable<AbstractSitecoreDataHandler> datas) :this(new TemporaryConfigurationLoader(loader, datas))        
        {
            
        }
        /// <summary>
        /// A temporary class to bridge the gap between the original method of loading classes and the new solution
        /// </summary>
        private class TemporaryConfigurationLoader:IConfigurationLoader{

            IConfigurationLoader _loader;

            public TemporaryConfigurationLoader(IConfigurationLoader loader, IEnumerable<AbstractSitecoreDataHandler> datas)
            {
                _loader = loader;
                DataHandlers = loader.DataHandlers;
                datas.ForEach(x => DataHandlers.Add(x));
            }

            public IEnumerable<SitecoreClassConfig> Load()
            {
                return _loader.Load();
            }

            public IList<AbstractSitecoreDataHandler> DataHandlers
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Returns a delegate method that will load a class based on its constuctor
        /// </summary>
        /// <param name="classConfig">The SitecoreClassConfig to store the delegate method in</param>
        /// <param name="constructorParameters">The list of contructor parameters</param>
        /// <param name="delegateType">The type of the delegate function to create</param>
        /// <returns></returns>
        internal static void CreateConstructorDelegates(SitecoreClassConfig classConfig)
        {
            Type type = classConfig.Type;

            var constructors = type.GetConstructors();

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                var dynMethod = new DynamicMethod("DM$OBJ_FACTORY_" + type.Name, type, parameters.Select(x => x.ParameterType).ToArray(), type);

                ILGenerator ilGen = dynMethod.GetILGenerator();
                for (int i = 0; i < parameters.Count(); i++)
                {
                    ilGen.Emit(OpCodes.Ldarg, i);
                }

                ilGen.Emit(OpCodes.Newobj, constructor);

                ilGen.Emit(OpCodes.Ret);

                Type genericType = null;
                switch (parameters.Count())
                {
                    case 0:
                        genericType = typeof(Func<>);
                        break;
                    case 1:
                         genericType = typeof(Func<,>);
                         break;
                    case 2:
                         genericType = typeof(Func<,,>);
                         break;
                    case 3:
                         genericType = typeof(Func<,,,>);
                        break;
                    case 4:
                         genericType = typeof(Func<,,,,>);
                         break;
                    default:
                         throw new MapperException("Only supports constructors with  a maximum of 4 parameters");
                }

                var delegateType = genericType.MakeGenericType(parameters.Select(x=>x.ParameterType).Concat(new []{ type}).ToArray());


                classConfig.CreateObjectMethods[constructor] = dynMethod.CreateDelegate(delegateType);
            }
        }

        /// <summary>
        /// Creates an instance of the context that can be used by the calling class      
        /// </summary>
        /// <returns></returns>
        internal static InstanceContext GetContext()
        {
            if (StaticContext == null) throw new MapperException("Context has not been loaded");

            //due to changes in the way that handlers are created we should no longer need to clone the instance context
            return StaticContext; //.Clone() as InstanceContext;
        }

       

        

       
    }
}
