using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppMyRPG.Models;

namespace AppMyRPG.Controllers
{
    public class FLinkController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // GET: FLink
        public ActionResult List()
        {
            var list = db.FriendLink.OrderByDescending(m => m.FLID);

            return View(list.ToList());
        }
    }
}