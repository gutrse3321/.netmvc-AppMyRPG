using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AppMyRPG.Models
{
    public class ForgotPwdViewModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserUID { get; set; }

        [Required(ErrorMessage = "邮箱不能为空")]
        public string UserEmail { get; set; }
    }
}