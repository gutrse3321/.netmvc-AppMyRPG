using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppMyRPG.Models;
using System.Data.Entity;

namespace AppMyRPG.Areas.Admin.Controllers
{
    [Authorize(Users = "1,6")]
    public class StartController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // GET: Admin/Start
        public ActionResult Index()
        {
            ViewBag.NoticeCount = db.Notice.Count();
            ViewBag.GamesCount = db.Games.Count();
            ViewBag.UsersCount = db.UserInfo.Count() ;

            return View();
        }

        public PartialViewResult Notice()
        {
            var list = db.Notice.OrderByDescending(m=>m.NoticeTime).Take(4).ToList();

            return PartialView(list);
        }

        public PartialViewResult Game()
        {
            var list = db.Games.Include(m=>m.GameType).OrderByDescending(m => m.GameTime).Take(3).ToList();

            return PartialView(list);
        }

        public PartialViewResult UserInfo()
        {
            var list = db.UserInfo.OrderByDescending(m=>m.UserID).ToList();

            return PartialView(list);
        }
    }
}