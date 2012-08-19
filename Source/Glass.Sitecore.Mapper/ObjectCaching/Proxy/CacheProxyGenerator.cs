using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.ObjectCaching.Proxy
{

    public class CacheProxyGenerator
    {
        private static readonly Castle.DynamicProxy.ProxyGenerator _generator = new Castle.DynamicProxy.ProxyGenerator();
        private static readonly Castle.DynamicProxy.ProxyGenerationOptions _options = new Castle.DynamicProxy.ProxyGenerationOptions(new CacheProxyGeneratorHook());


        public static object CreateProxy(object originalTarget)
        {
            Type type = originalTarget.GetType();

            var proxy = _generator.CreateClassProxy(type, _options, new CacheMethodInterceptor(originalTarget));

            return proxy;
        }
    }
}
