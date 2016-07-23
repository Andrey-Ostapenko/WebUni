using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Common;
using livemenu.Common.Kernel.Settings;
using livemenu.Kernel.Account;
using livemenu.Kernel.Account.License;
using livemenu.Kernel.Authentication;
using livemenu.Models.Account;
using livemenu.Models.Widgets;

namespace livemenu.Controllers
{
    public partial class AccountController : LMController
    {
        private readonly IAccountService _accountService;
        private readonly IAccountManager _accountManager;
        private readonly IWebUniAccountProvider _WebUniAccountProvider;
        private readonly IWebUniLicenseProvider _WebUniLicenseProvider;
        private readonly IContainerFactory _containerFactory;

        public AccountController(IAccountService accountService, IAccountManager accountManager, IWebUniAccountProvider WebUniAccountProvider, IWebUniLicenseProvider WebUniLicenseProvider, IContainerFactory containerFactory)
        {
            _accountService = accountService;
            _accountManager = accountManager;
            _WebUniAccountProvider = WebUniAccountProvider;
            _WebUniLicenseProvider = WebUniLicenseProvider;
            _containerFactory = containerFactory;
        }


        public virtual ActionResult Index2()
        {
            var account = _WebUniAccountProvider.GetAccount();



            return View(account);
        }


        public virtual ActionResult Index()
        {
            var model = new WidgetsViewModel()
            {
               
                IsPartial = false,
                Widgets = new[]
                {
                    new SimpleWidgetModel
                    {
                        Url = MVC.Account.Internal(),
                        Title = "Профиль пользователя",
                        Settings =
                        { 
                            AvailableMenuItems = new List<StandartWidgetMenuItem>()
                            {
                                StandartWidgetMenuItem.Save
                            }
                        }
                    }
                }
            };

            return View(MVC.CMS.MicroModule.Views.Index, model);
        }

        private IPasswordChanger GetPasswordChanger()
        {
            return _containerFactory.Instantiate<IPasswordChanger>(_accountManager.GetCurrentUser().PasswordMethod);
        }

        public virtual ActionResult Password()
        {
            var pc = GetPasswordChanger();
            return View(pc.GetAccountPasswordModel());
        }
        public virtual ActionResult ChangePassword()
        {
            return View();
        }
        public virtual ActionResult SavePassword(AccountChangePasswordModel model)
        {
            
            if (ModelState.IsValid)
            {
                OperationWrapper(() =>
                {
                    var pc = GetPasswordChanger();
                    pc.ChangePassword(model);
                    return true;
                });


                return View(MVC.Account.Views.PasswordInternal);

            }
            return View(MVC.Account.Views.ChangePassword, model);
        }

        public virtual ActionResult Internal()
        {
            return View(MVC.Account.Views.Index, _accountService.GetMyProfile());
        }


        [HttpGet]
        public virtual ActionResult GetMyProfile()
        {
            return View(MVC.Account.Views.MyProfileInternal, _accountService.GetMyProfile());
        }


        [HttpPost]
        public virtual ActionResult Save(MyProfileModel model)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(
                    () =>
                    {
                        _accountService.Save(model);
                    });

                return View(MVC.Account.Views.MyProfileInternal, _accountService.GetMyProfile());
            }
            else
            {
                return View(MVC.Account.Views.MyProfileInternal, model);
            }

        }


        public virtual ActionResult License()
        {
            var license = _WebUniLicenseProvider.GetLicense();
            if (license.HasLicense)
            {
                var model = new AccountWebUniLicenseModel()
                {
                    HasLicense = true,
                    WebUniLicense = license,
                    EndDate =
                        license.Type == WebUniLicenseType.Rent
                            ? license.StartDate.Value.AddMonths(license.MonthCount.Value)
                            : DateTime.MaxValue,

                    TypeName = livemenu.Helpers.Helpers.GetWebUniLicenseTariffName(license.Tariff),
                    TariffName = livemenu.Helpers.Helpers.GetWebUniLicenseTariffName(license.Tariff),
                    PeriodName =
                        license.Type == WebUniLicenseType.Rent
                            ? livemenu.Helpers.Helpers.GetWebUniLicensePeriodName(license.MonthCount.Value)
                            : string.Empty,
                };
                return View(model);
            }
            else
            {
                return View(new AccountWebUniLicenseModel
                {
                    HasLicense = false
                });
            }
        }
    }
}