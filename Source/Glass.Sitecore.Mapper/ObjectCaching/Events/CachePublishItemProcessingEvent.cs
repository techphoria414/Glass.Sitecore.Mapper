using System;
using System.Linq;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace Glass.Sitecore.Mapper.ObjectCaching.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class CachePublishItemProcessingEvent
    {
        /// <summary>
        /// Updates the cache.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void UpdateCache(object sender, EventArgs args)
        {
            var theArgs = args as ItemProcessingEventArgs;

            if (theArgs != null)
            {
                
                var currentItem = theArgs.Context.PublishHelper.GetSourceItem(theArgs.Context.ItemId);

                //does the item exist?
                if (currentItem != null)
                {
                    var ci = ObjectCache.CacheItemList.SingleOrDefault(x => x.TemplateID == currentItem.TemplateID.Guid);

                    //only try and update the cache if we have the item type in memory
                    if (ci != null)
                    {
                        ObjectCacheFactory.Create().PubishEvent(currentItem);
                    }
                }
            }
        }
    }
}
