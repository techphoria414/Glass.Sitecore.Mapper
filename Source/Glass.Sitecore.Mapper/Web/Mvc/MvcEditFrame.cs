using System;
using System.Web.UI;
using Sitecore.Web.UI.WebControls;

namespace Glass.Sitecore.Mapper.Web.Mvc
{
    public class MvcEditFrame : IDisposable 
    {
        public const string DefaultEditButons = "/sitecore/content/Applications/WebEdit/Edit Frame Buttons/Default";

        private bool _disposed;
        private readonly HtmlTextWriter _writer;
        private readonly EditFrame _editFrame;

        public MvcEditFrame(HtmlTextWriter writer, EditFrame editFrame)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            _editFrame = editFrame;

            this._writer = writer;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        /// <summary>
        /// Releases unmanaged and, optionally, managed resources used by the current instance of the <see cref="T:System.Web.Mvc.Html.MvcForm"/> class.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
                return;
            this._disposed = true;
            _editFrame.RenderLastPart(_writer);
        }

        /// <summary>
        /// Ends the form and disposes of all form resources.
        /// </summary>
        public void EndForm()
        {
            this.Dispose(true);
        }
        
    }
}