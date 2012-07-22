using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.IO;

namespace Glass.Sitecore.Mapper.Web.Ui
{
    public class GlassEditFrame : IDisposable
    {
        EditFrame _frame;
        HtmlTextWriter _writer;
        public GlassEditFrame(string buttons, HttpContext context)
        {
            _frame = new EditFrame();
            _frame.Buttons = buttons;
            _writer = new HtmlTextWriter(context.Response.Output);

        }
        public void RenderFirstPart()
        {
            _frame.RenderFirstPart(_writer);
        }



        public void Dispose()
        {
            _frame.RenderLastPart(_writer);
        }
    }
}
