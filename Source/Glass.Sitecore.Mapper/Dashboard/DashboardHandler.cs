using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Collections.Specialized;
using Glass.Sitecore.Mapper.Dashboard.Web;

namespace Glass.Sitecore.Mapper.Dashboard
{
    public class DashboardHandler : IHttpHandler
    {



        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            
            //this is a very simple router
            
            string[] parts = context.Request.Path.Replace(".gls",string.Empty).Split(new[]{'/'}, StringSplitOptions.RemoveEmptyEntries);

            string controllerName = parts[0];
            string actionName = parts.Count() > 1 ? parts[1] : "Index";
            
            var controller = GetController(controllerName);

            controller.Context = context;
            controller.GlassContext = Context.StaticContext;

            var view = InvokeAction(controller, actionName, context.Request.QueryString);

            view.Response(context.Response);


        }

        private static readonly Type _controllerBase = typeof(AbstractController);

        public AbstractView InvokeAction(AbstractController controller, string action, NameValueCollection parameters)
        {
            Type cType = controller.GetType();
            var method = cType.GetMethods().FirstOrDefault(x => x.Name.ToLower() == action.ToLower());

            if (method == null) throw new HttpException(404, "Not Found");

            var paraInfos = method.GetParameters();

            var keys = parameters.Keys.Cast<string>();

            //this will contain the list of parameters in the correct order
            List<string> finalParams = new List<string>();

            //get the paramaters in the correct order
            foreach (var paraInfo in paraInfos)
            {
                var key = keys.FirstOrDefault(x => x.ToLower() == paraInfo.Name.ToLower());
                if (key.IsNotNullOrEmpty())
                {
                    finalParams.Add(parameters[key]);
                }
                //if we can find a parameter for the method throw a not found
                else
                    throw new HttpException(404, "Not Found");
            }

            var view = method.Invoke(controller, finalParams.ToArray());

            if (view is AbstractView)
                return view as AbstractView;
            else
                throw new HttpException(500, "Incorrect view");

        }

        public AbstractController GetController(string controllerName)
        {
            var contType = Controllers.FirstOrDefault(x => x.Name.ToLower() == controllerName.ToLower() + "controller");

            if (contType == null) throw new HttpException(404, "Not Found");

            var controller = Activator.CreateInstance(contType) as AbstractController;

            if (controller == null) throw new HttpException(404, "Not Found");


            return controller;
        }


        private IEnumerable<Type> Controllers
        {
            get
            {
                var ass = Assembly.GetExecutingAssembly();
                foreach (var type in ass.GetTypes())
                {
                    if (type.IsSubclassOf(_controllerBase))
                    {
                        yield return type;
                    }
                    continue;
                }
            }
        }

     



    }
}
