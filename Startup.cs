using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Owin;

namespace livemenu
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            var appfolder = string.Empty;
            if (HttpContext.Current != null && !HttpContext.Current.IsDebuggingEnabled)
            {
                appfolder = "/admin";
            }
            string signalrPath = appfolder + "/signalr";

            app.MapSignalR();
            GlobalHost.HubPipeline.RequireAuthentication();
            //todo: DI
        }
    }
}