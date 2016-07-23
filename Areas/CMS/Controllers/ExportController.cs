using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using livemenu.Excel.CRM;
using livemenu.Kernel.CMS.Export;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Areas.CMS.Controllers
{
    public class ExportModel
    {
        public string test { get; set; }
    }
    public partial class ExportController : CMSController
    {
        private readonly ICatalogItemExporter _catalogItemExporter;

        public ExportController(ICatalogItemExporter catalogItemExporter)
        {
            _catalogItemExporter = catalogItemExporter;
        }

        //public virtual ActionResult Index(string code)
        //{
        //    var downloadFileName = string.Format("test.WebUni", code, DateTime.Now.ToString("dd-MM-yyyy-HH_mm_ss"));
        //    Response.ContentType = "application/zip";
        //    Response.AddHeader("Content-Disposition", "filename=" + downloadFileName);

        //    _catalogItemExporter.ExportToStream(Response.OutputStream, code);
        //    return new EmptyResult();
        //}

        //public virtual ActionResult Import(string code, string file)
        //{
        //    OperationWrapper(() =>
        //    {
        //        _notificationBus.Notificate(new NotificationMessage()
        //        {
        //            Text = "Импорт запущен"
        //        });

        //        var path = Server.MapPath("/Admin/" + file);
        //      //  var prefix = Request.Url.IsLoopback ? "" : "~";
        //        //var path =  Server.MapPath(prefix + file);

        //        _catalogItemExporter.ImportFromFile(path, code);
        //        return true;
        //    }, "Импорт успешно завершен");
        //    return new EmptyResult();
        //}
    }
}