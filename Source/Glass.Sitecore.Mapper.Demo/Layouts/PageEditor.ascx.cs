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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Glass.Sitecore.Mapper.Demo.Application.Domain;
using Glass.Sitecore.Mapper.PageEditor;

namespace Glass.Sitecore.Mapper.Demo.Layouts
{
    public partial class PageEditor : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SitecoreContext context = new SitecoreContext();

            //var demo = context.GetCurrentItem<PageEditorDemo>();

            //mainBody.Text = Html.Editable<PageEditorDemo>(
            //    x => x.Body,
            //    demo,
            //    context);

            //mainImage.Text = Html.Editable<PageEditorDemo>(
            //    x => x.Image,
            //    x => string.Format("<img src='{0}' alt='{1}' />", x.Image.Src, x.Image.Alt), 
            //    demo,
            //    context);

            //subContent.Text = Html.Editable<PageEditorDemoSub>(
            //    x => x.Content,
            //    demo.AnotherItem,
            //    context);
                

            var target= context.GetCurrentItem<DemoClass>();

            test1.Text = Html.Editable<DemoClass>(
                x => x.Image, //the field that you want to edit
                x => string.Format("<img src='{0}' />", x.Image.Src), //the content to output when not in page editing
                target, //the item to target for editing
                context); //the current ISitecoreService used to load the target

            test2.Text = Html.Editable<DemoClass>(
               x => x.Body, //the field that you want to edit
               target, //the item to target for editing
               context); //the current ISitecoreService used to load the target

            test3.Text = Html.Editable<DemoClass>(
               x => x.Image, //the field that you want to edit
               x => string.Format("<img src='{0}' />", x.Image.Src), //the content to output when not in page editing
               target); //the item to target for editing

            test4.Text = Html.Editable<DemoClass>(
               x => x.Body, //the field that you want to edit
               target); //the item to target for editing

            test5.Text = Html.Editable<DemoClass>(
               x => x.Image, //the field that you want to edit
               x => string.Format("<img src='{0}' />", x.Image.Src), //the content to output when not in page editing
               target, //the item to target for editing
               "master"); //the current ISitecoreService used to load the target

            test6.Text = Html.Editable<DemoClass>(
               x => x.Body, //the field that you want to edit
               target, //the item to target for editing
               "master"); //the current ISitecoreService used to load the target

            test7.Text = Html.Editable<DemoSubClass>(
                x => x.Body,
                target.SubClass,
                context);
                

        }
    }
}