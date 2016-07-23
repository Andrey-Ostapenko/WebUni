using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Common.Kernel.Order;
using livemenu.Common.Kernel.Settings;
using Uni.Payment.Robokassa;

namespace livemenu.Areas.Payment.Controllers
{
    public partial class RobokassaController : Controller
    {
        private readonly ISettingsService _settingsService;
        private readonly IOrderMakerService _orderMakerService;

        public RobokassaController(ISettingsService settingsService, IOrderMakerService orderMakerService)
        {
            _settingsService = settingsService;
            _orderMakerService = orderMakerService;
        }

        private RobokassaConfig GetRobokassConfig()
        {
            var set = _settingsService.GetRobokassaSettings();
            var config = new RobokassaConfig
            {
                Login = set.ShopID,
                Mode = set.IsTestMode ? RobokassaMode.Test : RobokassaMode.Production,
                Pass1 = set.Password1,
                Pass2 = set.Password2
            };
            return config;
        }

        public virtual ActionResult Confirm(RobokassaConfirmationRequest confirmationRequest)
        {
            try
            {
                var config = GetRobokassConfig();
                if (confirmationRequest.IsQueryValid(config, RobokassaQueryType.ResultURL))
                {
                    processOrder(confirmationRequest);

                    return Content("OK"); // content for robot
                }
            }
            catch (Exception) { }
            return Content("ERR");
        }

        // So called "Success Url" in terms of Robokassa documentation.
        // Customer is redirected to this url after successful payment. 

        public virtual ActionResult Success(RobokassaConfirmationRequest confirmationRequest)
        {
            try
            {
                var config = GetRobokassConfig();
                if (confirmationRequest.IsQueryValid(config, RobokassaQueryType.SuccessURL))
                {
                    return Redirect("/" + _settingsService.GetRobokassaSettings().SuccessPageUrl);

                    //return View(new LayoutViewModel
                    //{
                    //    CurrentPageCode = "success",
                    //    LayoutModel = lm,
                    //    CurrentUrl = MVC.Payment.Robokassa.Success(),
                    //});
                }
            }
            catch (Exception) { }
            return Redirect("/" + _settingsService.GetRobokassaSettings().FailPageUrl);

        }

        // So called "Fail Url" in terms of Robokassa documentation.
        // Customer is redirected to this url after unsuccessful payment.

        public virtual ActionResult Fail()
        {
            return Redirect("/" + _settingsService.GetRobokassaSettings().FailPageUrl);
        }

        private void processOrder(RobokassaConfirmationRequest confirmationRequest)
        {
            _orderMakerService.IncomeRecived(confirmationRequest.InvId, confirmationRequest.GetSum(), OrderIncomeRecivedVerificationMode.OnlyFullPrice);
        }
    }
}