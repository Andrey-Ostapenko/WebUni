using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using livemenu.Common.Entities;
using livemenu.Common.Kernel.Helpers;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Configuration;
using livemenu.Kernel.Rights;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Models.Auth
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly AccessMode _accessMode;
        private readonly SimpleRightValue[] _sr;

        public CustomAuthorizeAttribute(AccessMode accessMode = AccessMode.Standart, SimpleRightValue[] sr = null)
        {
            _accessMode = accessMode;
            _sr = sr ?? new SimpleRightValue[0];
        }

        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        public static ActionResult RedirectToAccessDeniedRoute = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "AccessDenied" }));

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var accessMode = Kernel.Authentication.Account.GetAccessMode(CurrentUser.Method);
                if (_accessMode == AccessMode.Admin && AccessMode.Admin != accessMode)
                {
                    filterContext.Result = RedirectToAccessDeniedRoute;
                }
                else if (_accessMode == AccessMode.OnlyWebUni)
                {
                    var mconf = ServiceLocator.Current.GetInstance<IMainConfigService>();
                    if (!mconf.IsWebUniProject && !Debugger.IsAttached)
                    {
                        filterContext.Result = RedirectToAccessDeniedRoute;
                    }
                }

                var scope = ServiceLocator.Current.GetInstance<ISimpleRightsContextScope>();

                _sr.ForEach(s =>
                {
                    if (!scope.CheckAccess(s))
                    {
                        filterContext.Result = RedirectToAccessDeniedRoute;
                    }
                });
            }
            base.OnAuthorization(filterContext);
        }
    }
}