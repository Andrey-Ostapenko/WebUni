using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using livemenu.Kernel.Authentication;

namespace livemenu.Models.Auth
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public int UserId { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] roles { get; set; }

        public long? RightSubjectID { get; set; }
        public AuthenticationMethod Method { get; set; }
    }
}