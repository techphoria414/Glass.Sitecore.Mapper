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

using System.Reflection;
using Sitecore.Data.Items;
using System.Collections;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Proxies
{
    public class ProxyGenerator
    {
        private static readonly Castle.DynamicProxy.ProxyGenerator _generator = new Castle.DynamicProxy.ProxyGenerator();

       
        public static object CreateProxy(SitecoreClassConfig config,  ISitecoreService service, Item item){
            var options = new Castle.DynamicProxy.ProxyGenerationOptions(new ProxyGeneratorHook() );
            object proxy = null;

            Type type = config.Type;

            if (type.IsInterface)
            {
                proxy = _generator.CreateInterfaceProxyWithoutTarget(type, new InterfaceMethodInterceptor(config, item, service));
            }
            else
            {
                proxy = _generator.CreateClassProxy(type, options, new ProxyClassInterceptor(type,
                   service,
                   item));
            }
            return proxy;

        }
       
    }
}
