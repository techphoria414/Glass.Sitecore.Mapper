using System;
namespace Glass.Sitecore.Mapper.ObjectCaching
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICacheableObject
    {
        /// <summary>
        /// Gets or sets the cached object.
        /// </summary>
        /// <value>
        /// The cached object.
        /// </value>
        object CachedObject { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        string Database { get; set; }

        /// <summary>
        /// Gets or sets the item ID.
        /// </summary>
        /// <value>
        /// The item ID.
        /// </value>
        Guid ItemID { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        object Key { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        string Language { get; set; }

        /// <summary>
        /// Gets or sets the revision ID.
        /// </summary>
        /// <value>
        /// The revision ID.
        /// </value>
        Guid RevisionID { get; set; }

        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        string TargetType { get; set; }

        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        /// <value>
        /// The template ID.
        /// </value>
        Guid TemplateID { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        Type Type { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        int Version { get; set; }

        /// <summary>
        /// Toes the cached object information.
        /// </summary>
        /// <returns></returns>
        CachedObjectInformation ToCachedObjectInformation();
    }
}
