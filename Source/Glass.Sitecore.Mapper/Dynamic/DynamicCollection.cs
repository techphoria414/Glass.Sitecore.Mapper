using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Collections;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;

namespace Glass.Sitecore.Mapper.Dynamic
{
    public class DynamicCollection : DynamicObject, IEnumerable<DynamicItem>
    {
        IEnumerable<DynamicItem> _collection;

        public DynamicCollection(IEnumerable<DynamicItem> collection)
        {
            _collection = collection;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            string method = binder.Name;
            
            bool hasArg = args.Length == 1;

            result = null;
            
            switch (method)
            {
                case "First":
                    result = hasArg ? _collection.First((Func<dynamic, bool>)args[0]) : _collection.First();
                    
                    break;
                case "Last":
                    result = hasArg ? _collection.Last((Func<dynamic, bool>)args[0]) : _collection.Last();
                    break;
                case "FirstOrDefault":
                    result = hasArg ? _collection.FirstOrDefault((Func<dynamic, bool>)args[0]) : _collection.FirstOrDefault();
                    break;
                case "LastOrDefault":
                    result = hasArg ? _collection.LastOrDefault((Func<dynamic, bool>)args[0]) : _collection.LastOrDefault();
                    break;
                case "Count":
                    result = _collection.Count();
                    break;
                case "ElementAt":
                    result = _collection.ElementAt((int)args[0]);
                    break;
                case "Where":
                    var arrayWhere = _collection.Where((Func<dynamic, bool>)args[0]).Select(x=> x as DynamicItem); 
                    result = new DynamicCollection(arrayWhere);
                    break;
                case "Any":
                    result = hasArg ? _collection.Any((Func<dynamic, bool>)args[0]) : _collection.Any();
                    break;
                case "All":
                    result = _collection.All((Func<dynamic, bool>)args[0]);
                    break;
                case "Select":
                    var type =  args[0].GetType();
                    var generic2 = type.GetGenericArguments()[1];

                    var enumGeneric = typeof(List<>);
                    var enumType = enumGeneric.MakeGenericType(generic2);
                    
                    var list = Activator.CreateInstance(enumType);

                    foreach (var item in _collection)
                    {
                      var newItem =  type.InvokeMember("Invoke", BindingFlags.DeclaredOnly |
                            BindingFlags.Public | BindingFlags.NonPublic |
                            BindingFlags.Instance | BindingFlags.InvokeMethod, null, args[0], new object[]{ item});

                      enumType.InvokeMember("Add", BindingFlags.DeclaredOnly |
                         BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Instance | BindingFlags.InvokeMethod, null, list, new object[]{newItem});

                    }
                    result = list;
                    break;


            }

            return true;
        }

        public IEnumerator<DynamicItem> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }

}
