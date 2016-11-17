using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Xml;
using BLL;
using System.Configuration;
using System.Collections;
namespace WxPayAPI
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {
        public WxPayConfig() { }
        public WxPayConfig(int paymentid)
        {
            wx = (tbl_wx_config)tbl_agent.GetPayAcc(paymentid, (int)Order.PayType.WeiXin)[0];//21000代表微信权限编号
            APPID = wx.W_APPID;
            MCHID = wx.W_MCHID;
            SUB_MCHID = wx.W_SUB_MCHID;
            KEY = wx.W_KEY;
            APPSECRET = wx.W_APPSECRET;
            SSLCERT_PATH = wx.W_SSLCERT_PATH;
            SSLCERT_PASSWORD = wx.W_SSLCERT_PASSWORD;
            TICKET = wx.W_TICKET;
            is_entpay = wx.is_entpay;
            payment_id = wx.payment_id;
            Host = wx.Host;
            if (!string.IsNullOrEmpty(wx.W_ACCESS_TOKEN))
            {
                ACCESS_TOKEN = wx.W_ACCESS_TOKEN;
                ACCESS_TOKEN_TIME = DateTime.Parse(wx.W_EXPIRES_IN.ToString());
            }
        }
        public WxPayConfig(string mch_id)
        {
            wx = (tbl_wx_config)tbl_agent.PayAcc(int.Parse(mch_id), (int)Order.PayType.WeiXin, ref arrlist)[0];//21000代表微信权限编号
            APPID = wx.W_APPID;
            MCHID = wx.W_MCHID;
			SUB_MCHID = wx.W_SUB_MCHID;
            KEY = wx.W_KEY;
            APPSECRET = wx.W_APPSECRET;
            SSLCERT_PATH = wx.W_SSLCERT_PATH;
            SSLCERT_PASSWORD = wx.W_SSLCERT_PASSWORD;
            TICKET = wx.W_TICKET;
            is_entpay = wx.is_entpay;
            payment_id = wx.payment_id;
            Host = wx.Host;
            if(!string.IsNullOrEmpty(wx.W_ACCESS_TOKEN))
            {
                ACCESS_TOKEN = wx.W_ACCESS_TOKEN;
                ACCESS_TOKEN_TIME =DateTime.Parse(wx.W_EXPIRES_IN.ToString());
            }
            //XmlDocument xd = new XmlDocument();
            //xd.Load(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "alipay/WeiXin/wx_accounts.xml"));
            //XmlNode xn = null;
            ////xd.SelectNodes("").
            //xn = xd.DocumentElement.SelectSingleNode("//ACCOUNT[@id='" + mch_id + "']");
            //if (xn == null) xn = xd.GetElementsByTagName("DEFUALT")[0];
            //APPID = xn["APPID"].InnerText;
            //MCHID = xn["MCHID"].InnerText;
            //KEY = xn["KEY"].InnerText;
            //APPSECRET = xn["APPSECRET"].InnerText;
            //SSLCERT_PATH = xn["SSLCERT_PATH"].InnerText;
            //SSLCERT_PASSWORD = xn["SSLCERT_PASSWORD"].InnerText;
        }
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public int is_entpay;
        public int payment_id;
        public string APPID;
        public string MCHID;
        public string SUB_MCHID;
		public string KEY;
        public string APPSECRET;
        public string Host;
        public Dictionary<int, tbl_wx_config_gz> arrlist = new Dictionary<int,tbl_wx_config_gz>();

        public string TICKET;
        public string ACCESS_TOKEN;//全局access_token
        public DateTime ACCESS_TOKEN_TIME=DateTime.Now.AddHours(-3);//全局access_token更新时间

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public string SSLCERT_PATH ;
        public string SSLCERT_PASSWORD;



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public static string NOTIFY_URL =ConfigurationManager.AppSettings["host"].ToString()+"alipay/WeiXin/ResultNotifyPage.aspx";//
        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "120.24.65.238";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        //public const string PROXY_URL = "http://127.0.0.1";
        //public const string PROXY_URL = "";
        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 2;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 0;

        public tbl_wx_config wx = null;
        
        /// <summary>
        /// 返回清算对象
        /// </summary>
        /// <returns></returns>
        public tbl_wx_config GetSettlePayment()
        {
            if (wx != null)
            {
                return wx.GetSettlePayment();
            }
            return null;
        }
    }
}

