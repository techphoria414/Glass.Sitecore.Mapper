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
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Glass.Sitecore.Mapper.Configuration.Fluent
{
    public class SitecoreClass<T> : ISitecoreClass
    {
       
        IList<SitecoreProperty> _properties;
        SitecoreClassConfig _config;

        public SitecoreClass()
        {
            _properties = new List<SitecoreProperty>();
            _config = new SitecoreClassConfig();
            _config.Type = typeof(T);
            _config.ClassAttribute = new Configuration.Attributes.SitecoreClassAttribute();
            _config.Properties = _properties;

            
        }

        public SitecoreClass<T> TemplateId(Guid id)
        {
            _config.ClassAttribute.TemplateId = id.ToString();
            return this;
        }
        public SitecoreClass<T> BranchId(Guid id)
        {
            _config.ClassAttribute.BranchId = id.ToString();
            return this;
        }
       

        public SitecoreChildren<T> Children(Expression<Func<T, object>> ex)
        {
            SitecoreChildren<T> builder = new SitecoreChildren<T>();
            AddProperty(builder.Attribute, ex);
            return builder;
        }
        public SitecoreField<T> Field(Expression<Func<T, object>> ex){
            SitecoreField<T> builder = new SitecoreField<T>();
            AddProperty(builder.Attribute, ex);
            return builder;
        }
        public SitecoreId<T> Id(Expression<Func<T, object>> ex)
        {
            SitecoreId<T> info = new SitecoreId<T>();
            AddProperty(info.Attribute, ex);
            return info;
        }
        public SitecoreInfo<T> Info(Expression<Func<T, object>> ex)
        {
            SitecoreInfo<T> builder = new SitecoreInfo<T>();
            AddProperty(builder.Attribute, ex);
            return builder;
        }
        public SitecoreParent<T> Parent(Expression<Func<T, object>> ex)
        {
            SitecoreParent<T> builder = new SitecoreParent<T>();
            AddProperty(builder.Attribute, ex);
            return builder;
        }
        public SitecoreQuery<T> Query(Expression<Func<T, object>> ex)
        {
            SitecoreQuery<T> builder = new SitecoreQuery<T>();
            AddProperty(builder.Attribute, ex);
            return builder;
        }
        private void AddProperty(Configuration.Attributes.AbstractSitecorePropertyAttribute attr, Expression<Func<T, object>> ex){
            SitecoreProperty property = new SitecoreProperty();
            if (ex.Parameters.Count > 1)
                throw new MapperException("To many parameters in linq expression {0}".Formatted(ex.Body));
                
            
            property.Property = GetInfo(ex.Body.ToString(), ex.Parameters[0].Name);
            property.Attribute = attr;
            this._properties.Add(property);
        }
        
        private PropertyInfo GetInfo(string expressionBody, string parameter)
        {
            string regExTest = "{0}\\.(?'name'[^\\s\\)}}\\.]+)".Formatted(parameter);

            Match match = Regex.Match(expressionBody, regExTest);
            if (match == null || match.Groups["name"] == null) throw new MapperException("Can not determine property name from linq expression {0}.".Formatted(expressionBody));

            string name = match.Groups["name"].Value;

            PropertyInfo info = _config.Type.GetProperty(name);
            return info;
        }

        #region ISitecoreClass Members

        public SitecoreClassConfig Config
        {
            get { return _config; }
        }

        #endregion
    }
}
 