using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ParkSystemWinForms
{
    public class MD5Helper
    {
        public static string SHA1(string s)
        {
            s = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "SHA1").ToString();
            return s.ToLower();
        }
        public static string Md5(string str)
        {
            str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToString();
            return str.ToLower();
        }
    }
}
