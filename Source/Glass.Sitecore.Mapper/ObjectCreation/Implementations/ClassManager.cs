using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.ObjectCreation.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class ClassManager : ObjectManager, IObjectManager
    {
        /// <summary>
        /// </summary>
        /// <param name="service"></param>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <param name="type"></param>
        /// <param name="item"></param>
        /// <param name="constructorParameters"></param>
        /// <returns></returns>
        public override object CreateClass(ClassLoadingState state)
        {
            return CreateObject(state);
        }

       
    }
}
