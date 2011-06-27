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
using Castle.DynamicProxy;
using System.Reflection;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Data;

namespace Glass.Sitecore.Mapper.Proxies
{
    public class InterfaceMethodInterceptor : IInterceptor
    {
        SitecoreClassConfig _config;
        Item _item;
        ISitecoreService _service;

        Dictionary<string, object> _values;
        bool _isLoaded = false;

        public InterfaceMethodInterceptor(SitecoreClassConfig config, Item item, ISitecoreService service){
            _config = config;
            _item = item;
            _service = service;
            _values = new Dictionary<string, object>();
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {

            //do initial gets
            if (!_isLoaded)
            {
                foreach (var handler in _config.DataHandlers)
                {
                    var result = handler.GetValue(_item, _service);
                    _values[handler.Property.Name] = result;
                }
                _isLoaded = true;
            }

            if (invocation.Method.IsSpecialName)
            {
                if(invocation.Method.Name.StartsWith("get_") || invocation.Method.Name.StartsWith("set_")){
                    
                    string method = invocation.Method.Name.Substring(0, 4);
                    string name = invocation.Method.Name.Substring(4);
                    
                    if(method == "get_"){
                        var result = _values[name];
                        invocation.ReturnValue = result;
                    }
                    else if(method == "set_"){
                        _values[name] = invocation.Arguments[0];
                    }
                    else
                        throw new MapperException("Method with name {0}{1} on type {2} not supported.".Formatted(method,name, _config.Type.FullName));

                    
       
                }
               
            }

        }



        #endregion
    }
}
