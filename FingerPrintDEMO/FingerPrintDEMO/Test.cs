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
using System.IO;
using CuiHua.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static FingerPrintDEMO.FingerPrintHelper;
using System.Text.RegularExpressions;

namespace FingerPrintDEMO
{
    public partial class Test : Form
    {
        /// <summary>
        /// handle
        /// </summary>
        IntPtr pHandle = IntPtr.Zero;
        /// <summary>
        /// connection address
        /// </summary>
        int nAddr = 2;

        int count;

        /// <summary>
        /// matching score
        /// </summary>
        int iScore = 0;

        int iBufferID = 1;

        public Test()
        {
            InitializeComponent();
        }
        private void UpdateLog(string message)
        {
            if (richtxt_log.InvokeRequired)
            {
                richtxt_log.Invoke((MethodInvoker)delegate
                {
                    richtxt_log.AppendText(message + "\n");
                });
            }
            else
            {
                richtxt_log.AppendText(message + "\n");
            }
        }

        // This method continuously scans for fingerprints and performs matching
        private unsafe void ContinuousFingerprintScan()
        {

            return;
            while (true)
            {
                // Check for fingerprint input
                int n = FingerPrintHelper.PSGetImage(pHandle, nAddr);
                if (n == (int)ReturnValue.PS_OK)
                {
                    UpdateLog("Fingerprint retrieval successful.\n");
                    int t = 0;
                    byte[] imData = new byte[256 * 288];
                    fixed (byte* array = imData)
                    {
                        n = FingerPrintHelper.PSUpImage(pHandle, nAddr, array, out t);
                        if (n == (int)ReturnValue.PS_OK)
                        {
                            n = FingerPrintHelper.PSImgData2BMP(array, "c:/new.png");
                            if (n == (int)ReturnValue.PS_OK)
                            {
                                UpdateLog("Ok");
                            }
                        }
                    }
                    if (FingerPrintHelper.PSGenChar(pHandle, nAddr, 1) == (int)ReturnValue.PS_OK)
                    {
                        UpdateLog("Character file successfully filled into BufferA.\n");
                        int iScore = 0;

                        // Perform fingerprint matching
                        if (FingerPrintHelper.PSMatch(pHandle, nAddr, out iScore) == (int)ReturnValue.PS_OK)
                        {
                            UpdateLog("Comparison successful.\n");

                            // Retrieve matched fingerprint data
                            byte[] data = new byte[512];
                            fixed (byte* arry = data)
                            {
                                int length = 0;
                                if (FingerPrintHelper.PSUpChar(pHandle, nAddr, iBufferID, arry, out length) == (int)ReturnValue.PS_OK)
                                {
                                    string hexStr = FingerPrintHelper.ToHexString(data);
                                    UpdateLog(string.Format("Score: {0}\n", iScore));
                                    UpdateLog(string.Format("{0}\n", hexStr));

                                    // Clear fingerprint library if needed
                                    if (FingerPrintHelper.PSEmpty(pHandle, nAddr) == ReturnValue.PS_OK.ToInt())
                                    {
                                        UpdateLog("Flash fingerprint library cleared successfully.\n");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Call this method to start continuous fingerprint scanning
        private void StartContinuousScanning()
        {
            //Task.Run(() => ContinuousFingerprintScan());
        }


        private void Test_Load(object sender, EventArgs e)
        {
            //connect scanner
            if (FingerPrintHelper.PSOpenDeviceEx(out pHandle, 2, 1) == ReturnValue.PS_OK.ToInt())
            {
                //this.label1.Text = "连接成功";
                richtxt_log.AppendText("Connected\n");
                int incount;

                int result = FingerPrintHelper.PSTemplateNum(pHandle, nAddr, out incount);

                if (result == (int)ReturnValue.PS_OK)
                {
                    richtxt_log.AppendText("count " + incount + "\n");
                    count = incount;
                }

                StartContinuousScanning();
                //if (FingerPrintHelper.PSEmpty(pHandle, nAddr) == ReturnValue.PS_OK.ToInt())
                // {
                //    richtxt_log.AppendText("Flash cleared successfully\n");
                //}
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
                richtxt_log.AppendText("Scanned fingerprint successfully\n");
                //Generate charachter file
                if (FingerPrintHelper.PSGenChar(pHandle, nAddr, iBufferID) == (int)ReturnValue.PS_OK)
                {
                    richtxt_log.AppendText(string.Format("Charachter file successfully filled into {0} \n", iBufferID == 1 ? "BufferA" : "BufferB"));
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
                //        n = FingerPrintHelper.PSImgData2BMP(array, "c:/new.png");
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
                richtxt_log.AppendText("Template synthesis successful.\n");

                if (FingerPrintHelper.PSStoreChar(pHandle, nAddr, iBufferID, 229) == (int)ReturnValue.PS_OK)
                    {
                        richtxt_log.AppendText("Template stored in flash memory library successfully\n");
                    count = count++;
                    }
            }
        }
        private unsafe void btnmach_Click(object sender, EventArgs e)
        {
            if (FingerPrintHelper.PSGetImage(pHandle, nAddr) == (int)ReturnValue.PS_OK)
            {
                richtxt_log.AppendText("Scanned fingerprint successfully\n");
                //Generate charachter file
                if (FingerPrintHelper.PSGenChar(pHandle, nAddr, iBufferID) == (int)ReturnValue.PS_OK)
                {
                    if (FingerPrintHelper.PSHighSpeedSearch(pHandle, nAddr, iBufferID, 0, 232, out int iMbAdd, out int iScore)==(int)ReturnValue.PS_OK)
                    {

                        richtxt_log.AppendText("Score=" + iScore.ToString() + "\n");
                        richtxt_log.AppendText("Add=" + iMbAdd.ToString() + "\n");
                    }
                }
            }
            return;

            int n = FingerPrintHelper.PSGetImage(pHandle, nAddr);

            if (n == (int)ReturnValue.PS_OK)
            {
                richtxt_log.AppendText("Fingerprint retrieval successful.\n");

                if (FingerPrintHelper.PSGenChar(pHandle, nAddr, iBufferID) == (int)ReturnValue.PS_OK)
                {
                    richtxt_log.AppendText("Character file successfully filled into" + iBufferID.ToString() + ".\n");


                    iScore = 0;
                    if (FingerPrintHelper.PSMatch(pHandle, nAddr, out iScore) == (int)ReturnValue.PS_OK)
                    {

                        byte[] data = new byte[512];
                        fixed (byte* arry = data)
                        {
                            int length = 0;
                            if (FingerPrintHelper.PSUpChar(pHandle, nAddr, iBufferID, arry, out length) == (int)ReturnValue.PS_OK)
                            {
                                string hexStr = FingerPrintHelper.ToHexString(data);


                                int f = PSIdentify(pHandle, nAddr, out int _);


                                richtxt_log.AppendText(string.Format("Score{0}\n", iScore));

                                richtxt_log.AppendText("FingerId=" + f.ToString());

                                FingerPrintHelper.PSEmpty(pHandle, nAddr);

                                return;


                                richtxt_log.AppendText(BitConverter.ToString(data).Replace("-", string.Empty));
                                richtxt_log.AppendText(string.Format("Score{0}\n", iScore));
                                richtxt_log.AppendText(string.Format("{0}\n", hexStr));

                            }
                        }


                    }
                }
            }
        }
    }
}
