using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// MyLog 的摘要说明
/// </summary>
public class MyLog
{
    public MyLog()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    private static object Sms = new object();
    public static void SMS(string pLogText)
    {
        writelog(pLogText, "error/sms/", Sms);
    }

    private static object Payout = new object();
    public static void api_Payout(string pLogText)
    {
        writelog(pLogText, "error/api/Payout/", Payout);
    }
    private static object aWrite = new object();
    public static void api_Write(string pLogText)
    {
        writelog(pLogText, "error/api/Write/", aWrite);
    }
    private static object YunYing = new object();
    public static void thread_YunYing(string pLogText)
    {
        writelog(pLogText, "error/thread/YunYing/", YunYing);
    }
    private static object SheBei = new object();
    public static void thread_SheBei(string pLogText)
    {
        writelog(pLogText, "error/thread/SheBei/", SheBei);
    }
    private static object WxTk = new object();
    public static void thread_WxTk(string pLogText)
    {
        writelog(pLogText, "error/thread/WxTk/", WxTk);
    }

    private static object WxEpay = new object();
    public static void WxEnterprisePay(string pLogText)
    {
        writelog(pLogText, "error/wx/EnterprisePay/", WxEpay);
    }
    public static void writelog(string pLogText, string path, object obj)
    {
        lock (obj)
        {
            FileStream pLogFile = null;
            StreamWriter pLogWriter = null;
            string pPath = AppDomain.CurrentDomain.BaseDirectory + path;
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