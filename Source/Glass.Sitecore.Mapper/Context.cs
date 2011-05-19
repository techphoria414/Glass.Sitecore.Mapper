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
using log4net;
using Glass.Sitecore.Mapper.Data;

namespace Glass.Sitecore.Mapper
{
    public class Context
    {
        

        #region STATICS

        /// <summary>
        /// The context created with the classes loaded
        /// </summary>
        internal static InstanceContext StaticContext { get; set; }
        static readonly object _lock = new object();
        /// <summary>
        /// Indicates that the context has been loaded
        /// </summary>
        static bool _contextLoaded = false;

        #endregion

        ILog _log = new NullLog();
        public ILog Log
        {
            get
            {
                return _log;
            }
            set
            {
                _log = value;
            }
        
        }


        /// <summary>
        /// The context constructor should only be called once. After the context has been created call GetContext for specific copies
        /// </summary>
        /// <param name="loader"></param>
        public Context(IConfigurationLoader loader, IEnumerable<ISitecoreDataHandler> datas)
        {
            //the setup must only run if the context has not been setup
            //second attempts to setup a context should throw an error
            if (!_contextLoaded)
            {
                lock (_lock)
                {
                    if (!_contextLoaded)
                    {
                        Log.Info("Context loading");

                        var classes = loader.Load();
                        InstanceContext instance = new InstanceContext(classes, datas);
                        StaticContext = instance;

                        Log.Info("Context loaded");
                    }
                }
            }
            else
                Log.Info("Context already loaded");
        }

        /// <summary>
        /// Creates an instance of the context that can be used by the calling class      
        /// </summary>
        /// <returns></returns>
        internal static InstanceContext GetContext()
        {
            return StaticContext.Clone() as InstanceContext;
        }

       
    }
}
