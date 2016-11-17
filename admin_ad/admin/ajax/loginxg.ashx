<%@ WebHandler Language="C#" Class="loginxg" %>

using System;
using System.Web;
using DBUtility;
using System.Data;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
public class loginxg : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string f_action = context.Request.QueryString["action"].Trim();
        switch (f_action)
        {
            case "getwxinfo": GetWxInfo(context); break;
            case "bindwxinfo": BindWxInfo(context); break;
            case "delbind": DelBindWxInfo(context); break;
           
                
        }
    }
  
    public void DelBindWxInfo(HttpContext context)
    {
        string openid = context.Request.Form["openid"];


        if (context.Session["Admin_id"] == null || context.Session["Admin_acc"] == null || context.Session["admin_ad_flag"] == null || context.Session["admin_ad_flag"].ToString() == "0")
        {
            context.Session.Clear();

            context.Response.Write("[{\"result\":\"no\",\"info\":\"身份过期或者无权限,重新登陆\",\"url\":\"" + UrlHelper.GetNLBPathUrl("/admin_ad/login.aspx") + "\"}]");
            context.Response.End();
        }
         int sessionid = (int)context.Session["Admin_id"];
        if (string.IsNullOrEmpty(openid))
        {
            context.Response.Write("[{\"result\":\"no\",\"info\":\"参数不正确！\"}]");
            context.Response.End();
        }
        else
        {
            string sql=string.Format("select * from tbl_ad_admin where openid='{0}' and admin_id={1}",openid,sessionid);
            DataSet ds = DbHelperMySQL.Query(sql);
            DataTable dt0 = ds.Tables[0];
            if (dt0.Rows.Count > 0)     //已有数据，update
            {

                sql = string.Format("update tbl_ad_admin set openid='', appid='' where admin_id={0}", sessionid);
                int result = DbHelperMySQL.ExecuteSql(sql);
                if (result > 0)
                {
                    context.Response.Write("[{'result':'ok','info':'更新成功'}]");
                    context.Response.End();
                }
                else
                {
                    context.Response.Write("[{'result':'no','info':'更新失败'}]");
                    context.Response.End();
                }
                   
              
            }
            else
            {
                context.Response.Write("[{'result':'no','info':'解除绑定失败'}]");
                context.Response.End();
            }
            
        }
    }
    public void BindWxInfo(HttpContext context)
    {

        string openid = context.Request.Form["openid"];
        string appid = context.Request.Form["appid"];


        if (context.Session["Admin_id"] == null || context.Session["Admin_acc"] == null || context.Session["admin_ad_flag"] == null || context.Session["admin_ad_flag"].ToString() == "0")
        {
            context.Session.Clear();

            context.Response.Write("[{\"result\":\"no\",\"info\":\"身份过期或者无权限,重新登陆\",\"url\":\"" + UrlHelper.GetNLBPathUrl("/admin_ad/login.aspx") + "\"}]");
            context.Response.End();
        }
        int sessionid = (int)context.Session["Admin_id"];
        if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(appid))
        {
            context.Response.Write("[{\"result\":\"no\",\"info\":\"参数不正确！\"}]");
            context.Response.End();
        }
        else
        {
            string sql = string.Format("select * from tbl_ad_admin  where admin_id={0}", sessionid);
            DataSet ds = DbHelperMySQL.Query(sql);
            DataTable dt0 = ds.Tables[0];
            if (dt0.Rows.Count > 0)     //已有数据，update
            {

                if (dt0.Rows[0]["appid"].ToString() == "" && dt0.Rows[0]["openid"].ToString() == "")
                {
                    sql = string.Format("update tbl_ad_admin set openid='{1}', appid='{2}' where admin_id={0}", sessionid, openid, appid);
                    int result = DbHelperMySQL.ExecuteSql(sql);
                    if (result > 0)
                    {
                        context.Response.Write("[{'result':'ok','info':'更新成功'}]");
                        context.Response.End();
                    }
                    else
                    {
                        context.Response.Write("[{'result':'no','info':'更新失败'}]");
                        context.Response.End();
                    }

                }
                else
                {
                    context.Response.Write("[{'result':'no','info':'已绑定请解除绑定之后再重新绑定'}]");
                    context.Response.End();
                }
               

            }
            else
            {
                context.Response.Write("[{'result':'no','info':'绑定失败'}]");
                context.Response.End();
            }

        }
        
       
    }

    private string GetIPs()
    {
        string ips = "";
        if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
        {
            ips += System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            ips += "|";
        }
        ips += System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        return ips;
    }

    public void GetWxInfo(HttpContext context)
    {
        try
        {
          string code = context.Request.Form["code"];
                 code = HttpUtility.UrlDecode(code);
                 string type = "";
                 type = context.Request.QueryString["type"];

                 if (context.Session["Admin_id"] == null || context.Session["Admin_acc"] == null || context.Session["admin_ad_flag"] == null || context.Session["admin_ad_flag"].ToString() == "0")
                 {
                     context.Session.Clear();

                     context.Response.Write("[{\"result\":\"no\",\"info\":\"身份过期或者无权限,重新登陆\",\"url\":\"" + UrlHelper.GetNLBPathUrl("/admin_ad/login.aspx") + "\"}]");
                     context.Response.End();
                 }
            
           
            if (string.IsNullOrEmpty(code))
            {
                
                context.Response.Write("[{\"result\":\"no\",\"info\":\"参数不正确！\"}]");
                 context.Response.End();
            }
            else
            {
                byte[] endata = Convert.FromBase64String(code);
                if (endata.Length < 8)
                {
                   
                    context.Response.Write("[{\"result\":\"no\",\"info\":\"非法二维码！\"}]");
                    context.Response.End();
                }
                int admin_id = BitConverter.ToInt32(endata, 4);
                int sessionid = (int)context.Session["Admin_id"];
                if (admin_id != sessionid)
                {
                   
                    context.Response.Write("[{\"result\":\"no\",\"info\":\"非法二维码！\"}]");
                    context.Response.End();
                }

                string sqlstr = string.Format("select * from tbl_ad_admin where admin_id={0};", admin_id);
                DataSet ds = DbHelperMySQL.Query(sqlstr);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    ds.Dispose();
                   
                    context.Response.Write("[{\"result\":\"no\",\"info\":\"非法二维码！\"}]");
                    context.Response.End();
                }
                string admin_appid = dt.Rows[0]["appid"].ToString();
                string admin_openid = dt.Rows[0]["openid"].ToString();
                dt.Dispose();
                ds.Dispose();


                string rest = "[{\"result\":\"no\",\"info\":\"NO Data\"}]";
                string connst = DBUtility.DbHelperMySQL.connectionString;
                using (MySqlConnection conn = new MySqlConnection(connst))
                {
                    DateTime returntime = DateTime.Now.AddSeconds(10);  //10秒没数据直接返回
                    sqlstr = "select * from tbl_ad_admin_qr where encode='" + code + "';";
                    conn.Open();
                    MySqlCommand query = new MySqlCommand(sqlstr, conn);
                    MySqlDataReader reader = null;
                    bool haddata = false;
                    string openid = "", appid = "";
                    int isused = 0;
                    Dictionary<string, string> json = new Dictionary<string, string>();

                    while (DateTime.Now < returntime)
                    {
                        reader = query.ExecuteReader();
                        if (reader.Read())
                        {
                            //
                            appid = reader["appid"].ToString();
                            openid = reader["openid"].ToString();
                            isused = int.Parse(reader["isused"].ToString());
                            json.Add("openid", openid);
                            json.Add("appid", appid);
                            string nickname = reader["wx_nickname"].ToString();
                            nickname = HttpUtility.UrlDecode(nickname);  //先转换编码
                            json.Add("nickname", nickname);
                            json.Add("headimgurl", reader["wx_headimgurl"].ToString());
                            rest = "[{'result':'ok','data':" + LitJson.JsonMapper.ToJson(json) + "}]";
                            haddata = true;
                        }
                        reader.Close();
                        
                        //2016lf
                        //rest = "[{'result':'ok','data':" + LitJson.JsonMapper.ToJson(json) + "}]";
                        //context.Response.Write(rest);
                        //context.Response.End();
                        //return;
                        
                        
                        if (haddata)
                        {
                            if (isused != 0)
                            {
                               
                                rest = "[{\"result\":\"no\",\"info\":\"此二维码已失效！\"}]";
                                break;
                            }
                            //
                           
                            if (type == null)
                            {
                                if (!string.IsNullOrEmpty(admin_openid))
                                {
                                    if (admin_appid != appid || admin_openid != openid)
                                    {

                                        rest = "[{\"result\":\"nob\",\"info\":\"" + admin_openid + "！\"}]";
                                       
                                        break;
                                    }
                                }
                            }
                          
                            List<string> tl = new List<string>();
                            tl.Add(string.Format("update tbl_ad_admin_qr set isused=1 where encode='{0}';", code));
                         
                            int result = DbHelperMySQL.ExecuteSqlTran(tl);
                            tl.Clear();
                            tl = null;
                            if (result > 0)
                            {
                                if (admin_appid=="")
                                   json.Add("enable", "1");
                                else
                                    json.Add("enable", "0");
                                   rest = "[{'result':'ok','data':" + LitJson.JsonMapper.ToJson(json) + "}]";
                            }
                            else
                            {
                                context.Session.Clear();
                                rest = "[{\"result\":\"no\",\"info\":\"系统错误！\"}]";
                            }
                            break;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    query.Dispose();
                }
                context.Response.Write(rest);
                context.Response.End();
            }
        }
  
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("Admin_Loginxg: " + ex.ToString());
            context.Response.Write("[{\"result\":\"no\",\"info\":\"参数不正确！\"}]");
            context.Response.End();
        }


     
       
    }

    

    private void DisableWxConfig(string mch_id, string appid)
    {
        string sqlstr = string.Format("update tbl_wx_transfer_config set enable=0 where mch_id={0} and wx_app_id!='{1}'", mch_id, appid);
        DbHelperMySQL.ExecuteSql(sqlstr);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }


}