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
using System.Reflection;
using Sitecore.Data.Items;
using Glass.Sitecore.Persistence.FieldTypes;
using Glass.Sitecore.Persistence.Data;
using Glass.Sitecore.Persistence.Configuration;

namespace Glass.Sitecore.Persistence.Proxies
{
    //Trying to work out how to do lazy loading on streams in to a class
    //Shelving this for the moment because it needs more thought

    //    public class ProxyBlobInterceptor : IInterceptor
    //    {
    //        object _parent = null;
    //        SitecoreProperty _property = null;
    //        InstanceContext _context = null;
    //        Item _item = null;

    //        public ProxyBlobInterceptor(object parent,
    //            SitecoreProperty property, InstanceContext context, Item item)
    //        {
    //            _parent = parent;
    //            _property = property;
    //            _context = context;
    //            _item = item;


    //        }

    //        #region IInterceptor Members

    //        public void Intercept(IInvocation invocation)
    //        {

    //            //create class
    //            Blob blob = new Blob();




    //            _property.Property.SetValue(_parent, blob, null);
    //            //if the invocation was kicked off by a set then we have to set the value on the actual object
    //            if (invocation.Method != null && Utility.IsSetMethod(invocation.Method))
    //            {
    //                invocation.Method.Invoke(blob, invocation.Arguments);
    //            }
    //            else if (invocation.Method != null && Utility.IsGetMethod(invocation.Method))
    //            {
    //                blob.Stream = SitecoreFieldStreamHandler handler = new SitecoreFieldStreamHandler();
    //              handler.GetValue(_parent, _item, _property, _context);
    //            }




    //            invocation.Proceed();
    //        }

    //        #endregion
    //    }
    //}
}