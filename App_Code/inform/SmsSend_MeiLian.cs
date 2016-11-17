using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inform
{
    /// <summary>
    /// SmsSend_MeiLian 的摘要说明
    /// </summary>
    public class SmsSend_MeiLian :SmsSend,ISmsSend
    {
        public SmsSend_MeiLian()
        {
            
        }

        public override int GetBalance(ref string restr)
        {
            try
            {
                string url = string.Format("http://m.5c.com.cn/api/query/index.php?username={0}&password_md5={1}&apikey={2}", username, Tools.MD5(password).ToLower(), apikey);
                restr = HttpGet(url);
                if (restr.StartsWith("error:"))
                {
                    string errmsg = string.Format("美联短信接口获取余额错误：{0}", restr);
                    MyLog.SMS(errmsg);
                    return -1;
                }
                int n = restr.IndexOf("/");
                if (n > 0)
                {
                    string bs = restr.Remove(n);
                    int balance = int.Parse(bs);
                    return balance;
                }
            }
            catch (Exception ex)
            {
                MyLog.SMS(ex.ToString());
            }
            return -99;
        }

        public bool SendSms(string mobile, string code, string ip, ref string restr)
        {
            try
            {
                if (!isloaded)
                    return false;
                string url = "https://m.5c.com.cn/api/send/index.php";
                string content = GetContent(code);
                string postdata = string.Format("username={0}&password_md5={1}&apikey={2}&mobile={3}&content={4}&encode=utf8", username, Tools.MD5(password).ToLower(), apikey, mobile, content);

                //restr = "success:98765";
                restr = HttpPost(url, postdata);
                if (restr.StartsWith("success:"))
                {
                    string smsgid = restr.Substring(8);
                    long msgid = long.Parse(smsgid);

                    if (SaveCode(mobile, code, msgid, ip))
                        return true;

                    return false;
                }
                string errmsg = string.Format("美联短信接口发送短信错误：{0}", restr);
                MyLog.SMS(errmsg);
            }
            catch (Exception ex)
            {
                MyLog.SMS(ex.ToString());
            }
            return false;
        }

        public bool SendSms(int mchid, int agentid, int channel, float amount, string mobile, string content, ref string restr)
        {
            try
            {
                if (!isloaded)
                    return false;
                string url = "https://m.5c.com.cn/api/send/index.php";
                string msg = GetContent(content);
                string postdata = string.Format("username={0}&password_md5={1}&apikey={2}&mobile={3}&content={4}&encode=utf8", username, Tools.MD5(password).ToLower(), apikey, mobile, msg);

                //restr = "success:98765";
                restr = HttpPost(url, postdata);
                if (restr.StartsWith("success:"))
                {
                    string smsgid = restr.Substring(8);
                    long msgid = long.Parse(smsgid);

                    msg = HttpUtility.UrlDecode(msg, System.Text.Encoding.UTF8);

                    if (SaveLog(mchid, agentid, channel, amount, mobile, msg, msgid))
                        return true;

                    return false;
                }
                string errmsg = string.Format("美联短信接口发送短信错误：{0}", restr);
                MyLog.SMS(errmsg);
            }
            catch (Exception ex)
            {
                MyLog.SMS(ex.ToString());
            }
            return false;
        }
    }
}