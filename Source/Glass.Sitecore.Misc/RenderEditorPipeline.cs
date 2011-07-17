using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Pipelines;
using Sitecore.Shell.Applications.ContentEditor.Pipelines.RenderContentEditor;
using Sitecore.Shell.Applications.ContentEditor;

namespace Glass.Sitecore.Misc
{
    public class RenderEditorPipeline : PipelineArgs
    {
        public void Process(RenderContentEditorArgs args)
        {
            EditorFormatter formatter = args.EditorFormatter;
            ThreeCol three = new ThreeCol();
            three.Arguments = formatter.Arguments;
            three.IsFieldEditor = formatter.IsFieldEditor;


            args.EditorFormatter = three;
        }
    }
}
