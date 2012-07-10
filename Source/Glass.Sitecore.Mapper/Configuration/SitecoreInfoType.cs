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

namespace Glass.Sitecore.Mapper.Configuration
{
    public enum SitecoreInfoType
    {
        /// <summary>
        /// No value has been set
        /// </summary>
        [DisplayName("Not set")]
        NotSet,
        /// <summary>
        /// The item's content path. The property type must be System.String
        /// </summary>
        [DisplayName("Content path")]
        ContentPath,
        /// <summary>
        /// The item's display name. The property type must be System.String
        /// </summary>
        [DisplayName("Display name")]
        DisplayName,
        /// <summary>
        /// The item's full path. The property type must be System.String
        /// </summary>
        [DisplayName("Full path")]
        FullPath,
        /// <summary>
        /// The item's key . The property type must be System.String
        /// </summary>
        [DisplayName("Key")]
        Key,
        /// <summary>
        /// The item's media URL. The property type must be System.String
        /// </summary>
        [DisplayName("Media URL")]
        MediaUrl,
        /// <summary>
        /// The item's path. The property type must be System.String
        /// </summary>
        [DisplayName("Path")]
        Path,
        /// <summary>
        /// The item's template Id. The property type must be System.Guid
        /// </summary>
        [DisplayName("Template ID")]
        TemplateId,
        /// <summary>
        /// The item's template name. The property type must be System.String
        /// </summary>
        [DisplayName("Template name")]
        TemplateName,
        /// <summary>
        /// The item's URL. The property type must be System.String
        /// </summary>
        [DisplayName("Url")]
        Url,
        /// <summary>
        //The item's Absolute URL. The property type must be System.String
        /// </summary>
        [Obsolete("Use SitecoreInfoType.Url with UrlOption = SitecoreInfoUrlOptions.AlwaysIncludeServerUrl")]
        [DisplayName("Full Url")]
        FullUrl,
        /// <summary>
        /// The item's version. The property type must be System.Int32
        /// </summary>
        [DisplayName("Version")]
        Version,
        /// <summary>
        /// The item's Name. The property type must be System.String
        /// </summary>
        [DisplayName("Name")]
        Name,
        /// <summary>
        /// The items language. The property type must be Sitecore.Globalization.Language
        /// </summary>
        [DisplayName("Language")]
        Language
    }
}
