using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSystemWinForms
{
    public static class FileHelper
    {
        /// <summary>  
        ///     编码方式  
        /// </summary>  
        private static readonly Encoding Encoding = Encoding.UTF8;

        /// <summary>  
        ///     递归取得文件夹下文件  
        /// </summary>  
        /// <param name="dir"></param>  
        /// <param name="list"></param>  
        public static void GetFiles(string dir, List<string> list)
        {
            GetFiles(dir, list, new List<string>());
        }

        /// <summary>  
        ///     递归取得文件夹下文件  
        /// </summary>  
        /// <param name="dir"></param>  
        /// <param name="list"></param>  
        /// <param name="fileExtsions"></param>  
        public static void GetFiles(string dir, List<string> list, List<string> fileExtsions)
        {
            //添加文件   
            string[] files = Directory.GetFiles(dir);
            if (fileExtsions.Count > 0)
            {
                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file);
                    if (extension != null && fileExtsions.Contains(extension))
                    {
                        list.Add(file);
                    }
                }
            }
            else
            {
                list.AddRange(files);
            }
            //如果是目录，则递归  
            DirectoryInfo[] directories = new DirectoryInfo(dir).GetDirectories();
            foreach (DirectoryInfo item in directories)
            {
                GetFiles(item.FullName, list, fileExtsions);
            }
        }



        /// <summary>  
        ///     写入文件  
        /// </summary>  
        /// <param name="filePath">文件名</param>  
        /// <param name="content">文件内容</param>  
        public static void WriteFile(string filePath, string content)
        {
            try
            {
                var fs = new FileStream(filePath, FileMode.Create);
                Encoding encode = Encoding;
                //获得字节数组  
                byte[] data = encode.GetBytes(content);
                //开始写入  
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流  
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void WriteFile2(string filePath, string content)
        {
            try
            {
                var fs = new FileStream(filePath, FileMode.Append);
                Encoding encode = Encoding;
                //获得字节数组  
                byte[] data = encode.GetBytes(content);
                //开始写入  
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流  
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        /// <summary>  
        ///     读取文件  
        /// </summary>  
        /// <param name="filePath"></param>  
        /// <returns></returns>  
        public static string ReadFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Encoding e = GetFileType(fs);
            fs.Close();

            return ReadFile(filePath, e);
        }

        /// <summary>  
        ///     读取文件  
        /// </summary>  
        /// <param name="filePath"></param>  
        /// <param name="encoding"></param>  
        /// <returns></returns>  
        public static string ReadFile(string filePath, Encoding encoding)
        {
            using (var sr = new StreamReader(filePath, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>  
        ///     读取文件  
        /// </summary>  
        /// <param name="filePath"></param>  
        /// <returns></returns>  
        public static List<string> ReadFileLines(string filePath)
        {
            var str = new List<string>();
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Encoding e = GetFileType(fs);
            fs.Close();

            using (var sr = new StreamReader(filePath, e))
            {
                String input;
                while ((input = sr.ReadLine()) != null)
                {
                    str.Add(input);
                }
            }
            return str;
        }


        /// <summary> 
        /// 通过给定的文件流，判断文件的编码类型 
        /// </summary> 
        /// <param name=“fs“>文件流</param> 
        /// <returns>文件的编码类型</returns> 
        public static System.Text.Encoding GetFileType(FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
            Encoding reVal = Encoding.Default;

            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;

        }


        /// <summary> 
        /// 判断是否是不带 BOM 的 UTF8 格式 
        /// </summary> 
        /// <param name=“data“></param> 
        /// <returns></returns> 
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
            byte curByte; //当前分析的字节. 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前 
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1 
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }


        /// <summary>  
        ///     复制文件夹（及文件夹下所有子文件夹和文件）  
        /// </summary>  
        /// <param name="sourcePath">待复制的文件夹路径</param>  
        /// <param name="destinationPath">目标路径</param>  
        public static void CopyDirectory(String sourcePath, String destinationPath)
        {
            var info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                String destName = Path.Combine(destinationPath, fsi.Name);

                if (fsi is FileInfo) //如果是文件，复制文件  
                    System.IO.File.Copy(fsi.FullName, destName);
                else //如果是文件夹，新建文件夹，递归  
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }


        /// <summary>  
        ///     删除文件夹（及文件夹下所有子文件夹和文件）  
        /// </summary>  
        /// <param name="directoryPath"></param>  
        public static void DeleteFolder(string directoryPath)
        {
            foreach (string d in Directory.GetFileSystemEntries(directoryPath))
            {
                if (System.IO.File.Exists(d))
                {
                    var fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = FileAttributes.Normal;
                    System.IO.File.Delete(d); //删除文件     
                }
                else
                    DeleteFolder(d); //删除文件夹  
            }
            Directory.Delete(directoryPath); //删除空文件夹  
        }


        /// <summary>  
        ///     清空文件夹（及文件夹下所有子文件夹和文件）  
        /// </summary>  
        /// <param name="directoryPath"></param>  
        public static void ClearFolder(string directoryPath)
        {
            foreach (string d in Directory.GetFileSystemEntries(directoryPath))
            {
                if (System.IO.File.Exists(d))
                {
                    var fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = FileAttributes.Normal;
                    System.IO.File.Delete(d); //删除文件     
                }
                else
                    DeleteFolder(d); //删除文件夹  
            }
        }

        /// <summary>  
        ///     取得文件大小，按适当单位转换  
        /// </summary>  
        /// <param name="filepath"></param>  
        /// <returns></returns>  
        public static string GetFileSize(string filepath)
        {
            string result = "0KB";
            if (System.IO.File.Exists(filepath))
            {
                var size = new FileInfo(filepath).Length;
                int filelength = size.ToString().Length;
                if (filelength < 4)
                    result = size + "byte";
                else if (filelength < 7)
                    result = Math.Round(Convert.ToDouble(size / 1024d), 2) + "KB";
                else if (filelength < 10)
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
                else if (filelength < 13)
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
                else
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
                return result;
            }
            return result;
        }

    }
}
