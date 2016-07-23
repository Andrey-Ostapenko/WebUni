using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace livemenu.Models.Login
{
    public class JsonLoginResult
    {
        public string RedirectUrl { get; set; }
        public bool Result { get; set; }
        public string Error { get; set; }
    }
}