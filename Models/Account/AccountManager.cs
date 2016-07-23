using System;
using System.Web;
using System.Web.Security;
using livemenu.Kernel.Authentication;
using livemenu.Models.Auth;

namespace livemenu.Models.Account
{
    internal class AccountManager : IAccountManager
    {
        public Kernel.Authentication.Account GetCurrentUser()
        {
            var principal = HttpContext.Current.User as CustomPrincipal;
            if (principal == null)
            {
                FormsAuthentication.SignOut();
                throw new UnauthorizedAccessException();
            }
            return new Kernel.Authentication.Account()
                {
                    Login = principal.Login,
                    FirstName = principal.FirstName,
                    LastName = principal.LastName,
                    Method = principal.Method,
                    ID = principal.UserId,
                    RightSubjectID = principal.RightSubjectID
                };

        }
    }
}