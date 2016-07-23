using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using livemenu.Common.Kernel.Settings;
using livemenu.Kernel.Executing;

namespace livemenu.Models.Login
{
    public class LoginViewModel
    {
        public string ReturnUrl { get; set; }
        [Required]
        //[DataType(DataType.EmailAddress)]
        public string Login { get; set; }

        public LMExecutingType LmExecutingType { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public string BaseApplicationName { get; set; }

        public ILoginPageSettings LoginPageSettings { get; set; }
    }
}