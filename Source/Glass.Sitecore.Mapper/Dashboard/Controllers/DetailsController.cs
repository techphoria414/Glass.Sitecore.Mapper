using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Sitecore.Mapper.Dashboard.Web;
using Glass.Sitecore.Mapper.Dashboard.Model;
using System.Web;
using Glass.Sitecore.Mapper.Configuration.Attributes;
using Glass.Sitecore.Mapper.Data;
using Glass.Sitecore.Mapper.Dashboard.Model.Data;

namespace Glass.Sitecore.Mapper.Dashboard.Controllers
{
    public class DetailsController : AbstractController
    {

        public AbstractView Index()
        {
            return new HtmlView("Html.Details.htm");
        }

        public AbstractView Get(string name)
        {
            var results = GlassContext.Classes.Where(x => x.Key.FullName == name);

            if (!results.Any()) throw new HttpException(404, "Not found");

            var result = results.First();

            var model = new GlassClass();
            model.Name = result.Key.FullName;
            model.Inherited = GetInheritedMembers(result.Key);
            model.Info = GetInformation(result);
            model.Derived = GetDerivedTypes(result.Key);
            model.Queries = GetQuery(result);
            model.Children = GetChildren(result);
            model.Parents = GetParents(result);
            model.Links = GetLinked(result);
            model.Items = GetItems(result);
            model.Fields = GetFields(result);
            
            model.BranchId = result.Value.BranchId;
            model.CodeFirst = result.Value.ClassAttribute.CodeFirst;
            if (result.Value.IdProperty != null)
                model.Id = result.Value.IdProperty.Property.Name;


            model.Template = new GlassTemplate();
            model.Template.Id = result.Value.TemplateId;

            if (GlassContext.Loaders.Any(x => x.Id == DashboardLoader.IdValue))
            {
                ISitecoreService master = new SitecoreService("master");

                model.Template.Checked = true;

                if ( model.Template.Id != Guid.Empty)
                {
                    var temp = master.GetItem<TemplateData>( model.Template.Id);
                    if (temp != null)
                    {
                        model.Template.Exists= true;
                        model.Template.Url = DashboardGlobals.TemplateUrl.Formatted(model.Template.Id.ToString("D"));
                    }
                }

            }


            return new JsonView(model);

        }
        


        private IEnumerable<GlassField> GetFields(KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreFieldAttribute);

            List<GlassField> fields = new List<GlassField>();


            foreach (var scProp in scProps)
            {
                var fieldAttr = scProp.Attribute as SitecoreFieldAttribute;
                var handler = cls.Value.DataHandlers.Where(x => x.Property == scProp.Property).FirstOrDefault() as AbstractSitecoreField;

               GlassField field=  new GlassField();

                   field.Name = scProp.Property.Name;
                   field.Type = DashboardUtilities.GetTypeName(scProp.Property.PropertyType);
                   field.FieldName = handler.FieldName;
                   field.FieldId= fieldAttr.FieldId;
                   field.ReadOnly = fieldAttr.ReadOnly;
                   field.CodeFirst = fieldAttr.CodeFirst;
                   field.SectionName = fieldAttr.SectionName;
                   field.FieldType = fieldAttr.FieldType.ToString();
                   field.FieldTitle = fieldAttr.FieldTitle;
                  field.FieldSource = fieldAttr.FieldSource;

                  fields.Add(field);
            }

            return fields;
        }

        private IEnumerable<GlassItem> GetItems(KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreItemAttribute);

            List<GlassItem> items = new List<GlassItem>();


            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreItemAttribute;
                var item = new GlassItem();
                   item.Name = scProp.Property.Name;
                   item.Type = DashboardUtilities.GetTypeName(scProp.Property.PropertyType);
                   item.Id = propAttr.Id;
                   item.Path = propAttr.Path;
                   item.IsLazy = propAttr.IsLazy;

