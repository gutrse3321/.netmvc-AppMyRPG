using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AppMyRPG.Models;
using System.IO;
using System.Net;
using PagedList;

namespace AppMyRPG.Areas.Admin.Controllers
{
    [Authorize(Users ="1")]
    public class GameController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();

        // GET: Admin/Game
        public ActionResult Article()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Article(string order, string currentFilter,string selectString,int? page)
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
            var list = db.Games.Include(m => m.GameType).OrderByDescending(m => m.GameID);

            if (!string.IsNullOrEmpty(selectString))
            {
                list = list.Where(m => m.GameName.Contains(selectString)).OrderByDescending(m => m.GameTime);
            }
            switch (order)
            {
                case "time":
                    list = list.OrderByDescending(m => m.GameTime);
                    break;
                case "cmt":
                    list = list.OrderByDescending(m => m.GameCmtCount);
                    break;
                case "hot":
                    list = list.OrderByDescending(m => m.GameHot);
                    break;
                default:
                    list = list.OrderByDescending(m => m.GameTime);
                    break;
            }
            //分页
            int pageSize = 7;
            //如果page是 null，则返回 1
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber,pageSize));
        }

        //添加新游戏
        public ActionResult Add()
        {
            //绑定分类下拉框
            var list = db.GameType.ToList();
            SelectList selectList = new SelectList(list, "TypeID", "TypeName");
            ViewBag.selList = selectList.AsEnumerable();

            return View();
        }

        [HttpPost]
        public ActionResult Add(Games ga, HttpPostedFileBase GameFace, IEnumerable<HttpPostedFileBase> GameImg)
        {
            //上传封面
            string facename = string.Empty;
            if (GameFace != null)
            {
                //默认强转.jpg格式
                facename = string.Format("gameface_{0}.jpg", Guid.NewGuid().ToString("N"));
                var facepath = Path.Combine(Server.MapPath("~/Images/face/Game"), facename);
                GameFace.SaveAs(facepath);
            }
            //声明存入数据库字段
            string face = string.Format("/Images/face/Game/{0}", facename);
            //上传截图
            int i = 0;
            string shot_1 = string.Format("screenshot{0}_1.jpg", Guid.NewGuid().ToString("N"));
            string shot_2 = string.Format("screenshot{0}_2.jpg", Guid.NewGuid().ToString("N"));
            string shot_3 = string.Format("screenshot{0}_3.jpg", Guid.NewGuid().ToString("N"));
            string[] img = new string[3];
            string[] shotArg = { shot_1, shot_2, shot_3 };
            foreach (var file in GameImg)
            {
                if (file != null)
                {
                    var shotName = shotArg[i];
                    img[i] = string.Format("/Images/Games/{0}", shotName);
                    var path = Path.Combine(Server.MapPath("~/Images/Games"), shotName);
                    file.SaveAs(path);
                    i++;
                }
            }

            //固定进行的输入
            ga.TypeID = Convert.ToInt32(Request.Form["selitem"]);
            ga.LinkID = Guid.NewGuid().ToString("D");
            ga.Link.LinkID = ga.LinkID;
            ga.GameTime = DateTime.Now;
            //用户并未设置，默认1
            ga.UserID = 1;
            //存入图片地址到数据
            ga.GameFace = face;
            ga.GameImg1 = img[0];
            ga.GameImg2 = img[1];
            ga.GameImg3 = img[2];
            db.Games.Add(ga);
            //出现一项或对个实体错误，关闭ValidateOnSaveEnabled 挖深错误信息
            //db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return RedirectToAction("Article");
        }

        //修改游戏信息
        public ActionResult Edit(int? gameid)
        {
            if (gameid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var e = db.Games.First(m => m.GameID == gameid);
            if (e == null)
            {
                return HttpNotFound();
            }
            var list = db.GameType.ToList();
            SelectList selectList = new SelectList(list, "TypeID", "TypeName");
            ViewBag.selList = selectList.AsEnumerable();

            return View(e);
        }

        [HttpPost]
        public ActionResult Edit(Games ga, HttpPostedFileBase GameFace, IEnumerable<HttpPostedFileBase> GameImg)
        {
            Games games = db.Games.First(m => m.GameID == ga.GameID);
            //上传封面
            string facename = string.Empty;
            if (GameFace != null)
            {
                facename = games.GameFace.Replace("/Images/face/Game/", "");
                var facepath = Path.Combine(Server.MapPath("~/Images/face/Game"), facename);
                GameFace.SaveAs(facepath);
            }
            string face = string.Format("/Images/face/Game/{0}", facename);
            //上传截图
            int i = 0;
            string shot_1 = games.GameImg1.Replace("/Images/Games/", "");
            string shot_2 = games.GameImg2.Replace("/Images/Games/", "");
            string shot_3 = games.GameImg3.Replace("/Images/Games/", "");
            string[] img = new string[3];
            string[] shotArg = { shot_1, shot_2, shot_3 };
            foreach (var file in GameImg)
            {
                if (file != null)
                {
                    var shotName = shotArg[i];
                    img[i] = string.Format("/Images/Games/{0}", shotName);
                    var path = Path.Combine(Server.MapPath("~/Images/Games"), shotName);
                    file.SaveAs(path);
                    i++;
                }
            }
            games.GameName = ga.GameName;
            games.Link.LinkURL = ga.Link.LinkURL;
            games.Link.LinkPWD = ga.Link.LinkPWD;
            games.GameDec = ga.GameDec;
            games.TypeID = ga.TypeID;
            games.GameFace = string.IsNullOrEmpty(facename) ? games.GameFace : face;
            games.GameImg1 = string.IsNullOrEmpty(img[0]) ? games.GameImg1 : img[0];
            games.GameImg2 = string.IsNullOrEmpty(img[1]) ? games.GameImg2 : img[1];
            games.GameImg3 = string.IsNullOrEmpty(img[2]) ? games.GameImg3 : img[2];
            db.SaveChanges();

            return RedirectToAction("Article");
        }
        
        public ActionResult Delete(int gameid)
        {
            var game = db.Games.Find(gameid);
            var link = db.Link.Find(game.LinkID);
            var comment = db.Comment.Where(m => m.GameID == gameid);
            //判断是否有评论，有的话删除
            if (comment != null)
            {
                db.Comment.RemoveRange(comment);
            }

            //提交数据库
            db.Games.Remove(game);
            db.Link.Remove(link);
            db.SaveChanges();

            return RedirectToAction("Article");
        }
    }
}