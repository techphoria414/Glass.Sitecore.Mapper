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
using Glass.Sitecore.Mapper.FieldTypes;
using Sitecore.Data.Fields;
using Sitecore.Data;
using Sitecore.Data.Items;
using Glass.Sitecore.Mapper.Configuration;
using Sitecore.Links;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreFieldLinkHandler : AbstractSitecoreField
    {

        public override object GetValue(object parent, global::Sitecore.Data.Items.Item item, Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
        {
            string fieldName = GetFieldName(property);

            Link link = new Link();
            LinkField field = new LinkField(item.Fields[fieldName]);
            link.Anchor = field.Anchor;
            link.Class = field.Class;
            link.Text = field.Text;
            link.Title = field.Title;
            link.Url = field.Url;
            link.TargetId = field.TargetID.Guid;
            link.Target = field.Target;

            return link;
        }

        public override void SetValue(object parent, global::Sitecore.Data.Items.Item item, object value, Glass.Sitecore.Mapper.Configuration.SitecoreProperty property, InstanceContext context)
        {
            string fieldName = GetFieldName(property);

            Link link = value as Link;
            LinkField field = new LinkField(item.Fields[fieldName]);

            if (field.TargetID.Guid != link.TargetId)
            {
                if (link.TargetId == Guid.Empty)
                {
                    ItemLink iLink = new ItemLink(item.Database.Name, item.ID, field.InnerField.ID, field.TargetItem.Database.Name, field.TargetID, field.TargetItem.Paths.FullPath);
                    field.RemoveLink(iLink);
                }
                else
                {
                    ID newId = new ID(link.TargetId);
                    Item target = item.Database.GetItem(newId);
                    if (target != null)
                    {
                        field.TargetID = newId;
                        ItemLink nLink = new ItemLink(item.Database.Name, item.ID, field.InnerField.ID, target.Database.Name, target.ID, target.Paths.FullPath);
                        field.UpdateLink(nLink);
                    }
                    else throw new MapperException("No item with ID {0}. Can not update Link field".Formatted(newId));
                }

            }


            field.Anchor = link.Anchor;
            field.Class = link.Class;
            field.Text = link.Text;
            field.Title = link.Title;
            field.Url = link.Url;
            field.TargetID = new ID(link.TargetId);
            field.Target = link.Target;

         
        }

        public override string SetFieldValue(object value, SitecoreProperty property, InstanceContext context)
        {
            throw new NotImplementedException();
        }

        public override Type TypeHandled
        {
            get { return typeof(FieldTypes.Link); }
        }

        public override object GetFieldValue(string fieldValue, object parent, Item item, SitecoreProperty property, InstanceContext context)
        {
            throw new NotImplementedException();
        }
    }
}
