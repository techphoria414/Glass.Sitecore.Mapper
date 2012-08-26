using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Proxies;
using Castle.DynamicProxy;

namespace Glass.Sitecore.Mapper.ObjectCaching.Proxy
{
    public class CacheInterfaceMethodInterceptor : IInterceptor
    {
        Dictionary<string, object> _values = new Dictionary<string, object>();

        InterfaceMethodInterceptor _subInterceptor;

        public CacheInterfaceMethodInterceptor(InterfaceMethodInterceptor subInterceptor)
        {
            _subInterceptor = subInterceptor;
        }

        public  void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {

            if (invocation.Method.IsSpecialName)
            {
                if (invocation.Method.Name.StartsWith("get_") || invocation.Method.Name.StartsWith("set_"))
                {

                    string method = invocation.Method.Name.Substring(0, 4);
                    string name = invocation.Method.Name.Substring(4);

                    if (method == "get_" && _values.ContainsKey(name))
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

            _subInterceptor.Intercept(invocation);
        } 
    }
}
