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
using Glass.Sitecore.Mapper.Configuration.Attributes;
using System.Linq.Expressions;

namespace Glass.Sitecore.Mapper.Configuration.Fluent
{
    public abstract class AbstractSitecoreAttributeBuilder<T>
    {
        public AbstractSitecoreAttributeBuilder(Expression<Func<T, object>> ex)
        {
            Expression = ex;
        }
        internal abstract AbstractSitecorePropertyAttribute Attribute { get; }
        internal Expression<Func<T, object>> Expression { get; private set; }
    }
}
