using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DBUtility;
using System.Drawing;
using LitJson;

namespace inform
{
    /// <summary>
    /// 微信模板发送
    /// </summary>
    public class WxTemplateSend
    {
        int tid = 0;
        string tkey = "";
        string appid = "";
        string appsecret = "";
        string template_id = "";
        int mchid = 0;
        int agentid = 0;
        List<wxTemplateChild> list = new List<wxTemplateChild>();

        /// <summary>
        /// 从数据库取得对应ID的模板结构
        /// </summary>
        /// <param name="templateid">模板ID</param>
        /// <returns></returns>
        public static WxTemplateSend GetWxTemplate(int templateid)
        {
            string sqlstr = string.Format("select t.*,c.W_APPSECRET from tbl_wx_message_template as t join tbl_wx_config as c on t.appid=c.W_APPID  where t.id={0} limit 1", templateid);
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            return GetWxTemplate(dt);
        }

        /// <summary>
        /// 从数据库取得默认微信模板结构
        /// </summary>
        /// <param name="tkey">定义的模板key</param>
        /// <returns></returns>
        public static WxTemplateSend GetWxTemplate(string tkey)
        {
            string original_id = System.Configuration.ConfigurationManager.AppSettings["message_original_id"];
            return GetWxTemplate(original_id, tkey);
        }

        /// <summary>
        /// 从数据库取得微信模板结构
        /// </summary>
        /// <param name="appid">微信公众号appid</param>
        /// <param name="tkey">定义的模板key</param>
        /// <returns></returns>
        public static WxTemplateSend GetWxTemplateForAppid(string appid, string tkey)
        {
            string sqlstr = string.Format("select t.*,c.W_APPSECRET from tbl_wx_message_template as t join tbl_wx_config as c on t.appid=c.W_APPID  where t.appid='{0}' and t.tkey='{1}' limit 1;", appid, tkey);
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            return GetWxTemplate(dt);
        }

        /// <summary>
        /// 从数据库取得微信模板结构
        /// </summary>
        /// <param name="ororiginal_id">微信公众号源ID</param>
        /// <param name="tkey">定义的模板key</param>
        /// <returns></returns>
        public static WxTemplateSend GetWxTemplate(string ororiginal_id, string tkey)
        {
            string sqlstr = string.Format("select t.*,c.W_APPSECRET from tbl_wx_message_template as t join tbl_wx_config as c on t.appid=c.W_APPID  where c.W_ORIGINAL_ID='{0}' and t.tkey='{1}' limit 1;", ororiginal_id, tkey);
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            return GetWxTemplate(dt);
        }

        private static WxTemplateSend GetWxTemplate(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                WxTemplateSend wx = new WxTemplateSend();
                wx.tid = (int)row["id"];
                wx.tkey = row["tkey"].ToString();
                wx.appid = row["appid"].ToString();
                wx.template_id = row["template_id"].ToString();
                wx.appsecret = row["W_APPSECRET"].ToString();
                string key = "";
                string data = "";

                for (int i = 1; i <= 6; i++)
                {
                    key = "data" + i.ToString();
                    if (!row[key].Equals(DBNull.Value))
                    {
                        data = row[key].ToString();
                        wxTemplateChild child = new wxTemplateChild(data);
                        wx.list.Add(child);
                    }
                }

                return wx;
            }
            return null;
        }

        /// <summary>
        /// 商家ID
        /// </summary>
        public int MchID
        {
            get
            {
                return mchid;
            }
            set
            {
                mchid = value;
            }
        }

        /// <summary>
        /// 代理ID
        /// </summary>
        public int AgentID
        {
            get
            {
                return agentid;
            }
            set
            {
                agentid = value;
            }
        }

        /// <summary>
        /// APPID
        /// </summary>
        public string APPID
        {
            get
            {
                return this.appid;
            }
        }

        /// <summary>
        /// 是否可以发送
        /// </summary>
        public bool IsReady
        {
            get
            {
                foreach (wxTemplateChild child in list)
                    if (!child.IsReady)
                        return false;
                return true;
            }
        }

        /// <summary>
        /// 内容数量
        /// </summary>
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// 内容设置值
        /// </summary>
        /// <param name="name">对应键值</param>
        /// <returns></returns>
        public wxTemplateChild this[string name]
        {
            get
            {
                foreach(wxTemplateChild child in list)
                {
                    if (child.Key == name)
                        return child;
                }
                return null;
            }
        }

        /// <summary>
        /// 内容设置值
        /// </summary>
        /// <param name="index">对应索引</param>
        /// <returns></returns>
        public wxTemplateChild this[int index]
        {
            get
            {
                if (index >= Count || index < 0)
                    return null;
                return list[index];
            }
        }

