using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AppMyRPG.Models;
using PagedList;
using System.Net;
using System.Security;

namespace AppMyRPG.Areas.Admin.Controllers
{
    [Authorize(Users = "1")]
    public class GameTypeController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();

        public ActionResult Article()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Article(int? order,int? page)
        {
            //保持排序顺序在分页时相同
            var list = db.GameType.OrderBy(m => m.TypeID);
            switch (order)
            {
                case 1:list = list.OrderByDescending(m => m.TypeID);
                    break;
                case 2:list = list.OrderByDescending(m => m.OIID == order);
                    break;
                case 3:list = list.OrderByDescending(m => m.OIID == order);
                    break;
                default:list = list.OrderBy(m => m.TypeID);
                    break;
            }

            //分页
            int pageSize = 5;
            //若page为null 则pageNumber=1
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Add()
        {
            //绑定下拉框数据
            ViewBag.OIID = new SelectList(db.OutIn, "OIID", "OIName");
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Add(GameType gt)
        {
            //前台验证通过执行添加
            gt.TypeClass = 0;
            db.GameType.Add(gt);
            db.SaveChanges();
            
            return RedirectToAction("Article");
        }

        public ActionResult Edit(int? typeid)
        {
            //验证获取的typeid是否为空，空的返回404错误页面
            if (typeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //验证是否查找到typeid
            var gametype = db.GameType.First(m => m.TypeID == typeid);
            if (gametype == null)
            {
                return HttpNotFound();
            }
            //根据typeid绑定下拉框数据
            ViewBag.OIID = new SelectList(db.OutIn, "OIID", "OIName", gametype.OIID);

            return View(gametype);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(GameType gt)
        {
            //前台验证通过执行修改
            //Entry获取信息及操作
            db.Entry(gt).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Article");
        }

        public ActionResult Delete(int typeid)
        {
            //重复提交验证 返回404错误页面
            var gametype = db.GameType.Find(typeid);
            if (gametype == null)
            {
                return HttpNotFound();
            }
            db.GameType.Remove(gametype);
            db.SaveChanges();

            return RedirectToAction("Article");
        }
    }
}