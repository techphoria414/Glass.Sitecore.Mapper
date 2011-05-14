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
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using System.Reflection;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Persistence.Proxies
{
    public class ProxyClassInterceptor : IInterceptor 
    {
        InstanceContext _context =  null;
        Item _item = null;
        Type _type;
        object _actual = null;


        public ProxyClassInterceptor(Type type, InstanceContext context, Item item)
        {
            _context = context;
            _item = item;
            _type = type;
            
        }


        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            //create class
            if (_actual == null)
            {
                _actual = _context.MakeClass(_item, _type);
            }

            invocation.ReturnValue = invocation.Method.Invoke(_actual, invocation.Arguments);

        }

        #endregion

      
    }
}
