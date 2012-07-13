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
using Sitecore.Links;

namespace Glass.Sitecore.Mapper.Configuration
{
    [Flags]
    public enum SitecoreInfoUrlOptions
    {
        [DisplayName("Default")]
        Default = 0x00,
        
        [DisplayName("Add aspx")]
        AddAspxExtension = 0x01,

        [DisplayName("Include server url")]
        AlwaysIncludeServerUrl = 0x02,

        [DisplayName("Encode name")]
        EncodeNames = 0x04,
      
        /// <summary>
        /// Do not use with LanguageEmbeddingAsNeeded, LanguageEmbeddingNever
        /// </summary>
        [DisplayName("Embed language always")]
        LanguageEmbeddingAlways = 0x08,
        /// <summary>
        /// Do not use with LanguageEmbeddingAlways, LanguageEmbeddingNever
        /// </summary>
        [DisplayName("Embed language as needed")]
        LanguageEmbeddingAsNeeded = 0x16,
        /// <summary>
        /// Do not use with LanguageEmbeddingAlways, LanguageEmbeddingAsNeeded
        /// </summary>
        [DisplayName("Never embed language")]
        LanguageEmbeddingNever = 0x32,

        /// <summary>
        /// Do not use with LanguageLocationQueryString
        /// </summary>
        [DisplayName("Language in file path")]
        LanguageLocationFilePath = 0x64,
        /// <summary>
        /// Do not use with LanguageLocationFilePath
        /// </summary>
        [DisplayName("Language in query string")]
        LanguageLocationQueryString = 0x128,


        [DisplayName("Shorten URLs")]
        ShortenUrls = 0x256,
        [DisplayName("Site resolving")]
        SiteResolving = 0x512,
        [DisplayName("Use display name")]
        UseUseDisplayName = 0x1024
        

    }

    
}
