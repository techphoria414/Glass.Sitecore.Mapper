using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Dashboard.Html;

namespace Glass.Sitecore.Mapper.Dashboard
{
    public class ClassDetailsResponse : AbstractResponse
    {
        public override string Content()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<h2>Class Details</h2>");

            if(Request.QueryString["class"].IsNullOrEmpty())
            {  sb.Append( "<p>No class set</p>");
                return sb.ToString();

            }
         
            var clsName = Request.QueryString["class"];
            var results  = Context.StaticContext.Classes.Where(x => x.Key.FullName == clsName);
            
            if(!results.Any()){
                sb.AppendFormat("<p>Class with name {0} not found.</p>", clsName);
                    return sb.ToString();
            }

            var cls = results.First();
            var idProp = cls.Value.Properties.Where(x=>x.Attribute is SitecoreIdAttribute);

            sb.AppendFormat("<h2>{0}</h2>", clsName);

            sb.Append("<h3>Details</h3>");            
            sb.Append(Tables.TableOpen());
            sb.AppendFormat("<tr><th>Inherits</th><td>{0}</td></tr>", GetInheritedMembers(cls.Key));
            sb.AppendFormat("<tr><th>Sub-Types</th><td>{0}</td></tr>", GetDerivedTypes(cls.Key));
            sb.AppendFormat("<tr><th>Code First</th><td>{0}</td></tr>", cls.Value.ClassAttribute.CodeFirst);

            if (cls.Value.ClassAttribute.TemplateId.IsNotNullOrEmpty())
                sb.AppendFormat("<tr><th>Template Id</th><td>{0}</td></tr>", cls.Value.ClassAttribute.TemplateId);

            if (cls.Value.ClassAttribute.BranchId.IsNotNullOrEmpty())
                sb.AppendFormat("<tr><th>Branch Id</th><td>{0}</td></tr>", cls.Value.ClassAttribute.BranchId);
            
            if(idProp.Any())
                sb.AppendFormat("<tr><th>Id Property</th><td>{0}</td></tr>", idProp.First().Property.Name);
            sb.Append("</table>");


            GetFields(sb, cls);

            GetInformation(sb, cls);

            GetItems(sb, cls);
            
            GetLinked(sb, cls);

            GetParents(sb, cls);

            GetChildren(sb, cls);

             GetQuery(sb, cls);

            return sb.ToString();

        }

        private void GetFields(StringBuilder sb, KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps  = cls.Value.Properties.Where(x => x.Attribute is SitecoreFieldAttribute);
            
            if(!scProps.Any()) return;

            sb.Append("<h3>Fields</h3>");
            sb.Append(Tables.TableOpen());
            sb.Append("<tr><th>Property Name</th><th>Field Name</th><th>Return Name</th><th>Field Id</th><th>Read Only</th><th>Code First</th><th>Section Name</th><th>Type</th><th>Field Title</th><th>Field Source</th></tr>");

            foreach (var scProp in scProps)
            {
                var fieldAttr = scProp.Attribute as SitecoreFieldAttribute;
                var handler = cls.Value.DataHandlers.Where(x => x.Property == scProp.Property).FirstOrDefault() as AbstractSitecoreField;

                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td></tr>",
                    scProp.Property.Name,
                    GetTypeName(scProp.Property.PropertyType),
                    handler.FieldName,
                    fieldAttr.FieldId,
                    fieldAttr.ReadOnly,
                    fieldAttr.CodeFirst,
                    fieldAttr.SectionName,
                    fieldAttr.FieldType,
                    fieldAttr.FieldTitle,
                    fieldAttr.FieldSource
                    );
            }

