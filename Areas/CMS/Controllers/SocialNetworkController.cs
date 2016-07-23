using livemenu.Kernel.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class SocialNetworkController : CMSController
    {
        private readonly ISocialNetworkService _socialNetworkService;

        public SocialNetworkController(ISocialNetworkService socialNetworkService)
        {
            _socialNetworkService = socialNetworkService;
        }

        [HttpGet]
        public virtual JsonResult SearchSocialNetwork(string id)
        {
            var socialNetworks = new List<SelectItem>();

            var lid = id.ToLower();
            var socials = _socialNetworkService.RawDataQueryable.Where(x => x.Name.ToLower().Contains(lid));

            foreach (var sn in socials)
            {
                socialNetworks.Add(new SelectItem
                {
                    id = (int)sn.ID,
                    text = sn.Name
                });
            }

            return Json(socialNetworks, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetSocialNetwork(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var socialNetworks = _socialNetworkService.GetAll();

            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    var sn = socialNetworks.FirstOrDefault(m => m.ID == idInt);
                    if (sn == null)
                        continue;
                    items.Add(new SelectItem
                    {
                        id = (int)sn.ID,
                        text = sn.Name
                    });
                }
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}