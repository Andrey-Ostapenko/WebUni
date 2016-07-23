// <auto-generated /> 
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.
 
// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC
 
using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace T4MVC
{
    public class OrderMailerController
    {

        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _Layout = "_Layout";
                public readonly string ClientActivationRequest = "ClientActivationRequest";
                public readonly string ClientForgotPassword = "ClientForgotPassword";
                public readonly string ClientForgotPasswordRuEn = "ClientForgotPasswordRuEn";
                public readonly string ClientFourDigitActivationRequest = "ClientFourDigitActivationRequest";
                public readonly string ClientFourDigitActivationRequestRuEn = "ClientFourDigitActivationRequestRuEn";
                public readonly string ClientOrderCreated = "ClientOrderCreated";
                public readonly string ClientOrderCreatedRuEn = "ClientOrderCreatedRuEn";
                public readonly string ClientOrderIncomeRecived = "ClientOrderIncomeRecived";
                public readonly string OrderCreated = "OrderCreated";
                public readonly string OrderIncomeRecieved = "OrderIncomeRecieved";
            }
            public readonly string _Layout = "~/Views/OrderMailer/_Layout.cshtml";
            public readonly string ClientActivationRequest = "~/Views/OrderMailer/ClientActivationRequest.cshtml";
            public readonly string ClientForgotPassword = "~/Views/OrderMailer/ClientForgotPassword.cshtml";
            public readonly string ClientForgotPasswordRuEn = "~/Views/OrderMailer/ClientForgotPasswordRuEn.cshtml";
            public readonly string ClientFourDigitActivationRequest = "~/Views/OrderMailer/ClientFourDigitActivationRequest.cshtml";
            public readonly string ClientFourDigitActivationRequestRuEn = "~/Views/OrderMailer/ClientFourDigitActivationRequestRuEn.cshtml";
            public readonly string ClientOrderCreated = "~/Views/OrderMailer/ClientOrderCreated.cshtml";
            public readonly string ClientOrderCreatedRuEn = "~/Views/OrderMailer/ClientOrderCreatedRuEn.cshtml";
            public readonly string ClientOrderIncomeRecived = "~/Views/OrderMailer/ClientOrderIncomeRecived.cshtml";
            public readonly string OrderCreated = "~/Views/OrderMailer/OrderCreated.cshtml";
            public readonly string OrderIncomeRecieved = "~/Views/OrderMailer/OrderIncomeRecieved.cshtml";
        }
    }

}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
