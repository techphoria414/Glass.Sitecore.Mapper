using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Sitecore.Data;
using System.Collections.Specialized;

namespace Glass.Sitecore.Mapper
{
    /// <summary>
    /// This class contains a set of helpers that make converting items mapped in Glass.Sitecore.Mapper to HTML
    /// </summary>
    public static class Html
    {

        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the specifed service to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Class.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="service">The service that will be used to load and save data</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public static string Editable<T>(Expression<Func<T, object>> field, T target, ISitecoreService service)
        {
            return Editable<T>(field, null, target, service);
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
        public static string Editable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target, ISitecoreService service)
        {
            return PageEditor.Editor.Editable<T>(field, standardOutput, target, service.Database);
        }
        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Uses the Context database (Sitecore.Context.Database) to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Class.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public static string Editable<T>(Expression<Func<T, object>> field, T target)
        {
          return Editable<T>(field, null, target);
        }
        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Uses the Context database (Sitecore.Context.Database) to write data.
        /// </summary>
        /// <typeparam name="T">A class loaded by Class.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public static string Editable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target)
        {
            return PageEditor.Editor.Editable<T>(field, standardOutput, target, global::Sitecore.Context.Database);
        }
        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the database specified
        /// </summary>
        /// <typeparam name="T">A class loaded by Class.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="database">The database to save data to</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public static string Editable<T>(Expression<Func<T, object>> field, T target, string database)
        {
            return Editable<T>(field, null, target, database);
        }
        /// <summary>
        /// Makes the field editable using the Sitecore Page Editor. Using the database specified
        /// </summary>
        /// <typeparam name="T">A class loaded by Class.Sitecore.Mapper</typeparam>
        /// <param name="field">The field that should be made editable</param>
        /// <param name="target">The target object that contains the item to be edited</param>
        /// <param name="standardOutput">The output to display when the Sitecore Page Editor is not being used</param>
        /// <param name="database">The database to save data to</param>
        /// <returns>HTML output to either render the editable controls or normal HTML</returns>
        public static string Editable<T>(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput, T target, string database)
        {
            Database db = global::Sitecore.Configuration.Factory.GetDatabase(database);

            return PageEditor.Editor.Editable<T>(field, standardOutput, target, db);
        }

        /// <summary>
        /// Renders HTML for an image
        /// </summary>
        /// <param name="image">The image to render</param>
        /// <param name="attributes">Additional attributes to add. Do not include alt or src</param>
        /// <returns>An img HTML element</returns>
        public static string RenderImage(FieldTypes.Image image, NameValueCollection attributes)
        {
            string format = "<img src='{0}' alt='{1}' class='{2}' {3}/>";

            string cls = attributes.AllKeys.Any(x=> x =="class") ? attributes["class"] : image.Class;

            //should there be some warning about these removals?
            attributes.Remove("class");
            attributes.Remove("alt");
            attributes.Remove("src");

            
            return format.Formatted(image.Src, image.Alt, cls, Utility.ConvertAttributes(attributes));
        }

        /// <summary>
        /// Render HTML for a link
        /// </summary>
        /// <param name="link">The link to render</param>
        /// <param name="attributes">Addtiional attributes to add. Do not include href or title</param>
        /// <returns>An "a" HTML element </returns>
        public static string RenderLink(FieldTypes.Link link, NameValueCollection attributes)
        {
            string format = "<a href='{0}{1}' title='{2}' target='{3}' class='{4}' {5}>{6}</a>";

            string cls = attributes.AllKeys.Any(x=> x =="class") ? attributes["class"] : link.Class;
            string anchor = link.Anchor.IsNullOrEmpty() ? "" : "#"+link.Anchor;
            string target = attributes.AllKeys.Any(x => x == "target") ? attributes["target"] : link.Target;
            
            attributes.Remove("href");
            attributes.Remove("title");
            attributes.Remove("target");
            attributes.Remove("class");


            return format.Formatted(link.Url, anchor, link.Title, target, cls, Utility.ConvertAttributes(attributes), link.Text);

        }

    }
}
  
