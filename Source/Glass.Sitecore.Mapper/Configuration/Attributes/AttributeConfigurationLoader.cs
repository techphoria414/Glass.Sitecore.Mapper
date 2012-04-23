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

namespace Glass.Sitecore.Mapper.Configuration.Attributes
{
    public class AttributeConfigurationLoader : AbstractConfigurationLoader
    {
       

        IEnumerable<string> _namespaces;

        public AttributeConfigurationLoader(params string [] namespaces)
        {
            _namespaces = namespaces;
        }

        #region AbstractConfigurationLoader Members

        public override IEnumerable<SitecoreClassConfig> Load()
        {
            if (_namespaces == null || _namespaces.Count() == 0) return new List<SitecoreClassConfig>();

            IDictionary<Type, SitecoreClassConfig> classes = LoadClasses();

            classes.ForEach(x => x.Value.Properties = GetProperties(x.Value.Type));

            return classes.Select(x => x.Value);
        }


        public virtual IDictionary<Type, SitecoreClassConfig> LoadClasses()
        {
            var classes =  new Dictionary<Type, SitecoreClassConfig>();
            foreach (string space in _namespaces)
            {
                string[] parts = space.Split(',');
                var namespaceClasses = GetClass(parts[1], parts[0]);
                namespaceClasses.ForEach(cls =>
                {
                    //stops duplicates being added
                    if (!classes.ContainsKey(cls.Type))
                    {
                        classes.Add(cls.Type, cls);
                    }
                });

            };

            return classes;
        }
      

        #endregion


        public static  IEnumerable<SitecoreProperty> GetProperties(Type type)
        {
            IEnumerable<PropertyInfo> properties = Utility.GetAllProperties(type);

            return properties.Select(x =>
            {
               return GetProperty(x);
            }).Where(x=>x != null).ToList();
            
        }

        public static SitecoreProperty GetProperty(PropertyInfo info)
        {
            var attrs = info.GetCustomAttributes(true);
            var attr = attrs.FirstOrDefault(y => y is AbstractSitecorePropertyAttribute) as AbstractSitecorePropertyAttribute;

            if (attr != null)
            {
                return new SitecoreProperty()
                {
                    Attribute = attr,
                    Property = info
                };
            }
            else return null;
        }

     

        private IEnumerable<SitecoreClassConfig> GetClass(string assembly, string namesp)
        {
            Assembly assem = Assembly.Load(assembly);

            if (assem != null)
            {
                try
                {

                    return assem.GetTypes().Select(x =>
                    {
                        if (x != null && x.Namespace != null && (x.Namespace.Equals(namesp) || x.Namespace.StartsWith(namesp + ".")))
                        {
                            IEnumerable<object> attrs = x.GetCustomAttributes(true);
                            SitecoreClassAttribute attr = attrs.FirstOrDefault(y => y is SitecoreClassAttribute) as SitecoreClassAttribute;

                            if (attr != null)
                            {
                                var config = new SitecoreClassConfig()
                                {
                                    Type = x,
                                    ClassAttribute = attr,

                                };
                                //TODO need to wrap in exception handler
                                if (!attr.BranchId.IsNullOrEmpty()) config.BranchId = new Guid(attr.BranchId);
                                if (!attr.TemplateId.IsNullOrEmpty()) config.TemplateId = new Guid(attr.TemplateId);

                                return config;
                            }
                        }
                        return null;
                    }).Where(x => x != null).ToList();

                }
                catch (ReflectionTypeLoadException ex)
                {

                    
                    throw new MapperException("Failed to load types {0}".Formatted(ex.LoaderExceptions.First().Message), ex);

                }
            }
            else
            {
                return new List<SitecoreClassConfig>();
            }
            
        }
    }
}
