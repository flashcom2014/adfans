using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBUtility;
using System.Data;
using System.Net;
using System.IO;
using MySql.Data.MySqlClient;

namespace inform
{
    /// <summary>
    /// SmsSend 的摘要说明
    /// </summary>
    public abstract class SmsSend
    {
        public static Dictionary<int, int> ksclist = new Dictionary<int, int>();
        public int expmins = 10;

        public string compname = "";
        public string tkey = "";
        public int id = 0;
        public int sms_count = 0;
        public string username = "";
        public string password = "";
        public string apikey = "";
        public float level = 0f;
        public int templateid = 0;
        public string template = "";
        public bool isloaded = false;
        public bool issavecode = false;

        public static ISmsSend GetSmsTemplate(string tkey)
        {
            string sqlstr = string.Format("select * from sms_template where sms_count>0 and enable=1 and tkey='{0}' order by rvalue limit 1", tkey);
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                ISmsSend isms = null;
                string compname = row["company_name"].ToString();
                switch (compname)
                {
                    //case "HuaXing":
                    //    isms = new SmsSend_HuaXing();
                    //    break;
                    case "MeiLian":
                        isms = new SmsSend_MeiLian();
                        break;
                }
                if (isms != null)
                    isms.LoadData(row);

                return isms;
            }
            return null;
        }

        public SmsSend()
        {
        }

        public SmsSend(string key)
        {
            this.tkey = key;
        }

        public bool LoadData(DataRow row)
        {
            if (row == null)
                return false;
            id = (int)row["id"];
            compname = row["company_name"].ToString();
            sms_count = (int)row["sms_count"];
            username = row["username"].ToString();
            password = row["password"].ToString();
            tkey = row["tkey"].ToString();
            apikey = row["apikey"].ToString();
            level = (float)row["level"];
            templateid = (int)row["sn"];
            template = row["template"].ToString();
            isloaded = true;
            issavecode = row["savecode"].ToString() == "0" ? false : true;

            return true;
        }

        protected string HttpPost(string url, string postdata)
        {
            System.Text.Encoding myEncode = System.Text.Encoding.UTF8;
            byte[] postBytes = System.Text.Encoding.ASCII.GetBytes(postdata);


            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            req.ContentLength = postBytes.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }

