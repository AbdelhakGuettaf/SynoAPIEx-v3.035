using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CuiHua.Common;
using static FingerPrintDEMO.FingerPrintHelper;

namespace FingerPrintDEMO
{
    public partial class Test : Form
    {
        /// <summary>
        /// 句柄
        /// </summary>
        IntPtr pHandle = IntPtr.Zero;
        /// <summary>
        /// 连接地址
        /// </summary>
        int nAddr = 2;

        /// <summary>
        /// 匹配分数
        /// </summary>
        int iScore = 0;

        int iBufferID = 1;

        public Test()
        {
            InitializeComponent();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            //连接指纹
            if (FingerPrintHelper.PSOpenDeviceEx(out pHandle, 2, 1) == ReturnValue.PS_OK.ToInt())
            {
                //this.label1.Text = "连接成功";
                richtxt_log.AppendText("指纹连接成功\n");
                if (FingerPrintHelper.PSEmpty(pHandle, nAddr) == ReturnValue.PS_OK.ToInt())
                {
                    richtxt_log.AppendText("清除闪存指纹libaray成功\n");
                }
            }
        }

        private void btn_trycon_Click(object sender, EventArgs e)
        {

        }

        private unsafe void btn_finger_Click(object sender, EventArgs e)
        {
            int n = FingerPrintHelper.PSGetImage(pHandle, nAddr);
            if (n == (int)ReturnValue.PS_OK)
            {
                richtxt_log.AppendText("获取指纹成功\n");
                //生成字符文件
                if (FingerPrintHelper.PSGenChar(pHandle, nAddr, iBufferID) == (int)ReturnValue.PS_OK)
                {
                    richtxt_log.AppendText(string.Format("字符文件填充到{0}成功\n", iBufferID == 1 ? "BufferA" : "BufferB"));
                }
                iBufferID = iBufferID == 1 ? 2 : 1;
                //存储图片到本地
                //int t = 0;
                //byte[] data = new byte[256 * 288];
                //fixed (byte* array = data)
                //{
                //    n = FingerPrintHelper.PSUpImage(pHandle, nAddr, array, out t);
                //    if (n == (int)ReturnValue.PS_OK)
                //    {
                //        n = FingerPrintHelper.PSImgData2BMP(array, "D:/new.png");
                //        if (n == (int)ReturnValue.PS_OK)
                //        {

                //        }
                //    }
                //}

                //搜索现有指纹模板
                //int num = 0;
                //if (FingerPrintHelper.PSTemplateNum(pHandle, nAddr, out num) == (int)ReturnValue.PS_OK)
                //{
                //    int index = 0;
                //    for (int i = 0; i < 4; i++)
                //    {
                //        IndexTable_STATUS userContent;
                //        List<int> figerIDList = new List<int>();
                //        if (FingerPrintHelper.PSReadIndexTable(pHandle, nAddr, i, out userContent) == (int)ReturnValue.PS_OK)
                //        {
                //            foreach (byte item in userContent.UserContent)
                //            {
                //                string str = Convert.ToString(item, 2);
                //                for (int j = str.Length - 1; j >= 0; j--)
                //                {
                //                    if (str[j] == '1')
                //                    {
                //                        figerIDList.Add(index);
                //                    }
                //                    index++;
                //                }
                //            }
                //            if (figerIDList.Count > 0)
                //            {

                //            }
                //        }
                //    }
                //}
                //加载指纹模板到A或B
                //if (FingerPrintHelper.PSLoadChar(pHandle, nAddr, 1, 0) == (int)ReturnValue.PS_OK)
                //{
                //    byte[] data = new byte[512];
                //    fixed (byte* arry = data)
                //    {
                //        int length = 0;
                //        if (FingerPrintHelper.PSUpChar(pHandle, nAddr, iBufferID, arry, out length) == (int)ReturnValue.PS_OK)
                //        {
                //            string hexStr = FingerPrintHelper.ToHexString(data);
                //            if (data.Length > 0)
                //            {

                //            }
                //        }
                //    }
                //}
            }
        }

        private unsafe void btngenchar1_Click(object sender, EventArgs e)
        {
            if (FingerPrintHelper.PSRegModule(pHandle, nAddr) == (int)ReturnValue.PS_OK)
            {
                richtxt_log.AppendText("合成模板成功\n");
                if (FingerPrintHelper.PSStoreChar(pHandle, nAddr, iBufferID, 0) == (int)ReturnValue.PS_OK)
                {
                    richtxt_log.AppendText("存储模板到闪存库成功\n");
                    if (FingerPrintHelper.PSLoadChar(pHandle, nAddr, 1, 0) == (int)ReturnValue.PS_OK)
                    {
                        richtxt_log.AppendText("模板储存到BufferB区成功\n");
                    }
                }
            }
        }

        private unsafe void btnmach_Click(object sender, EventArgs e)
        {
            int n = FingerPrintHelper.PSGetImage(pHandle, nAddr);
            if (n == (int)ReturnValue.PS_OK)
            {
                richtxt_log.AppendText("获取指纹成功\n");
                //生成字符文件
                if (FingerPrintHelper.PSGenChar(pHandle, nAddr, 1) == (int)ReturnValue.PS_OK)
                {
                    richtxt_log.AppendText("字符文件填充到BufferA成功\n");
                    iScore = 0;
                    if (FingerPrintHelper.PSMatch(pHandle, nAddr, out iScore) == (int)ReturnValue.PS_OK)
                    {
                        richtxt_log.AppendText("比对成功\n");
                        byte[] data = new byte[512];
                        fixed (byte* arry = data)
                        {
                            int length = 0;
                            if (FingerPrintHelper.PSUpChar(pHandle, nAddr, iBufferID, arry, out length) == (int)ReturnValue.PS_OK)
                            {
                                string hexStr = FingerPrintHelper.ToHexString(data);
                                richtxt_log.AppendText(string.Format("匹配分数{0}\n", iScore));
                                richtxt_log.AppendText(string.Format("{0}\n", hexStr));
                                if (FingerPrintHelper.PSEmpty(pHandle, nAddr) == ReturnValue.PS_OK.ToInt())
                                {
                                    richtxt_log.AppendText("清除闪存指纹libaray成功\n");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
