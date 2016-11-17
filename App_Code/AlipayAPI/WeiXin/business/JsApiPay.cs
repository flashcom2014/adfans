using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Security;
using LitJson;
using BLL;
using System.Configuration;

namespace WxPayAPI
{
    public class JsApiPay
    {
        public event EventHandler UserAuthdeny;

        /// <summary>
        /// 保存页面对象，因为要在类的方法中使用Page的Request对象
        /// </summary>
        private Page page { get; set; }

        /// <summary>
        /// openid用于调用统一下单接口
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// access_token用于获取收货地址js函数入口参数
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public string expires_in { get; set; }

        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 全局global_access_token
        /// </summary>
        public string global_access_token { get; set; }
        /// <summary>
        /// 全局jsapi_ticket
        /// </summary>
        public string jsapi_ticket { get; set; }

        // <summary>
        /// 全局global_access_token 超时时间，单位（秒）
        /// </summary>
        public string global_expires_in { get; set; }

        // <summary>
        /// 微信公众号是否被关注
        /// </summary>
        public string subscribe { get; set; }

        /// <summary>
        /// 商品金额，用于统一下单
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string des { get; set; }

        /// <summary>
        /// 统一下单接口返回结果
        /// </summary>
        public WxPayData unifiedOrderResult { get; set; }

        /// <summary>
        /// 设备ID_产品ID
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string order_id { get; set; }

		public JsonData userinfo { get; set; }
        public string nickname { get; set; }
        public string headimgurl { get; set; }

		public JsApiPay(Page page)
        {
            this.page = page;
        }

        public JsApiPay()
        {

        }

