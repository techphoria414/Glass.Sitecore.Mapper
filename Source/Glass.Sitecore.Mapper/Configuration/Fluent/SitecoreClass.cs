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
    /// <summary>
    /// Indicates that the .Net class can be loaded by Glass.Sitecore.Mapper
    /// </summary>
    public class SitecoreClass<T> : ISitecoreClass, ISitecoreClassFields<T>, ISitecoreClassInfos<T>, ISitecoreClassQueries<T>, ISitecoreClassItems<T>, ISitecoreLinkedItems<T>
    {
       
        List<SitecoreProperty> _properties;
        SitecoreClassConfig _config;


        public SitecoreClass(params AbstractSitecoreAttributeBuilder<T> [] configs):base()
        {
           
        }

        public SitecoreClass()
        {
            _properties = new List<SitecoreProperty>();
            _config = new SitecoreClassConfig();
            _config.Type = typeof(T);
            _config.ClassAttribute = new Configuration.Attributes.SitecoreClassAttribute();
            _config.Properties = _properties;

            
        }
        /// <summary>
        /// Indicates the template to use when trying to create an item
        /// </summary>
        public SitecoreClass<T> TemplateId(Guid id)
        {
            _config.ClassAttribute.TemplateId = id.ToString();
            return this;
        }
        /// <summary>
        /// Indicates the branch to use when trying to create and item. If a template id is also specified the template Id will be use instead.
        /// </summary>
        public SitecoreClass<T> BranchId(Guid id)
        {
            _config.ClassAttribute.BranchId = id.ToString();
            return this;
        }
       

        public SitecoreChildren<T> Children(Expression<Func<T, object>> ex)
        {
            SitecoreChildren<T> builder = new SitecoreChildren<T>(ex);
            AddProperty(builder);
            return builder;
        }

        


        public SitecoreField<T> Field(Expression<Func<T, object>> ex){
            SitecoreField<T> builder = new SitecoreField<T>(ex);
            AddProperty(builder);
            
            return builder;
        }

      

        public SitecoreId<T> Id(Expression<Func<T, object>> ex)
        {
            SitecoreId<T> builder = new SitecoreId<T>(ex);
            AddProperty(builder);
            return builder;
        }
        public SitecoreInfo<T> Info(Expression<Func<T, object>> ex)
        {
            SitecoreInfo<T> builder = new SitecoreInfo<T>(ex);
            AddProperty(builder);
            return builder;
        }
        public SitecoreParent<T> Parent(Expression<Func<T, object>> ex)
        {
            SitecoreParent<T> builder = new SitecoreParent<T>(ex);
            AddProperty(builder);
            return builder;
        }
        public SitecoreQuery<T> Query(Expression<Func<T, object>> ex)
        {
            SitecoreQuery<T> builder = new SitecoreQuery<T>(ex);
            AddProperty(builder);
            return builder;
        }

        public SitecoreItem<T> Item(Expression<Func<T, object>> ex)
        {
            SitecoreItem<T> builder = new SitecoreItem<T>(ex);
            AddProperty(builder);
            return builder;
        }

        public SitecoreLinked<T> Linked(Expression<Func<T, object>> ex)
        {
            SitecoreLinked<T> builder = new SitecoreLinked<T>(ex);
            AddProperty(builder);
            return builder;
        }


        public SitecoreClass<T> Fields(Action<ISitecoreClassFields<T>> fields)
        {
            fields.Invoke(this);
            return this;
        }

        public SitecoreClass<T> Infos(Action<ISitecoreClassInfos<T>> infos)
        {
            infos.Invoke(this);
            return this;
        }
        public SitecoreClass<T> Queries(Action<ISitecoreClassQueries<T>> queries)
        {
            queries.Invoke(this);
            return this;
        }
        public SitecoreClass<T> Items(Action<ISitecoreClassItems<T>> items)
        {
            items.Invoke(this);
            return this;
        }
        public SitecoreClass<T> Links(Action<ISitecoreLinkedItems<T>> links)
        {
            links.Invoke(this);
            return this;
        }
        



        private void AddProperty(AbstractSitecoreAttributeBuilder<T> builder){
            Configuration.Attributes.AbstractSitecorePropertyAttribute attr = builder.Attribute;
            Expression<Func<T, object>> ex = builder.Expression;
            SitecoreProperty property = new SitecoreProperty();
            if (ex.Parameters.Count > 1)
                throw new MapperException("To many parameters in linq expression {0}".Formatted(ex.Body));
                
            
            property.Property = Utility.GetPropertyInfo(_config.Type, ex.Body);
            property.Attribute = attr;
            this._properties.Add(property);
        }
        
       

        #region ISitecoreClass Members

        public SitecoreClassConfig Config
        {
            get { return _config; }
        }

        #endregion

        
    }
    #region Interfaces

    public interface ISitecoreClassFields<T>
    {
        SitecoreField<T> Field(Expression<Func<T, object>> ex);
    }
    public interface ISitecoreClassInfos<T>
    {
        SitecoreInfo<T> Info(Expression<Func<T, object>> ex);
    }
    public interface ISitecoreClassQueries<T>
    {
        SitecoreQuery<T> Query(Expression<Func<T, object>> ex);
    }
    public interface ISitecoreClassItems<T>
    {
        SitecoreItem<T> Item(Expression<Func<T, object>> ex);
    }
    public interface ISitecoreLinkedItems<T>
    {
        SitecoreLinked<T> Linked(Expression<Func<T, object>> ex);
    }

    #endregion
}
 