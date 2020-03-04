using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ParkSystemWinForms
{
    public static class SerOrDerModel<T> where T : new()
    {
        //序列化操作
        public static void Serialize(T t, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, t);
            }
        }

        //反序列化操作
        public static T Deserialize(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {

                BinaryFormatter bf = new BinaryFormatter();
                T t = (T)bf.Deserialize(fs);
                return t;
            }
        }
    }
}