        /// <summary>
        /// 批量设置键值
        /// </summary>
        /// <param name="args">键值队列</param>
        /// <returns>如果设置的键值数量不同会返回false</returns>
        public bool SetValues(params string[] args)
        {
            if (args.Length != list.Count)
                return false;
            for (int i = 0; i < args.Length; i++)
                list[i].SetValue(args[i]);
            return true;
        }

        /// <summary>
        /// 批量设置颜色
        /// </summary>
        /// <param name="args">颜色队列</param>
        /// <returns>如果设置的颜色数量不同会返回false</returns>
        public bool SetColors(params string[] args)
        {
            if (args.Length != list.Count)
                return false;
            for (int i = 0; i < args.Length; i++)
                list[i].SetColor(args[i]);
            return true;
        }

        /// <summary>
        /// 批量设置颜色
        /// </summary>
        /// <param name="args">颜色队列</param>
        /// <returns>如果设置的颜色数量不同会返回false</returns>
        public bool SetColors(params Color[] args)
        {
            if (args.Length != list.Count)
                return false;
            for (int i = 0; i < args.Length; i++)
                list[i].SetColor(args[i]);
            return true;
        }

        /// <summary>
        /// 设置统一颜色
        /// </summary>
        /// <param name="color">颜色</param>
        public void SetSameColor(string color)
        {
            foreach (wxTemplateChild child in list)
                child.SetColor(color);
        }

        /// <summary>
        /// 设置统一颜色
        /// </summary>
        /// <param name="color">颜色</param>
        public void SetSameColor(Color color)
        {
            foreach (wxTemplateChild child in list)
                child.SetColor(color);
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="url">此消息连接的地址</param>
        /// <returns></returns>
        public JsonData SendTemplate(string openid, string url)
        {
            if (!IsReady)
                throw new Exception("模板内容未完成设置");
            string posturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=ACCESS_TOKEN";
            string postdata = "{\"touser\":\"" + openid + "\",\"template_id\":\"" + template_id + "\",\"url\":\"" + url + "\", \"data\":{";
            foreach (wxTemplateChild child in list)
                postdata += child.ToString() + ",";
            postdata = postdata.Remove(postdata.Length - 1);    //去除最后的逗号
            postdata += "}}";
            //
            var wx = WxPayAPI.AccessToken_Manage.GetWxConfig(appid, appsecret);
            JsonData json = wx.GetJson(posturl, postdata);

            string errmsg = "";
            long msgid = 0;
            if (JsonMapper.IsKey(json, "errmsg"))
                errmsg = json["errmsg"].ToString();
            if (JsonMapper.IsKey(json, "msgid"))
                msgid = long.Parse(json["msgid"].ToString());

            string sqlstr = string.Format("select sn from tbl_wx_value_add_users where appid='{0}' and openid='{1}';", appid, openid);
            DataSet ds = DbHelperMySQL.Query(sqlstr);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                int wx_user_id = (int)dt.Rows[0]["sn"];
                sqlstr = string.Format("insert into tbl_wx_message_log(mch_id,agent_id,wx_user_id,tid,errmsg,msgid) values({0},{1},{2},{3},'{4}',{5})", mchid, agentid, wx_user_id, tid, errmsg, msgid);
                int result = DbHelperMySQL.ExecuteSql(sqlstr);
            }
            dt.Dispose();
            ds.Dispose();

            return json;
        }
    }

    public class wxTemplateChild
    {
        private string key = "";
        private string value = "";
        private string mycolor = "#4169E1";
        private bool isready = false;

        public wxTemplateChild(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get
            {
                return key;
            }
        }

        public bool IsReady
        {
            get
            {
                return isready;
            }
        }

        public void SetValue(string value)
        {
            this.value = value;
            this.isready = true;
        }

        public void SetValue(string value, string color)
        {
            this.value = value;
            this.mycolor = color;
            this.isready = true;
        }

        public void SetValue(string value, Color color)
        {
            SetColor(color);
            SetValue(value);
        }

        public void SetColor(string color)
        {
            this.mycolor = color;
        }

        public void SetColor(Color color)
        {
            this.mycolor = string.Format("#{0}{1}{2}", Convert.ToString(color.R, 16), Convert.ToString(color.G, 16), Convert.ToString(color.B, 16));
        }

        public override string ToString()
        {
            if (!isready)
                throw new Exception(string.Format("“{0}”的内容未设置", key));
            string restr = "\"" + key + "\":{\"value\":\"" + value + "\",\"color\":\"" + mycolor + "\"}";
            return restr;
        }
    }
}