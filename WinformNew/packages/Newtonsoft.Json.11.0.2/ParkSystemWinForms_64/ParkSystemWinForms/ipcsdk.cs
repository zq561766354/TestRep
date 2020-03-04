using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ParkSystemWinForms
{
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct ICE_OSDAttr_S
    {
        public uint u32OSDLocationVideo;
        public uint u32ColorVideo;
        public uint u32DateVideo;
        public uint u32License;
        public uint u32CustomVideo;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szCustomVideo;
        public uint u32OSDLocationJpeg;
        public uint u32ColorJpeg;
        public uint u32DateJpeg;
        public uint u32Algo;
        public uint u32DeviceID;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szDeviceID;
        public uint u32DeviceName;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szDeviceName;
        public uint u32CamreaLocation;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szCamreaLocation;
        public uint u32SubLocation;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szSubLocation;
        public uint u32ShowDirection;
        public uint u32CameraDirection;
        public uint u32CustomJpeg;
        public uint u32ItemDisplayMode;
        public uint u32DateMode;
        public uint u32BgColor;
        public uint u32FontSize;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 48)]
        public string szCustomJpeg;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 384)]
        public string szCustomVideo6;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 384)]
        public string szCustomJpeg6;
    }
    public partial class ipcsdk
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate void ICE_IPCSDK_OnPlate(
                    System.IntPtr pvParam,
                    [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP,
                    [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcNumber,
                    [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcColor,
                    System.IntPtr pcPicData,
                    uint u32PicLen,
                    System.IntPtr pcCloseUpPicData,
                    uint u32CloseUpPicLen,
                    short s16PlatePosLeft,
                    short s16PlatePosTop,
                    short s16PlatePosRight,
                    short s16PlatePosBottom,
                    float fPlateConfidence,
                    uint u32VehicleColor,
                    uint u32PlateType,
                    uint u32VehicleDir,
                    uint u32AlarmType,
                    uint u32SerialNum,
                    uint u32Reserved2,
                    uint u32Reserved3,
                    uint u32Reserved4);

        public delegate void ICE_IPCSDK_OnFrame_Planar(System.IntPtr pvParam, uint u32Timestamp, System.IntPtr pu8DataY, System.IntPtr pu8DataU, System.IntPtr pu8DataV, int s32LinesizeY, int s32LinesizeU, int s32LinesizeV, int s32Width, int s32Height);

        public delegate void ICE_IPCSDK_OnPastPlate(
            System.IntPtr pvParam,
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP,
            uint u32CapTime,
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcNumber,
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcColor,
            System.IntPtr pcPicData,
            uint u32PicLen,
            System.IntPtr pcCloseUpPicData,
            uint u32CloseUpPicLen,
            short s16PlatePosLeft,
            short s16PlatePosTop,
            short s16PlatePosRight,
            short s16PlatePosBottom,
            float fPlateConfidence,
            uint u32VehicleColor,
            uint u32PlateType,
            uint u32VehicleDir,
            uint u32AlarmType,
            uint u32Reserved1,
            uint u32Reserved2,
            uint u32Reserved3,
            uint u32Reserved4);

        public delegate void ICE_IPCSDK_OnSerialPort(System.IntPtr pvParam, 
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP, 
            System.IntPtr pcData, uint u32Len);

        public delegate void ICE_IPCSDK_OnSerialPort_RS232(System.IntPtr pvParam, 
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP, 
            System.IntPtr pcData, uint u32Len);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_Init", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_Init();

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_Fini", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_Fini();

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_OpenPreview",CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr ICE_IPCSDK_OpenPreview([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP, 
            byte u8OverTCP, byte u8MainStream, uint hWnd, ICE_IPCSDK_OnPlate pfOnPlate, System.IntPtr pvPlateParam);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_OpenPreview_Passwd", CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr ICE_IPCSDK_OpenPreview_Passwd([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP,
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcPasswd,
            byte u8OverTCP, byte u8MainStream, uint hWnd, ICE_IPCSDK_OnPlate pfOnPlate, System.IntPtr pvPlateParam);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_Close", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_Close(System.IntPtr hSDK);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_OpenGate", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_OpenGate(System.IntPtr hSDK);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_BeginTalk", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_BeginTalk(System.IntPtr hSDK);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_EndTalk", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_EndTalk(System.IntPtr hSDK);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_Trigger", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_Trigger(System.IntPtr hSDK, StringBuilder pcNumber, StringBuilder pcColor, byte[] pcPicData, uint u32PicSize, ref uint pu32PicLen);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_GetStatus", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_GetStatus(System.IntPtr hSDK);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_Capture", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_Capture(System.IntPtr hSDK, byte[] pcPicData, uint u32PicSize, ref uint pu32PicLen);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_Reboot", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_Reboot(System.IntPtr hSDK);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_Open", CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr ICE_IPCSDK_Open([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP, 
            byte u8OverTCP, ushort u16RTSPPort, ushort u16ICEPort, ushort u16OnvifPort, byte u8MainStream, uint pfOnStream, System.IntPtr pvStreamParam, uint pfOnFrame, System.IntPtr pvFrameParam);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_Open_Passwd", CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr ICE_IPCSDK_Open_Passwd([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP,
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcPasswd,
            byte u8OverTCP, ushort u16RTSPPort, ushort u16ICEPort, ushort u16OnvifPort, byte u8MainStream, uint pfOnStream, System.IntPtr pvStreamParam, uint pfOnFrame, System.IntPtr pvFrameParam);


        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SetFrameCallback", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_SetFrameCallback(System.IntPtr hSDK, ICE_IPCSDK_OnFrame_Planar pfOnFrame, System.IntPtr pvParam);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SetPlateCallback", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_SetPlateCallback(System.IntPtr hSDK, ICE_IPCSDK_OnPlate pfOnPlate, System.IntPtr pvParam);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SyncTime", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_SyncTime(System.IntPtr hSDK, ushort u16Year, byte u8Month, byte u8Day, byte u8Hour, byte u8Min, byte u8Sec);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_TransSerialPort", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_TransSerialPort(System.IntPtr hSDK, string pcData, uint u32Len);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_GetDevID", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_GetDevID(System.IntPtr hSDK, StringBuilder szDevID);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SetSerialPortCallBack", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_SetSerialPortCallBack(System.IntPtr hSDK, ICE_IPCSDK_OnSerialPort pfOnSerialPort, System.IntPtr pvSerialPortParam);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_StartRecord", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_StartRecord(System.IntPtr hSDK, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcFileName);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_StopRecord", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_StopRecord(System.IntPtr hSDK);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SetOSDCfg", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_SetOSDCfg(System.IntPtr hSDK, ref ICE_OSDAttr_S pstOSDAttr);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_StartStream", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_StartStream(System.IntPtr hSDK, byte u8MainStream, uint hWnd);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_StopStream", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_StopStream(System.IntPtr hSDK);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_OpenDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr ICE_IPCSDK_OpenDevice([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_OpenDevice_Passwd", CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr ICE_IPCSDK_OpenDevice_Passwd([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP,
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcPasswd);


        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_TriggerExt", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_TriggerExt(System.IntPtr hSDK);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SetPastPlateCallBack", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_SetPastPlateCallBack(System.IntPtr hSDK, ICE_IPCSDK_OnPastPlate pfOnPastPlate, System.IntPtr pvPastPlateParam);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_WriteUserData", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_WriteUserData(System.IntPtr hSDK, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcData);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_ReadUserData", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_ReadUserData(System.IntPtr hSDK, byte[] pcData, int nSize);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_GetIPAddr", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_GetIPAddr(System.IntPtr hSDK, ref uint pu32IP, ref uint pu32Mask, ref uint pu32Gateway);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SetIPAddr", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_SetIPAddr(System.IntPtr hSDK, uint u32IP, uint u32Mask, uint u32Gateway);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SearchDev", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_SearchDev(StringBuilder szDevs);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_LogConfig", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_LogConfig(int openLog, string logPath);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_GetAlarmOutConfig", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_GetAlarmOutConfig(System.IntPtr hSDK, uint u32Index, ref uint pu32IdleState, ref uint pu32DelayTime, ref uint pu32Reserve);


        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SetAlarmOutConfig", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_SetAlarmOutConfig(System.IntPtr hSDK, uint u32Index, uint u32IdleState, uint u32DelayTime, uint u32Reserve);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_ControlAlarmOut", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_ControlAlarmOut(System.IntPtr hSDK, uint u32Index);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_Broadcast", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_Broadcast(System.IntPtr hSDK, ushort nIndex);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_BroadcastGroup", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_BroadcastGroup(System.IntPtr hSDK, 
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIndex);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SetCity", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_SetCity(System.IntPtr hSDK, uint u32Index);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_TransSerialPort_RS232", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_TransSerialPort_RS232(System.IntPtr hSDK, string pcData, uint u32Len);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_SetSerialPortCallBack_RS232", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ICE_IPCSDK_SetSerialPortCallBack_RS232(System.IntPtr hSDK, ICE_IPCSDK_OnSerialPort_RS232 pfOnSerialPort, System.IntPtr pvSerialPortParam);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_WriteUserData_Binary", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_WriteUserData_Binary(System.IntPtr hSDK, 
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcData, 
            uint nOffset, uint nLen);

        [System.Runtime.InteropServices.DllImportAttribute("ice_ipcsdk.dll", EntryPoint = "ICE_IPCSDK_ReadUserData_Binary", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ICE_IPCSDK_ReadUserData_Binary(System.IntPtr hSDK, byte[] pcData, uint nSize, uint nOffset, uint nLen);
    }
}
