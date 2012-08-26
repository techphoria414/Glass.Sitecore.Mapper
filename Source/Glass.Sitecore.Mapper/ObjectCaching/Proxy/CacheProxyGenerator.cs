using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Sitecore.Mapper.Proxies;

namespace Glass.Sitecore.Mapper.ObjectCaching.Proxy
{

    public class CacheProxyGenerator
    {
        private static readonly Castle.DynamicProxy.ProxyGenerator _generator = new Castle.DynamicProxy.ProxyGenerator();
        private static readonly Castle.DynamicProxy.ProxyGenerationOptions _options = new Castle.DynamicProxy.ProxyGenerationOptions(new CacheProxyGeneratorHook());


        public static object CreateProxy(object originalTarget)
        {
            Type type = originalTarget.GetType();

            object proxy = null ;

            //you can't proxy a proxy.
            if (originalTarget is IProxyTargetAccessor)
            {
                var oldProxy = originalTarget as IProxyTargetAccessor;
                var interceptors = oldProxy.GetInterceptors();
                if (interceptors.Any(x => x is InterfaceMethodInterceptor))
                {
                    var subInterceptor = interceptors.First(x => x is InterfaceMethodInterceptor).CastTo<InterfaceMethodInterceptor>();



                    return _generator.CreateInterfaceProxyWithoutTarget(
                        subInterceptor.Config.Type,
                        new CacheInterfaceMethodInterceptor(subInterceptor));
                        
                }
                else if (interceptors.Any(x => x is ProxyClassInterceptor))
                {

                }

            }
            else
            {
                proxy = _generator.CreateClassProxy(type, _options, new CacheMethodInterceptor(originalTarget));
            }

            return proxy;
        }
    }
}
