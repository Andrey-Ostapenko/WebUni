using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElFinder;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Executing;
using livemenu.Models;
using livemenu.Models.FileManager;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Controllers
{
    public partial class FileManagerController : BaseController
    {
        public string RootPath = "~/BWP/Resources/Users/Root";
        public string UsersPath = "~/BWP/Resources/Users";
        public string ThumbPath = "~/BWP/Resources/Users/Thumb";


        private Connector _connector;

        public Connector Connector
        {
            get
            {
                if (_connector == null)
                {
                    FileSystemDriver driver = new FileSystemDriver();
                    DirectoryInfo thumbsStorage = new DirectoryInfo(Server.MapPath(ThumbPath));
                    //driver.AddRoot(new Root(new DirectoryInfo(@"C:\Program Files"))
                    //{
                    //    IsLocked = true,
                    //    IsReadOnly = true,
                    //    IsShowOnly = true,
                    //    ThumbnailsStorage = thumbsStorage,
                    //    ThumbnailsUrl = "Thumbnails/"
                    //});

                    var folder = string.Empty;
                    //if (System.Web.HttpContext.Current != null && !System.Web.HttpContext.Current.IsDebuggingEnabled)
                    //{
                    folder = "/admin";
                    //}

                    driver.AddRoot(new Root(new DirectoryInfo(Server.MapPath(RootPath)), "/BWP/Resources/Users/Root/")
                    {
                        Alias = "Root",
                        StartPath = new DirectoryInfo(Server.MapPath(RootPath)),
                        ThumbnailsStorage = thumbsStorage,
                        MaxUploadSizeInMb = 30,
                        ThumbnailsUrl = folder + "/FileManager/Thumbs/"
                    });
                    _connector = new Connector(driver);
                }
                return _connector;
            }
        }

        public virtual ActionResult Index(LMExecutingType et = LMExecutingType.LM) 
        {
            var context = ServiceLocator.Current.GetInstance<ILMExecutingContext>();
            context.SetExecutingType(et, true);

            return View(MVC.FileManager.Views.ResponsiveLayoutIndex, new ResponsiveLayoutModel
            {
                CustomID = "cms-fm",
                Name = "Менеджер файлов",
                InstructionLink = "http://www.WebUni.ru/platform-cms-filemanager"
            });
        }

        public virtual ActionResult Internal()
        {
            return View(MVC.FileManager.Views.Index, new FileManagerModel()
            {
                IsModal = false
            });
        }

        public virtual ActionResult Modal(bool onlyDir = false)
        {
            return View(MVC.FileManager.Views.Index, new FileManagerModel()
            {
                OnlyDirectory = onlyDir,
                IsModal = true
            });
        }

        public virtual ActionResult Editor()
        {
            return View(MVC.FileManager.Views.Index, new FileManagerModel()
            {
                IsExternal = true
            });
        }

        public virtual ActionResult Connect()
        {
            return Connector.Process(this.HttpContext.Request);
        }

        public virtual ActionResult SelectFile(string target)
        {
            return Json(Connector.GetFileByHash(target).FullName);
        }

        public virtual ActionResult Thumbs(string tmb)
        {
            return Connector.GetThumbnail(Request, Response, tmb);
        }
    }
}