        /// <summary>
        /// 自设参数获取用户OpenID
        /// </summary>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">密钥</param>
        /// <param name="querylist">设置的参数</param>
        /// <param name="host">公众绑定域名</param>
        /// <param name="isbase">是否静默获取</param>
        public void GetOpenidAndAccessTokenHost(string appid, string appsecret, string querylist, string host, bool isbase)
        {
            string tquery = page.Request.Url.Query;
            bool hasquery = tquery.IndexOf(querylist) > 0;
            if (querylist == "")
                hasquery = true;
            string code = page.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code) && hasquery)    //是否微信已返回code
            {
                Log.Debug(this.GetType().ToString(), "Get code : " + code);

                string goh = page.Request.QueryString["goh"];
                if (!string.IsNullOrEmpty(goh)) //判断是否要跳转到原先的域名中去
                {
                    string query = HttpUtility.UrlDecode(page.Request.Url.Query);
                    query = query.Replace("?goh=" + goh + "&", "?");
                    query = query.Replace("&goh=" + goh, "");
                    string url = HttpUtility.UrlDecode(goh) + page.Request.Url.AbsolutePath.Substring(1) + query;
                    page.Response.Redirect(url);
                    page.Response.End();
                }
                else
                {
                    if (code == "authdeny") //用户拒绝
                    {
                        openid = "";
                        if (UserAuthdeny != null)
                            UserAuthdeny(this, EventArgs.Empty);
                    }
                    else
                    {
                        GetOpenidAndAccessTokenFromCode(appid, appsecret, code, isbase);
                    }
                }
            }
            else
            {
                string basehost = host;
                string webhost = "";
                if (page.Request.UrlReferrer != null)   //从Referer中获取源地址，可以跳过阿里负载均衡,知道源地址
                {
                    Uri referer = page.Request.UrlReferrer;
                    webhost = referer.Scheme + "://" + referer.Host + "/";
                }
                else
                {
                    string https = basehost.Remove(basehost.IndexOf(":"));  //默认判断hosts的
                    webhost = https + "://" + page.Request.Url.Host + "/";
                }
                //
                string querystr = querylist;
                if (!basehost.Contains(webhost) && querylist.IndexOf("goh=") < 0)
                {
                    if (string.IsNullOrEmpty(querystr))
                        querystr = "?goh=" + HttpUtility.UrlEncode(webhost);
                    else
                        querystr += "&goh=" + HttpUtility.UrlEncode(webhost);
                }
                Log.Debug(this.GetType().ToString(), "GetWxCode：QueryStr->" + querystr);
                GetWxCodeHost(appid, querystr, host, isbase);
            }
        }

        /// <summary>
        /// 自动获取用户OpenID
        /// </summary>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">密钥</param>
        /// <param name="host">公众号绑定的域名</param>
        /// <param name="isbase">是否静默获取</param>
        public void GetOpenidAndAccessTokenHost(string appid, string appsecret, string host, bool isbase)
        {
            string code = page.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))    //是否微信已返回code
            {
                Log.Debug(this.GetType().ToString(), "Get code : " + code);
                string goh = page.Request.QueryString["goh"];
                if (!string.IsNullOrEmpty(goh)) //判断是否要跳转到原先的域名中去
                {
                    string query = HttpUtility.UrlDecode(page.Request.Url.Query);
                    query = query.Replace("?goh=" + goh + "&", "?");
                    query = query.Replace("&goh=" + goh, "");
                    string url = HttpUtility.UrlDecode(goh) + page.Request.Url.AbsolutePath.Substring(1) + query;
                    page.Response.Redirect(url);
                    page.Response.End();
                }
                else
                {
                    if (code == "authdeny") //用户拒绝
                    {
                        openid = "";
                        if (UserAuthdeny != null)
                            UserAuthdeny(this, EventArgs.Empty);
                    }
                    else
                    {
                        GetOpenidAndAccessTokenFromCode(appid, appsecret, code, isbase);
                    }
                }
            }
            else
            {
                string basehost = host;
                string webhost = "";
                if (page.Request.UrlReferrer != null)   //从Referer中获取源地址，可以跳过阿里负载均衡,知道源地址
                {
                    Uri referer = page.Request.UrlReferrer;
                    webhost = referer.Scheme + "://" + referer.Host + "/";
                }
                else
                {
                    string https = basehost.Remove(basehost.IndexOf(":"));  //默认判断hosts的
                    webhost = https + "://" + page.Request.Url.Host + "/";
                }
                //
                string querystr = page.Request.Url.Query;
                if (!basehost.Contains(webhost))
                {
                    if (string.IsNullOrEmpty(querystr))
                        querystr = "?goh=" + HttpUtility.UrlEncode(webhost);
                    else
                        querystr += "&goh=" + HttpUtility.UrlEncode(webhost);
                }
                Log.Debug(this.GetType().ToString(), "GetWxCode：QueryStr->" + querystr);
                GetWxCodeHost(appid, querystr, host, isbase);
            }
        }

        /// <summary>
        /// 通过Code取得OpenID
        /// </summary>
        /// <param name="appid">appid</param>
        /// <param name="querystr">密钥</param>
        /// <param name="host">公众号绑定的域名</param>
        /// <param name="isbase">是否静默获取</param>
        public void GetWxCodeHost(string appid, string querystr, string host, bool isbase)
        {
            string path = page.Request.Url.AbsolutePath;
            string basehost = host;
            //string host = context.Request.Url.Host;

            string reqpage = basehost + path.Substring(1);
            if (!querystr.StartsWith("?") && querystr != "")  //如果参数前面不带问号就加上
                querystr = "?" + querystr;

            string requrl = reqpage + querystr;
            requrl = HttpUtility.UrlEncode(requrl);
            WxPayData data = new WxPayData();
            data.SetValue("appid", appid);
            data.SetValue("redirect_uri", requrl);
            data.SetValue("response_type", "code");
            if (isbase)
                data.SetValue("scope", "snsapi_base");
            else
                data.SetValue("scope", "snsapi_userinfo");//
            data.SetValue("state", "" + HttpUtility.UrlEncode(DateTime.Now.ToString()) + "" + "#wechat_redirect");
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();

            page.Response.Redirect(url);
            page.Response.End();
        }

        /// <summary>
        /// 自设参数获取OpenID，可跨域名
        /// </summary>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">密钥</param>
        /// <param name="querylist">参数(不用用“goh”作为参数名)</param>
        /// <param name="isbase">是否静密获取</param>
        public void GetOpenidAndAccessToken(string appid, string appsecret, string querylist, bool isbase)
        {
            string tquery = page.Request.Url.Query;
            bool hasquery = tquery.IndexOf(querylist) > 0;
            if (querylist == "")
                hasquery = true;
            string code = page.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code) && hasquery)    //是否微信已返回code
            {
                Log.Debug(this.GetType().ToString(), "Get code : " + code);
               
                string goh = page.Request.QueryString["goh"];
                if (!string.IsNullOrEmpty(goh)) //判断是否要跳转到原先的域名中去
                {
                    string query = HttpUtility.UrlDecode(page.Request.Url.Query);
                    query = query.Replace("?goh=" + goh + "&", "?");
                    query = query.Replace("&goh=" + goh, "");
                    string url = HttpUtility.UrlDecode(goh) + page.Request.Url.AbsolutePath.Substring(1) + query;
                    page.Response.Redirect(url);
                    page.Response.End();
                }
                else
                {
                    if (code == "authdeny") //用户拒绝
                    {
                        openid = "";
                        if (UserAuthdeny != null)
                            UserAuthdeny(this, EventArgs.Empty);
                    }
                    else
                    {
                        GetOpenidAndAccessTokenFromCode(appid, appsecret, code, isbase);
                    }
                }
            }
            else
            {
                string basehost = ConfigurationManager.AppSettings["hosts"].ToString();
                string webhost = "";
                if (page.Request.UrlReferrer != null)   //从Referer中获取源地址，可以跳过阿里负载均衡,知道源地址
                {
                    Uri referer = page.Request.UrlReferrer;
                    webhost = referer.Scheme + "://" + referer.Host + "/";
                }
                else
                {
                    string https = basehost.Remove(basehost.IndexOf(":"));  //默认判断hosts的
                    webhost = https + "://" + page.Request.Url.Host + "/";
                }
                //
                string querystr = querylist;
                if (!basehost.Contains(webhost) && querylist.IndexOf("goh=") < 0)
                {
                    if (string.IsNullOrEmpty(querystr))
                        querystr = "?goh=" + HttpUtility.UrlEncode(webhost);
                    else
                        querystr += "&goh=" + HttpUtility.UrlEncode(webhost);
                }
                Log.Debug(this.GetType().ToString(), "GetWxCode：QueryStr->" + querystr);
                GetWxCode(page, appid, querystr, false, isbase);
            }
        }

        /// <summary>
        /// 获取OpenID,能自动跨域名
        /// </summary>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">密钥</param>
        /// <param name="isbase">是否静密获取</param>
        public void GetOpenidAndAccessToken(string appid, string appsecret, bool isbase)
        {
            string code = page.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))    //是否微信已返回code
            {
                Log.Debug(this.GetType().ToString(), "Get code : " + code);
                string goh = page.Request.QueryString["goh"];
                if (!string.IsNullOrEmpty(goh)) //判断是否要跳转到原先的域名中去
                {
                    string query = HttpUtility.UrlDecode(page.Request.Url.Query);
                    query = query.Replace("?goh=" + goh + "&", "?");
                    query = query.Replace("&goh=" + goh, "");
                    string url = HttpUtility.UrlDecode(goh) + page.Request.Url.AbsolutePath.Substring(1) + query;
                    page.Response.Redirect(url);
                    page.Response.End();
                }
                else
                {
                    if (code == "authdeny") //用户拒绝
                    {
                        openid = "";
                        if (UserAuthdeny != null)
                            UserAuthdeny(this, EventArgs.Empty);
                    }
                    else
                    {
                        GetOpenidAndAccessTokenFromCode(appid, appsecret, code, isbase);
                    }
                }
            }
            else
            {
                string basehost = ConfigurationManager.AppSettings["hosts"].ToString() + "/";
                string webhost = "";
                if (page.Request.UrlReferrer != null)   //从Referer中获取源地址，可以跳过阿里负载均衡,知道源地址
                {
                    Uri referer = page.Request.UrlReferrer;
                    webhost = referer.Scheme + "://" + referer.Host + "/";
                }
                else
                {
                    string https = basehost.Remove(basehost.IndexOf(":"));  //默认判断hosts的
                    webhost = https + "://" + page.Request.Url.Host + "/";
                }
                //
                string querystr = page.Request.Url.Query;
                if (!basehost.Contains(webhost))
                {
                    if (string.IsNullOrEmpty(querystr))
                        querystr = "?goh=" + HttpUtility.UrlEncode(webhost);
                    else
                        querystr += "&goh=" + HttpUtility.UrlEncode(webhost);
                }
                Log.Debug(this.GetType().ToString(), "GetWxCode：QueryStr->" + querystr);
                GetWxCode(page, appid, querystr, false, isbase);
            }
        }

        public void GetOpenidAndAccessTokenFromCode(string appid, string appsercret, string code, bool isbase)
        {
            string temp = "";
            try
            {
                //构造获取openid及access_token的url
                WxPayData data = new WxPayData();
                data.SetValue("appid", appid);
                data.SetValue("secret", appsercret);
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();

                //请求url以获取数据
                string result = HttpService.Get(url);

                Log.Debug(this.GetType().ToString(), "GetOpenidAndAccessTokenFromCode response : " + result);

                //保存access_token，用于收货地址获取
                JsonData jd = JsonMapper.ToObject(result);
                if (JsonMapper.IsKey(jd, "errcode"))
                {
                    Common.Scheduler.SaveExLog("ErrCode: " + code);
                    page.Response.Write(result);
                    page.Response.End();
                }
                temp = result;
                access_token = (string)jd["access_token"];
                expires_in = jd["expires_in"].ToString();
                refresh_token = (string)jd["refresh_token"];
                //获取用户openid
                openid = (string)jd["openid"];
                if (!isbase)
                    GetUserInfo();
                Log.Debug(this.GetType().ToString(), "Get openid : " + openid);
                Log.Debug(this.GetType().ToString(), "Get access_token : " + access_token);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                Common.Scheduler.SaveExLog(temp);
                throw new WxPayException(ex.ToString());
            }
        }

        /**
        * 
        * 网页授权获取用户基本信息的全部过程
        * 详情请参看网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * 第一步：利用url跳转获取code
        * 第二步：利用code去获取openid和access_token
        * 
        */
        public void GetOpenidAndAccessToken(WxPayConfig wx)
        {
            if (!string.IsNullOrEmpty(page.Request.QueryString["code"]))
            {
                //获取code码，以获取openid和access_token
                string code = page.Request.QueryString["code"];
                Log.Debug(this.GetType().ToString(), "Get code : " + code);

                GetOpenidAndAccessTokenFromCode(code, wx);
                
                if (page.Request.QueryString["state"] != null)
                    Common.Scheduler.SaveExLog(
                        "新用户发起授权登陆:" + openid + 
                        " 设备名称:" + page.Request.QueryString["device_name"] + 
                        "\r\n请求的时间:" + page.Request.QueryString["state"] +
                        "\r\n请求完的时间:" + DateTime.Now);
            }
            else
            {
				//构造网页授权获取code的URL
				string scheme = page.Request.Url.Scheme;
                string host = page.Request.Url.Host;
                string path = page.Request.RawUrl;
				//string redirect_uri = HttpUtility.UrlEncode("http://" + host + path);
				string redirect_uri = HttpUtility.UrlEncode(scheme + "://" + host + path);
                WxPayData data = new WxPayData();
                data.SetValue("appid", wx.APPID);
                //data.SetValue("redirect_uri", redirect_uri);
                data.SetValue("redirect_uri", GetRedirect(HttpContext.Current, false));
                data.SetValue("response_type", "code");
                data.SetValue("scope", "snsapi_base");
                data.SetValue("state", "" + DateTime.Now + "" + "#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
                Log.Debug(this.GetType().ToString(), "Will Redirect to URL : " + url);
                try
                {
                    //page.Response.Write(url);
                    //page.Response.End();
                    //触发微信返回code码    
                    page.Response.Redirect(url);//Redirect函数会抛出ThreadAbortException异常，不用处理这个异常
                    page.Response.End();
                }
                catch (Exception ex)
                {
                    //if (ex.Message.Contains("正在中止线程"))
                    //{
                    //    return;
                    //}
                }
            }
        }
        //用户手动拉起授权
        public void GetOpenidAndAccessToken0(WxPayConfig wx)
        {
            if (!string.IsNullOrEmpty(page.Request.QueryString["code"]))
            {
                //获取code码，以获取openid和access_token
                string code = page.Request.QueryString["code"];
                Log.Debug(this.GetType().ToString(), "Get code : " + code);
                if (code == "authdeny") //用户拒绝
                {
                    page.Response.Write("<span style='color:#FF0000;font-size:20px'>" + "您已取消授权" + "</span>");
                    page.Response.End();
                }
                else
                {
                    GetOpenidAndAccessTokenFromCode(code, wx);
                }
                if (page.Request.QueryString["state"] != null)
                    Common.Scheduler.SaveExLog(
                        "新用户发起授权登陆:" + openid +
                        " 设备名称:" + page.Request.QueryString["device_name"] +
                        "\r\n请求的时间:" + page.Request.QueryString["state"] +
                        "\r\n请求完的时间:" + DateTime.Now);
            }
            else
            {
                //构造网页授权获取code的URL
                string host = page.Request.Url.Host;
                string path = page.Request.RawUrl;
                string redirect_uri = HttpUtility.UrlEncode("http://" + host + path);
                WxPayData data = new WxPayData();
                data.SetValue("appid", wx.APPID);
                data.SetValue("redirect_uri", GetRedirect(HttpContext.Current, false));
                data.SetValue("response_type", "code");
                data.SetValue("scope", "snsapi_userinfo");//
                data.SetValue("state", ""+DateTime.Now+"" + "#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
                Log.Debug(this.GetType().ToString(), "Will Redirect to URL : " + url);
                try
                {
                    //page.Response.Write(url);
                    //page.Response.End();
                    //触发微信返回code码         
                    page.Response.Redirect(url);//Redirect函数会抛出ThreadAbortException异常，不用处理这个异常
                    page.Response.End();
                }
                catch (Exception ex)
                {
                    //if (ex.Message.Contains("正在中止线程"))
                    //{
                    //    return;
                    //}
                }
            }
        }
        /**
	    * 
	    * 通过code换取网页授权access_token和openid的返回数据，正确时返回的JSON数据包如下：
	    * {
	    *  "access_token":"ACCESS_TOKEN",
	    *  "expires_in":7200,
	    *  "refresh_token":"REFRESH_TOKEN",
	    *  "openid":"OPENID",
	    *  "scope":"SCOPE",
	    *  "unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
	    * }
	    * 其中access_token可用于获取共享收货地址
	    * openid是微信支付jsapi支付接口统一下单时必须的参数
        * 更详细的说明请参考网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * @失败时抛异常WxPayException
	    */
        public void GetOpenidAndAccessTokenFromCode(string code, WxPayConfig wx)
        {
            string temp = "";
            try
            {
                //构造获取openid及access_token的url
                WxPayData data = new WxPayData();
                data.SetValue("appid", wx.APPID);
                data.SetValue("secret", wx.APPSECRET);
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();

                //请求url以获取数据
                string result = HttpService.Get(url);

                Log.Debug(this.GetType().ToString(), "GetOpenidAndAccessTokenFromCode response : " + result);

                //保存access_token，用于收货地址获取
                JsonData jd = JsonMapper.ToObject(result);
                if (JsonMapper.IsKey(jd, "errcode"))
                {
                    if (jd["errcode"].ToString() == "40029")
                    {
                        page.Response.Redirect(page.Request.Path + "?device_name=" + page.Request.QueryString["device_name"]);
                        page.Response.End();
                    }
                    else
                    {
                        page.Response.Write(result);
                        page.Response.End();
                    }
                }
                temp = result;
                access_token = (string)jd["access_token"];
                expires_in = jd["expires_in"].ToString();
                refresh_token = (string)jd["refresh_token"];
                //获取用户openid
                openid = (string)jd["openid"];

                Log.Debug(this.GetType().ToString(), "Get openid : " + openid);
                Log.Debug(this.GetType().ToString(), "Get access_token : " + access_token);
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                Common.Scheduler.SaveExLog(temp);
                throw new WxPayException(ex.ToString());
            }
        }
        public bool GetOpenidRefresh_token(WxPayConfig wx)
        {
            bool Is_auth = true;
            try
            {
                string url = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + wx.APPID + "&grant_type=refresh_token&refresh_token=" + refresh_token + "";
                string result = HttpService.Get(url);

                //保存
                JsonData jd = JsonMapper.ToObject(result);

                if (!JsonMapper.IsKey(jd, "errcode"))
                {
                    access_token = (string)jd["access_token"];
                    expires_in = jd["expires_in"].ToString();
                    refresh_token = (string)jd["refresh_token"];

                    //获取用户openid
                    openid = (string)jd["openid"];
                }
                else
                {
                    Is_auth = false;
                    Common.Scheduler.SaveExLog("用户没有授权或授权已过期Wx:" + url + result);
                }
            }
            catch (Exception ex)
            {
                throw new WxPayException(ex.ToString());
            }
            return Is_auth;
        }

        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public WxPayData GetUnifiedOrderResult(WxPayConfig wx)
        {
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", string.IsNullOrEmpty(des) ? " " : des);
            data.SetValue("attach", order_id);
            data.SetValue("out_trade_no", order_id);
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", product_id);
            data.SetValue("product_id", product_id);
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);

            WxPayData result = WxPayApi.UnifiedOrder(data, wx);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!" + data.ToJson() + result.ToJson());
            }

            unifiedOrderResult = result;
            return result;
        }

        public WxPayData GetUnifiedOrderResult(WxPayConfig wx, string notify_url)
        {
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", string.IsNullOrEmpty(des) ? " " : des);
            data.SetValue("attach", order_id);
            data.SetValue("out_trade_no", order_id);
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", product_id);
            data.SetValue("product_id", product_id);
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);
            if (!string.IsNullOrEmpty(notify_url))
                data.SetValue("notify_url", notify_url);

            WxPayData result = WxPayApi.UnifiedOrder(data, wx);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!" + data.ToJson() + result.ToJson());
            }

            unifiedOrderResult = result;
            return result;
        }


        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        public string GetJsApiParameters(WxPayConfig wx)
        {
            Log.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");

            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign(wx));

            string parameters = jsApiParam.ToJson();

            Log.Debug(this.GetType().ToString(), "Get jsApiParam : " + parameters);
            return parameters;
        }

        public bool GetAccess_token(tbl_wx_config wxconfig)
        {
            string result = "", url = "";
            try
            {
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + wxconfig.W_APPID + "&secret=" + wxconfig.W_APPSECRET + "";
                result = HttpService.Get(url);
                JsonData jd = JsonMapper.ToObject(result);
                global_access_token = (string)jd["access_token"];
                global_expires_in = jd["expires_in"].ToString();
            }
            catch (Exception ex)
            {
                throw new WxPayException(ex.ToString() + url + result);
            }
            return true;
        }
        //通过全局ACCESS_TOKEN得到OPENTID以及用户信息
        public bool GetAccess_token(WxPayConfig wx)
        {
            string result = "", url = "";
            try
            {
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + wx.APPID + "&secret=" + wx.APPSECRET + "";
                result = HttpService.Get(url);
                JsonData jd = JsonMapper.ToObject(result);
                global_access_token = (string)jd["access_token"];
                global_expires_in = jd["expires_in"].ToString();
            }
            catch (Exception ex)
            {
                throw new WxPayException(ex.ToString() + url + result);
            }
            return true;
        }
        //通过全局ACCESS_TOKEN得到OPENTID以及用户信息
        public bool GetUser()
        {
            string result = "", url = "";
            try
            {
                url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + global_access_token + "&openid=" + openid + "&lang=zh_CN";
                result = HttpService.Get(url);
                JsonData jd = JsonMapper.ToObject(result);

               //page.Response.Write(result);
               //page.Response.End();
                if (JsonMapper.IsKey(jd, "errcode"))
                {
                    if (jd["errcode"].ToString().Trim() == "40001")
                    { 
                        return false;
                    }
					return false;
                }
                subscribe = jd["subscribe"].ToString();
				userinfo = jd;
			}
            catch (Exception ex)
            {
                throw new WxPayException(ex.ToString() + url + result);
            }
            return true;
        }
        public void GetUserInfo()
        {
            string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);

            string result = HttpService.Get(url);

            JsonData jd = JsonMapper.ToObject(result);
            if (JsonMapper.IsKey(jd, "errcode"))
                return;
            nickname = jd["nickname"].ToString();
            headimgurl = jd["headimgurl"].ToString();
            //openid = jd["openid"].ToString();
            userinfo = jd;

            Common.Scheduler.SaveEventLog("用户授权登陆: '" + openid + "', 昵称 :'" + nickname + "', 头像:'" + headimgurl + "'");
        }

        /**
	    * 
	    * 获取收货地址js函数入口参数,详情请参考收货地址共享接口：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_9
	    * @return string 共享收货地址js函数需要的参数，json格式可以直接做参数使用
	    */
        public string GetEditAddressParameters(WxPayConfig wx)
        {
            string parameter = "";
            try
            {
                string host = page.Request.Url.Host;
                string path = page.Request.Path;
                string queryString = page.Request.Url.Query;
                //这个地方要注意，参与签名的是网页授权获取用户信息时微信后台回传的完整url
                string url = "http://" + host + path + queryString;

                //构造需要用SHA1算法加密的数据
                WxPayData signData = new WxPayData();
                signData.SetValue("appid", wx.APPID);
                signData.SetValue("url", url);
                signData.SetValue("timestamp", WxPayApi.GenerateTimeStamp());
                signData.SetValue("noncestr", WxPayApi.GenerateNonceStr());
                signData.SetValue("accesstoken", access_token);
                string param = signData.ToUrl();

                Log.Debug(this.GetType().ToString(), "SHA1 encrypt param : " + param);
                //SHA1加密
                string addrSign = FormsAuthentication.HashPasswordForStoringInConfigFile(param, "SHA1");
                Log.Debug(this.GetType().ToString(), "SHA1 encrypt result : " + addrSign);

                //获取收货地址js函数入口参数
                WxPayData afterData = new WxPayData();
                afterData.SetValue("appId", wx.APPID);
                afterData.SetValue("scope", "jsapi_address");
                afterData.SetValue("signType", "sha1");
                afterData.SetValue("addrSign", addrSign);
                afterData.SetValue("timeStamp", signData.GetValue("timestamp"));
                afterData.SetValue("nonceStr", signData.GetValue("noncestr"));

                //转为json格式
                parameter = afterData.ToJson();
                Log.Debug(this.GetType().ToString(), "Get EditAddressParam : " + parameter);
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayException(ex.ToString());
            }

            return parameter;
        }
        public string GetJsApiParameters0(tbl_wx_config wx)
        {
            string path = page.Request.RawUrl.Substring(1);
            string host = ConfigurationManager.AppSettings["hosts"].ToString();


            //string url = host + path;
            string url = host.Remove(host.IndexOf(":")) + "://" + page.Request.Url.Host + "/" + path;

            WxPayData jsApiParam = new WxPayData();
            string timestamp = WxPayApi.GenerateTimeStamp();
            string nonceStr = WxPayApi.GenerateNonceStr();
            jsApiParam.SetValue("appId", wx.W_APPID);
            jsApiParam.SetValue("timestamp", timestamp);
            jsApiParam.SetValue("nonceStr", nonceStr);
            string param = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", wx.W_TICKET, nonceStr, timestamp, url);
            //SHA1加密
            string signature = FormsAuthentication.HashPasswordForStoringInConfigFile(param, "SHA1");
            jsApiParam.SetValue("signature", signature);

            string parameters = jsApiParam.ToJson();
            return parameters;
        }
        public bool GetJsapiTicket()
        {
            string result = "", url = "";
            try
            {
                url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + global_access_token + "&type=jsapi";
                result = HttpService.Get(url);
                JsonData jd = JsonMapper.ToObject(result);
                if (JsonMapper.IsKey(jd, "errmsg") && jd["errmsg"].ToString() == "ok")
                {
                    jsapi_ticket = jd["ticket"].ToString();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new WxPayException("GetJsapiTicket error!" + ex.ToString() + url + result);
            }
            return false;

        }

        public static string GetRedirect(HttpContext context, bool onlyhttp)
        {
            string path = context.Request.RawUrl;
            string keys = onlyhttp ? "host" : "hosts";
            string basehost = ConfigurationManager.AppSettings[keys].ToString();

            string requrl = basehost + path.Substring(1);
            requrl = HttpUtility.UrlEncode(requrl);

            return requrl;
        }

        public static void GetWxCode(HttpContext context, string appid, string querystr, bool onlyhttp)
        {

            GetWxCode(context.Request, context.Response, appid, querystr, onlyhttp, false);
        }

        public static void GetWxCode(Page page, string appid, string querystr, bool onlyhttp)
        {
            GetWxCode(page.Request, page.Response, appid, querystr, onlyhttp, false);
        }

        public static void GetWxCode(HttpContext context, string appid, string querystr, bool onlyhttp, bool isbase)
        {
            GetWxCode(context.Request, context.Response, appid, querystr, onlyhttp, isbase);
        }

        public static void GetWxCode(Page page, string appid, string querystr, bool onlyhttp, bool isbase)
        {
            GetWxCode(page.Request, page.Response, appid, querystr, onlyhttp, isbase);
        }

        public static void GetWxCode(HttpRequest Request, HttpResponse Response, string appid, string querystr, bool onlyhttp, bool isbase)
        {
            string path = Request.Url.AbsolutePath;
            string keys = onlyhttp ? "host" : "hosts";
            string basehost = ConfigurationManager.AppSettings[keys].ToString();
            //string host = context.Request.Url.Host;

            string reqpage = basehost + path/*.Substring(1)*/;
            if (!querystr.StartsWith("?") && querystr != "")  //如果参数前面不带问号就加上
                querystr = "?" + querystr;

            string requrl = reqpage + querystr;
            requrl = HttpUtility.UrlEncode(requrl);
            //string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE&connect_redirect=1#wechat_redirect", appid, requrl);
            WxPayData data = new WxPayData();
            data.SetValue("appid", appid);
            data.SetValue("redirect_uri", requrl);
            data.SetValue("response_type", "code");
            if(isbase)
                data.SetValue("scope", "snsapi_base");
            else
                data.SetValue("scope", "snsapi_userinfo");//
            data.SetValue("state", "" + HttpUtility.UrlEncode(DateTime.Now.ToString()) + "" + "#wechat_redirect");
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();

            Response.Redirect(url);
            Response.End();
        }
    }
}