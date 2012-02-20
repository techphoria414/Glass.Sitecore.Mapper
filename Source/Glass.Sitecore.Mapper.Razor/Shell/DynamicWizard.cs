
using NVelocity;
using NVelocity.App;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.IO;
using Sitecore.Shell.Framework;
using Sitecore.Shell.Framework.CommandBuilders;
using Sitecore.Text;
using Sitecore.Text.NVelocity;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XmlControls;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Web;
using Sitecore.Shell;
namespace Sitecore.Sharedsource.RazorForSitecore.Wizards
{
    /// <summary>
    /// Represents a Razor CreateView Wizard.
    /// </summary>
    public class CreateView : WizardForm
    {
        /// <summary></summary>
        protected DataContext DataContext;
        /// <summary></summary>
        protected TreeviewEx LocationTreeview;
        /// <summary></summary>
        protected DataContext FileDataContext;
        /// <summary></summary>
        protected TreeviewEx FileLocationTreeview;
        /// <summary></summary>
        protected Literal Welcome;
        /// <summary></summary>
        protected Edit FileName;
        /// <summary></summary>
        protected Edit ModelType;
        /// <summary></summary>
        protected XmlControl FirstPage;
        /// <summary></summary>
        protected XmlControl Name;
        /// <summary></summary>
        protected XmlControl Location;
        /// <summary></summary>
        protected XmlControl FileLocation;
        /// <summary></summary>
        protected XmlControl LastPage;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Shell.Applications.Layouts.IDE.Wizards.NewFileWizard.IDENewFileWizardForm" /> class.
        /// </summary>
        public CreateView()
        {
            base.EventsEnabled = true;
        }
        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            base.HandleMessage(message);
            Item item;
            if (!(message.Name == "newfile:refresh"))
            {
                if (!string.IsNullOrEmpty(message["id"]))
                {
                    item = this.DataContext.GetItem(message["id"]);
                }
                else
                {
                    item = this.DataContext.GetFolder();
                }
                Dispatcher.Dispatch(message, item);
                return;
            }
            item = this.FileDataContext.GetItem(message.Arguments["parentid"]);
            if (item == null)
            {
                return;
            }
            Item item2 = item.Children[message.Arguments["name"]];
            if (item2 != null)
            {
                this.FileDataContext.SetFolder(item2.Uri);
                this.FileLocationTreeview.SetSelectedItem(item2);
            }
        }


