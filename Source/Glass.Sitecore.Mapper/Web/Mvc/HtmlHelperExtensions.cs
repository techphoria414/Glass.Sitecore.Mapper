using System.Web.Mvc;
using System.Web.UI;
using Glass.Sitecore.Mapper.Web.Ui;
using Sitecore.Data;
using Sitecore.Web.UI.WebControls;

namespace Glass.Sitecore.Mapper.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static MvcEditFrame BeginEditFrame(this HtmlHelper htmlHelper)
        {
            return BeginEditFrame(htmlHelper, "");
        }

        public static MvcEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString());
        }

        public static MvcEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource)
        {
            return BeginEditFrame(htmlHelper, dataSource, MvcEditFrame.DefaultEditButons);
        }

        public static MvcEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource, ID buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString(), buttons.ToString());
        }

        public static MvcEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource, string buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString(), buttons);
        }

        public static MvcEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource, ID buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource, buttons.ToString());
        }

        public static MvcEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource, string buttons)
        {
            var editFrame = new EditFrame()
                {
                    Buttons = buttons,
                    DataSource = dataSource
                };

            return BeginEditFrame(htmlHelper, editFrame);
        }

        public static MvcEditFrame BeginEditFrame(this HtmlHelper htmlHelper, EditFrame editFrame)
        {
            var writter = new HtmlTextWriter(htmlHelper.ViewContext.Writer);
            editFrame.RenderFirstPart(writter);

            return new MvcEditFrame(writter, editFrame);
        }
    }
}