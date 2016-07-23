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
namespace livemenu.Controllers
{
    public partial class FileManagerController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public FileManagerController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected FileManagerController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SelectFile()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SelectFile);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Thumbs()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Thumbs);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public FileManagerController Actions { get { return MVC.FileManager; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "FileManager";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "FileManager";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Internal = "Internal";
            public readonly string Modal = "Modal";
            public readonly string Editor = "Editor";
            public readonly string Connect = "Connect";
            public readonly string SelectFile = "SelectFile";
            public readonly string Thumbs = "Thumbs";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Internal = "Internal";
            public const string Modal = "Modal";
            public const string Editor = "Editor";
            public const string Connect = "Connect";
            public const string SelectFile = "SelectFile";
            public const string Thumbs = "Thumbs";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string et = "et";
        }
        static readonly ActionParamsClass_Modal s_params_Modal = new ActionParamsClass_Modal();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Modal ModalParams { get { return s_params_Modal; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Modal
        {
            public readonly string onlyDir = "onlyDir";
        }
        static readonly ActionParamsClass_SelectFile s_params_SelectFile = new ActionParamsClass_SelectFile();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SelectFile SelectFileParams { get { return s_params_SelectFile; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SelectFile
        {
            public readonly string target = "target";
        }
        static readonly ActionParamsClass_Thumbs s_params_Thumbs = new ActionParamsClass_Thumbs();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Thumbs ThumbsParams { get { return s_params_Thumbs; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Thumbs
        {
            public readonly string tmb = "tmb";
        }
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
                public readonly string FileManagerLayout = "FileManagerLayout";
                public readonly string Index = "Index";
                public readonly string ResponsiveLayoutIndex = "ResponsiveLayoutIndex";
            }
            public readonly string FileManagerLayout = "~/Views/FileManager/FileManagerLayout.cshtml";
            public readonly string Index = "~/Views/FileManager/Index.cshtml";
            public readonly string ResponsiveLayoutIndex = "~/Views/FileManager/ResponsiveLayoutIndex.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_FileManagerController : livemenu.Controllers.FileManagerController
    {
        public T4MVC_FileManagerController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, LMExecutingType et);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index(LMExecutingType et)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "et", et);
            IndexOverride(callInfo, et);
            return callInfo;
        }

        [NonAction]
        partial void InternalOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Internal()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Internal);
            InternalOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ModalOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, bool onlyDir);

        [NonAction]
        public override System.Web.Mvc.ActionResult Modal(bool onlyDir)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Modal);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "onlyDir", onlyDir);
            ModalOverride(callInfo, onlyDir);
            return callInfo;
        }

        [NonAction]
        partial void EditorOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Editor()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Editor);
            EditorOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ConnectOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Connect()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Connect);
            ConnectOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SelectFileOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string target);

        [NonAction]
        public override System.Web.Mvc.ActionResult SelectFile(string target)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SelectFile);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "target", target);
            SelectFileOverride(callInfo, target);
            return callInfo;
        }

        [NonAction]
        partial void ThumbsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string tmb);

        [NonAction]
        public override System.Web.Mvc.ActionResult Thumbs(string tmb)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Thumbs);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tmb", tmb);
            ThumbsOverride(callInfo, tmb);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
