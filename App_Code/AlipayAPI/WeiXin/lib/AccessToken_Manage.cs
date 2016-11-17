using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Data;
using DBUtility;
using LitJson;
using System.Threading;
using MySql.Data.MySqlClient;

namespace WxPayAPI
{
    /// <summary>
    /// 微信AccessToken管理类
    /// </summary>
    [Serializable]
    public static class AccessToken_Manage
    {
        private static Dictionary<string, WX_config> conlist = new Dictionary<string, WX_config>();

        public static WX_config GetWxConfig(string appid)
        {
            if (conlist.ContainsKey(appid))
                return conlist[appid];
            return null;
        }

        public static WX_config GetWxConfig(string appid, string appsecret)
        {
            if (conlist.ContainsKey(appid))
                return conlist[appid];

            return AddWxConfig(appid, appsecret);
        }

        public static WX_config GetWxConfig(BLL.tbl_wx_config config)
        {
            string appid = config.W_APPID;
            if (conlist.ContainsKey(appid))
                return conlist[appid];

            WX_config wx = new WX_config(appid, config.W_APPSECRET);
            conlist[appid] = wx;
            return wx;
        }

        public static WX_config GetWxConfig(WxPayConfig config)
        {
            string appid = config.APPID;
            if (conlist.ContainsKey(appid))
                return conlist[appid];

            WX_config wx = new WX_config(appid, config.APPSECRET);
            conlist[appid] = wx;
            return wx;
        }

        public static WX_config AddWxConfig(string appid, string appsecret)
        {
            WX_config wx = new WX_config(appid, appsecret);
            conlist[appid] = wx;
            return wx;
        }
    }

    [Serializable]
    public class WX_config
    {
        private static int myid = Guid.NewGuid().GetHashCode();    //随机生成一个ID

        private string appid = "";
        private string appsecret = "";
        private string accesstoken = "";
        private DateTime expirestime;

        private DateTime refreshtime;

        WebClient web = new WebClient();
        Random ra = new Random(Guid.NewGuid().GetHashCode());
        Mutex mutex = new Mutex();
        int minm = 60;  //刷新提前最小秒数
        int maxm = 600; //刷新提前最大秒数
        

        public WX_config(string appid, string appsecret)
        {
            this.appid = appid;
            this.appsecret = appsecret;
            
            //

            web.Encoding = System.Text.Encoding.UTF8;

            //刷新前都会检查一遍数据库
            RefreshAccessToken();
        }

        public string AccessToken
        {
            get
            {
                mutex.WaitOne(3000);
                string token = accesstoken;
                mutex.ReleaseMutex();
                return token;
            }
        }

        public string APPID
        {
            get
            {
                return appid;
            }
        }

        public DateTime ExpiresTime
        {
            get
            {
                return expirestime;
            }
        }

