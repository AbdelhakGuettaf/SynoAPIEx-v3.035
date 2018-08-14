using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Library
{
    //  
    //  非原创
    //
    public class LogHelper
    {
        #region 记录日志
        private static string s_LogFilePath = AppDomain.CurrentDomain.BaseDirectory;
        private static object m_LockLog = "loglock";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sMessage"></param>
        public static void AsnyWriteLog(string sMessage, string title = "异常信息")
        {
            try
            {
                AsyncWriteLog synThread = new AsyncWriteLog(WriteLog);
                synThread.BeginInvoke(sMessage, title, null, null);
            }
            catch { }

        }
        private delegate void AsyncWriteLog(string sMessage, string titel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sMessage"></param>
        private static void WriteLog(string sMessage, string title = "异常信息")
        {
            lock (m_LockLog)
            {
                string FilePath = s_LogFilePath + "\\Log";
                if (!System.IO.Directory.Exists(FilePath))
                    System.IO.Directory.CreateDirectory(FilePath);
                string sFileName = FilePath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                TextTraceListener objDebugTextListener = new TextTraceListener(sFileName, true, title);
                objDebugTextListener.WriteLine(sMessage);
                objDebugTextListener.Dispose();
                objDebugTextListener = null;
            }
        }
        #endregion
    }
}
