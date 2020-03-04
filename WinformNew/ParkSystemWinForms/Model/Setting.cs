using ParkSystemWinForms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSystemWinForms.Model
{
    /// <summary>
    /// 参数
    /// </summary>
    [Serializable]
    public class Setting
    {
        //停车场
        public ParkingLot ParkLot { get; set; }

        //出入口列表
        public List<Guard> Guards { get; set; }

        //图片路径保存目录
        public string ImagePath { get; set; }

        //入场屏幕音量
        public int InVolume { get; set; }

        //出场屏幕音量
        public int OutVolume { get; set; }

        //入口多相机并发间隔时间(毫秒)
        public int InDelay { get; set; }

        //出口多相机并发间隔时间(毫秒)
        public int OutDelay { get; set; }

        public int ScreenInDelay { get; set; }

        public int ScreenOutDelay { get; set; }

        //入场显示屏行数
        public int InLine { get; set; }

        //出场显示屏行数
        public int OutLine { get; set; }

        public Server Serv { get; set; }

        public bool EnabledWhiteList { get; set; }

        public bool EnabledWhiteListNoOrder { get; set; }
        public bool EnabledWhiteListUsed { get; set; }
        public bool EnabledMonthlyPass { get; set; }
        public bool EnabledYellowReCalculation { get; set; }
        public bool EnabledFreeTime { get; set; }
        public bool EnabledShowBnt { get; set; }
        public bool EnabledWLGO { get; set; }
        public bool EnableOneSpaceMoreCars { get; set; }

        public bool EnabledShowLeftCount { get; set; }

        public List<string> WhileList { get; set; }

        //出场未真正驶离，防止第二次抓拍间隔时间
        public int MistakenOutSec { get; set; }

        //入场单据延时误判处理间隔时间(分钟)
        public int MistakenEntranceMin { get; set; }
        public int HourSync { get; set; }
        public int MinSync { get; set; }
        //酒店白名单包月
        public bool EnabledHotel { get; set; }
        //是否更新停车场泊位数
        public bool IsUpdateBerthNum { get; set; }
        //断网续传频率(秒)
        public int brokenNetOrderFre { get; set; }
        //是否显示播报欢迎光临
        public bool IsWelcome { get; set; }
        //接口超时时间
        public int apiTimeout { get; set; }
        //自动检测余位间隔
        public int CheckLeftCountSec { get; set; }
        //出场未识别检测时间间隔
        public int UnidentifiedTimerMin { get; set; }
    }

    /// <summary>
    /// 停车场
    /// </summary>
    [Serializable]
    public class ParkingLot
    {
        //内码-主键
        public string Id { get; set; }

        //编号
        public string No { get; set; }

        //名称
        public string Name { get; set; }

        //类型
        public int PType { get; set; }

        //地址
        public string Addr { get; set; }

        //泊位数量
        public int BerthNum { get; set; }


    }

    /// <summary>
    /// 入出口
    /// </summary>
    [Serializable]
    public class Guard
    {
        //编号或者名称
        public string No { get; set; }

        //是否是出口
        public bool IsExit { get; set; }

        //主相机
        public GuardItem Primary { get; set; }

        //副相机集合
        public List<GuardItem> Secondaries { get; set; }
    }

    /// <summary>
    /// 出入口子项
    /// </summary>
    [Serializable]
    public class GuardItem
    {
        //相机IP地址
        public string IP { get; set; }

        //屏幕类型
        public int ScreenType { get; set; }

        //屏幕IP地址
        public string ScreenIP { get; set; }
    }



    /// <summary>
    /// 服务端配置
    /// </summary>
    [Serializable]
    public class Server
    {
        //api对应的域名或者ip地址
        public string Url { get; set; }

        //心跳检测频率(秒)
        public int HeartBeatFreq { get; set; }

        //心跳检测最大失败次数
        public int HeartBeatMaxRetryCount { get; set; }

        //支付检查频率(秒)
        public int PayCheckFreq { get; set; }

        //支付检查最大次数
        public int PayCheckMaxRetryCount { get; set; }

        //道闸常开发送频率(秒)
        public int GateKeepOpenFreq { get; set; }

        //后台数据同步频率(分)
        public int DataSyncFreq { get; set; }
    }

}
