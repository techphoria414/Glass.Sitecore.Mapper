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
using System.Web;
using Glass.Sitecore.Persistence.Configuration.Attributes;

namespace Glass.Sitecore.Persistence.Demo.Application.Domain
{
    [SitecoreClass(TemplateId="{D3F8D040-C346-4154-9AEA-847583FBD364}")]
    public class DemoClass
    {

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }

        [SitecoreField]
        public virtual string Title { get; set; }

        [SitecoreField]
        public virtual string Body { get; set; }
        
        [SitecoreField]
        public virtual IEnumerable<DemoClass> Links { get; set; }

    }

    [SitecoreClass]
    public class ItemA
    {
        [SitecoreChildren]
        public virtual IEnumerable<ItemB> Children { get; set; }

    }

    [SitecoreClass]
    public class ItemB
    {
    }


    
}
