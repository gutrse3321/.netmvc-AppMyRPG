using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppMyRPG.Models;

namespace AppMyRPG.Controllers
{
    public class GameController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // GET: Game
        public ActionResult Detail(int? id)
        {
            //请求的id为空时,返回错误页面
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //查询是否存在 返回错误页面
            var game = db.Games.First(m => m.GameID == id);
            if (game == null)
            {
                return HttpNotFound();
            }
            game.GameHot = game.GameHot + 1;
            db.SaveChanges();
            
            return View(game);
        }

        public ActionResult EroDetail(int? id)
        {
            //验证用户是否邮箱验证
            if (Session["UID"] != null)
            {
                string sessionUid = Session["UID"].ToString();
                var uid = db.UserInfo.First(m => m.UserUID == sessionUid);
                if (uid.UserRegState == 0)
                {
                    return RedirectToAction("Notice","Home");
                }
            }
            else
            {
                return RedirectToAction("Notice", "Home");
            }
            //请求的id为空时,跳转公告页面
            if (id == null)
            {
                return RedirectToAction("Notice", "Home");
            }
            //查询是否存在 返回错误页面
            var game = db.Games.First(m => m.GameID == id);
            if (game == null)
            {
                return HttpNotFound();
            }
            game.GameHot = game.GameHot + 1;
            db.SaveChanges();

            return View(game);
        }
    }
}