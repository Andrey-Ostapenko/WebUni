using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using DevExpress.Web.Mvc;
using livemenu.Kernel.Authentication;
using livemenu.ModelBinders;
using livemenu.Models.Auth;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;

namespace livemenu
{
    class DoubleModelBinder : IModelBinder
    {
        #region IModelBinder Members

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string numStr = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;
            double res;

            if (!double.TryParse(numStr, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out res))
            {
                if (bindingContext.ModelType == typeof(double?))
                    return null;
                throw new ArgumentException();
            }

            if (bindingContext.ModelType == typeof(double?))
                return new Nullable<double>(res);
            else
                return res;
        }

        #endregion
    }
    public class MvcApplication : System.Web.HttpApplication
    {
        public static bool PreventGZip = false;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
          


            T4MVCHelpers.ProcessVirtualPath = name => name;

            LMBootstrapper.Initialize();
           
           
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Web.Mvc.ModelBinders.Binders.DefaultBinder = new DevExpressEditorsBinder();

            System.Web.Mvc.ModelBinders.Binders.Add(typeof(IDictionary<string, object>), new DictionaryModelBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(double), new DoubleModelBinder());


            DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;

            GlobalHost.Configuration.DisconnectTimeout = new System.TimeSpan(24, 0, 0); // signalR
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var dotcount = Request.Url.Host.Count(x => x == '.');

            if (!Request.Url.IsLoopback && dotcount == 1 && !Request.Url.Host.StartsWith("www"))
            {
                UriBuilder builder = new UriBuilder(Request.Url);
                builder.Host = "www." + Request.Url.Host;
                Response.Redirect(builder.ToString(), true);
            }
            else
            {
                //if (!Debugger.IsAttached && Request.Params["nogzip"] == null && !PreventGZip)
                //{

                //    HttpApplication app = (HttpApplication)sender;
                //    string acceptEncoding = app.Request.Headers["Accept-Encoding"];
                //    Stream prevUncompressedStream = app.Response.Filter;

                //    if (acceptEncoding == null || acceptEncoding.Length == 0)
                //        return;

                //    acceptEncoding = acceptEncoding.ToLower();

                //    if (acceptEncoding.Contains("gzip"))
                //    {
                //        // gzip
                //        app.Response.Filter = new GZipStream(prevUncompressedStream,
                //            CompressionMode.Compress);
                //        app.Response.AppendHeader("Content-Encoding", "gzip");
                //    }
                //    else if (acceptEncoding.Contains("deflate"))
                //    {
                //        // defalte
                //        app.Response.Filter = new DeflateStream(prevUncompressedStream, CompressionMode.Compress);
                //        app.Response.AppendHeader("Content-Encoding", "deflate");
                //    }
                //}
            }


        }

        protected void Application_EndRequest()
        {
            GC.Collect();
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            DevExpressHelper.Theme = "MetropolisBlue";
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                if (serializeModel == null || (serializeModel.Method == AuthenticationMethod.Local && !serializeModel.RightSubjectID.HasValue))
                {
                    FormsAuthentication.SignOut();
                    return;
                    
                }

                var newUser = new CustomPrincipal(authTicket.Name)
                    {
                        FirstName = serializeModel.FirstName,
                        LastName = serializeModel.LastName,
                        Login = serializeModel.Login,
                        Method = serializeModel.Method,
                        UserId = serializeModel.ID,
                        RightSubjectID = serializeModel.RightSubjectID

                    };

                HttpContext.Current.User = newUser;
            }



            //string cultureName = null;
            //// Attempt to read the culture cookie from Request
            //HttpCookie cultureCookie = Request.Cookies["_culture"];
            //if (cultureCookie != null)
            //    cultureName = cultureCookie.Value;
            //else
            //    cultureName = Request.UserLanguages != null && Request.UserLanguages.Any() ? Request.UserLanguages[0] : "ru"; // obtain it from HTTP header AcceptLanguages

            // Validate culture name
           var cultureName = "ru-RU";


            // Modify current thread's culture            
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
           
            
    //           var cultureName = "ru-RU";
    //        var nfInfo = new System.Globalization.CultureInfo(cultureName, false)
    //        {
    //            NumberFormat =
    //{
    //    NumberDecimalSeparator = "."
    //}
    //        };

    //        // Modify current thread's culture            
    //        Thread.CurrentThread.CurrentCulture = nfInfo;
    //        Thread.CurrentThread.CurrentUICulture = nfInfo;
        }

     

        protected void Application_Error(object sender, EventArgs e) 
        {
            Exception exception = System.Web.HttpContext.Current.Server.GetLastError();
            //TODO: Handle Exception
        }

    }
}
