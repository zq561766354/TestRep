using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bealead.ICEIPC;
using ParkSystemWinForms.Model;

namespace ParkSystemWinForms
{
    //余位满，临时订单
    public class OrderIpc
    {
        public IPC ipcCaramer { get; set; }
        public string licensePlateColor { get; set; }
        public string licensePlateNo { get; set; }
        public string picturePath { get; set; }
        public string pictureName { get; set; }
        public string platePath { get; set; }
        public string plateName { get; set; }
        public string imageBase64 { get; set; }
        public int memberId { get; set; }
    }
}
