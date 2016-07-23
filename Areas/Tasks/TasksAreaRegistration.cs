﻿using System.Web.Mvc;

namespace livemenu.Areas.Tasks
{
    public class TasksAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Tasks";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute(
          "Tasks_default_Home",
          "Tasks/{controller}/{action}/{id}",
          new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Tasks" },
          new[] { "livemenu.Areas.Tasks.Controllers" }
      );

            context.MapRoute(
                "Tasks_default",
                "Tasks/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                    new[] { "livemenu.Areas.Tasks.Controllers" }
            );
        }
    }
}