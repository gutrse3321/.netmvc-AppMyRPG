using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AppMyRPG.Models;
using PagedList;

namespace AppMyRPG.Controllers
{
    public class HomeController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // 最新更新
        public ActionResult Index(string order,string selectString,string currentFilter,int? page)
        {
            //给分类下拉框赋值
            var typeName = db.GameType.Where(m => m.OIID == 2).ToList();
            List<string> name = new List<string>();
            foreach (var item in typeName)
            {
                name.Add(item.TypeName);
            }
            ViewBag.OrderNum = name;

            //保持分类在分页时相同
            ViewBag.CurrentSort = order;
            //确保正确分页
            if (selectString != null)
            {
                page = 1;
            }
            else
            {
                currentFilter = selectString;
            }
            //设置搜索信息
            ViewBag.CurrentFilter = selectString;
            var list = db.Games.Where(m=>m.GameType.OIID==2).OrderByDescending(m => m.GameTime);
            if (!string.IsNullOrEmpty(selectString))
            {
                list = list.Where(m => m.GameName.Contains(selectString)
                || m.GameType.TypeName.Contains(selectString))
                .OrderByDescending(m=>m.GameTime);
            }
            //分类
            if (!string.IsNullOrEmpty(order))
            {
                list = list.Where(m => m.GameType.TypeName.Contains(order)).OrderByDescending(m => m.GameTime);
            }
            //分页
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber,pageSize));
        }

        //热度排行
        public ActionResult Hot(string order,string selectString, string currentFilter, int? page)
        {
            //给分类下拉框赋值
            var typeName = db.GameType.Where(m => m.OIID == 2).ToList();
            List<string> name = new List<string>();
            foreach (var item in typeName)
            {
                name.Add(item.TypeName);
            }
            ViewBag.OrderNum = name;

            //保持分类在分页时相同
            ViewBag.CurrentSort = order;

            //确保正确分页
            if (selectString != null)
            {
                page = 1;
            }
            else
            {
                currentFilter = selectString;
            }
            //设置搜索信息
            ViewBag.CurrentFilter = selectString;
            var list = db.Games.Where(m=>m.GameType.OIID==2).OrderByDescending(m => m.GameHot);
            if (!string.IsNullOrEmpty(selectString))
            {
                list = list.Where(m => m.GameName.Contains(selectString)
                || m.GameType.TypeName.Contains(selectString))
                .OrderByDescending(m => m.GameHot);
            }
            //分类
            if (!string.IsNullOrEmpty(order))
            {
                list = list.Where(m => m.GameType.TypeName.Contains(order)).OrderByDescending(m => m.GameHot);
            }
            //分页
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        //里站
        public ActionResult Ero(string typeorder,string order, string selectString, string currentFilter,string orderFilter, int? page)
        {
            //验证用户是否邮箱验证
            if (Session["UID"] != null)
            {
                string sessionUid = Session["UID"].ToString();
                var uid = db.UserInfo.First(m => m.UserUID == sessionUid);
                if (uid.UserRegState == 0)
                {
                    return RedirectToAction("Notice", "Home");
                }
            }
            else
            {
                return RedirectToAction("Notice", "Home");
            }
            //给分类下拉框赋值
            var typeName = db.GameType.Where(m => m.OIID == 2).ToList();
            List<string> name = new List<string>();
            foreach (var item in typeName)
            {
                name.Add(item.TypeName);
            }
            ViewBag.OrderNum = name;

            //确保顺序在分页时相同
            ViewBag.CurrentSort = order;
            //确认正确分页
            if (selectString != null)
            {
                page = 1;
            }
            else
            {
                selectString = currentFilter;
            }
            if (typeorder != null)
            {
                page = 1;
            }
            else
            {
                typeorder = orderFilter;
            }
            //设置动态信息
            ViewBag.CurrentFilter = selectString;
            ViewBag.OrderFilter = typeorder;
            var list = db.Games.Where(m => m.GameType.OIID == 3).OrderByDescending(m => m.GameTime);
            if (!string.IsNullOrEmpty(selectString))
            {
                list = list.Where(m => m.GameName.Contains(selectString)
                || m.GameType.TypeName.Contains(selectString))
                .OrderByDescending(m => m.GameTime);
            }
            //分类
            if (!string.IsNullOrEmpty(typeorder))
            {
                list = list.Where(m => m.GameType.TypeName.Contains(order)).OrderByDescending(m => m.GameTime);
            }
            //排序
            switch (order)
            {
                case "time":list = list.OrderBy(m => m.GameTime);break;
                case "timedesc":list = list.OrderByDescending(m => m.GameTime);break;
                case "hot":list = list.OrderByDescending(m => m.GameHot);break;
                default: list = list.OrderByDescending(m => m.GameTime); break;
            }
            //分页
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Notice(int? page)
        {
            var list = db.Notice.OrderByDescending(m => m.NoticeTime);

            //分页
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber,pageSize));
        }
    }
}