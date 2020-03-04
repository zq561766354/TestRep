using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ParkSystemWinForms
{
    public class ConfigHelper
    {
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string AppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString().Trim();
        }
        /// <summary>
        /// 根据name取connectionString值
        /// </summary>
        /// <param name="name"></param>
        public static string ConnectionStrings(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString.Trim();
        }
        
    }
}
