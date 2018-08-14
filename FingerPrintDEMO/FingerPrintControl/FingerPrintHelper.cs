using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FingerPrintControl
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
        /// 从PC下载字符文件到BufferA或BufferB
        /// </summary>
        [DllImport("SynoAPIEx.dll", CharSet = CharSet.Unicode)]
        public static unsafe extern int PSDownChar(IntPtr hHandle, int nAddr, int iBufferID, byte* pTemplet, int iTempletLength);

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


        /// <summary>
        /// 数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes)
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

        /// <summary>
        /// 字符串转16进制字节组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] strToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");   //去除空格
            if ((hexString.Length % 2) != 0)     //判断hexstring的长度是否为偶数
            {
                hexString += "";
            }
            byte[] returnBytes = new byte[hexString.Length / 2];  //声明一个长度为hexstring长度一半的字节组returnBytes
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);  //将hexstring的两个字符转换成16进制的字节组
            }
            return returnBytes;
        }
    }

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
        [Description("手指太干")]
        PS_FP_TOO_DRY = 0x04,
        [Description("手指太湿")]
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
        [Description("必须移动手指")]
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
