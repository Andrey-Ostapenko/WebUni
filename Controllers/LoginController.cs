using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using livemenu.Common.Kernel.Settings;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Configuration;
using livemenu.Kernel.Executing;
using livemenu.Kernel.Users;
using livemenu.Models.Auth;
using livemenu.Models.Login;
using Newtonsoft.Json;

namespace livemenu.Controllers
{
    public partial class LoginController : LMController
    {
        private static readonly ActionResult DefaultRedirectUrl = MVC.Home.Index();
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IConfigurationService _configurationService;
        private readonly IUserService _userService;
        private readonly ISettingsService _settingsService;

        public LoginController(IAuthenticationManager authenticationManager, IConfigurationService configurationService, IUserService userService, ISettingsService settingsService)
        {
            _authenticationManager = authenticationManager;
            _configurationService = configurationService;
            _userService = userService;
            _settingsService = settingsService;
        }

        [AllowAnonymous]
        public virtual ActionResult Last()
        {
            return Content(last ?? "null");
        }

        public static string last;

        [AllowAnonymous]
        public virtual ActionResult Index(string returnUrl = null)
        {
            if (HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction(DefaultRedirectUrl);
            var current = LMExecutingType.CMS;
            
            if (returnUrl != null && !returnUrl.Contains("signalr"))
                last = returnUrl;


            if (returnUrl == null || returnUrl.ToLower() == "/" || returnUrl.ToLower() == "/admin/" || returnUrl.ToLower() == "/admin" || returnUrl.ToLower() == "admin")
            {
            }
            else
            {
                var mret = returnUrl.ToLower().Replace("/admin", string.Empty);
                current = Enum.GetValues(typeof(LMExecutingType)).Cast<LMExecutingType>().FirstOrDefault(ss => mret != null
                   && (mret.StartsWith("/" + ss.ToString().ToLower()) || returnUrl.StartsWith(ss.ToString().ToLower())));
            }

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
                LmExecutingType = current,
                LoginPageSettings = _settingsService.GetLoginPageSettings(),
                BaseApplicationName = _configurationService.GetBWPConfig().BaseApplication.Name
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult Login(LoginViewModel viewModel)
        {
            bool result = false;

            if (ModelState.IsValid)
            {
                var request = BuildAuthenticationRequest(viewModel);
                var response = _authenticationManager.Authenticate(request);

                if (response.Result)
                {
                    var userInfo = _authenticationManager.GetAuthenticatedUserInfo(response.Method.Value, request);

                    int lID = 0;
                    long? lrs = null;


                    if (response.Method == AuthenticationMethod.Local)
                    {
                        lID = (int)((LocalUserInfo)userInfo).ID;
                        lrs = ((LocalUserInfo)userInfo).RightSubjectID;
                    }
                    var serializeModel = new CustomPrincipalSerializeModel
                    {
                        Login = viewModel.Login,
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        Method = response.Method.Value,
                        ID = lID,
                        RightSubjectID = lrs

                    };
                    

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        viewModel.Login,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(60),
                        false,
                        userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    result = response.Result;
                }
            }

            var redirectUrl = string.IsNullOrEmpty(viewModel.ReturnUrl) ? Url.Action(DefaultRedirectUrl) : viewModel.ReturnUrl;

            return Json(new JsonLoginResult() { Result = result, Error = "Логин и/или пароль введены неверно", RedirectUrl = redirectUrl });




        }

        private AuthenticationRequest BuildAuthenticationRequest(LoginViewModel loginViewModel)
        {
            return new AuthenticationRequest() { Password = loginViewModel.Password, UserName = loginViewModel.Login };
        }

        public virtual ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
           
            return RedirectToAction(MVC.Login.Index());
        }

    }
}