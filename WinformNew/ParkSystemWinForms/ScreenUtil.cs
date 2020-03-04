using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParkSystemWinForms
{
    public class ScreenUtil
    {
        public static byte[] ScreenByteValue(string content, int lineNumber, int time, int color)
        {
            string hexContent = HexCodeHelper.ToHex(content, "GBK", false);
            byte[] contentByte = HexCodeHelper.strToToHexByte(hexContent);
            int dataLength = 4 + contentByte.Length; //数据长度
            int length = 15 + contentByte.Length;   //总的数据位数
            byte[] showValueByte = new byte[length]; //显示数据byte数组

            //生成CRC校验，去除帧头和帧尾
            int dataLen = length - 3;  //CRC校验的byte数组长度
            byte[] contextBytes = new byte[dataLen];
            contextBytes[0] = Convert.ToByte("01", 16);
            contextBytes[1] = Convert.ToByte("64", 16);
            contextBytes[2] = Convert.ToByte("00", 16); //3位预留
            contextBytes[3] = Convert.ToByte("26", 16); //命令
            contextBytes[4] = Convert.ToByte("00", 16);
            contextBytes[5] = Convert.ToByte(Convert.ToString(dataLength, 16), 16);
            contextBytes[6] = Convert.ToByte(Convert.ToString(lineNumber, 16), 16);
            contextBytes[7] = Convert.ToByte(Convert.ToString(time, 16), 16);
            contextBytes[8] = Convert.ToByte(Convert.ToString(color, 16), 16);
            //contextBytes[7] = Convert.ToByte(Convert.ToString(color, 16), 16);
            //contextBytes[8] = Convert.ToByte(Convert.ToString(time, 16), 16);
            contextBytes[9] = Convert.ToByte("00", 16);  //保留位
            int m = 0;
            foreach (byte b in contentByte)
            {
                contextBytes[m + 10] = contentByte[m];
                m++;

            }
            int n = m + 10;
            contextBytes[n] = Convert.ToByte("00", 16);  //保留位
            contextBytes[n + 1] = Convert.ToByte("00", 16);  //保留位
            byte[] CRCBytes = HexCodeHelper.CRC16(contextBytes, dataLen);

            //生成发送数据byte数组
            showValueByte[0] = Convert.ToByte("AA", 16);
            showValueByte[1] = Convert.ToByte("55", 16);  //2位帧头
            showValueByte[2] = Convert.ToByte("01", 16);
            showValueByte[3] = Convert.ToByte("64", 16);
            showValueByte[4] = Convert.ToByte("00", 16); //3位预留
            showValueByte[5] = Convert.ToByte("26", 16); //命令
            showValueByte[6] = Convert.ToByte("00", 16);
            showValueByte[7] = Convert.ToByte(Convert.ToString(dataLength, 16), 16);
            showValueByte[8] = Convert.ToByte(Convert.ToString(lineNumber, 16), 16);
            showValueByte[9] = Convert.ToByte(Convert.ToString(time, 16), 16);
            showValueByte[10] = Convert.ToByte(Convert.ToString(color, 16), 16);
            showValueByte[11] = Convert.ToByte("00", 16);  //保留位
            int i = 0;
            foreach (byte b in contentByte)
            {
                showValueByte[i + 12] = contentByte[i];
                i++;

            }
            int j = i + 12;
            showValueByte[j] = CRCBytes[1];
            showValueByte[j + 1] = CRCBytes[0];
            showValueByte[j + 2] = Convert.ToByte("AF", 16);
            return showValueByte;
        }


        public static byte[] BroadcastValue(string content, byte[] broadcastTipcontentFront, byte[] broadcastTipcontentBack)
        {
            string hexContent = HexCodeHelper.ToHex(content, "GBK", false);
            byte[] contentByte = HexCodeHelper.strToToHexByte(hexContent);
            int contentLen = contentByte.Length;
            int broadcastTipcontentFrontLen = 0;
            int broadcastTipcontentBackLen = 0;
            if (broadcastTipcontentFront != null)
                broadcastTipcontentFrontLen = broadcastTipcontentFront.Length;
            if (broadcastTipcontentBack != null)
                broadcastTipcontentBackLen = broadcastTipcontentBack.Length;
            byte[] broadReturnValueByte = new byte[contentLen + broadcastTipcontentFrontLen + broadcastTipcontentBackLen];


            int m = 0;

            if (broadcastTipcontentFront != null)
            {
                foreach (byte b in broadcastTipcontentFront)
                {
                    broadReturnValueByte[m] = broadcastTipcontentFront[m];
                    m++;
                }
            }
            int n = 0;
            int k = m + n;

            foreach (byte b in contentByte)
            {
                broadReturnValueByte[k + n] = contentByte[n];
                n++;
            }

            int p = 0;
            int q = k + n;
            if (broadcastTipcontentBack != null)
            {
                foreach (byte b in broadcastTipcontentBack)
                {
                    broadReturnValueByte[q + p] = broadcastTipcontentBack[p];
                    p++;
                }
            }
            return BroadcastValue(broadReturnValueByte);


        }

        //设置音量
        public static byte[] SetVoice(int voice)  //1-10
        {
            //string hexContent = HexUtil.ToHex(content, "GBK", false);
            //byte[] contentByte = strToToHexByte(hexContent);
            int length = 12;   //总的数据位数
            byte[] broadcastVoiceByte = new byte[length]; //显示数据byte数组

            //生成CRC校验，去除帧头和帧尾
            int dataLen = length - 3;  //CRC校验的byte数组长度
            byte[] contextBytes = new byte[dataLen];
            contextBytes[0] = Convert.ToByte("03", 16);
            contextBytes[1] = Convert.ToByte("64", 16);
            contextBytes[2] = Convert.ToByte("00", 16); //3位预留
            contextBytes[3] = Convert.ToByte("F0", 16); //命令
            contextBytes[4] = Convert.ToByte("00", 16);
            contextBytes[5] = Convert.ToByte("01", 16);
            contextBytes[6] = Convert.ToByte(Convert.ToString(voice, 16), 16);
            contextBytes[7] = Convert.ToByte("00", 16);  //保留位
            contextBytes[8] = Convert.ToByte("00", 16);  //保留位
            byte[] CRCBytes = HexCodeHelper.CRC16(contextBytes, dataLen);

            //生成发送数据byte数组
            broadcastVoiceByte[0] = Convert.ToByte("AA", 16);
            broadcastVoiceByte[1] = Convert.ToByte("55", 16);  //2位帧头
            broadcastVoiceByte[2] = Convert.ToByte("03", 16);
            broadcastVoiceByte[3] = Convert.ToByte("64", 16);
            broadcastVoiceByte[4] = Convert.ToByte("00", 16); //3位预留
            broadcastVoiceByte[5] = Convert.ToByte("F0", 16); //命令
            broadcastVoiceByte[6] = Convert.ToByte("00", 16);
            broadcastVoiceByte[7] = Convert.ToByte("01", 16);
            broadcastVoiceByte[8] = Convert.ToByte(Convert.ToString(voice, 16), 16);
            broadcastVoiceByte[9] = CRCBytes[1];
            broadcastVoiceByte[10] = CRCBytes[0];
            broadcastVoiceByte[11] = Convert.ToByte("AF", 16);
            return broadcastVoiceByte;
            
        }

        public static byte[] BroadcastValue(byte[] contentByte)
        {
            int dataLength = contentByte.Length; //数据长度
            int length = 11 + contentByte.Length;   //总的数据位数
            byte[] broadcastValueByte = new byte[length]; //显示数据byte数组

            //生成CRC校验，去除帧头和帧尾
            int dataLen = length - 3;  //CRC校验的byte数组长度
            byte[] contextBytes = new byte[dataLen];
            contextBytes[0] = Convert.ToByte("03", 16);
            contextBytes[1] = Convert.ToByte("64", 16);
            contextBytes[2] = Convert.ToByte("00", 16); //3位预留
            contextBytes[3] = Convert.ToByte("22", 16); //命令
            contextBytes[4] = Convert.ToByte("00", 16);
            contextBytes[5] = Convert.ToByte(Convert.ToString(dataLength, 16), 16);
            int m = 0;
            foreach (byte b in contentByte)
            {
                contextBytes[m + 6] = contentByte[m];
                m++;

            }
            int n = m + 6;
            contextBytes[n] = Convert.ToByte("00", 16);  //保留位
            contextBytes[n + 1] = Convert.ToByte("00", 16);  //保留位
            byte[] CRCBytes = HexCodeHelper.CRC16(contextBytes, dataLen);

            //生成发送数据byte数组
            broadcastValueByte[0] = Convert.ToByte("AA", 16);
            broadcastValueByte[1] = Convert.ToByte("55", 16);  //2位帧头
            broadcastValueByte[2] = Convert.ToByte("03", 16);
            broadcastValueByte[3] = Convert.ToByte("64", 16);
            broadcastValueByte[4] = Convert.ToByte("00", 16); //3位预留
            broadcastValueByte[5] = Convert.ToByte("22", 16); //命令
            broadcastValueByte[6] = Convert.ToByte("00", 16);
            broadcastValueByte[7] = Convert.ToByte(Convert.ToString(dataLength, 16), 16);
            int i = 0;
            foreach (byte b in contentByte)
            {
                broadcastValueByte[i + 8] = contentByte[i];
                i++;

            }
            int j = i + 8;
            broadcastValueByte[j] = CRCBytes[1];
            broadcastValueByte[j + 1] = CRCBytes[0];
            broadcastValueByte[j + 2] = Convert.ToByte("AF", 16);
            return broadcastValueByte;
        }


        public static byte[] BroadcastNum(int numberType, Int32 num)
        {

            byte[] contentByte = BitConverter.GetBytes(num);
            Array.Reverse(contentByte);
            int length = 16;   //总的数据位数
            byte[] broadcastValueByte = new byte[length]; //显示数据byte数组

            //生成CRC校验，去除帧头和帧尾
            int dataLen = length - 3;  //CRC校验的byte数组长度
            byte[] contextBytes = new byte[dataLen];
            contextBytes[0] = Convert.ToByte("03", 16);
            contextBytes[1] = Convert.ToByte("64", 16);
            contextBytes[2] = Convert.ToByte("00", 16); //3位预留
            contextBytes[3] = Convert.ToByte("2C", 16); //命令
            contextBytes[4] = Convert.ToByte("00", 16);
            contextBytes[5] = Convert.ToByte("05", 16);
            contextBytes[6] = Convert.ToByte(Convert.ToString(numberType, 16), 16);

            int m = 0;
            foreach (byte b in contentByte)
            {
                contextBytes[m + 7] = contentByte[m];
                m++;

            }
            int n = m + 7;
            contextBytes[n] = Convert.ToByte("00", 16);  //保留位
            contextBytes[n + 1] = Convert.ToByte("00", 16);  //保留位

            byte[] CRCBytes = HexCodeHelper.CRC16(contextBytes, dataLen);

            //生成发送数据byte数组
            broadcastValueByte[0] = Convert.ToByte("AA", 16);
            broadcastValueByte[1] = Convert.ToByte("55", 16);  //2位帧头
            broadcastValueByte[2] = Convert.ToByte("03", 16);
            broadcastValueByte[3] = Convert.ToByte("64", 16);
            broadcastValueByte[4] = Convert.ToByte("00", 16); //3位预留
            broadcastValueByte[5] = Convert.ToByte("2C", 16); //命令
            broadcastValueByte[6] = Convert.ToByte("00", 16);
            broadcastValueByte[7] = Convert.ToByte("05", 16);
            broadcastValueByte[8] = Convert.ToByte(Convert.ToString(numberType, 16), 16);

            int p = 0;
            foreach (byte b in contentByte)
            {
                broadcastValueByte[p + 9] = contentByte[p];
                p++;

            }
            int q = p + 9;

            broadcastValueByte[q] = CRCBytes[1]; //保留位
            broadcastValueByte[q + 1] = CRCBytes[0]; //保留位
            broadcastValueByte[q + 2] = Convert.ToByte("AF", 16); //保留位

            return broadcastValueByte;
        }

        //设置广告
        public static byte[] SetAdvertisement(string content, int lineNum, int color)
        {

            string hexContent = HexCodeHelper.ToHex(content, "GBK", false);
            byte[] contentByte = HexCodeHelper.strToToHexByte(hexContent);
            int dataLength = 3 + contentByte.Length; //数据长度(行号+颜色+保留位) 
            int length = 14 + contentByte.Length;   //总的数据位数
            byte[] showValueByte = new byte[length]; //显示数据byte数组

            //生成CRC校验，去除帧头和帧尾
            int dataLen = length - 3;  //CRC校验的byte数组长度
            byte[] contextBytes = new byte[dataLen];
            contextBytes[0] = Convert.ToByte("03", 16);
            contextBytes[1] = Convert.ToByte("64", 16);
            contextBytes[2] = Convert.ToByte("00", 16); //3位预留
            contextBytes[3] = Convert.ToByte("25", 16); //命令
            contextBytes[4] = Convert.ToByte("00", 16);
            contextBytes[5] = Convert.ToByte(Convert.ToString(dataLength, 16), 16);
            contextBytes[6] = Convert.ToByte(Convert.ToString(lineNum, 16), 16);
            contextBytes[7] = Convert.ToByte(Convert.ToString(color, 16), 16);
            contextBytes[8] = Convert.ToByte("00", 16); //预留
            int m = 0;
            foreach (byte b in contentByte)
            {
                contextBytes[m + 9] = contentByte[m];
                m++;

            }
            int n = m + 9;
            contextBytes[n] = Convert.ToByte("00", 16);  //校验位
            contextBytes[n + 1] = Convert.ToByte("00", 16);  //校验位
            byte[] CRCBytes = HexCodeHelper.CRC16(contextBytes, dataLen);

            //生成发送数据byte数组
            showValueByte[0] = Convert.ToByte("AA", 16);
            showValueByte[1] = Convert.ToByte("55", 16);  //2位帧头
            showValueByte[2] = Convert.ToByte("03", 16);
            showValueByte[3] = Convert.ToByte("64", 16);
            showValueByte[4] = Convert.ToByte("00", 16); //3位预留
            showValueByte[5] = Convert.ToByte("25", 16); //命令
            showValueByte[6] = Convert.ToByte("00", 16);
            showValueByte[7] = Convert.ToByte(Convert.ToString(dataLength, 16), 16);
            showValueByte[8] = Convert.ToByte(Convert.ToString(lineNum, 16), 16);
            showValueByte[9] = Convert.ToByte(Convert.ToString(color, 16), 16);
            showValueByte[10] = Convert.ToByte("00", 16);  //保留位
            int i = 0;
            foreach (byte b in contentByte)
            {
                showValueByte[i + 11] = contentByte[i];
                i++;

            }
            int j = i + 11;
            showValueByte[j] = CRCBytes[1];
            showValueByte[j + 1] = CRCBytes[0];
            showValueByte[j + 2] = Convert.ToByte("AF", 16);
            return showValueByte;
        }

        //设置余位
        //lineNum:余位显示行号（0-4，0=取消余位显示）
        //characterColor：汉字颜色（1：红色；2：绿色；3：黄色）
        //numberColor：数字颜色（1：红色；2：绿色；3：黄色）
        //characterType：汉字类型（0-“余位”，1-“剩余”，2-“空位”，3-“空余”，4-“车位”）
        //leftCount：余位数
        public static byte[] SetRemainingPosition(int lineNum, int characterColor, int numberColor, int characterType, int leftCount)
        {
            byte[] contentByte = BitConverter.GetBytes(leftCount);
          
            int length = 17;  //总的数据位数
            byte[] showValueByte = new byte[length]; //显示数据byte数组

            //生成CRC校验，去除帧头和帧尾
            int dataLen = length - 3;  //CRC校验的byte数组长度
            byte[] contextBytes = new byte[dataLen];
            contextBytes[0] = Convert.ToByte("03", 16);
            contextBytes[1] = Convert.ToByte("64", 16);
            contextBytes[2] = Convert.ToByte("00", 16); //以上3位预留
            contextBytes[3] = Convert.ToByte("14", 16); //命令
            contextBytes[4] = Convert.ToByte("00", 16);
            contextBytes[5] = Convert.ToByte("06", 16); //6位长度
            contextBytes[6] = Convert.ToByte(Convert.ToString(lineNum, 16), 16);
            contextBytes[7] = Convert.ToByte(Convert.ToString(characterColor, 16), 16);
            contextBytes[8] = Convert.ToByte(Convert.ToString(numberColor, 16), 16);
            contextBytes[9] = Convert.ToByte(Convert.ToString(characterType, 16), 16);
            contextBytes[10] = contentByte[1];//余位的高位
            contextBytes[11] = contentByte[0];//余位数的低位
            contextBytes[12] = Convert.ToByte("00", 16);  //校验位
            contextBytes[13] = Convert.ToByte("00", 16);  //校验位
            byte[] CRCBytes = HexCodeHelper.CRC16(contextBytes, dataLen);

            //生成发送数据byte数组
            showValueByte[0] = Convert.ToByte("AA", 16);
            showValueByte[1] = Convert.ToByte("55", 16);  //2位帧头
            showValueByte[2] = Convert.ToByte("03", 16);
            showValueByte[3] = Convert.ToByte("64", 16);
            showValueByte[4] = Convert.ToByte("00", 16); //3位预留
            showValueByte[5] = Convert.ToByte("14", 16); //命令
            showValueByte[6] = Convert.ToByte("00", 16);
            showValueByte[7] = Convert.ToByte("06", 16); //6位长度
            showValueByte[8] = Convert.ToByte(Convert.ToString(lineNum, 16), 16);
            showValueByte[9] = Convert.ToByte(Convert.ToString(characterColor, 16), 16);
            showValueByte[10] = Convert.ToByte(Convert.ToString(numberColor, 16), 16);
            showValueByte[11] = Convert.ToByte(Convert.ToString(characterType, 16), 16);
            showValueByte[12] = contentByte[1]; //余位的高位
            showValueByte[13] = contentByte[0]; //余位数的低位
            showValueByte[14] = CRCBytes[1];
            showValueByte[15] = CRCBytes[0];
            showValueByte[16] = Convert.ToByte("AF", 16);
            return showValueByte;
        }

        //设置时间
        public static byte[] SetTime(DateTime now) 
        {
            int year = now.Year - 2000; //日期传入协议会自动加2000
            int month = now.Month;
            int day = now.Day;
            int hour = now.Hour;
            int min = now.Minute;
            int sec = now.Second;

            int length = 17;  //总的数据位数
            byte[] showValueByte = new byte[length]; //显示数据byte数组

            //生成CRC校验，去除帧头和帧尾
            int dataLen = length - 3;  //CRC校验的byte数组长度
            byte[] contextBytes = new byte[dataLen];
            contextBytes[0] = Convert.ToByte("03", 16);
            contextBytes[1] = Convert.ToByte("64", 16);
            contextBytes[2] = Convert.ToByte("00", 16); //以上3位预留
            contextBytes[3] = Convert.ToByte("10", 16); //命令
            contextBytes[4] = Convert.ToByte("00", 16);
            contextBytes[5] = Convert.ToByte("06", 16); //6位长度
            contextBytes[6] = Convert.ToByte(Convert.ToString(year, 16), 16);
            contextBytes[7] = Convert.ToByte(Convert.ToString(month, 16), 16);
            contextBytes[8] = Convert.ToByte(Convert.ToString(day, 16), 16);
            contextBytes[9] = Convert.ToByte(Convert.ToString(hour, 16), 16);
            contextBytes[10] = Convert.ToByte(Convert.ToString(min, 16), 16); 
            contextBytes[11] = Convert.ToByte(Convert.ToString(sec, 16), 16); 
            contextBytes[12] = Convert.ToByte("00", 16);  //校验位
            contextBytes[13] = Convert.ToByte("00", 16);  //校验位
            byte[] CRCBytes = HexCodeHelper.CRC16(contextBytes, dataLen); //生成校验位

            //生成发送数据byte数组
            showValueByte[0] = Convert.ToByte("AA", 16);
            showValueByte[1] = Convert.ToByte("55", 16);  //2位帧头
            showValueByte[2] = Convert.ToByte("03", 16);
            showValueByte[3] = Convert.ToByte("64", 16);
            showValueByte[4] = Convert.ToByte("00", 16); //3位预留
            showValueByte[5] = Convert.ToByte("10", 16); //命令
            showValueByte[6] = Convert.ToByte("00", 16);
            showValueByte[7] = Convert.ToByte("06", 16); //6位长度
            showValueByte[8] = Convert.ToByte(Convert.ToString(year, 16), 16);
            showValueByte[9] = Convert.ToByte(Convert.ToString(month, 16), 16);
            showValueByte[10] = Convert.ToByte(Convert.ToString(day, 16), 16);
            showValueByte[11] = Convert.ToByte(Convert.ToString(hour, 16), 16);
            showValueByte[12] = Convert.ToByte(Convert.ToString(min, 16), 16);
            showValueByte[13] = Convert.ToByte(Convert.ToString(sec, 16), 16);
            showValueByte[14] = CRCBytes[1];
            showValueByte[15] = CRCBytes[0];
            showValueByte[16] = Convert.ToByte("AF", 16);
            return showValueByte;

        }

        //设置时间显示方式
        //lineNum:余位显示行号（0-4，0=不显示时间）
        //color：数字颜色（1：红色；2：绿色；3：黄色）
        //showMode：显示方式，默认传0
        public static byte[] SetTimeShowModel(int lineNum,int color,int showMode)
        {
            int length = 14;  //总的数据位数
            byte[] showValueByte = new byte[length]; //显示数据byte数组

            //生成CRC校验，去除帧头和帧尾
            int dataLen = length - 3;  //CRC校验的byte数组长度
            byte[] contextBytes = new byte[dataLen];
            contextBytes[0] = Convert.ToByte("03", 16);
            contextBytes[1] = Convert.ToByte("64", 16);
            contextBytes[2] = Convert.ToByte("00", 16); //以上3位预留
            contextBytes[3] = Convert.ToByte("F6", 16); //命令
            contextBytes[4] = Convert.ToByte("00", 16);
            contextBytes[5] = Convert.ToByte("03", 16); //3位长度
            contextBytes[6] = Convert.ToByte(Convert.ToString(lineNum, 16), 16);
            contextBytes[7] = Convert.ToByte(Convert.ToString(color, 16), 16);
            contextBytes[8] = Convert.ToByte(Convert.ToString(showMode, 16), 16);
            contextBytes[9] = Convert.ToByte("00", 16);  //校验位
            contextBytes[10] = Convert.ToByte("00", 16);  //校验位
            byte[] CRCBytes = HexCodeHelper.CRC16(contextBytes, dataLen); //生成校验位

            //生成发送数据byte数组
            showValueByte[0] = Convert.ToByte("AA", 16);
            showValueByte[1] = Convert.ToByte("55", 16);  //2位帧头
            showValueByte[2] = Convert.ToByte("03", 16);
            showValueByte[3] = Convert.ToByte("64", 16);
            showValueByte[4] = Convert.ToByte("00", 16); //3位预留
            showValueByte[5] = Convert.ToByte("F6", 16); //命令
            showValueByte[6] = Convert.ToByte("00", 16);
            showValueByte[7] = Convert.ToByte("03", 16); //3位长度
            showValueByte[8] = Convert.ToByte(Convert.ToString(lineNum, 16), 16);
            showValueByte[9] = Convert.ToByte(Convert.ToString(color, 16), 16);
            showValueByte[10] = Convert.ToByte(Convert.ToString(showMode, 16), 16);
            showValueByte[11] = CRCBytes[1];
            showValueByte[12] = CRCBytes[0];
            showValueByte[13] = Convert.ToByte("AF", 16);
            return showValueByte;

        }
    }
}
