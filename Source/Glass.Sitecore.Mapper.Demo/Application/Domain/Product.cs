using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Sitecore.Mapper.Configuration.Attributes;

namespace Glass.Sitecore.Mapper.Demo.Application.Domain
{
    [SitecoreClass]
    public class Product
    {

        [SitecoreField("Related")]
        public virtual IEnumerable<Product> OtherProducts { get; set; }

        [SitecoreField]
        public virtual Glass.Sitecore.Mapper.FieldTypes.Link Manufacturer { get; set; }

        [SitecoreField("Alternative Product")]
        public virtual Product AlternativeProduct { get; set; }

        [SitecoreField]
        public virtual string Title { get; set; }

        [SitecoreField]
        public virtual string Description { get; set; }



        [SitecoreField("Release Date")]
        public virtual DateTime ReleaseDate { get; set; }

        [SitecoreField]
        public virtual double Price { get; set; }

        [SitecoreId]
        public virtual Guid Id { get; set; }

        [SitecoreInfo(Glass.Sitecore.Mapper.Configuration.SitecoreInfoType.Key)]
        public virtual string Name { get; set; }

        [SitecoreInfo(Glass.Sitecore.Mapper.Configuration.SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
    }

    public class Home { }

    [SitecoreClass]
    public class ProductsLanding
    {

        [SitecoreQuery("/sitecore/content/home/products/*[@Featured='1']")]
        public virtual IEnumerable<Product> FeaturedProducts { get; set; }

        [SitecoreParent]
        public virtual Home Home { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<Product> Products { get; set; }



    }

    [SitecoreClass]
    public interface ICommon
    {
        [SitecoreField]
        string Title { get; set; }
    }
    [SitecoreClass]
    public interface ITagged
    {
        [SitecoreField]
        IEnumerable<Tag> Tags { get; set; }
    }

    [SitecoreClass]
    public interface IProduct : ICommon, ITagged
    {
        [SitecoreField]
        int Price { get; set; }

        [SitecoreField]
        IEnumerable<IProduct> RelatedProducts { get; set; }
    }
    [SitecoreClass]
    public class Tag { }

}