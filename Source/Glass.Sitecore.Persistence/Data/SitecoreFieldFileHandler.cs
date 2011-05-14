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
using Glass.Sitecore.Persistence.FieldTypes;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Persistence.Data
{
    public class SitecoreFieldFileHandler : AbstractSitecoreField
    {

        public override object GetValue(object parent, global::Sitecore.Data.Items.Item item, Glass.Sitecore.Persistence.Configuration.SitecoreProperty property, InstanceContext context)
        {

            string fieldName = GetFieldName(property);

            FileField field = new FileField(item.Fields[fieldName]);
            File file = new File();
            file.Src = field.Src;
            file.Id = field.MediaID.Guid;

            return file;
        }

        public override void SetValue(object parent, global::Sitecore.Data.Items.Item item, object value, Glass.Sitecore.Persistence.Configuration.SitecoreProperty property, InstanceContext context)
        {
            string fieldName = GetFieldName(property);

            File  file = value as File;
            FileField field = new FileField(item.Fields[fieldName]);

            if (field.MediaID.Guid != file.Id)
            {
                if (file.Id == Guid.Empty)
                {
                    ItemLink link = new ItemLink(item.Database.Name, item.ID, null, field.MediaItem.Database.Name, field.MediaID, field.MediaItem.Paths.FullPath);
                    field.RemoveLink(link);
                }
                else
                {
                    ID newId = new ID(file.Id);
                    Item target = item.Database.GetItem(newId);
                    if (target != null)
                    {
                        field.MediaID = newId;
                        ItemLink link = new ItemLink(item.Database.Name, item.ID, null, target.Database.Name, target.ID, target.Paths.FullPath);
                        field.UpdateLink(link);
                    }
                    else throw new PersistenceException("No item with ID {0}. Can not update File Item field".Formatted(newId));
                }
            }

        }

        public override object GetFieldValue(string fieldValue, object parent, global::Sitecore.Data.Items.Item item, Glass.Sitecore.Persistence.Configuration.SitecoreProperty property, InstanceContext context)
        {
            throw new NotImplementedException();
        }

        public override string SetFieldValue(Type returnType, object value, InstanceContext context)
        {
            throw new NotImplementedException();
        }

        public override Type TypeHandled
        {
            get { return typeof(File); }
        }
    }
}
