using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Executing;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Controllers
{
    public partial class InstructionController : LMController
    {
        public virtual ActionResult Index(LMExecutingType type)
        {
            //подменяем контекст на заданный
            var context = ServiceLocator.Current.GetInstance<ILMExecutingContext>();
            context.SetExecutingType(type, true);

            return View(type);
        }
    }
}