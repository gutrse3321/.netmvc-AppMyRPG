using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AppMyRPG.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home",new { area=""});
        }
    }
}