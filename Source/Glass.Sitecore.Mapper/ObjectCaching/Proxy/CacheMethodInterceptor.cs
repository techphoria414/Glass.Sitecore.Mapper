using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Sitecore.Mapper.Configuration;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper.ObjectCaching.Proxy
{
    public class CacheMethodInterceptor : IInterceptor
    {

        Dictionary<string, object> _values;
        object _originalTarget;

        public CacheMethodInterceptor(object originalTarget)
        {
            _values = new Dictionary<string, object>();
            _originalTarget = originalTarget;                
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {

            if (invocation.Method.IsSpecialName)
            {
                if (invocation.Method.Name.StartsWith("get_") || invocation.Method.Name.StartsWith("set_"))
                {
                    //Must be a property

                     string method = invocation.Method.Name.Substring(0, 4);
                    string name = invocation.Method.Name.Substring(4);

                    //if the dictionary contains the name then a value must have been set
                    if (_values.ContainsKey(name) && method == "get_")
                    {
                        invocation.ReturnValue = _values[name];
                        return;
                    }
                    else if (method == "set_")
                    {
                        _values[name] = invocation.Arguments[0];
                        return;
                    }
                    
                }
            }
                
            invocation.ReturnValue = invocation.Method.Invoke(_originalTarget, invocation.Arguments);

        }



        #endregion
    }
}
