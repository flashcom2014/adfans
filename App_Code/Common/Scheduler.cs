using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.IO;
using System.Text;
using Common;
using System.Configuration;
using DBUtility;
using System.Globalization;
using System.Data;
using LitJson;
using System.Collections;

namespace Common
{

	public static class Scheduler
    {
        
		
		
        
        #region
        private static object o = new object();
        /// <summary>
        /// 保存出错日志
        /// </summary>
        /// <param name="pLogText"></param>
        public static void SaveEventLog(string pLogText)
        {
            lock (o)
            {
                FileStream pLogFile = null;
                StreamWriter pLogWriter = null;
                string pPath = AppDomain.CurrentDomain.BaseDirectory + "/Sys/EventLog/";
                if (!Directory.Exists(pPath))
                    Directory.CreateDirectory(pPath);

                pLogFile = new FileStream(pPath + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
                pLogWriter = new StreamWriter(pLogFile, Encoding.GetEncoding("UTF-8"));
                pLogWriter.WriteLine(DateTime.Now.ToString() + ":" + pLogText + "\r\n");
                pLogWriter.Close();
                pLogFile.Close();
            }
        }
        /// <summary>
        /// 保存出错异常日志
        /// </summary>
        /// <param name="pLogText"></param>
        public static void SaveExLog(string pLogText)
        {
            lock (o)
            {
                FileStream pLogFile = null;
                StreamWriter pLogWriter = null;
                string pPath = AppDomain.CurrentDomain.BaseDirectory + "/error/ex/";
                if (!Directory.Exists(pPath))
                    Directory.CreateDirectory(pPath);

                pLogFile = new FileStream(pPath + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
                pLogWriter = new StreamWriter(pLogFile, Encoding.GetEncoding("UTF-8"));
                pLogWriter.WriteLine(DateTime.Now.ToString() + ":" + pLogText + "\r\n");
                pLogWriter.Close();
                pLogFile.Close();
            }
        }
        /// <summary>
        /// 保存Socket出错日志
        /// </summary>
        /// <param name="pLogText"></param>
        public static void SaveSocketExLog(string pLogText)
        {
            lock (o)
            {
                FileStream pLogFile = null;
                StreamWriter pLogWriter = null;
                string pPath = AppDomain.CurrentDomain.BaseDirectory + "/error/socket/";
                if (!Directory.Exists(pPath))
                    Directory.CreateDirectory(pPath);

                pLogFile = new FileStream(pPath + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
                pLogWriter = new StreamWriter(pLogFile, Encoding.GetEncoding("UTF-8"));
                pLogWriter.WriteLine(DateTime.Now.ToString() + ":" + pLogText + "\r\n");
                pLogWriter.Close();
                pLogFile.Close();
            }
        }
        /// <summary>
        /// 保存Socket出错日志
        /// </summary>
        /// <param name="pLogText"></param>
        public static void SaveServerSocketExLog(string pLogText)
        {
            lock (o)
            {
                FileStream pLogFile = null;
                StreamWriter pLogWriter = null;
                string pPath = AppDomain.CurrentDomain.BaseDirectory + "/error/serversocket/";
                if (!Directory.Exists(pPath))
                    Directory.CreateDirectory(pPath);

                pLogFile = new FileStream(pPath + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
                pLogWriter = new StreamWriter(pLogFile, Encoding.GetEncoding("UTF-8"));
                pLogWriter.WriteLine(DateTime.Now.ToString() + ":" + pLogText + "\r\n");
                pLogWriter.Close();
                pLogFile.Close();
            }
        }

    }
        #endregion
}