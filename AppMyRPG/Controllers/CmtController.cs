using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppMyRPG.Models;
using PagedList;

namespace AppMyRPG.Controllers
{
    public class CmtController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // GET: Cmt
        public ActionResult List()
        {
            var list = db.Comment.Take(7).OrderByDescending(m => m.CommentTime);

            return View(list.ToList());
        }

        public ActionResult Comment(int gameid)
        {
            //根据gameid查询游戏的评论
            var list = db.Comment.Where(m => m.GameID == gameid).OrderByDescending(m => m.CommentTime);

            return View(list.ToList());
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Comment c,int id)
        {
            
            int gameid = id;
            //查询发表的用户
            if (Session["UID"] != null)
            {
                string sessionUid = Session["UID"].ToString();
                var user = db.UserInfo.First(m => m.UserUID == sessionUid);
                c.UserID = user.UserID;
                c.GameID = gameid;
                c.CommentTime = DateTime.Now;
                //游戏表游戏的评论加一
                var game = db.Games.First(m => m.GameID == gameid);
                game.GameCmtCount = game.GameCmtCount + 1;

                db.Comment.Add(c);
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("LoginHolder", "UserInfo");
            }
            
            return RedirectToAction("Detail", "Game", new { id = gameid});
        }

        public ActionResult UserCmt(int userid)
        {
            //查询所有此用户id的评论
            var user = db.Comment.Where(m => m.UserID == userid).Take(10).OrderByDescending(m => m.CommentTime);

            return View(user.ToList());
        }

        public ActionResult Delete(int cmtid)
        {
            //验证重复提交
            var cmt = db.Comment.Find(cmtid);
            if (cmt == null)
            {
                return HttpNotFound();
            }
            db.Comment.Remove(cmt);
            db.SaveChanges();

            return RedirectToAction("Detail","UserInfo",new { UserID = cmt.UserID});
        }
    }
}