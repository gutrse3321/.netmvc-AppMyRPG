using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace AppMyRPG
{
    public class MySec
    {
        public static string GetMD5(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str,"MD5").ToLower();
        }   
    }
}