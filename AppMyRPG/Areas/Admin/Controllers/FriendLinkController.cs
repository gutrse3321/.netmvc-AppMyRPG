using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppMyRPG.Models;
using PagedList;
using System.Net;
using System.Data.Entity;

namespace AppMyRPG.Areas.Admin.Controllers
{
    [Authorize(Users = "1,6")]
    public class FriendLinkController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // GET: Admin/FriendLink
        public ActionResult Article(int? order,int? page)
        {
            var list = db.FriendLink.OrderBy(m => m.FLID);
            //排序
            switch (order)
            {
                case 1:list = list.OrderBy(m => m.FLID);break;
                case 2:list = list.OrderByDescending(m => m.FLID);break;
                default:list = list.OrderBy(m => m.FLID);break;
            }

            //分页
            int pageSize = 7;
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FriendLink fl)
        {
            //前端验证是否有错误
            if (ModelState.IsValid)
            {
                db.FriendLink.Add(fl);
                db.SaveChanges();
            }
            else
            {
                return View(fl);
            }

            return RedirectToAction("Article");
        }

        public ActionResult Edit(int? flid)
        {
            //验证获取的flid是否为空，空的返回错误页面
            if (flid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //验证是否查找到flid
            var friendlink = db.FriendLink.First(m => m.FLID == flid);
            if (friendlink == null)
            {
                return HttpNotFound();
            }

            return View(friendlink);
        }

        [HttpPost]
        public ActionResult Edit(FriendLink fl)
        {
            //前端验证是否有错误
            if (ModelState.IsValid)
            {
                db.Entry(fl).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                return View(fl);
            }

            return RedirectToAction("Article");
        }

        public ActionResult Delete(int flid)
        {
            //重复提交验证 返回错误页面
            var friendlink = db.FriendLink.Find(flid);
            if (friendlink == null)
            {
                return HttpNotFound();
            }
            db.FriendLink.Remove(friendlink);
            db.SaveChanges();

            return RedirectToAction("Article");
        }
    }
}