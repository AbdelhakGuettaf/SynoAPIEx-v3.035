using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TM.Library;
using System.Timers;
using System.Threading;

namespace FingerPrintControl
{
    public delegate void FingerPrintCompleted(object sender, string hexStr);
    public partial class FingerPrintControl : UserControl
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

        /// <summary>
        /// 缓存区ID A=1 B=2
        /// </summary>
        int iBufferID = 1;

        /// <summary>
        /// 执行命令后返回代码
        /// </summary>
        int n;

        /// <summary>
        /// 锁
        /// </summary>
        string printLock = "printLock";
        string checkLock = "checkLock";


        public event FingerPrintCompleted OnFingerPrintCompleted;
        ToolTip _ToolTip;
        //检测手机计时器
        System.Timers.Timer aTimer;
        //比对合成模板计时器
        System.Timers.Timer Checktimer;
        /// <summary>
        /// 显示提示控件
        /// </summary>
        ToolTip ToolTip
        {
            get
            {
                if (_ToolTip == null)
                {
                    _ToolTip = new ToolTip();
                    _ToolTip.AutomaticDelay = 500;
                    _ToolTip.IsBalloon = false;
                    _ToolTip.ToolTipIcon = ToolTipIcon.Info;
                    _ToolTip.ShowAlways = false;
                }
                return _ToolTip;
            }
        }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        void ShowTip(string title, string msg)
        {
            ToolTip.ToolTipTitle = title;
            ThisDelegate(() => { ToolTip.Show(msg, this, 0, this.Height, 4000); });
        }
        public FingerPrintControl()
        {
            InitializeComponent();
            this.BorderStyle = BorderStyle.FixedSingle;
            this.ResumeLayout(false);
        }

        //加载事件
        private void FingerPrintControl_Load(object sender, EventArgs e)
        {
            //Init();
        }

        //单击事件
        private void panel_pic_Click(object sender, EventArgs e)
        {
            Init();
        }

