using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace TM.Library
{
    public class TextTraceListener : TraceListener, IDisposable
    {
        /// <summary>
        /// 是否及时关闭
        /// </summary>
        private bool m_bTimelyClose;
        /// <summary>
        /// 是否内部调用写
        /// </summary>
        private bool m_IsInnerCallWrite;
        /// <summary>
        /// 文件流
        /// </summary>
        private FileStream m_objInnerStream;
        /// <summary>
        /// 流
        /// </summary>
        private Stream m_objStream;
        /// <summary>
        /// 锁
        /// </summary>
        private static string m_sLocker;
        /// <summary>
        /// 路径
        /// </summary>
        private string m_sLogFileName;

        private string m_TitleName;
        /// <summary>
        /// 析构函数
        /// </summary>
        ~TextTraceListener()
        {
            base.Dispose();
        }
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static TextTraceListener()
        {
            m_sLocker = "Locker";
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="objStream"></param>
        public TextTraceListener(Stream objStream, string title = "异常信息")
        {
            this.m_bTimelyClose = false;
            this.m_objInnerStream = null;
            m_TitleName = title;
            this.Init(objStream);
        }
        /// <summary>
        /// 瓯子函数
        /// </summary>
        /// <param name="sWriteFile"></param>
        /// <param name="bTimely"></param>
        public TextTraceListener(string sWriteFile, bool bTimely, string title = "异常信息")
        {
            this.m_bTimelyClose = bTimely;
            this.m_objInnerStream = null;
            m_TitleName = title;
            if (!this.m_bTimelyClose)
            {
                this.m_objInnerStream = new FileStream(sWriteFile, FileMode.OpenOrCreate);
                this.Init(this.m_objInnerStream);
            }
            else
            {
                this.m_sLogFileName = sWriteFile;
            }
        }
        /// <summary>
        /// 初始化流
        /// </summary>
        /// <param name="objStream"></param>
        private void Init(Stream objStream)
        {
            this.m_objStream = objStream;
            this.m_objStream.Seek(0L, SeekOrigin.End);
            this.m_IsInnerCallWrite = false;
        }

        /// <summary>
        /// 
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this.m_objInnerStream != null)
            {
                this.m_objInnerStream.Close();
            }
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 写入信息
        /// </summary>
        /// <param name="message"></param>
        public override void Write(string message)
        {
            lock (m_sLocker)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("");
                builder.AppendLine(string.Format("========================【{0}】=========================", m_TitleName));
                builder.Append(DateTime.Now.ToString("MM-dd HH:mm:ss"));
                builder.Append(" P");
                Process currentProcess = Process.GetCurrentProcess();
                builder.Append(currentProcess.Id);
                builder.Append(" T");
                builder.Append(Thread.CurrentThread.ManagedThreadId.ToString());
                builder.AppendFormat("/{0}:", currentProcess.Threads.Count);
                builder.Append(message);
                builder.AppendLine("");
                builder.AppendLine("===================================================================");
                byte[] bytes = Encoding.Default.GetBytes(builder.ToString());
                if (!this.m_bTimelyClose)
                {
                    this.m_objStream.Write(bytes, 0, bytes.Length);
                    if (!this.m_IsInnerCallWrite)
                    {
                        //清除此流的所有缓冲区
                        this.m_objStream.Flush();
                    }
                }
                else
                {
                    using (this.m_objInnerStream = new FileStream(this.m_sLogFileName, FileMode.OpenOrCreate))
                    {
                        this.m_objInnerStream.Seek(0L, SeekOrigin.End);
                        this.Init(this.m_objInnerStream);
                        this.m_objStream.Write(bytes, 0, bytes.Length);
                        this.m_objStream.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// 空行写入流
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            lock (m_sLocker)
            {
                if (!this.m_bTimelyClose)
                {
                    this.m_IsInnerCallWrite = true;
                    this.Write(message);
                    this.m_objStream.WriteByte(13);
                    this.m_objStream.WriteByte(10);
                    this.m_objStream.Flush();
                    this.m_IsInnerCallWrite = false;
                }
                else
                {
                    using (this.m_objInnerStream = new FileStream(this.m_sLogFileName, FileMode.OpenOrCreate))
                    {
                        this.m_objInnerStream.Seek(0L, SeekOrigin.End);
                        this.Init(this.m_objInnerStream);
                        this.m_bTimelyClose = false;
                        this.m_IsInnerCallWrite = true;
                        this.Write(message);
                        this.m_objStream.WriteByte(13);
                        this.m_objStream.WriteByte(10);
                        this.m_objStream.Flush();
                        this.m_IsInnerCallWrite = false;
                    }
                    this.m_bTimelyClose = true;
                }
            }
        }
        public override bool IsThreadSafe
        {
            get
            {
                return true;
            }
        }
    }
}
