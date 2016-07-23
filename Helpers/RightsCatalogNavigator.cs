using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.ASPxTreeList;
using livemenu.Kernel.Catalog;
using livemenu.Kernel.Rights;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Helpers
{
    public class RightsCatalogNavigator
    {
        private readonly long _rsID;
        private readonly bool _isRoot;
        private readonly IRightsManager _rightsManager;

        public RightsCatalogNavigator(long rsID, bool isRoot)
        {
            _rightsManager = (IRightsManager)ServiceLocator.Current.GetService(typeof(IRightsManager));
            _rsID = rsID;
            _isRoot = isRoot;
        }

        public void VirtualModeCreateChildren(TreeListVirtualModeCreateChildrenEventArgs e)
        {
            long id = e.NodeObject == null ? (long)HttpContext.Current.Session["NewCatalogInitItemID"] : ((NewCatalogItemRightsModel)e.NodeObject).ID;

            var children = new List<NewCatalogItemRightsModel>();

            if (id < 0)
            {
                children.Add(_rightsManager.GetRootCatalogitem(Math.Abs(id), _rsID));
                e.Children = children;
            }
            else
            {
                children = GetCatalogItemsChildren(id, _rsID).ToList();
                e.Children = children.ToList();
            }

        }

        public IEnumerable<NewCatalogItemRightsModel> GetCatalogItemsChildren(long parentID, long rsID)
        {

            return _rightsManager.GetCatalogItemsChildrenRights(parentID, rsID);
        }

        static Dictionary<string, long> Map
        {
            get
            {
                const string key = "DX_RightsCatalogNavigator_MAP";
                if (HttpContext.Current.Session[key] == null)
                    HttpContext.Current.Session[key] = new Dictionary<string, long>();
                return HttpContext.Current.Session[key] as Dictionary<string, long>;
            }
        }

        static long GetNodeID(string guid, long id)
        {
            if (!Map.ContainsKey(guid))
                Map[guid] = id;
            return Map[guid];
        }



        public static void VirtualModeNodeCreating(TreeListVirtualModeNodeCreatingEventArgs e)
        {
            var node = ((NewCatalogItemRightsModel)e.NodeObject);
            e.NodeKeyValue = node.ID;
            e.IsLeaf = !node.HasChildren;

            GetNodeID(node.ID.ToString(), node.ID);
            e.SetNodeValue("Name", node.Name);
            e.SetNodeValue("TypeName", node.TypeName);
            e.SetNodeValue("IsActive", node.IsActive);
            e.SetNodeValue("ViewAllowed", node.ViewAllowed);
            e.SetNodeValue("EditAllowed", node.EditAllowed);
            e.SetNodeValue("ViewDenied", node.ViewDenied);
            e.SetNodeValue("EditDenied", node.EditDenied);
            e.SetNodeValue("IsViewSync", node.IsViewSync);
            e.SetNodeValue("IsEditSync", node.IsEditSync);
            e.SetNodeValue("HasMicroModules", node.HasMicroModules);
        }
    }
}