        /// <summary>
        /// Raises the load event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>
        /// <remarks>
        /// This method notifies the server control that it should perform actions common to each HTTP
        /// request for the page it is associated with, such as setting up a database query. At this
        /// stage in the page lifecycle, server controls in the hierarchy are created and initialized,
        /// view state is restored, and form controls reflect client-side data. Use the IsPostBack
        /// property to determine whether the page is being loaded in response to a client postback,
        /// or if it is being loaded and accessed for the first time.
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);

            var x = Context.Item.ID;


            if (Context.ClientPage.IsEvent)
            {
                return;
            }
            UrlString settings = CreateView.GetSettings();
            if (settings == null)
            {
                return;
            }
            string value = settings["icon"];
            if (!string.IsNullOrEmpty(value))
            {
                this.FirstPage["Icon"] = value;
                this.Name["Icon"] = value;
                this.Location["Icon"] = value;
                this.FileLocation["Icon"] = value;
                this.LastPage["Icon"] = value;
            }
            string text = settings["welcome"];
            if (!string.IsNullOrEmpty(text))
            {
                this.Welcome.Text = text;
            }
            string value2 = settings["name"];
            if (!string.IsNullOrEmpty(value2))
            {
                this.FileName.Value = value2;
            }
            string text2 = settings["path"];
            if (!string.IsNullOrEmpty(text2))
            {
                this.DataContext.Root = text2;
            }
            string text3 = Registry.GetString("/Current_User/History/IDE.New.File.Wizard");
            string text4 = settings["filepath"];
            if (!string.IsNullOrEmpty(text4) && !text3.StartsWith(text4, StringComparison.InvariantCultureIgnoreCase))
            {
                text3 = text4;
            }
            if (!string.IsNullOrEmpty(text3))
            {
                this.FileDataContext.Folder = text3;
            }


            string locationId = WebUtil.GetQueryString("locationId");
            Database master = Sitecore.Configuration.Factory.GetDatabase("master");
            Item location = master.GetItem(ID.Parse(locationId));
            ItemUri folderUri = new ItemUri(location);
            this.DataContext.SetFolder(folderUri);

        }
        /// <summary>
        /// Called when the active page is changing.
        /// </summary>
        /// <param name="page">The page that is being left.</param>
        /// <param name="newpage">The new page that is being entered.</param>
        /// <returns>
        /// True, if the change is allowed, otherwise false.
        /// </returns>
        /// <remarks>Set the newpage parameter to another page ID to control the
        /// path through the wizard pages.</remarks>
        protected override bool ActivePageChanging(string page, ref string newpage)
        {
            Assert.ArgumentNotNull(page, "page");
            Assert.ArgumentNotNull(newpage, "newpage");
            if (page == "Name" && newpage == "Model")
            {
                if (this.FileName.Value.Length == 0)
                {
                    SheerResponse.Alert("You must specify a name for the file.", new string[0]);
                    return false;
                }
                if (!Regex.IsMatch(this.FileName.Value, Settings.ItemNameValidation, RegexOptions.ECMAScript))
                {
                    SheerResponse.Alert("The name contains invalid characters.", new string[0]);
                    return false;
                }
                if (this.FileName.Value.Length > Settings.MaxItemNameLength)
                {
                    SheerResponse.Alert("The name is too long.", new string[0]);
                    return false;
                }
                string value = this.FileName.Value;
                if (value.IndexOfAny(Path.GetInvalidPathChars()) >= 0 || value == ".")
                {
                    SheerResponse.Alert("The name contains invalid characters.", new string[0]);
                    return false;
                }
            }
            if (page == "Model" && newpage == "Location")
            {
                if (this.ModelType.Value.Length == 0)
                {
                    //No model
                }
                else
                {
                    Type modelType = Type.GetType(this.ModelType.Value);

                    if (modelType == null)
                    {
                        SheerResponse.Alert("The model type does not exist.", new string[0]);
                        return false;
                    }
                }
            }
            if (page == "Location" && newpage == "FileLocation")
            {
                Item selectionItem = this.LocationTreeview.GetSelectionItem();
                if (selectionItem != null && !selectionItem.Access.CanCreate())
                {
                    SheerResponse.Alert(Translate.Text("You do not have permission to create an item rendering at \"{0}\".", new object[]
					{
						selectionItem.Paths.Path
					}), new string[0]);
                    return false;
                }
            }
            if (page == "FileLocation" && newpage == "LastPage")
            {
                UrlString settings = CreateView.GetSettings();
                if (settings != null)
                {
                    Item selectionItem2 = this.FileLocationTreeview.GetSelectionItem();
                    if (selectionItem2 != null)
                    {
                        string value2 = this.FileName.Value;
                        string @string = StringUtil.GetString(new string[]
						{
							settings["ext"]
						});
                        string string2 = StringUtil.GetString(new string[]
						{
							selectionItem2["Path"]
						});
                        string filename = CreateView.GetFilename(value2, string2, @string);

                        string razorFilename = CreateView.GetFilename(value2, string2, "cshtml");


                        if (File.Exists(FileUtil.MapPath(filename)) || File.Exists(FileUtil.MapPath(razorFilename)))
                        {
                            SheerResponse.Alert(Translate.Text("A file with the path \"{0}\" or \"{1}\" already exists.\n\nEnter a different name.", new object[]
							{
								filename, razorFilename
							}), new string[0]);
                            return false;
                        }
                    }
                }
            }
            return base.ActivePageChanging(page, ref newpage);
        }
        /// <summary>
        /// Called when the active page has been changed.
        /// </summary>
        /// <param name="page">The page that has been entered.</param>
        /// <param name="oldPage">The page that was left.</param>
        protected override void ActivePageChanged(string page, string oldPage)
        {
            Assert.ArgumentNotNull(page, "page");
            Assert.ArgumentNotNull(oldPage, "oldPage");
            this.NextButton.Header = "Next >";
            if (page == "FileLocation")
            {
                this.NextButton.Header = "Create >";
            }
            base.ActivePageChanged(page, oldPage);
            if (page == "LastPage")
            {
                this.BackButton.Disabled = true;
                this.CreateFile();
                Sitecore.Context.ClientPage.SendMessage(this, "item:refreshchildren");
            }
        }
        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <returns>The file.</returns>
        protected bool CreateFile()
        {
            Item selectionItem = this.LocationTreeview.GetSelectionItem();
            if (selectionItem == null)
            {
                SheerResponse.Alert("Location not found.", new string[0]);
                return false;
            }
            Item selectionItem2 = this.FileLocationTreeview.GetSelectionItem();
            if (selectionItem2 == null)
            {
                SheerResponse.Alert("File location not found.", new string[0]);
                return false;
            }
            UrlString settings = CreateView.GetSettings();
            if (settings == null)
            {
                return false;
            }
            string value = this.FileName.Value;
            string modelType = this.ModelType.Value;
            string sublayoutFilenameValue = value;
            string razorFilenameValue = value;

            string @string = StringUtil.GetString(new string[]
			{
				settings["ext"]
			});
            string sublayoutExtension = @string;
            string razorExtension = "cshtml";

            string string2 = StringUtil.GetString(new string[]
			{
				selectionItem2["Path"]
			});
            string sublayoutPath = string2;

            string string3 = StringUtil.GetString(new string[]
			{
				settings["filetemplate"]
			});
            string sublayoutTemplate = string3;


            string string4 = StringUtil.GetString(new string[]
			{
				settings["master"]
			});
            string string5 = StringUtil.GetString(new string[]
			{
				settings["field"], 
				"Path"
			});
            bool flag = StringUtil.GetString(new string[]
			{
				settings["useidentifier"]
			}) == "1";
            Registry.SetString("/Current_User/History/IDE.New.File.Wizard", string2);
            if (!selectionItem.Access.CanCreate())
            {
                SheerResponse.Alert(Translate.Text("You do not have permission to create an item at \"{0}\".", new object[]
				{
					selectionItem.Paths.Path
				}), new string[0]);
                return false;
            }
            if (!Regex.IsMatch(value, Settings.ItemNameValidation, RegexOptions.ECMAScript))
            {
                SheerResponse.Alert("The name contains invalid characters.", new string[0]);
                return false;
            }
            if (value.Length > Settings.MaxItemNameLength)
            {
                SheerResponse.Alert("The name is too long.", new string[0]);
                return false;
            }
            string sublayoutFilename = CreateView.GetFilename(value, sublayoutPath, sublayoutExtension);

            string razorFilename = CreateView.GetFilename(value, sublayoutPath, razorExtension);


            if (sublayoutFilename.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                SheerResponse.Alert("The name contains invalid characters.", new string[0]);
                return false;
            }
            if (File.Exists(FileUtil.MapPath(sublayoutFilename)) || File.Exists(FileUtil.MapPath(razorFilename)))
            {
                SheerResponse.Alert(Translate.Text("A file with the path \"{0}\" or \"{1}\" already exists.\n\nEnter a different name.", new object[]
				{
					sublayoutFilename,
                    razorFilename
				}), new string[0]);
                return false;
            }
            BranchItem master = Client.ContentDatabase.Branches.GetMaster(string4);
            if (master == null)
            {
                SheerResponse.Alert(Translate.Text("Branch \"{0}\" not found.", new object[]
				{
					string4
				}), new string[0]);
                return false;
            }
            Item item = selectionItem.Add(value, master);
            Assert.IsNotNull(item, "added item");

            this.CreateFileFromTemplate(item, sublayoutFilename, sublayoutTemplate);

            string sublayoutParameters = "razorPath=" + razorFilename;
            string razorFileTemplate;

            item.Editing.BeginEdit();
            item["Path"] = sublayoutFilename;

            if (modelType.Length <= 0)
            {
                razorFileTemplate = "/sitecore modules/Shell/RazorForSitecore/Templates/razorview.cshtml";
                this.CreateFileFromTemplate(item, razorFilename, razorFileTemplate);
            }
            else
            {
                if (Type.GetType(modelType) == null)
                {
                    SheerResponse.Alert("The model type does not exist.", new string[0]);
                    return false;
                }

                razorFileTemplate = "/sitecore modules/Shell/RazorForSitecore/Templates/razorview.model.cshtml";

                string modelTypeWithoutAssembly = modelType.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];

                this.CreateFileFromTemplateAndFormat(item, razorFilename, razorFileTemplate, modelTypeWithoutAssembly);

                sublayoutParameters += "&modelType=" + modelType;
            }

            item["Parameters"] = sublayoutParameters;

            item.Appearance.Icon = "applications/32x32/document_attachment.png";
            if (flag)
            {
                item[string5] = Regex.Replace(Path.GetFileNameWithoutExtension(sublayoutFilename), "\\W", string.Empty);
            }
            else
            {
                item[string5] = sublayoutFilename;
            }
            item.Editing.EndEdit();
            Log.Audit(this, "Create file: {0}, file: {1}", new string[]
			{
				AuditFormatter.FormatItem(item), 
				sublayoutFilename
			});
            Log.Audit(this, "Create file: {0}, file: {1}", new string[]
			{
				AuditFormatter.FormatItem(item), 
				razorFilename
			});
            SheerResponse.SetDialogValue(string.Concat(new object[]
			{
				"id=", 
				item.ID, 
				"&filename=", 
				sublayoutFilename, 
				"&settingsid=", 
				WebUtil.GetQueryString("id")
			}));
            return true;
        }
        /// <summary>
        /// Creates the new folder.
        /// </summary>
        /// <param name="shortID">The short ID.</param>
        protected void CreateFolder(string shortID)
        {
            Assert.ArgumentNotNullOrEmpty(shortID, "shortID");
            Assert.IsTrue(ShortID.IsShortID(shortID), "is short id");
            Item item = this.FileDataContext.GetItem(ShortID.DecodeID(shortID));
            if (item == null)
            {
                SheerResponse.Alert("The item could not be found.\n\nIt may have been deleted by another user.", new string[0]);
                return;
            }
            CommandBuilder commandBuilder = new CommandBuilder("newfile:refresh");
            commandBuilder.Add("parentid", item.ID.ToString());
            Files.NewFolder(item["Path"], commandBuilder.ToString());
        }
        /// <summary>
        /// Gets the file context menu.
        /// </summary>
        protected void GetFileContextMenu()
        {
            string text = Context.ClientPage.ClientRequest.Source;
            if (text.IndexOf("_") >= 0)
            {
                text = text.Substring(text.IndexOf("_") + 1);
            }
            Menu menu = new Menu();
            menu.Add("MINewFolder", "New folder", "Applications/16x16/folder_new.png", string.Empty, "CreateFolder(\"" + text + "\")", false, string.Empty, MenuItemType.Normal);
            SheerResponse.ShowContextMenu(Context.ClientPage.ClientRequest.Control, string.Empty, menu);
        }
        /// <summary>
        /// Creates the file from template.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="fileTemplate">The file template.</param>
        /// <returns>The file from template.</returns>
        private void CreateFileFromTemplate(Item item, string filename, string fileTemplate)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(filename, "filename");
            Assert.ArgumentNotNull(fileTemplate, "fileTemplate");
            string text = string.Empty;
            if (!string.IsNullOrEmpty(fileTemplate))
            {
                text = FileUtil.ReadUTF8File(fileTemplate);
                try
                {
                    Velocity.Init();
                    VelocityContext velocityContext = new VelocityContext();
                    VelocityHelper.PopulateFromSitecoreContext(velocityContext);
                    velocityContext.Put("fullFilename", filename);
                    velocityContext.Put("filename", Path.GetFileName(filename));
                    velocityContext.Put("filenameWithoutExtension", Path.GetFileNameWithoutExtension(filename));
                    velocityContext.Put("identifier", Regex.Replace(Path.GetFileNameWithoutExtension(filename), "\\W", string.Empty));
                    velocityContext.Put("translate", new Translate());
                    velocityContext.Put("item", item);
                    velocityContext.Put("parent", item.Parent);
                    text = VelocityHelper.Evaluate(velocityContext, text, "Velocity Layout Replacer");
                }
                catch (Exception arg_DB_0)
                {
                    Exception arg = arg_DB_0;
                    Log.Error(string.Format("Error parsing file template {0}: {1}.", fileTemplate, arg), this);
                }
            }
            FileUtil.WriteUTF8File(filename, text);
        }

        private void CreateFileFromTemplateAndFormat(Item item, string filename, string fileTemplate, params string[] args)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(filename, "filename");
            Assert.ArgumentNotNull(fileTemplate, "fileTemplate");
            string text = string.Empty;
            if (!string.IsNullOrEmpty(fileTemplate))
            {
                text = FileUtil.ReadUTF8File(fileTemplate);
                try
                {
                    Velocity.Init();
                    VelocityContext velocityContext = new VelocityContext();
                    VelocityHelper.PopulateFromSitecoreContext(velocityContext);
                    velocityContext.Put("fullFilename", filename);
                    velocityContext.Put("filename", Path.GetFileName(filename));
                    velocityContext.Put("filenameWithoutExtension", Path.GetFileNameWithoutExtension(filename));
                    velocityContext.Put("identifier", Regex.Replace(Path.GetFileNameWithoutExtension(filename), "\\W", string.Empty));
                    velocityContext.Put("translate", new Translate());
                    velocityContext.Put("item", item);
                    velocityContext.Put("parent", item.Parent);
                    text = VelocityHelper.Evaluate(velocityContext, text, "Velocity Layout Replacer");
                }
                catch (Exception arg_DB_0)
                {
                    Exception arg = arg_DB_0;
                    Log.Error(string.Format("Error parsing file template {0}: {1}.", fileTemplate, arg), this);
                }
            }

            text = string.Format(text, args);

            FileUtil.WriteUTF8File(filename, text);
        }


        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <param name="ext">The ext.</param>
        /// <returns>The filename.</returns>
        private static string GetFilename(string name, string path, string ext)
        {
            Assert.ArgumentNotNull(name, "name");
            Assert.ArgumentNotNull(path, "path");
            Assert.ArgumentNotNull(ext, "ext");
            string text = name;
            if (!ext.StartsWith("."))
            {
                ext = "." + ext;
            }
            if (!text.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
            {
                text += ext;
            }
            if (!text.StartsWith("/"))
            {
                text = FileUtil.MakePath(path, text, '/');
            }
            return Assert.ResultNotNull<string>(text);
        }
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns>The settings.</returns>
        private static UrlString GetSettings()
        {
            string queryString = WebUtil.GetQueryString("id");
            Item item = Client.CoreDatabase.GetItem(queryString);
            if (item != null)
            {
                return new UrlString(item["Custom Data"]);
            }
            return null;
        }
    }
}


