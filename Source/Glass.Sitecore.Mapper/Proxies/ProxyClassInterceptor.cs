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
using Castle.DynamicProxy;
using System.Reflection;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper.Proxies
{
    public class ProxyClassInterceptor : IInterceptor 
    {
        ISitecoreService _service = null;
        Item _item = null;
        Type _type;
        object _actual = null;
        bool _inferType = false;

        public ProxyClassInterceptor(Type type, ISitecoreService service, Item item, bool inferType)
        {
            _service = service;
            _item = item;
            _type = type;
            _inferType = inferType;
            
        }


        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            //create class
            if (_actual == null)
            {
                _actual = _service.CreateClass(false, _inferType, _type, _item);
            }

            invocation.ReturnValue = invocation.Method.Invoke(_actual, invocation.Arguments);

        }

        #endregion

      
    }
}
