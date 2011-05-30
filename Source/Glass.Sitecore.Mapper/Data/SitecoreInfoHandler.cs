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
using Sitecore.Links;
using Glass.Sitecore.Mapper.Configuration;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreInfoHandler : AbstractSitecoreDataHandler
    {
        #region ISitecoreDataHandler Members

        public override bool WillHandle(Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, IEnumerable<AbstractSitecoreDataHandler> datas, Dictionary<Type, SitecoreClassConfig> classes)
        {
            return property.Attribute is SitecoreInfoAttribute;
        }

        public override object GetValue(object target, global::Sitecore.Data.Items.Item item, Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
        {
            SitecoreInfoAttribute attr = property.Attribute as SitecoreInfoAttribute;

            switch (attr.Type)
            {
                case SitecoreInfoType.ContentPath:
                    return item.Paths.ContentPath;
                case SitecoreInfoType.DisplayName:
                    return item.DisplayName;                
                case SitecoreInfoType.FullPath:
                    return item.Paths.FullPath;
                case SitecoreInfoType.Key:
                    return item.Key;                    
                case SitecoreInfoType.MediaUrl:
                    global::Sitecore.Data.Items.MediaItem media = new global::Sitecore.Data.Items.MediaItem(item);
                    return global::Sitecore.Resources.Media.MediaManager.GetMediaUrl(media);
                    break;
                case SitecoreInfoType.Path:
                    return item.Paths.Path;                    
                case SitecoreInfoType.TemplateId:
                    return item.TemplateID.Guid;
                case SitecoreInfoType.TemplateName:
                    return item.TemplateName;                
                case SitecoreInfoType.Url:
                    return LinkManager.GetItemUrl(item);
                case SitecoreInfoType.Version:
                    return item.Version.Number;
                default:
                    throw new  NotSupportedException("Value {0} not supported".Formatted(attr.Type.ToString()));
            }
            

        }

        public override void SetValue(object target, global::Sitecore.Data.Items.Item item, object value, Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
        {
            SitecoreInfoAttribute attr = property.Attribute as SitecoreInfoAttribute;
            throw new NotSupportedException("You can not save SitecoreInfo {0}".Formatted(attr.Type));
        }

        public override bool CanSetValue(SitecoreProperty property)
        {
             return false; 
        }

        #endregion
    }
}
