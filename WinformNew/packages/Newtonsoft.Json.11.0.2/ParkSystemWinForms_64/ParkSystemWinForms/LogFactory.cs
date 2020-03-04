using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;

namespace ParkSystemWinForms
{
    public class LogFactory
    {
        static LogFactory()
        {
            FileInfo configFile = new FileInfo("/log4net.config");

            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        public static LogHelper GetLogger(Type type)
        {
            return new LogHelper(LogManager.GetLogger(type));
        }

        public static LogHelper GetLogger(string str)
        {
            return new LogHelper(LogManager.GetLogger(str));
        }
    }
}