        public DateTime RefershTime
        {
            get
            {
                return refreshtime;
            }
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        public JsonData GetUserInfo(string openid)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token=ACCESS_TOKEN&openid={0}&lang=zh_CN", openid);
            return GetJson(url);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        public WxUserInfo GetUserInfoEx(string openid)
        {
            JsonData json = GetUserInfo(openid);
            if (json != null)
                return JsonMapper.ToObject<WxUserInfo>(json.ToJson());
            return null;
        }

        /// <summary>
        /// 创建临时关注二维码
        /// </summary>
        /// <param name="expire_seconds">过期时间（秒）</param>
        /// <param name="scene_id">对应ID</param>
        /// <returns></returns>
        public JsonData CreateQrcode(int expire_seconds, int scene_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=ACCESS_TOKEN";
            string postdata = "{\"expire_seconds\": " + expire_seconds.ToString() + ", \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id.ToString() + "}}}";
            return GetJson(url, postdata);
        }

        /// <summary>
        /// 创建永久关注二维码
        /// </summary>
        /// <param name="scene_id">对应ID</param>
        /// <returns></returns>
        public JsonData CreateLimitQrcode(int scene_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=ACCESS_TOKEN";
            string postdata = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id.ToString() + "}}}";
            return GetJson(url, postdata);
        }

        /// <summary>
        /// 创建永久关注二维码
        /// </summary>
        /// <param name="scene_str">对应编码</param>
        /// <returns></returns>
        public JsonData CreateLimitQrcode(string scene_str)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=ACCESS_TOKEN";
            string postdata = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"" + scene_str + "\"}}}";
            return GetJson(url, postdata);
        }

        /// <summary>
        /// 长链接转短链接
        /// </summary>
        /// <param name="longurl"></param>
        /// <returns></returns>
        public JsonData ShortUrl(string longurl)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/shorturl?access_token=ACCESS_TOKEN";
            string postdata = "{\"action\":\"long2short\",\"long_url\":\"" + longurl + "\"}";
            return GetJson(url, postdata);
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="name">标签名</param>
        /// <returns></returns>
        public JsonData CreateTags(string name)
        {
            if (name.Length > 30)
                throw new Exception("标签字符不能超过30个字符");
            string url = "https://api.weixin.qq.com/cgi-bin/tags/create?access_token=ACCESS_TOKEN";
            string postdata = "{  \"tag\" : {   \"name\" : \"" + name + "\"  }}";
            return GetJson(url, postdata);
        }

        /// <summary>
        /// 获取已创建的标签列表
        /// </summary>
        /// <returns></returns>
        public JsonData GetTags()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/tags/get?access_token=ACCESS_TOKEN";
            return GetJson(url);
        }

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="id">标签ID</param>
        /// <param name="name">新标签名</param>
        /// <returns></returns>
        public JsonData EditTags(int id, string name)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/tags/update?access_token=ACCESS_TOKEN";
            string postdata = "{  \"tag\" : {    \"id\" : " + id.ToString() + ",    \"name\" : \"" + name + "\"  }}";
            return GetJson(url, postdata);
        }

        #region 客服消息

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="context">文本内容</param>
        /// <returns></returns>
        public JsonData SendText(string openid, string context)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
            string postdata = "{\"touser\":\"" + openid + "\", \"msgtype\":\"text\", \"text\": {\"content\":\"" + context + "\"}}";
            return GetJson(url, postdata);
        }

        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="media_id">上传图片的media_id</param>
        /// <returns></returns>
        public JsonData SendImages(string openid, string media_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
            string postdata = "{\"touser\":\"" + openid + "\",\"msgtype\":\"image\",\"image\":{\"media_id\":\"" + media_id + "\"}}";
            return GetJson(url, postdata);
        }

        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="media_id">上传语音的media_id</param>
        /// <returns></returns>
        public JsonData SendVoice(string openid, string media_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
            string postdata = "{\"touser\":\"" + openid + "\",\"msgtype\":\"voice\",\"voice\":{\"media_id\":\"" + media_id + "\"}}";
            return GetJson(url, postdata);
        }

        /// <summary>
        /// 发送音乐消息
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="title">音乐消息的标题</param>
        /// <param name="description">音乐消息的描述</param>
        /// <param name="musicurl">音乐链接</param>
        /// <param name="hqmusicurl">高品质音乐链接，wifi环境优先使用该链接播放音乐</param>
        /// <param name="thumb_media_id">缩略图的媒体ID</param>
        /// <returns></returns>
        public JsonData SendMusic(string openid, string title, string description, string musicurl, string hqmusicurl, string thumb_media_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
            string postdata = "{\"touser\":\"" + openid + "\",\"msgtype\":\"music\",\"music\":{\"title\":\"" + title + "\",\"description\":\"" + description + "\",\"musicurl\":\"" + musicurl + "\",\"hqmusicurl\":\"" + hqmusicurl + "\",\"thumb_media_id\":\"" + thumb_media_id + "\" }}";
            return GetJson(url, postdata);
        }

        /// <summary>
        /// 发送图文消息（点击跳转到外链） 图文消息条数限制在8条以内
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="list">消息列表</param>
        /// <returns></returns>
        public JsonData SendNews(string openid, List<wxnews> list)
        {
            if (list.Count > 8)
                throw new Exception("消息内容不能超过8条");
            if (list.Count == 0)
                throw new Exception("消息内容不能为空");
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
            string postdata = "{\"touser\":\"" + openid + "\",\"msgtype\":\"news\",\"news\":{\"articles\": [";
            foreach (wxnews news in list)
                postdata += news.ToString() + ",";
            postdata = postdata.Remove(postdata.Length - 1);    //删除最后一个逗号
            postdata += "]}}";
            return GetJson(url, postdata);
        }

        #endregion



        /// <summary>
        /// 根据URL获取数据，其中ACCESS_TOKEN会被替换为实际ACCESS_TOKEN
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public JsonData GetJson(string url)
        {
            CheckRefresh();

            int errcount = 0;
            ag:
            string theurl = url.Replace("ACCESS_TOKEN", AccessToken);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string json = client.DownloadString(theurl);
            client.Dispose();
            client = null;
            JsonData jd = JsonMapper.ToObject(json);
            if (CheckAccessTokenOverDue(jd))
            {
                errcount++;
                if (errcount >= 2)
                    return null;
                if (RefreshAccessToken())
                {
                    goto ag;
                }
                else
                {
                    return null;
                }
            }
            return jd;
        }

        /// <summary>
        /// 根据URL地址POST获取数据，其中ACCESS_TOKEN会被替换为实际ACCESS_TOKEN
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public JsonData GetJson(string url, string postdata)
        {
            CheckRefresh();

            int errcount = 0;
        ag:
            string theurl = url.Replace("ACCESS_TOKEN", AccessToken);
            byte[] pdata = System.Text.Encoding.UTF8.GetBytes(postdata);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string json = client.UploadString(theurl, postdata);
            client.Dispose();
            client = null;
            JsonData jd = JsonMapper.ToObject(json);
            if (CheckAccessTokenOverDue(jd))
            {
                errcount++;
                if (errcount >= 2)
                    return null;
                if (RefreshAccessToken())
                {
                    goto ag;
                }
                else
                {
                    return null;
                }
            }
            return jd;
        }

        private bool CheckAccessTokenOverDue(JsonData jd)
        {
            if (JsonMapper.IsKey(jd, "errcode"))
            {
                int errcode = (int)jd["errcode"];
                switch(errcode)
                {
                    case 40014:
                    case 40001:
                    case 42001:
                    case 42007:
                        refreshtime = DateTime.Now.AddSeconds(-1);  //把刷新时间设置为过时，可以立即刷新
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查是否需要刷新ACCESS_TOKEN，需要就刷新
        /// </summary>
        private void CheckRefresh()
        {
            if (DateTime.Now >= refreshtime)
                RefreshAccessToken();
        }

        /// <summary>
        /// 获取新的ACCESS_TOKEN
        /// </summary>
        /// <returns></returns>
        private bool RefreshAccessToken()
        {
            mutex.WaitOne(3000);
            if (DateTime.Now < refreshtime || GetAKFData())
            {
                mutex.ReleaseMutex();
                return true;
            }
            try
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, appsecret);
                string result = web.DownloadString(url);
                JsonData jd = JsonMapper.ToObject(result);
                if (JsonMapper.IsKey(jd, "access_token"))
                {
                    accesstoken = jd["access_token"].ToString();
                    int expires_in = (int)jd["expires_in"];
                    DateTime now = DateTime.Now;
                    expirestime = now.AddSeconds(expires_in);
                    int sec = ra.Next(minm, maxm); //提前1到10分钟刷新
                    refreshtime = expirestime.AddSeconds(0 - sec);
                    //写入数据库
                    SetAKFData();
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                mutex.ReleaseMutex();
            }
            return false;

        }

        private void SetAKFData()
        {
            string sqlstr = "call SetAccessToken(@appid,@accesstoken,@webid,@expires,@rowcount);";
            MySqlParameter mappid = new MySqlParameter("@appid", MySqlDbType.VarChar, 100);
            mappid.Value = appid;
            MySqlParameter mtoken = new MySqlParameter("@accesstoken", MySqlDbType.VarChar, 250);
            mtoken.Value = accesstoken;
            MySqlParameter mwebid = new MySqlParameter("@webid", MySqlDbType.Int32);
            mwebid.Value = myid;
            MySqlParameter mexpires = new MySqlParameter("@expires", MySqlDbType.DateTime);
            mexpires.Value = expirestime;
            MySqlParameter mcount = new MySqlParameter("@rowcount", MySqlDbType.Int32);
            mcount.Value = 10;

            int result = DbHelperMySQL.ExecuteSql(sqlstr, mappid, mtoken, mwebid, mexpires, mcount);
        }

        /// <summary>
        /// 从数据库中取得ACCESS_TOKEN
        /// </summary>
        /// <returns>需要刷新ACCESS_TOKEN返回true,</returns>
        private bool GetAKFData()
        {
            string sqlstr = string.Format("select * from tbl_wx_accesstoken where wx_appid='{0}' and wx_expires>'{1}' order by wx_expires desc limit 1", appid, DateTime.Now.ToString());
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DateTime exptime = (DateTime)dt.Rows[0]["wx_expires"];
                int webid = (int)dt.Rows[0]["web_id"];
                if (webid != myid && exptime > expirestime)
                {
                    expirestime = exptime;
                    accesstoken = dt.Rows[0]["wx_accesstoken"].ToString();
                    //生成随机刷新时间
                    int sec = ra.Next(minm, maxm); //提前1到10分钟刷新
                    refreshtime = expirestime.AddSeconds(0 - sec);
                }
                if (DateTime.Now >= refreshtime)
                    return false;
                
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查ACCESS_TOKEN是否有效
        /// </summary>
        /// <returns></returns>
        public bool CheckAccessToken()
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token={0}", AccessToken);
            string result = web.DownloadString(url);
            JsonData jd = JsonMapper.ToObject(result);
            if (JsonMapper.IsKey(jd, "ip_list"))
            {
                
                return true;
            }
            return false;
        }
    }

    public class wxnews
    {
        public string title = "";
        public string description = "";
        public string url = "";
        public string picurl = "";

        public wxnews(string title, string desc, string url, string picurl)
        {
            this.title = title;
            this.description = desc;
            this.url = url;
            this.picurl = picurl;
        }

        public override string ToString()
        {
            string restr = "{\"title\":\"" + title + "\",\"description\":\"" + description + "\",\"url\":\"" + url + "\",\"picurl\":\"" + picurl + "\"}";
            return restr;
        }
    }

    public class WxUserInfo
    {
        public int subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public int subscribe_time { get; set; }
        public string unionid { get; set; }
        public string remark { get; set; }
        public int groupid { get; set; }
        public int[] tagid_list { get; set; }

        /// <summary>
        /// 获取关注时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetSubscribeTime()
        {
            DateTime bdate = new DateTime(1970, 1, 1);
            return bdate.AddSeconds(subscribe_time).ToLocalTime();
        }

        /// <summary>
        /// 是否已关注
        /// </summary>
        /// <returns></returns>
        public bool IsSubscribe()
        {
            if (subscribe == 1 && nickname != null)
                return true;
            return false;
        }
    }
}