                using (WebResponse res = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        string strResult = sr.ReadToEnd();
                        return strResult;
                    }
                }

            }
            catch (WebException ex)
            {
                return "无法连接到服务器\r\n错误信息：" + ex.Message;
            }
        }

        protected string HttpGet(string url)
        {
            Uri uri = new Uri(url);
            WebRequest webReq = WebRequest.Create(uri);

            try
            {
                using (HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse())
                {
                    using (Stream respStream = webResp.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(respStream, System.Text.Encoding.UTF8))
                        {
                            string strRes = objReader.ReadToEnd();
                            return strRes;
                        }
                    }
                }

            }
            catch (WebException ex)
            {
                return "无法连接到服务器/r/n错误信息：" + ex.Message;
            }
        }

        protected string GetContent(string code)
        {
            string changestr = "[" + tkey + "]";
            string content = template.Replace(changestr, code);
            content = HttpUtility.UrlEncode(content, System.Text.Encoding.UTF8);
            return content;
        }

        protected bool SaveLog(int mchid, int agentid, int channel, float amount, string mobile, string content, long sms_id)
        {
            int result = 1;
            string sqlstr = "insert into tbl_sms_send_log(template_id,mch_id,agent_id,mobile,channel,amount,content,sms_id) values(@template_id,@mch_id,@agent_id,@mobile,@channel,@amount,@content,@sms_id);";
            MySqlParameter mtid = new MySqlParameter("@template_id", MySqlDbType.Int32);
            mtid.Value = templateid;
            MySqlParameter mmchid = new MySqlParameter("@mch_id", MySqlDbType.Int32);
            mmchid.Value = mchid;
            MySqlParameter magent_id = new MySqlParameter("@agent_id", MySqlDbType.Int32);
            magent_id.Value = agentid;
            MySqlParameter mmobile = new MySqlParameter("@mobile", MySqlDbType.VarChar, 15);
            mmobile.Value = mobile;
            MySqlParameter mchannel = new MySqlParameter("@channel", MySqlDbType.Int32);
            mchannel.Value = channel;
            MySqlParameter mamount = new MySqlParameter("@amount", MySqlDbType.Float);
            mamount.Value = amount;
            MySqlParameter mcontent = new MySqlParameter("@content", MySqlDbType.VarChar, 150);
            mcontent.Value = content;
            MySqlParameter msms_id = new MySqlParameter("@sms_id", MySqlDbType.Int64);
            msms_id.Value = sms_id;
            result = DbHelperMySQL.ExecuteSql(sqlstr, mtid, mmchid, magent_id, mmobile, mchannel, mamount, mcontent, msms_id);


            if (result > 0)
            {
                if (ksclist.ContainsKey(id))
                {
                    ksclist[id]++;
                    if (ksclist[id] % 100 == 0)  //每发100条检查一次余额
                    {
                        string restr = "";
                        int balance = GetBalance(ref restr);
                        if (balance >= 0)
                        {
                            UpdateBalance(balance);
                        }
                        else
                        {
                            SendUpdate();
                        }
                    }
                }
                else
                {
                    ksclist[id] = 1;
                    SendUpdate();
                }
                return true;
            }
            return false;
        }

        protected bool SaveCode(string mobile, string code, long smsid, string ip)
        {
            int result = 1;
            if (issavecode)
            {
                DateTime expirestime = DateTime.Now.AddMinutes(expmins);
                string sqlstr = "insert into tbl_sms_code(template_id,mobile,code,msgid,expires_time,ip) values(@template_id,@mobile,@code,@msgid,@expires_time,@ip);";
                MySqlParameter mtid = new MySqlParameter("@template_id", MySqlDbType.Int32);
                mtid.Value = templateid;
                MySqlParameter mmobile = new MySqlParameter("@mobile", MySqlDbType.VarChar, 15);
                mmobile.Value = mobile;
                MySqlParameter mcode = new MySqlParameter("@code", MySqlDbType.VarChar, 12);
                mcode.Value = code;
                MySqlParameter mmsgid = new MySqlParameter("@msgid", MySqlDbType.Int64);
                mmsgid.Value = smsid;
                MySqlParameter mexptime = new MySqlParameter("@expires_time", MySqlDbType.DateTime);
                mexptime.Value = expirestime;
                MySqlParameter mip = new MySqlParameter("@ip", MySqlDbType.VarChar, 15);
                mip.Value = ip;
                //
                result = DbHelperMySQL.ExecuteSql(sqlstr, mtid, mmobile, mcode, mmsgid, mexptime, mip);
            }
            if (result > 0)
            {
                if (ksclist.ContainsKey(id))
                {
                    ksclist[id]++;
                    if (ksclist[id] % 100 == 0)  //每发100条检查一次余额
                    {
                        string restr = "";
                        int balance = GetBalance(ref restr);
                        if (balance >= 0)
                        {
                            UpdateBalance(balance);
                        }
                        else
                        {
                            SendUpdate();
                        }
                    }
                }
                else
                {
                    ksclist[id] = 1;
                    SendUpdate();
                }
                return true;
            }
            return false;
        }

        protected void SendUpdate()
        {
            string sqlstr = "update tbl_sms_config set sms_count=sms_count-1,sendcount=sendcount+1 where id=" + id.ToString();
            int result = DbHelperMySQL.ExecuteSql(sqlstr);
        }

        protected void UpdateBalance(int balance)
        {
            string sqlstr = string.Format("update tbl_sms_config set sms_count={0},sendcount=sendcount+1 where id={1}", balance, id);
            int result = DbHelperMySQL.ExecuteSql(sqlstr);
        }

        public abstract int GetBalance(ref string restr);
        
    }

}