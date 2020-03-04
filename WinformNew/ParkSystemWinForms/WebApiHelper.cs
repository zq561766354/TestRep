using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;


namespace ParkSystemWinForms
{
    public class WebApiHelper
    {
        public static string HttpPost(string url, string body)
        {      
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                request.Timeout = ParkSystemUtility.timout * 1000;
                //request.ContentType = "application/x-www-form-urlencoded";

                byte[] buffer = encoding.GetBytes(body);
                request.ContentLength = buffer.Length;


                //byte[] bytes=new byte[4+buffer.Length+img.Length];
                //Array.Copy(buffer, bytes, 0, buffer.Length);

                //at
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
     
        }
        public static string HttpGet(string url)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }


        
    }
}