            sb.Append("</table>");
          
        }

        private void GetItems(StringBuilder sb, KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreItemAttribute);

            if (!scProps.Any()) return;

            sb.Append("<h3>Items</h3>");
            sb.Append(Tables.TableOpen());

            sb.AppendFormat("<tr><th>Property Name</th><th>Return Type</th><th>Id</th><th>Path</th><th>Lazy</th></tr>");

            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreItemAttribute;
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                    scProp.Property.Name,
                    GetTypeName(scProp.Property.PropertyType),
                    propAttr.Id,
                    propAttr.Path,
                    propAttr.IsLazy);
            }

            sb.Append("</table>");

        }

        private void GetLinked(StringBuilder sb, KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreLinkedAttribute);
            if (!scProps.Any()) return;

            sb.Append("<h3>Linked</h3>");
            sb.Append(Tables.TableOpen());

            sb.AppendFormat("<tr><th>Property Name</th><th>Return Type</th><th>Option</th><th>InferType</th><th>Lazy</th></tr>");

            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreLinkedAttribute;
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                    scProp.Property.Name,
                    GetTypeName(scProp.Property.PropertyType),
                    propAttr.Option,
                    propAttr.InferType,
                    propAttr.IsLazy);
            }

            sb.Append("</table>");
           
        }

        private void GetParents(StringBuilder sb, KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreParentAttribute);
            if (!scProps.Any()) return;

            sb.Append("<h3>Parent</h3>");
            sb.Append(Tables.TableOpen());

            sb.AppendFormat("<tr><th>Property Name</th><th>Return Type</th><th>InferType</th><th>Lazy</th></tr>");

            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreParentAttribute;
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                    scProp.Property.Name,
                    GetTypeName(scProp.Property.PropertyType),
                    propAttr.InferType,
                    propAttr.IsLazy);
            }

            sb.Append("</table>");
           
        }

        private void GetChildren(StringBuilder sb, KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreChildrenAttribute);
            if (!scProps.Any()) return;


            sb.Append("<h3>Children</h3>");
            sb.Append(Tables.TableOpen());

            sb.AppendFormat("<tr><th>Property Name</th><th>Return Type</th><th>InferType</th><th>Lazy</th></tr>");

            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreChildrenAttribute;
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                    scProp.Property.Name,
                    GetTypeName(scProp.Property.PropertyType),
                    propAttr.InferType,
                    propAttr.IsLazy);
            }

            sb.Append("</table>");
          
        }

        private void GetQuery(StringBuilder sb, KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreQueryAttribute);
            if (!scProps.Any()) return;


            sb.Append("<h3>Query</h3>");
            sb.Append(Tables.TableOpen());

            sb.AppendFormat("<tr><th>Property Name</th><th>Return Type</th><th>InferType</th><th>Lazy</th><th>Is Relative</th><th>Query</th><th>Use Query Context</th></tr>");

            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreQueryAttribute;
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>",
                    scProp.Property.Name,
                    GetTypeName(scProp.Property.PropertyType),
                    propAttr.InferType,
                    propAttr.IsLazy,
                    propAttr.IsRelative,
                    propAttr.Query,
                    propAttr.UseQueryContext);
            }

            sb.Append("</table>");
           
        }

        private void GetInformation(StringBuilder sb, KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreInfoAttribute);
            if (!scProps.Any()) return;


            sb.Append("<h3>Information</h3>");
            sb.Append(Tables.TableOpen());

            sb.AppendFormat("<tr><th>Property Name</th><th>Return Type</th><th>Type</th></tr>");

            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreInfoAttribute;
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",
                    scProp.Property.Name,
                    GetTypeName(scProp.Property.PropertyType),
                    propAttr.Type);
            }

            sb.Append("</table>");
        }

        public string GetInheritedMembers(Type type)
        {
            StringBuilder sb = new StringBuilder();

            if(Context.StaticContext.Classes.Any(x=>x.Key == type.BaseType))
                sb.AppendFormat("{0},", GetTypeName(type.BaseType));

            var interfaces = type.GetInterfaces();
          
            if (interfaces.Any())
            {
                foreach (var superType in interfaces.OrderBy(x=>x.Name))
                {
                    sb.AppendFormat(" {0},", GetTypeName(superType));
                }
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();

        }

        public string GetDerivedTypes(Type type)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var clsType in Context.StaticContext.Classes)
            {
                if (type.IsAssignableFrom(clsType.Key) && clsType.Key != type)
                    sb.AppendFormat(" {0},", GetTypeName(clsType.Key));

            }
            if (sb.Length > 0) 
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();

        }
    }
}
