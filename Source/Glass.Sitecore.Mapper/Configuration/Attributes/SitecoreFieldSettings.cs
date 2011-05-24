using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Sitecore.Mapper.Configuration.Attributes
{
    public enum SitecoreFieldSettings
    {
        /// <summary>
        /// The field carries out its default behaviour
        /// </summary>
        Default,
        /// <summary>
        /// If used on a Rich Text field it stops the contents going through the render process 
        /// and returns the raw HTML of the field
        /// </summary>
        RichTextRaw,
        /// <summary>
        /// If the property type is another classes loaded by the  Mapper, indicates that the class should not be lazy loaded.
        /// </summary>
        DontLoadLazily 

    }
}
