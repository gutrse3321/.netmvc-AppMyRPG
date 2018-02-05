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
    [Authorize(Users = "1")]
    public class UserTypeController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        public ActionResult Article(int? order,int? page)
        {
            //保持排序顺序在分页时相同
            var list = db.UserGroup.OrderBy(m=>m.GroupID);
            switch (order)
            {
                case 1:list = list.OrderBy(m => m.GroupID);break;
                case 2:list = list.OrderByDescending(m => m.GroupID);break;
                default:list = list.OrderBy(m => m.GroupID);break;
            }

            //分页
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(UserGroup ug)
        {
            //查询是否有相同名称存在
            var gname = db.UserGroup.Where(m => m.GroupName == ug.GroupName);
            if (gname.Count() > 0)
            {
                ModelState.AddModelError("GroupName", "权限名称已存在");
                return View(ug);
            }
            //写入数据库
            db.UserGroup.Add(ug);
            db.SaveChanges();

            return RedirectToAction("Article");
        }

        public ActionResult Edit(int? groupid)
        {
            //验证获取的id，空返回错误页面
            if (groupid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //验证对象是否存在
            var usergroup = db.UserGroup.First(m => m.GroupID == groupid);
            if (usergroup == null)
            {
                return HttpNotFound();
            }

            return View(usergroup);
        }

        [HttpPost]
        public ActionResult Edit(UserGroup ug)
        {
            //查询是否有相同名称存在
            var gname = db.UserGroup.Where(m => m.GroupName == ug.GroupName);
            if (gname.Count() > 0)
            {
                ModelState.AddModelError("GroupName", "权限名称未修改或已存在");
                return View(ug);
            }
            //验证通过执行修改
            //获取实体信息及操作
            db.Entry(ug).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Article");
        }

        public ActionResult Delete(int groupid)
        {
            //重复提交 返回错误页面
            var usergroup = db.UserGroup.Find(groupid);
            if (usergroup == null)
            {
                return HttpNotFound();
            }
            db.UserGroup.Remove(usergroup);
            db.SaveChanges();

            return RedirectToAction("Article");
        }
    }
}