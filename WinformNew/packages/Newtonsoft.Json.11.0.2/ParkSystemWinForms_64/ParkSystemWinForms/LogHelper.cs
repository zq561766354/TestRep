using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ParkSystemWinForms
{
    public class LogHelper
    {
        private ILog logger;

        public LogHelper(ILog log)
        {
            this.logger = log;
        }
        public void Debug(object message)
        {
            this.logger.Debug(message);
        }
        public void Debug(object message, Exception e)
        {
            this.logger.Debug(message, e);
        }
        public void Error(object message)
        {
            this.logger.Error(message);
        }
        public void Error(object message, Exception e)
        {
            this.logger.Error(message, e);
        }
    }
}
