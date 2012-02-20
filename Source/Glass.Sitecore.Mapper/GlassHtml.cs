using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Sitecore.Data;
using System.Collections.Specialized;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using Glass.Sitecore.Mapper.Data;

namespace Glass.Sitecore.Mapper
{
    /// <summary>
    /// This class contains a set of helpers that make converting items mapped in Glass.Sitecore.Mapper to HTML
    /// </summary>
    public class GlassHtml
    {
        Database _db;

        /// <param name="service">The service that will be used to load and save data</param>
        public GlassHtml(ISitecoreService service):this(service.Database){
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="database">The database that will be used to load and save data</param>
        public GlassHtml(string database):this(global::Sitecore.Configuration.Factory.GetDatabase(database))
        {
        }
        /// <param name="database">The database that will be used to load and save data</param>
        public GlassHtml(Database database)
        {
            _db = database;
        }

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Class.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public string Editable<T>(Expression<Func<T, object>> field, T target)
        {
            return MakeEditable<T>(field, null, target, _db);
        }
        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor.  Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Class.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="service">The service that will be used to load and save data</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public string Editable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target)
        {
            return MakeEditable<T>(field, standardOutput, target, _db);
        }

        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <returns>An img HTML element</returns>
        public string RenderImage(FieldTypes.Image image)
        {
            return RenderImage(image, null);
        }
       
        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <param name="attributes">Additional attributes to add. Do not include alt or src</param>
        /// <returns>An img HTML element</returns>
        public string RenderImage(FieldTypes.Image image, NameValueCollection attributes)
        {
            if (image == null) return "";

            if (attributes == null) attributes = new NameValueCollection();

            string format = "<img src='{0}' {1}/>";
            
            //should there be some warning about these removals?
            AttributeCheck(attributes, "class", image.Class);
            AttributeCheck(attributes, "alt", image.Alt);
            if(image.Height > 0)
                AttributeCheck(attributes, "height", image.Height.ToString());
            if(image.Width > 0)
                AttributeCheck(attributes, "width", image.Width.ToString());
            
            return format.Formatted(image.Src, Utility.ConvertAttributes(attributes));
        }

        /// <summary>
        /// Checks it and attribute is part of the NameValueCollection and updates it with the 
        /// default if it isn't.
        /// </summary>
        /// <param name="collection">The collection of attributes</param>
        /// <param name="name">The name of the attribute in the collection</param>
        /// <param name="defaultValue">The default value for the attribute</param>
        public void AttributeCheck(NameValueCollection collection, string name, string defaultValue)
        {
            if (collection[name].IsNullOrEmpty() && !defaultValue.IsNullOrEmpty())
                collection[name] = defaultValue;
        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <returns>An "a" HTML element </returns>
        public string RenderLink(FieldTypes.Link link)
        {

            return RenderLink(link, null, string.Empty);

        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional attributes to add. Do not include href or title</param>
        /// <returns>An "a" HTML element </returns>
        public  string RenderLink(FieldTypes.Link link, NameValueCollection attributes)
        {

            return RenderLink(link, attributes, string.Empty);

        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional attributes to add. Do not include href or title</param>
        /// <param name="contents">Content to go in the link instead of the standard text</param>
        /// <returns>An "a" HTML element </returns>
        public string RenderLink(FieldTypes.Link link, NameValueCollection attributes, string contents)
        {
            if (link == null) return "";
            if (attributes == null) attributes = new NameValueCollection();

            string format = "<a href='{0}{1}' title='{2}' target='{3}' class='{4}' {5}>{6}</a>";

            string cls = attributes.AllKeys.Any(x => x == "class") ? attributes["class"] : link.Class;
            string anchor = link.Anchor.IsNullOrEmpty() ? "" : "#" + link.Anchor;
            string target = attributes.AllKeys.Any(x => x == "target") ? attributes["target"] : link.Target;


            AttributeCheck(attributes, "class", link.Class);
            AttributeCheck(attributes, "target", link.Target);
            AttributeCheck(attributes, "title", link.Title);

            attributes.Remove("href");


            return format.Formatted(link.Url, anchor, link.Title, target, cls, Utility.ConvertAttributes(attributes), contents.IsNullOrEmpty() ?  link.Text : contents);

        }

        /// <summary>
        /// Indicates if the site is in editing mode
        /// </summary>
        public  bool IsInEditingMode
        {
            get
            {
                return global::Sitecore.Context.Site.DisplayMode ==
                            global::Sitecore.Sites.DisplayMode.Edit
                            && global::Sitecore.Context.PageMode.IsPageEditorEditing;
            }
        }

        private string MakeEditable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target, Database database)
        {
            try
            {

                if (this.IsInEditingMode)
                {
                    if (field.Parameters.Count > 1)
                        throw new MapperException("To many parameters in linq expression {0}".Formatted(field.Body));

                    var site = global::Sitecore.Context.Site;
                    Type type = typeof(T);

                    InstanceContext context = Context.GetContext();

                    Guid id = Guid.Empty;

                    try
                    {
                        id = context.GetClassId(type, target);
                    }
                    catch (SitecoreIdException ex)
                    {
                        throw new MapperException("Page editting error. Type {0} can not be used for editing. See inner exception".Formatted(typeof(T).FullName), ex);
                    }

                    var scClass = context.GetSitecoreClass(typeof(T));

                    var prop = Utility.GetPropertyInfo(type, field.Body);

                    if (prop == null) throw new MapperException("Page editting error. Could not find property {0} on type {1}".Formatted(field.Body, type.FullName));

                    var dataHandler = scClass.DataHandlers.FirstOrDefault(x => x.Property == prop);

                    var item = database.GetItem(new ID(id));

                    using (new ContextItemSwitcher(item))
                    {
                        FieldRenderer renderer = new FieldRenderer();
                        renderer.Item = item;
                        renderer.FieldName = ((AbstractSitecoreField)dataHandler).FieldName;
                        renderer.Parameters = "";
                        return renderer.Render();
                    }
                }
                else
                {
                    if (standardOutput != null)
                        return standardOutput.Compile().Invoke(target);
                    else
                        return field.Compile().Invoke(target).ToString();
                }
            }
            catch (Exception ex)
            {
                return "The following exception occured: {0} \n\r {1}".Formatted(ex.Message, ex.StackTrace);
            }
        }

    }
}
  
