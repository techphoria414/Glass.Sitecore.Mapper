using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper
{
    public class ClassLoadingState
    {

        public ClassLoadingState(ISitecoreService service, bool isLazy, bool inferType, Type type, global::Sitecore.Data.Items.Item item, Guid related, params object[] constructorParameters)
        {
            Service = service;
            IsLazy = isLazy;
            InferType = inferType;
            Type = type;
            Item = item;
            ConstructorParameters = constructorParameters;
            Related = related;
        }
        public ISitecoreService Service { get; set; } 
        public bool IsLazy { get; set; }
        public bool InferType { get; set; }
        public Type Type { get; set; }  
        public global::Sitecore.Data.Items.Item Item { get; set; }
        public object[] ConstructorParameters { get; set; }
        public Guid Related { get; set; }
    }
}
