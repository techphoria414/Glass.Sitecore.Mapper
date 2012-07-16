using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace Glass.Sitecore.Mapper.Data
{
    public class SitecoreFieldNameValueCollectionHandler : AbstractSitecoreField
    {
        public override object GetFieldValue(string fieldValue, global::Sitecore.Data.Items.Item item, ISitecoreService service)
        {

            return HttpUtility.ParseQueryString(fieldValue);
        }

        public override string SetFieldValue(object value, ISitecoreService service)
        {
            NameValueCollection collection = value as NameValueCollection;

            if (collection != null)
            {
                return Utility.ConstructQueryString(collection);
            }
            else return string.Empty;

        }

        public override Type TypeHandled
        {
            get { return typeof(NameValueCollection); }
        }
    }
}
