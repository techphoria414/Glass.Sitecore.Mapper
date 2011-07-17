using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Applications.ContentManager;
using Sitecore.Text.Diff;
using Sitecore.Web.UI;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore;
namespace Glass.Sitecore.Misc
{
    /// <summary></summary>
    public class ThreeCol : EditorFormatter
    {
        /// <summary>
        /// Renders the translator.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="field">The field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="readOnly">if set to <c>true</c> this instance is read only.</param>
        public override void RenderField(System.Web.UI.Control parent, Editor.Field field, Item fieldType, bool readOnly)
        {
            Assert.ArgumentNotNull(parent, "parent");
            Assert.ArgumentNotNull(field, "field");
            Assert.ArgumentNotNull(fieldType, "fieldType");
            GridPanel gridPanel = new GridPanel();
            gridPanel.Width = new Unit(100.0, UnitType.Percentage);
            gridPanel.Columns = 6;
            Context.ClientPage.AddControl(parent, gridPanel);

            Border border = new Border();
            Context.ClientPage.AddControl(gridPanel, border);
            gridPanel.SetExtensibleProperty(border, "width", "25%");
            gridPanel.SetExtensibleProperty(border, "valign", "top");
            base.RenderField(border, field, fieldType, readOnly);
            
            Space control = new Space("8", "1");
            Context.ClientPage.AddControl(gridPanel, control);
            
            border = new Border();
            Context.ClientPage.AddControl(gridPanel, border);
            gridPanel.SetExtensibleProperty(border, "width", "25%");
            gridPanel.SetExtensibleProperty(border, "valign", "top");

          

            string text = Context.ClientPage.ServerProperties["TranslatingLanguage"] as string;
            Language language;
            if (!string.IsNullOrEmpty(text))
            {
                language = Language.Parse(text);
            }
            else
            {
                language = base.Arguments.Language;
            }
            string @string = StringUtil.GetString(Context.ClientPage.ServerProperties["ComparingVersion1"], global::Sitecore.Data.Version.Latest.ToString());
            string string2 = StringUtil.GetString(Context.ClientPage.ServerProperties["ComparingVersion2"], global::Sitecore.Data.Version.Latest.ToString());
            Editor.Field field2 = ThreeCol.GetField(field, language);
            var lang =  global::Sitecore.Data.Managers.LanguageManager.GetLanguages(field.ItemField.Database)[1];
            Editor.Field field3 = ThreeCol.GetField(field, lang);
            string value = string.Empty;
            System.Web.UI.Control editor;
            if (field.ItemField.Translatable && @string != string2)
            {
                editor = ThreeCol.Diff(field.ItemField.Item, field.ItemField.ID, language, global::Sitecore.Data.Version.Parse(@string), global::Sitecore.Data.Version.Parse(string2));
                value = "border:1px solid #dcdcdc;padding:3px 0px 2px 1px;font:9pt verdana; color:graytext";
            }
            else
            {
                editor = this.GetFieldEditor(fieldType);
            }
            gridPanel.SetExtensibleProperty(border, "style", "border:thin solid blue");



            ThreeCol.RenderTranslateButton(parent, field2);
            base.AddEditorControl(border, editor, field2, false, true, field2.ItemField.Value);


            gridPanel.SetExtensibleProperty(border, "style", "border:thin solid blue");

           
            if (field.ItemField.Translatable && @string != string2)
            {
                editor = ThreeCol.Diff(field.ItemField.Item, field.ItemField.ID, language, global::Sitecore.Data.Version.Parse(@string), global::Sitecore.Data.Version.Parse(string2));
                value = "border:1px solid #dcdcdc;padding:3px 0px 2px 1px;font:9pt verdana; color:graytext";
            }
            else
            {
                editor = this.GetFieldEditor(fieldType);
            }


            control = new Space("8", "1");
            Context.ClientPage.AddControl(gridPanel, control);

            border = new Border();
            Context.ClientPage.AddControl(gridPanel, border);
            gridPanel.SetExtensibleProperty(border, "width", "25%");
            gridPanel.SetExtensibleProperty(border, "valign", "top");

            base.AddEditorControl(border, editor, field3, false, true, field3.ItemField.Value);
            gridPanel.SetExtensibleProperty(border, "style", "border:thin solid red");
        }
        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        /// <param name="length">The length.</param>
        private static void Append(StringBuilder builder, string value, int index, int length)
        {
            Assert.ArgumentNotNull(builder, "builder");
            Assert.ArgumentNotNull(value, "value");
            if (length > 0 && index >= 0)
            {
                builder.Append(StringUtil.Mid(value, index, length));
            }
        }
        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        /// <param name="length">The length.</param>
        /// <param name="color">The color.</param>
        private static void Append(StringBuilder builder, string value, int index, int length, string color)
        {
            Assert.ArgumentNotNull(builder, "builder");
            Assert.ArgumentNotNull(value, "value");
            Assert.ArgumentNotNull(color, "color");
            if (length > 0 && index >= 0)
            {
                builder.Append("<span style=\"color:" + color + "\">");
                builder.Append(StringUtil.Mid(value, index, length));
                builder.Append("</span>");
            }
        }
        /// <summary>
        /// Compares the specified values.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns></returns>
        private static string Compare(string value1, string value2)
        {
            Assert.ArgumentNotNull(value1, "value1");
            Assert.ArgumentNotNull(value2, "value2");
            DiffEngine diffEngine = new DiffEngine();
            value1 = StringUtil.RemoveTags(value1);
            value2 = StringUtil.RemoveTags(value2);
            DiffListHtml source = new DiffListHtml(value1);
            DiffListHtml destination = new DiffListHtml(value2);
            diffEngine.ProcessDiff(source, destination, DiffEngineLevel.SlowPerfect);
            ArrayList arrayList = diffEngine.DiffReport();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < arrayList.Count; i++)
            {
                DiffResultSpan diffResultSpan = arrayList[i] as DiffResultSpan;
                if (diffResultSpan != null)
                {
                    switch (diffResultSpan.Status)
                    {
                        case DiffResultSpanStatus.NoChange:
                            {
                                ThreeCol.Append(stringBuilder, value1, diffResultSpan.SourceIndex, diffResultSpan.Length);
                                break;
                            }
                        case DiffResultSpanStatus.Replace:
                            {
                                ThreeCol.Append(stringBuilder, value1, diffResultSpan.SourceIndex, diffResultSpan.Length, "red; text-decoration:line-through");
                                ThreeCol.Append(stringBuilder, value2, diffResultSpan.DestIndex, diffResultSpan.Length, "green");
                                break;
                            }
                        case DiffResultSpanStatus.DeleteSource:
                            {
                                ThreeCol.Append(stringBuilder, value1, diffResultSpan.SourceIndex, diffResultSpan.Length, "red; text-decoration:line-through");
                                break;
                            }
                        case DiffResultSpanStatus.AddDestination:
                            {
                                ThreeCol.Append(stringBuilder, value2, diffResultSpan.DestIndex, diffResultSpan.Length, "green");
                                break;
                            }
                    }
                }
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Diffs the specified versions.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldID">The field ID.</param>
        /// <param name="language">The language.</param>
        /// <param name="version1">The version1.</param>
        /// <param name="version2">The version2.</param>
        /// <returns>The diff.</returns>
        private static System.Web.UI.Control Diff(Item item, ID fieldID, Language language, global::Sitecore.Data.Version version1, global::Sitecore.Data.Version version2)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(fieldID, "fieldID");
            Assert.ArgumentNotNull(language, "language");
            Assert.ArgumentNotNull(version1, "version1");
            Assert.ArgumentNotNull(version2, "version2");
            Error.AssertObject(item, "item");
            Item item2 = item.Database.Items[item.ID, language, version1];
            Item item3 = item.Database.Items[item.ID, language, version2];
            string text = ThreeCol.Compare(item2[fieldID], item3[fieldID]);
            return new LiteralControl(text);
        }
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        private static Editor.Field GetField(Editor.Field field, Language language)
        {
            Assert.ArgumentNotNull(field, "field");
            Assert.ArgumentNotNull(language, "language");
            Item item = field.ItemField.Item.Database.Items[field.ItemField.Item.ID, language];
            return new Editor.Field(item.Fields[field.ItemField.ID], field.TemplateField);
        }
        /// <summary>
        /// Gets the field editor.
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns></returns>
        private System.Web.UI.Control GetFieldEditor(Item fieldType)
        {
            Assert.ArgumentNotNull(fieldType, "fieldType");
            return base.GetEditor(fieldType);
        }
        /// <summary>
        /// Renders the translate button.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="field">The field.</param>
        private static void RenderTranslateButton(System.Web.UI.Control parent, Editor.Field field)
        {
            Assert.ArgumentNotNull(parent, "parent");
            Assert.ArgumentNotNull(field, "field");
            if (!Settings.WorldLingo.Enabled || !field.ItemField.Translatable)
            {
                return;
            }
            global::Sitecore.Web.UI.HtmlControls.Button button = new global::Sitecore.Web.UI.HtmlControls.Button();
            Context.ClientPage.AddControl(parent, button);
            button.Class = "scEditorTranslateButton";
            button.RenderAs = RenderAs.Literal;
            button.ID = field.ControlID + "_menu";
            button.Header = "Translate";
            button.Click = "Translate(\"" + field.ItemField.ID + "\")";
            button.Float = "right";
        }
    }
}