                   items.Add(item);

            }

            return items;

        }


        private IEnumerable<GlassLinked> GetLinked(KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreLinkedAttribute);

            List<GlassLinked> links = new List<GlassLinked>();


            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreLinkedAttribute;
                var link = new GlassLinked();

                link.Name = scProp.Property.Name;
                link.Type = DashboardUtilities.GetTypeName(scProp.Property.PropertyType);
                link.Option = propAttr.Option.ToString();
                link.InferType = propAttr.InferType;
                link.IsLazy = propAttr.IsLazy;

                links.Add(link);
            }

            return links;

        }

        private IEnumerable<GlassParent> GetParents(KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreParentAttribute);

            List<GlassParent> parents = new List<GlassParent>();


            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreParentAttribute;
                var parent = new GlassParent();

                parent.Name = scProp.Property.Name;
                parent.Type = DashboardUtilities.GetTypeName(scProp.Property.PropertyType);
                parent.InferType = propAttr.InferType;
                parent.IsLazy = propAttr.IsLazy;

                parents.Add(parent);
            }

            return parents;

        }


        private IEnumerable<GlassChildren> GetChildren(KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreChildrenAttribute);

            List<GlassChildren> childrens = new List<GlassChildren>();

            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreChildrenAttribute;
                    GlassChildren child = new GlassChildren();

                child.Name =     scProp.Property.Name;
                 child.Type= DashboardUtilities.GetTypeName(scProp.Property.PropertyType);
                    child.InferType = propAttr.InferType;
                    child.IsLazy = propAttr.IsLazy;

                    childrens.Add(child);
            }
            return childrens;

        }

        private IEnumerable<GlassClassSummary> GetInheritedMembers(Type type)
        {
            List<GlassClassSummary> classes = new List<GlassClassSummary>();
            
            if (GlassContext.Classes.Any(x => x.Key == type.BaseType))
                classes.Add(DashboardUtilities.GetTypeName(type.BaseType));

            var interfaces = type.GetInterfaces();

            if (interfaces.Any())
            {
                foreach (var superType in interfaces.OrderBy(x => x.Name))
                {
                    classes.Add(DashboardUtilities.GetTypeName(superType));
                }
            }

            return classes;

        }

        private IEnumerable<GlassInfo> GetInformation(KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreInfoAttribute);
            
            List<GlassInfo> infos = new List<GlassInfo>();


            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreInfoAttribute;
             
                var info = new GlassInfo();
                
                info.Name = scProp.Property.Name;
                info.Type = DashboardUtilities.GetTypeName(scProp.Property.PropertyType);
                info.InfoType = propAttr.Type.ToString();

                infos.Add(info);
            }

            return infos;
        }

        private  IEnumerable<GlassClassSummary> GetDerivedTypes(Type type)
        {
            List<GlassClassSummary> classes = new List<GlassClassSummary>();

            foreach (var clsType in GlassContext.Classes)
            {
               
                if (type.IsAssignableFrom(clsType.Key) && clsType.Key != type)
                    classes.Add(DashboardUtilities.GetTypeName(clsType.Key) );

            }

            return classes;
        }

        private IEnumerable<GlassQuery> GetQuery( KeyValuePair<Type, Configuration.SitecoreClassConfig> cls)
        {
            var scProps = cls.Value.Properties.Where(x => x.Attribute is SitecoreQueryAttribute);

            List<GlassQuery> queries = new List<GlassQuery>();


            foreach (var scProp in scProps)
            {
                var propAttr = scProp.Attribute as SitecoreQueryAttribute;
                var query = new GlassQuery();

                query.Name = scProp.Property.Name;
                query.Type = DashboardUtilities.GetTypeName(scProp.Property.PropertyType);
                query.InferType = propAttr.InferType;
                query.IsLazy = propAttr.IsLazy;
                query.IsRelative = propAttr.IsRelative;
                query.Query = propAttr.Query;
                query.UseQueryContext = propAttr.UseQueryContext;

                queries.Add(query);
            }

            return queries;

        }

        public object TemplateData { get; set; }
    }
}
