using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AppMyRPG.Models;
using PagedList;

namespace AppMyRPG.Areas.Admin.Controllers
{
    [Authorize(Users = "1,6")]
    public class CommentController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // GET: Admin/Comment
        public ActionResult Article(string order,string currentFilter,string selectString,int? page)
        {
            //保持排序顺序在分页时相同
            ViewBag.CurrentSort = order;
            //确保正确分页
            if (selectString != null)
            {
                page = 1;
            }
            else
            {
                selectString = currentFilter;
            }
            //设置搜索信息
            ViewBag.CurrentFilter = selectString;
            var list = db.Comment.OrderByDescending(m => m.CommentID);
            if (!string.IsNullOrEmpty(selectString))
            {
                list = list.Where(m => m.CommentText.Contains(selectString)
                || m.Games.GameName.Contains(selectString)
                || m.UserInfo.UserUID.Contains(selectString)
                || m.UserInfo.UserNickname.Contains(selectString)
                ).OrderByDescending(m => m.CommentID);
            }
            switch (order)
            {
                case "time":list = list.OrderBy(m => m.CommentTime);break;
                case "timedesc":list = list.OrderByDescending(m => m.CommentTime);break;
                default:list = list.OrderByDescending(m => m.CommentID);break;
            }
            //分页
            int pageSize = 7;
            //若page为空 pageNumber=1
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Delete(int cmtid)
        {
            //重复提交 返回错误页面
            var comment = db.Comment.Find(cmtid);
            if (comment == null)
            {
                return HttpNotFound();
            }
            //游戏表游戏评论减一
            var game = db.Games.First(m => m.GameID == comment.GameID);
            game.GameCmtCount = game.GameCmtCount - 1;

            db.Comment.Remove(comment);
            db.SaveChanges();

            return RedirectToAction("Article");
        }
    }
}