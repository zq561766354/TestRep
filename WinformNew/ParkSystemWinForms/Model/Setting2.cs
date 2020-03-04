using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSystemWinForms.Model
{
    [Serializable]
    public class Setting2
    {

        public Setting2() 
        {
            Guards = new List<Guard>();
        }

        public string Name { get; set; }

        public string ID { get; set; }

        public List<Guard> Guards { get; set; }
    }
}
