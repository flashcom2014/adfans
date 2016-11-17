using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxPayAPI
{
    /// <summary>
    /// 微信被动回复接口类
    /// </summary>
    public static class WxReplyData
    {
        public static string ReplyTransferCustomerService(string Original_id, string Openid)
        {
            string rexml = "<xml><ToUserName><![CDATA[" + Openid + "]]></ToUserName>";
            rexml += "<FromUserName><![CDATA[" + Original_id + "]]></FromUserName>";
            rexml += "<CreateTime>" + GetTimeInt().ToString() + "</CreateTime>";
            rexml += "<MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>";

            return rexml;
        }

        public static string ReplyTransferCustomerService(string Original_id, string Openid, string KfAccount)
        {
            string rexml = "<xml><ToUserName><![CDATA[" + Openid + "]]></ToUserName>";
            rexml += "<FromUserName><![CDATA[" + Original_id + "]]></FromUserName>";
            rexml += "<CreateTime>" + GetTimeInt().ToString() + "</CreateTime>";
            rexml += "<MsgType><![CDATA[transfer_customer_service]]></MsgType>";
            rexml += "<TransInfo><KfAccount><![CDATA[" + KfAccount + "]]></KfAccount></TransInfo></xml>";
            return rexml;
        }

        public static string ReplyText(string Original_id, string Openid, string Content)
        {
            string rexml = "<xml><ToUserName><![CDATA[" + Openid + "]]></ToUserName>";
            rexml += "<FromUserName><![CDATA[" + Original_id + "]]></FromUserName>";
            rexml += "<CreateTime>" + GetTimeInt().ToString() + "</CreateTime>";
            rexml += "<MsgType><![CDATA[text]]></MsgType>";
            rexml += "<Content><![CDATA[" + Content + "]]></Content></xml>";
            return rexml;
        }

        public static string ReplyImage(string Original_id, string Openid, string MediaId)
        {
            string rexml = "<xml><ToUserName><![CDATA[" + Openid + "]]></ToUserName>";
            rexml += "<FromUserName><![CDATA[" + Original_id + "]]></FromUserName>";
            rexml += "<CreateTime>" + GetTimeInt().ToString() + "</CreateTime>";
            rexml += "<MsgType><![CDATA[image]]></MsgType>";
            rexml += "<Image><MediaId><![CDATA[" + MediaId + "]]></MediaId></Image></xml>";
            return rexml;
        }

        public static string ReplyVoice(string Original_id, string Openid, string MediaId)
        {
            string rexml = "<xml><ToUserName><![CDATA[" + Openid + "]]></ToUserName>";
            rexml += "<FromUserName><![CDATA[" + Original_id + "]]></FromUserName>";
            rexml += "<CreateTime>" + GetTimeInt().ToString() + "</CreateTime>";
            rexml += "<MsgType><![CDATA[voice]]></MsgType>";
            rexml += "<Voice><MediaId><![CDATA[" + MediaId + "]]></MediaId></Voice></xml>";
            return rexml;
        }

        public static string ReplyVideo(string Original_id, string Openid, string MediaId, string Title, string Description)
        {
            string rexml = "<xml><ToUserName><![CDATA[" + Openid + "]]></ToUserName>";
            rexml += "<FromUserName><![CDATA[" + Original_id + "]]></FromUserName>";
            rexml += "<CreateTime>" + GetTimeInt().ToString() + "</CreateTime>";
            rexml += "<MsgType><![CDATA[video]]></MsgType>";
            rexml += "<Video><MediaId><![CDATA[" + MediaId + "]]></MediaId>";
            rexml += "<Title><![CDATA[" + Title + "]]></Title>";
            rexml += "<Description><![CDATA[" + Description + "]]></Description></Video></xml>";
            return rexml;
        }

        public static string ReplyMusic(string Original_id, string Openid, string Title, string Description, string MusicUrl, string HQMusicUrl, string ThumbMediaId)
        {
            string rexml = "<xml><ToUserName><![CDATA[" + Openid + "]]></ToUserName>";
            rexml += "<FromUserName><![CDATA[" + Original_id + "]]></FromUserName>";
            rexml += "<CreateTime>" + GetTimeInt().ToString() + "</CreateTime>";
            rexml += "<MsgType><![CDATA[music]]></MsgType>";
            rexml += "<Music><Title><![CDATA[" + Title + "]]></Title>";
            rexml += "<Description><![CDATA[" + Description + "]]></Description>";
            rexml += "<MusicUrl><![CDATA[" + MusicUrl + "]]></MusicUrl>";
            rexml += "<HQMusicUrl><![CDATA[" + HQMusicUrl + "]]></HQMusicUrl>";
            rexml += "<ThumbMediaId><![CDATA[" + ThumbMediaId + "]]></ThumbMediaId></Music></xml>";
            return rexml;
        }

        public static string ReplyNews(string Original_id, string Openid, NewsItem[] Items)
        {
            string rexml = "<xml><ToUserName><![CDATA[" + Openid + "]]></ToUserName>";
            rexml += "<FromUserName><![CDATA[" + Original_id + "]]></FromUserName>";
            rexml += "<CreateTime>" + GetTimeInt().ToString() + "</CreateTime>";
            rexml += "<MsgType><![CDATA[news]]></MsgType>";
            rexml += "<ArticleCount>" + Items.Length.ToString() + "</ArticleCount><Articles>";
            foreach (NewsItem item in Items)
            {
                rexml += "<item><Title><![CDATA[" + item.Title + "]]></Title>";
                rexml += "<Description><![CDATA[" + item.Description + "]]></Description>";
                rexml += "<PicUrl><![CDATA[" + item.PicUrl + "]]></PicUrl>";
                rexml += "<Url><![CDATA[" + item.Url + "]]></Url></item>";
            }
            rexml += "</Articles></xml>";
            return rexml;
        }

        private static int GetTimeInt()
        {
            DateTime now = DateTime.Now.ToUniversalTime();
            DateTime d2 = new DateTime(1970, 1, 1);
            TimeSpan ts = now - d2;
            return (int)ts.TotalSeconds;
        }
    }

    public class NewsItem
    {
        public string Title = "";
        public string Description = "";
        public string PicUrl = "";
        public string Url = "";

        public NewsItem(string title, string desc, string picurl, string url)
        {
            this.Title = title;
            this.Description = desc;
            this.PicUrl = picurl;
            this.Url = url;
        }
    }
}