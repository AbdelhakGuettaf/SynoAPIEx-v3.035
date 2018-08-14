using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FingerPrintDEMO
{
    public class FingerPrintHelper
    {
        /// <summary>
        ///  连接指纹仪
        /// </summary>
        [DllImport("SynoAPIEx.dll")]
        public static extern int PSOpenDeviceEx(out IntPtr pHandle, int nDeviceType, int iCom = 1, int iBaud = 1, int nPackageSize = 2, int iDevNum = 0);

        /// <summary>
        /// 检测手指获取图像
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
        public static extern int PSGetImage(IntPtr hHandle, int nAddr);

        /// <summary>
        /// 上传原始图像
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
        public static extern unsafe int PSUpImage(IntPtr hHandle, int nAddr, byte* pImageData, out int pFileName);

        /// <summary>
        /// 保存略缩图到本地
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
        public static extern unsafe int PSImgData2BMP(byte* pImageData, string pImageFile);

        /// <summary>
        /// 生成字符文件
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
        public static extern unsafe int PSGenChar(IntPtr hHandle, int nAddr, int iBufferID);

        /// <summary>
        /// 匹配芯片上的两个字符文件
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
        public static extern int PSMatch(IntPtr hHandle, int nAddr, out int iScore);


        /// <summary>
        /// 将BufferA的字符文件与BufferB的字符文件组合，并生成模板文件
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
        public static extern int PSRegModule(IntPtr hHandle, int nAddr);


        /// <summary>
        /// 将BufferA或BufferB的字符文件存储到闪存指纹库
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
        public static extern int PSStoreChar(IntPtr hHandle, int nAddr, int iBufferID, int iPageID);

        /// <summary>
        /// 从闪存指纹库中将模板传送到BufferA或BufferB 
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
        public static extern int PSLoadChar(IntPtr hHandle, int nAddr, int iBufferID, int iPageID);

        /// <summary>
        /// 清除闪存指纹libaray  
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
        public static extern int PSEmpty(IntPtr hHandle, int nAddr);

        /// <summary>
        /// 将BufferA或BufferB中的字符文件传输到PC 
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern int PSUpChar(IntPtr hHandle, int nAddr, int iBufferID, byte* pTemplet, out int iTempletLength);

        /// <summary>
        /// 获取有效指纹数
        /// </summary>
        /// <param name="hHandle"></param>
        /// <param name="nAddr"></param>
        /// <param name="iMbNum"></param>
        /// <returns></returns>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
        public static extern int PSTemplateNum(IntPtr hHandle, int nAddr, out int iMbNum);

        /// <summary>
        /// 获取现有指纹库
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
        public static extern int PSReadIndexTable(IntPtr hHandle, int nAddr, int nPage, out IndexTable_STATUS UserContent);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct IndexTable_STATUS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] UserContent;
        };


        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "

        {
            string hexString = string.Empty;

            if (bytes != null)

            {

                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)

                {

                    strB.Append(bytes[i].ToString("X2"));

                }

                hexString = strB.ToString();

            }
            return hexString;
        }
    }




    ///// <summary>
    ///// 1、设置默认参数（波特率、包大小、安全等级）
    ///// 2、打开设备
    ///// 3、录入指纹
    ///// 4、搜索指纹
    ///// 5、读取指纹库（FingerID）
    ///// 6、删除指纹
    ///// 7、取消操作
    ///// 8、关闭设备
    ///// </summary>
    //public class SynoAPIExHelper
    //{
    //    private IntPtr pHandle = IntPtr.Zero;
    //    private int nAddr = 1;

    //    private ReturnValue ConvertRV(int rv)
    //    {
    //        return (ReturnValue)rv;
    //    }
    //    /// <summary>
    //    /// 打开指纹设备
    //    /// </summary>
    //    /// <returns>执行结果</returns>
    //    public ReturnValue OpenDevice()
    //    {
    //        int n = PSOpenDeviceEx(out pHandle, DEVICE_UDisk, nAddr);
    //        return ConvertRV(n);
    //    }
    //    /// <summary>
    //    /// 关闭指纹设备
    //    /// </summary>
    //    /// <returns>执行结果</returns>
    //    public ReturnValue CloseDevice()
    //    {
    //        int n = 0;
    //        if (pHandle != IntPtr.Zero)
    //        {
    //            n = PSCloseDeviceEx(pHandle);
    //        }
    //        pHandle = IntPtr.Zero;
    //        return ConvertRV(n);
    //    }


    //    /// <summary>
    //    /// 获取有效指纹个数
    //    /// </summary>
    //    /// <param name="num">指纹个数</param>
    //    /// <returns>执行结果</returns>
    //    public ReturnValue GetFingerNum(out int num)
    //    {
    //        num = 0;
    //        int n = PSTemplateNum(pHandle, nAddr, out num);
    //        return ConvertRV(n);
    //    }
    //    /// <summary>
    //    /// 清空所有指纹
    //    /// </summary>
    //    /// <returns>执行结果</returns>
    //    public ReturnValue ClearAllFinger()
    //    {
    //        int n = PSEmpty(pHandle, nAddr);
    //        return ConvertRV(n);
    //    }
    //    /// <summary>
    //    /// 删除指纹库中的一个指纹
    //    /// </summary>
    //    /// <param name="figerID">要删除的指纹id</param>
    //    /// <returns>执行结果</returns>
    //    public ReturnValue DelFinger(int figerID)
    //    {
    //        int n = PSDelChar(pHandle, nAddr, figerID, 1);
    //        return ConvertRV(n);
    //    }
    //    /// <summary>
    //    /// 录入指纹
    //    /// </summary>
    //    /// <param name="figerID">指纹id号</param>
    //    /// <returns>执行结果</returns>
    //    public ReturnValue AddFinger(out int figerID)
    //    {
    //        figerID = 0;
    //        int n = PSEnroll(pHandle, nAddr, out figerID);
    //        return ConvertRV(n);
    //    }
    //    /// <summary>
    //    /// 查找指纹库中的一个指纹
    //    /// </summary>
    //    /// <param name="figerID">要删除的指纹id</param>
    //    /// <returns>执行结果</returns>
    //    public ReturnValue FindFinger(out int figerID)
    //    {
    //        int n = PSIdentify(pHandle, nAddr, out figerID);
    //        return ConvertRV(n);
    //    }
    //    /// <summary>
    //    /// 获取现有指纹库
    //    /// </summary>
    //    /// <param name="figerIDList">现有指纹id列表</param>
    //    /// <returns></returns>
    //    public ReturnValue GetAllFinger(out List<int> figerIDList)
    //    {
    //        figerIDList = new List<int>();
    //        int num = 0;
    //        var n = GetFingerNum(out num);
    //        if (n != ReturnValue.PS_OK)
    //        {
    //            return n;
    //        }
    //        else
    //        {
    //            int index = 0;
    //            for (int i = 0; i < 4; i++)
    //            {
    //                IndexTable_STATUS userContent;
    //                var nn = PSReadIndexTable(pHandle, nAddr, i, out userContent);
    //                if (nn == (int)ReturnValue.PS_OK)
    //                {
    //                    foreach (byte item in userContent.UserContent)
    //                    {
    //                        string str = Convert.ToString(item, 2);
    //                        for (int j = str.Length - 1; j >= 0; j--)
    //                        {
    //                            if (str[j] == '1')
    //                            {
    //                                figerIDList.Add(index);
    //                            }
    //                            index++;
    //                        }
    //                    }
    //                }
    //            }
    //            return n;
    //        }
    //    }
    //    /// <summary>
    //    /// 保存当前指纹图片
    //    /// </summary>
    //    /// <param name="path">保存指纹图片的路径</param>
    //    /// <returns>执行结果</returns>
    //    public unsafe ReturnValue SaveFigerBmp(string path)
    //    {
    //        int n = PSGetImage(pHandle, nAddr);
    //        if (n == (int)ReturnValue.PS_OK)
    //        {
    //            int t = 0;
    //            byte[] data = new byte[256 * 288];
    //            fixed (byte* array = data)
    //            {
    //                n = PSUpImage(pHandle, nAddr, array, out t);
    //                if (n == (int)ReturnValue.PS_OK)
    //                {
    //                    n = PSImgData2BMP(array, path);
    //                }
    //            }
    //        }
    //        return ConvertRV(n);
    //    }

    //    #region api定义
    //    private static int PS_MAXWAITTIME = 2000;
    //    private static int DELAY_TIME = 150;
    //    ///////////////缓冲区//////////////////////////////
    //    private static int CHAR_BUFFER_A = 0x01;
    //    private static int CHAR_BUFFER_B = 0x02;
    //    private static int MODEL_BUFFER = 0x03;
    //    //////////////////串口号////////////////////////
    //    private static int COM1 = 0x01;
    //    private static int COM2 = 0x02;
    //    private static int COM3 = 0x03;
    //    //////////////////波特率////////////////////////
    //    //4API 函数接口库使用手册
    //    private static int BAUD_RATE_9600 = 0x01;
    //    private static int BAUD_RATE_19200 = 0x02;
    //    private static int BAUD_RATE_38400 = 0x04;
    //    private static int BAUD_RATE_57600 = 0x06;//default
    //    private static int BAUD_RATE_115200 = 0x0C;
    //    private static int MAX_PACKAGE_SIZE_ = 350;// 数据包最大长度
    //    private static int CHAR_LEN_AES1711 = 1024;// 512->1024 [2009.11.12] AES1711使用大小模版
    //    private static int CHAR_LEN_NORMAL = 512;// 512 通用版本使用大小的模版
    //    private static int DEVICE_USB = 0;
    //    private static int DEVICE_COM = 1;
    //    private static int DEVICE_UDisk = 2;
    //    private static int IMAGE_X = 256;
    //    private static int IMAGE_Y = 288;
    //    #endregion

    //    #region api Fuc
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
    //    private static extern int PSOpenDeviceEx(out IntPtr pHandle, int nDeviceType, int iCom = 1, int iBaud = 1, int nPackageSize = 2, int iDevNum = 0);
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
    //    private static extern int PSCloseDeviceEx(IntPtr hHandle);
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
    //    private static extern int PSGetImage(IntPtr hHandle, int nAddr);
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
    //    private static extern unsafe int PSUpImage(IntPtr hHandle, int nAddr, byte* pImageData, out int pFileName);
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Ansi)]
    //    private static extern unsafe int PSImgData2BMP(byte* pImageData, string pImageFile);
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
    //    private static extern int PSReadIndexTable(IntPtr hHandle, int nAddr, int nPage, out IndexTable_STATUS UserContent);
    //    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    //    private struct IndexTable_STATUS
    //    {
    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    //        public byte[] UserContent;
    //    };
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
    //    private static extern int PSEmpty(IntPtr hHandle, int nAddr);
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
    //    private static extern int PSTemplateNum(IntPtr hHandle, int nAddr, out int iMbNum);
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
    //    private static extern int PSDelChar(IntPtr hHandle, int nAddr, int iStartPageID, int nDelPageNum);
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
    //    private static extern int PSEnroll(IntPtr hHandle, int nAddr, out int nID);
    //    [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
    //    private static extern int PSIdentify(IntPtr hHandle, int nAddr, out int iMbAddress);
    //    #endregion
    //}
    /// <summary>
    /// 错误返回码
    /// </summary>
    public enum ReturnValue
    {
        [Description("指令执行完毕或OK")]
        PS_OK = 0x00,
        [Description("数据包接受错误")]
        PS_COMM_ERR = 0x01,
        [Description("传感器上没有手指")]
        PS_NO_FINGER = 0x02,
        [Description("录入指纹图像失败")]
        PS_GET_IMG_ERR = 0x03,
        PS_FP_TOO_DRY = 0x04,
        PS_FP_TOO_WET = 0x05,
        [Description("指纹图像太乱而生不成特征")]
        PS_FP_DISORDER = 0x06,
        [Description("指纹图像正常, 但特征点太少或面积太小而生不成特征")]
        PS_LITTLE_FEATURE = 0x07,
        [Description("指纹不匹配")]
        PS_NOT_MATCH = 0x08,
        [Description("没搜索到指纹")]
        PS_NOT_SEARCHED = 0x09,
        [Description("指纹特征合并失败")]
        PS_MERGE_ERR = 0x0a,
        [Description("访问指纹库时地址序号超出指纹库范围")]
        PS_ADDRESS_OVER = 0x0b,
        [Description("从指纹库读模板出错或无效")]
        PS_READ_ERR = 0x0c,
        [Description("上传特征失败")]
        PS_UP_TEMP_ERR = 0x0d,
        [Description("模块不能接受后续数据包")]
        PS_RECV_ERR = 0x0e,
        [Description("上传图像失败")]
        PS_UP_IMG_ERR = 0x0f,
        [Description("删除模板失败")]
        PS_DEL_TEMP_ERR = 0x10,
        [Description("清空指纹库失败")]
        PS_CLEAR_TEMP_ERR = 0x11,
        PS_SLEEP_ERR = 0x12,
        [Description("口令不正确")]
        PS_INVALID_PASSWORD = 0x13,
        [Description("指令执行完毕或OK")]
        PS_RESET_ERR = 0x14,
        [Description("缓冲区内没有有效原始图而生不成图像")]
        PS_INVALID_IMAGE = 0x15,
        PS_HANGOVER_UNREMOVE = 0X17,
        [Description("读写FLASH出错")]
        PS_RWFlash_ERR = 0x18,
        [Description("无效寄存器号")]
        PS_Invalid_ERR = 0x1a,
        [Description("地址码错误")]
        PS_AddressCode_ERR = 0x20,
        [Description("必须验证口令")]
        PS_Password_ERR = 0x21
    }
}
