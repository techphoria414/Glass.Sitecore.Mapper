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
        InstanceContext _context;

        Dictionary<string, object> _values;
        bool _isLoaded = false;

        public InterfaceMethodInterceptor(SitecoreClassConfig config, Item item, InstanceContext context){
            _config = config;
            _item = item;
            _context = context;
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
                    var result = handler.GetValue(_item, _context);
                    _values[handler.Property.Name] = result;
                }
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