        /// <summary>
        /// 计时器事件
        /// </summary>
        private void Objtimer_Tick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    lock (printLock)
                    {
                        if (FingerPrintFuc(() => { return FingerPrintHelper.PSGetImage(pHandle, nAddr); }, false))
                        {
                            if (FingerPrintFuc(() => { return FingerPrintHelper.PSGenChar(pHandle, nAddr, iBufferID); }))
                            {
                                if (iBufferID == 2)
                                {
                                    ThisDelegate(() => { pic_fingerprint.Image = global::FingerPrintControl.Properties.Resources.FingerPrint2; });
                                    aTimer.Stop();
                                    if (FingerPrintFuc(() => { return FingerPrintHelper.PSRegModule(pHandle, nAddr); }))
                                    {
                                        if (FingerPrintFuc(() => { return FingerPrintHelper.PSStoreChar(pHandle, nAddr, iBufferID, 0); }))
                                        {
                                            //合成后的模板在A区
                                            if (FingerPrintFuc(() => { return FingerPrintHelper.PSLoadChar(pHandle, nAddr, 1, 0); }))
                                            {
                                                ChangeText("请将手指放到指纹仪确认指纹");
                                                Checktimer = new System.Timers.Timer(1500);
                                                Checktimer.AutoReset = true;
                                                //开始计时
                                                Checktimer.Enabled = true;
                                                //确认指纹成功
                                                bool checkResult = false;
                                                Checktimer.Elapsed += (o, objevent) =>
                                                {
                                                    lock (checkLock)
                                                    {
                                                        if (checkResult)
                                                        {
                                                            return;
                                                        }
                                                        if ((n = FingerPrintHelper.PSGetImage(pHandle, nAddr)) == (int)ReturnValue.PS_OK)
                                                        {
                                                            //再次获取后的放在B区
                                                            if (FingerPrintFuc(() => { return FingerPrintHelper.PSGenChar(pHandle, nAddr, 2); }))
                                                            {
                                                                //比对两个区后就是是否成功
                                                                if (FingerPrintFuc(() => { return FingerPrintHelper.PSMatch(pHandle, nAddr, out iScore); }))
                                                                {
                                                                    string hexStr = UpChar();
                                                                    if (hexStr != string.Empty)
                                                                    {
                                                                        ThisDelegate(() => { pic_fingerprint.Image = global::FingerPrintControl.Properties.Resources.FingerPrint4; });
                                                                        checkResult = true;
                                                                        Checktimer.Stop();
                                                                        ClearVal();
                                                                        ChangeText("录入完成");
                                                                        OnFingerPrintCompleted(this, hexStr);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var returnValue = n.ToEnum<ReturnValue>();
                                                            if (returnValue != ReturnValue.PS_NO_FINGER)
                                                            {
                                                                ShowTip("提示", n.ToEnumDescriptionString(typeof(ReturnValue)));
                                                            }
                                                            else if (returnValue == ReturnValue.PS_LITTLE_FEATURE)
                                                            {
                                                                ShowTip("提示", "指纹不匹配");
                                                            }
                                                        }
                                                    }
                                                };
                                                Checktimer.Start();
                                            }
                                        }
                                    }
                                }
                                if (iBufferID == 1)
                                {
                                    ThisDelegate(() => { pic_fingerprint.Image = global::FingerPrintControl.Properties.Resources.FingerPrint1; });
                                    ChangeText("请抬起手指");
                                    iBufferID = 2;
                                    Thread.Sleep(2000);
                                    ChangeText("请将手指再次放到指纹仪");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowTip("指纹仪错误", "请再次重试");
                    LogHelper.AsnyWriteLog(ex.ToString());
                }
            });
        }

        /// <summary>
        /// 获取生成后的特征码  可存储到数据库
        /// </summary>
        /// <returns></returns>
        private unsafe string UpChar()
        {
            byte[] data = new byte[512];
            fixed (byte* arry = data)
            {
                int length = 0;
                if (FingerPrintHelper.PSUpChar(pHandle, nAddr, iBufferID, arry, out length) == (int)ReturnValue.PS_OK)
                {
                    string hexStr = FingerPrintHelper.ToHexString(data);
                    if (FingerPrintFuc(() => { return FingerPrintHelper.PSEmpty(pHandle, nAddr); }))
                    {
                        return hexStr;
                    }
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 比对现有指纹库
        /// </summary>
        /// <returns></returns>
        private unsafe bool DownChar(string hexstr)
        {

            //连接指纹
            if ((n = FingerPrintHelper.PSOpenDeviceEx(out pHandle, 2, 1)) == (int)ReturnValue.PS_OK)
            {
                //清除指纹库模板信息
                if (FingerPrintFuc(() => { return FingerPrintHelper.PSEmpty(pHandle, nAddr); }))
                {
                    byte[] tempbyte = FingerPrintHelper.strToHexByte(hexstr);
                    fixed (byte* arry = tempbyte)
                    {
                        if (FingerPrintHelper.PSDownChar(pHandle, nAddr, iBufferID, arry, 512) == (int)ReturnValue.PS_OK)
                        {
                            iBufferID++;
                            System.Timers.Timer upTimer = new System.Timers.Timer(1500);
                            upTimer.Elapsed += (o, e) =>
                            {
                                if (FingerPrintFuc(() => { return FingerPrintHelper.PSGetImage(pHandle, nAddr); }, false))
                                {
                                    if (FingerPrintFuc(() => { return FingerPrintHelper.PSGenChar(pHandle, nAddr, iBufferID); }))
                                    {
                                        //比对两个区后就是是否成功
                                        if (FingerPrintFuc(() => { return FingerPrintHelper.PSMatch(pHandle, nAddr, out iScore); }))
                                        {
                                            ThisDelegate(() => { MessageBox.Show(string.Format("比对成功，分数{0}", iScore)); });
                                        }
                                    }
                                }
                            };
                            upTimer.AutoReset = true;
                            upTimer.Enabled = true;
                            upTimer.Start();
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            ClearVal();
            ChangeText("请将手指放到指纹仪");
            Task.Run(() =>
            {
                try
                {
                    //连接指纹
                    if ((n = FingerPrintHelper.PSOpenDeviceEx(out pHandle, 2, 1)) == (int)ReturnValue.PS_OK)
                    {
                        //清除指纹库模板信息
                        if (FingerPrintFuc(() => { return FingerPrintHelper.PSEmpty(pHandle, nAddr); }))
                        {
                            //实例化Timer类，设置间隔时间为10000毫秒； 
                            aTimer = new System.Timers.Timer(1500);
                            aTimer.Elapsed += new ElapsedEventHandler(Objtimer_Tick);
                            aTimer.AutoReset = true;
                            aTimer.Enabled = true;
                            aTimer.Start();
                        }
                    }
                    else
                    {
                        ShowTip("连接指纹仪失败", "请确定已插入指纹仪后重试");
                        LogHelper.AsnyWriteLog("连接指纹仪失败");
                    }
                }
                catch (Exception ex)
                {
                    ShowTip("连接指纹仪失败", "请确定已插入指纹仪后重试");
                    LogHelper.AsnyWriteLog(ex.ToString());
                }
            });
        }

        /// <summary>
        /// 初始化参数值
        /// </summary>
        private void ClearVal()
        {
            if (aTimer != null)
            {
                aTimer.Stop();
                aTimer = null;
            }
            if (Checktimer != null)
            {
                Checktimer.Stop();
                Checktimer = null;
            }
            pHandle = IntPtr.Zero;
            nAddr = 2;
            iScore = 0;
            iBufferID = 1;
        }

        //改变提示文本的值
        private void ChangeText(string val)
        {
            ThisDelegate(() => { lab_log.Text = val; });
        }

        /// <summary>
        /// 在UI线程执行方法
        /// </summary>
        /// <param name="func">没有返回值的方法</param>
        private void ThisDelegate(Action func)
        {
            this.Invoke((EventHandler)delegate { func(); });
        }

        /// <summary>
        /// 执行指纹仪命令
        /// </summary>
        /// <param name="func">有返回值的方法</param>
        /// <returns></returns>
        private bool FingerPrintFuc(Func<int> func, bool isshow = true)
        {
            if ((n = func()) == (int)ReturnValue.PS_OK)
            {
                return true;
            }
            else
            {
                if (isshow)
                {
                    string msg = n.ToEnumDescriptionString(typeof(ReturnValue));
                    ShowTip("提示", msg);
                    LogHelper.AsnyWriteLog(msg);
                }
                return false;
            }
        }
    }
}
