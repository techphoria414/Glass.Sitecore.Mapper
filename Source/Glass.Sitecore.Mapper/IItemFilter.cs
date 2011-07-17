/*
   Copyright 2011 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Sitecore.Mapper
{
    /// <summary>
    /// The filter allows you to define which item should and shouldn't be loaded when a property is called
    /// </summary>
    public interface IItemFilter
    {
        /// <summary>
        /// Used to determine if the item should be loaded
        /// </summary>
        /// <param name="item">The item to test</param>
        /// <param name="service">The current service loading the item</param>
        /// <returns>True if the item should be loaded, false if it shouldn't</returns>
        bool LoadItem(Item item, ISitecoreService service);            
    }
}
