using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace livemenu.ModelBinders
{
    public class DictionaryModelBinder : IModelBinder
    {
        public static readonly string BindingExceptionKey = "exception";
        public static readonly string JsonExceptionDataKey = "json";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string json = string.Empty;
            try
            {
                if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                controllerContext.HttpContext.Request.InputStream.Position = 0;
                using (var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream))
                {
                    json = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(json))
                    {
                        return null;
                    }

                    return new JavaScriptSerializer().DeserializeObject(json);
                }
            }
            catch (Exception e)
            {
                e.Data.Add(JsonExceptionDataKey, json);

                var d = new Dictionary<string, object> { { BindingExceptionKey, e } };
                return d;
            }

        }
    }
}