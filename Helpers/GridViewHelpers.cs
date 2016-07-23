using System.Collections.Generic;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace livemenu.Helpers
{
    public class ExportType
    {
        public string Title { get; set; }
        public ExportMethod Method { get; set; }
    }
    public delegate ActionResult ExportMethod(GridViewSettings settings, object dataObject, string fileName);
    public enum GridViewExportType { None, Pdf, Xls, Xlsx, Rtf, Csv }

    /// <summary>
    /// v1
    /// </summary>
    public static class GridViewHelpers
    {
        static Dictionary<GridViewExportType, ExportType> exportTypes;
        public static Dictionary<GridViewExportType, ExportType> ExportTypes
        {
            get
            {
                if (exportTypes == null)
                    exportTypes = CreateExportTypes();
                return exportTypes;
            }
        }
        static Dictionary<GridViewExportType, ExportType> CreateExportTypes()
        {
            var types = new Dictionary<GridViewExportType, ExportType>();
            types.Add(GridViewExportType.Pdf, new ExportType { Title = "Выгрузить в PDF", Method = GridViewExtension.ExportToPdf });
            types.Add(GridViewExportType.Xls, new ExportType { Title = "Выгрузить в Excel (2003)", Method = GridViewExtension.ExportToXls });
            types.Add(GridViewExportType.Xlsx, new ExportType { Title = "Выгрузить в Excel", Method = GridViewExtension.ExportToXlsx });
            types.Add(GridViewExportType.Rtf, new ExportType { Title = "Выгрузить в RTF", Method = GridViewExtension.ExportToRtf });
            types.Add(GridViewExportType.Csv, new ExportType { Title = "Выгрузить в CSV", Method = GridViewExtension.ExportToCsv });
            return types;
        }

        public static string GridViewCreateButton(this HtmlHelper htmlHelper, string gvName)
        {
            return htmlHelper.CreateButton(string.Format("aspxGVScheduleCommand('{0}',['AddNew'],1)", gvName));
        }

        public static string GridViewStartEditButton(this HtmlHelper htmlHelper, string gvName, int index)
        {
            return htmlHelper.StartEditButton(string.Format("aspxGVScheduleCommand('{0}',['StartEdit',{1}],1)", gvName, index));
        }

        public static string GridViewCancelEditButton(this HtmlHelper htmlHelper, string gvName)
        {
            return htmlHelper.CancelEditButton(string.Format("aspxGVScheduleCommand('{0}',['CancelEdit'],1)", gvName));
        }

        public static string GridViewDeleteButton(this HtmlHelper htmlHelper, string gvName, int index)
        {
            return htmlHelper.DeleteButton(string.Format("aspxGVScheduleCommand('{0}',['Delete',{1}],1)", gvName, index));
        }
        public static string GridViewSaveButton(this HtmlHelper htmlHelper, string gvName)
        {
            return htmlHelper.SaveButton(string.Format("aspxGVScheduleCommand('{0}',['UpdateEdit'],1)", gvName));
        }
    }

    public static class TreeListHelpers
    {
        public static string TreeListCreateButton(this HtmlHelper htmlHelper, string gvName, string key)
        {
            return htmlHelper.CreateButton(string.Format("aspxTLStartEditNewNode('{0}','{1}')", gvName, key));
        }
        public static string TreeListStartEditButton(this HtmlHelper htmlHelper, string gvName, string key)
        {
            return htmlHelper.StartEditButton(string.Format("aspxTLStartEdit('{0}','{1}')", gvName, key));
        }

        public static string TreeListCancelEditButton(this HtmlHelper htmlHelper, string gvName)
        {
            return htmlHelper.CancelEditButton(string.Format("aspxTLCancelEdit('{0}')", gvName));
        }

        public static string TreeListDeleteButton(this HtmlHelper htmlHelper, string gvName, string key)
        {
            return htmlHelper.DeleteButton(string.Format("aspxTLDeleteNode('{0}','{1}')", gvName, key));
        }
        public static string TreeListSaveButton(this HtmlHelper htmlHelper, string gvName)
        {
            return htmlHelper.SaveButton(string.Format("aspxTLUpdateEdit('{0}')", gvName));
        }
    }


    public static class EditButtonHelpers
    {
        public static string CreateButton(string onclick, string addtitionalTags = "")
        {
            return string.Format(@"<a class='btn  btn-xs btn-info' onclick=""{0}"" " + addtitionalTags + "  > <i class='fa fa-plus'></i></a>", onclick);
        }


        public static string CreateButton(this HtmlHelper htmlHelper, string onclick, string addtitionalTags = "")
        {
            return string.Format(@"<a class='btn  btn-xs btn-info' onclick=""{0}"" " + addtitionalTags + "  > <i class='fa fa-plus'></i></a>", onclick);
        }

        public static string StartEditButton(this HtmlHelper htmlHelper, string onclick)
        {
            return string.Format(@"<a class='btn  btn-xs btn-complete' onclick=""{0}""><i class='fa fa-pencil'></i></a>", onclick);
        }

        public static string CancelEditButton(this HtmlHelper htmlHelper, string onclick)
        {
            return string.Format(@"<a class='btn  btn-xs btn-warning' onclick=""{0}""  >  <i class='fa fa-times'></i></a>", onclick);
        }

        public static string DeleteButton(this HtmlHelper htmlHelper, string onclick)
        {
            return string.Format(@"<a class='btn  btn-xs btn-danger' onclick=""{0}""  >  <i class='fa fa-trash'></i></a>", onclick);
        }

        public static string SaveButton(this HtmlHelper htmlHelper, string onclick)
        {
            return string.Format(@"<a class='btn btn-xs btn-success ' onclick=""{0}""><i class='fa fa-check'></i></a>", onclick);
        }
    }
}