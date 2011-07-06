using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Sitecore.Mapper.Demo.Application.Domain;
using Glass.Sitecore.Mapper.Configuration.Fluent;

namespace Glass.Sitecore.Mapper.Demo.Configuration
{
	public class ProductConfig
	{

        public void Config()
        {
            var product = new SitecoreClass<Product>();
            product.Field(x => x.Title);
            product.Id(x => x.Id);
            product.Info(x => x.Url).InfoType(Mapper.Configuration.SitecoreInfoType.Url);
            
                
                        
                           
                        
        }
	}
}