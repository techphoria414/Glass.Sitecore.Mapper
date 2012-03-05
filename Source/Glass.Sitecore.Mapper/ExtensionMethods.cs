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
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper;

namespace Glass.Sitecore
{
    public static class ExtensionMethods
    {
        public static string Formatted(this string target, params object[] args)
        {
            return string.Format(target, args);
        }
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> func)
        {
            foreach (TSource item in source)
            {
                func(item);
            }
            return source;
        }
        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }
        public static bool GuidTryParse(this string target, out Guid guid)
        {

            try
            {
                guid = new Guid(target);
                return true;
            }
            catch
            {
                guid = Guid.Empty;
                return false;

            }
        }

        public static Dictionary<Type, SitecoreClassConfig> ToDictionary(this IEnumerable<SitecoreClassConfig> classes)
        {
            
            Dictionary<Type, SitecoreClassConfig> dicClasses = new Dictionary<Type, SitecoreClassConfig>();
            foreach (var cls in classes)
            {
                try
                {
                    dicClasses.Add(cls.Type, cls);
                }
                catch (ArgumentException ex)
                {
                    throw new MapperException("You are trying to load the type {0} more than once. Check that the IConfigurationLoaders aren't loading the same classes.".Formatted(cls.Type.FullName), ex);
                }
            }

            return dicClasses;

        }

        public static T CastTo<T>(this object target)
        {
            return (T)target;
        }
    }
  
}
