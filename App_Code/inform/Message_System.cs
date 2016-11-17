using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using DBUtility;
using WxPayAPI;
using LitJson;

namespace inform
{
    /// <summary>
    /// 消息系统类
    /// </summary>
    public class Message_System
    {
        protected static WxPayConfig wxconfig = null;
        private static DateTime reloadtime;

        public static WxPayAPI.WxPayConfig GetWxConfig()
        {
            if (wxconfig != null && DateTime.Now < reloadtime)
                return wxconfig;
            string sqlstr = "select wx.* from tbl_settings as s join tbl_wx_config as wx on  cast(s.`values` as signed)=wx.payment_id where s.`keys`='ad_payment_id';";
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            if (dt.Rows.Count > 0)
            {
                int payment_id = (int)dt.Rows[0]["payment_id"];
                wxconfig = new WxPayConfig(payment_id);
                reloadtime = DateTime.Now.AddMinutes(10);
                return wxconfig;
            }
            return null;
        }

        public static void CleanWx()
        {
            wxconfig = null;
        }

        public static bool Inform_Message_Mch(int mchid, string tkey, string url, out string errmsg, params string[] args)
        {
            return Inform_Message(mchid, 0, tkey, url, out errmsg, args);
        }

        public static bool Inform_message_Agent(int agentid, string tkey, string url, out string errmsg, params string[] args)
        {
            return Inform_Message(0, agentid, tkey, url, out errmsg, args);
        }

        private static bool Inform_Message(int mchid, int agentid, string tkey, string url, out string errmsg, params string[] args)
        {
            errmsg = "";
            //检查是否开启对应开关和是否已绑定接收号
            string sqlstr = string.Format("select s.*,u.openid from tbl_value_add_services as s left join tbl_wx_value_add_users as u on s.wx_user_id=u.sn where mch_id={0} and agent_id={1};select * from tbl_inform_users as u join tbl_inform_template as t on u.inform_types=t.inform_types where t.tkey='{2}' and u.mch_id={0} and u.agent_id={1};", mchid, agentid, tkey);
            DataSet ds = DbHelperMySQL.Query(sqlstr);
            DataTable dt = ds.Tables[0];
            DataTable dtinform = ds.Tables[1];
            if (dt.Rows.Count > 0 && dtinform.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                int enable_wx = (int)row["enable_wx"];
                int wx_user_id = (int)row["wx_user_id"];
                int enable_sms = (int)row["enable_sms"];
                string mobile = row["mobile"].ToString();
                int sms_count = (int)row["sms_count"];
                float sms_one = (float)row["sms_one"];
                float balance = (float)row["balance"];
                //
                DataRow rowinform = dtinform.Rows[0];
                int wx_switch = (int)rowinform["wx_switch"];
                int sms_switch = (int)rowinform["sms_switch"];
                int inform_types = (int)rowinform["inform_types"];
                string logdata = rowinform["log_value"].ToString();

                //插入通知记录
                string key = "";
                for (int i = 0; i < args.Length; i++)
                {
                    key = "[keyword" + i.ToString() + "]";
                    logdata = logdata.Replace(key, args[i]);
                }
                sqlstr = string.Format("insert into tbl_inform_log(mch_id,agent_id,infrom_types,content) values({0},{1},{2},'{3}');", mchid, agentid, inform_types, logdata);
                int result = DbHelperMySQL.ExecuteSql(sqlstr);

                //判断微信
                if (enable_wx > 0 && wx_user_id > 0 && wx_switch > 0)
                {
                    string openid = row["openid"].ToString();
                    int wx_message_id = (int)rowinform["wx_message_id"];
                    string wx_message_key = rowinform["wx_message_key"].ToString();
                    WxTemplateSend wxsend = WxTemplateSend.GetWxTemplateForAppid(GetWxConfig().APPID, wx_message_key);
                    if (wxsend == null)
                        wxsend = WxTemplateSend.GetWxTemplate(wx_message_id);
                    if (wxsend != null)
                    {
                        wxsend.MchID = mchid;
                        wxsend.AgentID = agentid;
                        key = "";
                        string value = "";
                        for (int i = 1; i <= 6; i++)
                        {
                            //设置内容
                            key = "wx_value" + i.ToString();
                            if (!rowinform[key].Equals(DBNull.Value))
                            {
                                value = rowinform[key].ToString();
                                for (int j = 0; j < args.Length; j++)
                                {
                                    key = "[keyword" + j.ToString() + "]";
                                    value = value.Replace(key, args[j]);
                                }
                                wxsend[i - 1].SetValue(value);
                            }
                            //设置颜色
                            key = "wx_color" + i.ToString();
                            if (!rowinform[key].Equals(DBNull.Value))
                            {
                                value = rowinform[key].ToString();
                                wxsend[i - 1].SetColor(value);
                            }
                        }
                        //
                        if (!wxsend.IsReady)
                        {
                            errmsg = "微信模板未正确设置";
                            return false;
                        }
                        //
                        wxsend.SendTemplate(openid, url);
                    }
                    else
                    {
                        errmsg = string.Format("未找到ID为{0}的微信模板", wx_message_id);
                        return false;
                    }
                }
                //判断短信
                if (enable_sms > 0 && mobile != "" && sms_switch > 0)
                {
                    int channel = 0;    //先设定按条
                    float amount = sms_one;
                    //检查剩余条数
                    if (sms_count > 0)
                    {
                        channel = 1;
                        amount = 0f;
                    }
                    //
                    if (channel > 0 || balance > sms_one)   //短信余额或余额够用
                    {
                        if (amount > 0) //更新余额
                        {
                            balance -= amount;
                            sqlstr = string.Format("update tbl_value_add_services set balance={0},description='sendsms' where mch_id={1} and agent_id={2}", balance, mchid, agentid);
                            result = DbHelperMySQL.ExecuteSql(sqlstr);
                        }
                        else if (sms_count > 0) //更新剩余条数
                        {
                            sms_count--;
                            sqlstr = string.Format("update tbl_value_add_services set sms_count={0},description='sendsmsfornumber' where mch_id={1} and agent_id={2}", sms_count, mchid, agentid);
                            result = DbHelperMySQL.ExecuteSql(sqlstr);
                        }
                        //
                        string smstkey = rowinform["sms_tkey"].ToString();
                        string content = rowinform["sms_value"].ToString();
                        key = "";
                        for (int i = 0; i < args.Length; i++)
                        {
                            key = "[keyword" + i.ToString() + "]";
                            content = content.Replace(key, args[i]);
                        }
                        //content += "[" + DateTime.Now.ToString("MM-dd HH:mm") + "]";
                        content = content.Replace("[time]", "[" + DateTime.Now.ToString("MM-dd HH:mm") + "]");
                        ISmsSend sms = SmsSend.GetSmsTemplate(smstkey);
                        if (sms != null)
                        {
                            sms.SendSms(mchid, agentid, channel, amount, mobile, content, ref errmsg);

                        }
                        else
                        {
                            errmsg = string.Format("未打到短信模板Key为{0}的模板", smstkey);
                            return false;
                        }
                    }
                }
                dt.Dispose();
                dtinform.Dispose();
                ds.Dispose();
                return true;
            }
            dt.Dispose();
            dtinform.Dispose();
            ds.Dispose();
            errmsg = "没有找到对应账号或模板Key";
            return false;
        }

        
    }
}