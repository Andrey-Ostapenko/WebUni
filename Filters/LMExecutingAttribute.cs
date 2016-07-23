using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using livemenu.Kernel.Authentication;
using livemenu.Kernel.Executing;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Filters
{
    public class LMExecutingAttribute : ActionFilterAttribute
    {
        private readonly LMExecutingType _executingType;

        public LMExecutingAttribute(LMExecutingType executingType)
        {
            _executingType = executingType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = ServiceLocator.Current.GetInstance<ILMExecutingContext>();
            context.SetExecutingType(_executingType);
            base.OnActionExecuting(filterContext);
        }
    }
}