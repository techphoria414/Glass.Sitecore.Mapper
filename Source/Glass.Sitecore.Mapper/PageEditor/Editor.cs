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
using System.Linq.Expressions;
using Sitecore.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Data;
using Glass.Sitecore.Mapper.Data;

namespace Glass.Sitecore.Mapper.PageEditor
{
    public class Editor
    {
        

        public static string Editable<T>(Expression<Func<T, object>> field,Expression<Func<T, string>> standardOutput, T target, Database database)
        {
            try
            {

                if (global::Sitecore.Context.Site.DisplayMode ==
 global::Sitecore.Sites.DisplayMode.Edit
                    && global::Sitecore.Context.PageMode.IsPageEditorEditing
                    )
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

                    var prop = Utility.GetInfo(type, field.Body.ToString(), field.Parameters[0].Name);

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
