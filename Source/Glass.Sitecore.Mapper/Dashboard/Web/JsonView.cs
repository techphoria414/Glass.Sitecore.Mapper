using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Glass.Sitecore.Mapper.Dashboard.Web
{
    public class JsonView: AbstractView
    {
        object _model;

        public JsonView(object model)
        {
            _model = model;
        }

        public override void Response(System.Web.HttpResponse response)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var output = new StringBuilder();
            serializer.Serialize(_model, output);

            response.ContentType = "application/json";
            response.Write(output.ToString());
        }
    }
}
