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
using System.Reflection;
using Sitecore.Data.Items;
using System.Collections;
using Glass.Sitecore.Persistence.Proxies;

namespace Glass.Sitecore.Persistence
{
    public static class Utility
    {
        public static Type GetGenericArgument(Type type)
        {
            Type[] types = type.GetGenericArguments();
            if(types.Count() > 1) throw new PersistenceException("Type {0} has more than one generic argument".Formatted(type.FullName));
            return types[0];
        }
        public static Type GetGenericOuter(Type type)
        {
            return type.GetGenericTypeDefinition();
        }

        /// <summary>
        /// Will call the add method on a list via reflection to add the items
        /// </summary>
        /// <param name="items">Items to add to the list</param>
        /// <param name="list">The list to call the add method on</param>
        public static void CallAddMethod(IEnumerable<object> items, object list)
        {
            MethodInfo addMethod = list.GetType().GetMethod("Add");

            items.ForEach(x =>
            {
                addMethod.Invoke(list, new object[] { x });
            });
        }

        /// <summary>
        /// Creates a generic type via reflection
        /// </summary>
        /// <param name="type">The generic type to create e.g. List&lt;&gt;</param>
        /// <param name="arguments">The list of subtypes for the generic type, e.g string in List&lt;string&gt;</param>
        /// <returns></returns>
        public static object CreateGenericType(Type type, Type[] arguments)
        {
            return CreateGenericType(type, arguments, null);
        }
        /// <summary>
        /// </summary>
        /// <param name="type">The generic type to create e.g. List&lt;&gt;</param>
        /// <param name="arguments">The list of subtypes for the generic type, e.g string in List&lt;string&gt;</param>
        /// <param name="parameters"> List of parameters to pass to the constructor.</param>
        /// <returns></returns>
        public static object CreateGenericType(Type type, Type[] arguments, params  object[] parameters)
        {
            Type genericType = type.MakeGenericType(arguments);
            object obj;
            if (parameters != null && parameters.Count() > 0)
                obj = Activator.CreateInstance(genericType, parameters);
            else
                obj = Activator.CreateInstance(genericType);
            return obj;
            
        }

        public static bool IsSetMethod(MethodInfo info){
            return info.IsSpecialName && info.Name.StartsWith("set_");
        }
        public static bool IsGetMethod(MethodInfo info)
        {
            return info.IsSpecialName && info.Name.StartsWith("get_");
        }

        
    }
}
