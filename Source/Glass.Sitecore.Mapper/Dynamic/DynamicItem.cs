using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using System.Dynamic;
using Glass.Sitecore.Mapper.Configuration;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Sitecore.Web.UI.WebControls;
using Glass.Sitecore.Mapper.Data;
using Sitecore.Pipelines.RenderField;

namespace Glass.Sitecore.Mapper.Dynamic
{
    public class DynamicItem : DynamicObject
    {
        Item _item;

        public DynamicItem(Item item)
        {
            _item = item;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            string name = binder.Name;


            if (_item.Fields[name] != null)
            {
                FieldRenderer render = new FieldRenderer();
                render.FieldName = name;
                render.Item = _item;

                result = render.Render();
                return true;
            }

            SitecoreInfoType infoType;

            if (Enum.TryParse<SitecoreInfoType>(name, out infoType))
            {
                result = SitecoreInfoHandler.GetItemInfo(infoType, _item, null);
                return true;
            }


            switch (name)
            {
                case "Parent":
                    result = new DynamicItem(_item.Parent);
                    break;
                case "Children":
                    result = new DynamicCollection(_item.Children.Select(x => new DynamicItem(x)).ToArray());
                    break;
            }
            if (result != null) return true;
            
            throw new NotSupportedException("No field of Sitecore info matches the name {0} for item {1}".Formatted(name, _item.Paths.FullPath));

        }

       

      
    }

}
