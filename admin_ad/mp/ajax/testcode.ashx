<%@ WebHandler Language="C#" Class="testcode" %>

using System;
using System.Web;
using DBUtility;
using System.Data;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;

public class testcode : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string f_action = context.Request.QueryString["action"].Trim();
        switch (f_action)
        {

           
            case "getwxinfo": GetWxInfo(context); break;

        }
    }
    public void GetWxInfo(HttpContext context)
    {
        try
        {
          string code = context.Request.Form["code"];
                 code = HttpUtility.UrlDecode(code);
                

                 if (context.Session["ad_id"] == null || context.Session["ad_acc"] == null )
                 {
                     context.Session.Clear();

                     context.Response.Write("[{\"result\":\"no\",\"info\":\"身份过期,重新登陆\",\"url\":\"" + UrlHelper.GetNLBPathUrl("/admin_ad/login.aspx") + "\"}]");
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
                




                string sqlstr = "";
                string rest = "[{\"result\":\"no\",\"info\":\"NO Data\"}]";
                string connst = DBUtility.DbHelperMySQL.connectionString;
                using (MySqlConnection conn = new MySqlConnection(connst))
                {
                    DateTime returntime = DateTime.Now.AddSeconds(5);  //10秒没数据直接返回
                    sqlstr = "select * from tbl_ad_appid_qr where encode='" + code + "';";
                    conn.Open();
                    MySqlCommand query = new MySqlCommand(sqlstr, conn);
                    MySqlDataReader reader = null;
                    bool haddata = false;
                    string appid = "";
                    int isused = 0;
                    Dictionary<string, string> json = new Dictionary<string, string>();

                    while (DateTime.Now < returntime)
                    {
                        reader = query.ExecuteReader();
                        if (reader.Read())
                        {

                           
                            haddata = true;
                        }
                        reader.Close();
                        
                        
                        
                        
                        if (haddata)
                        {
                            if (isused != 0)
                            {
                               
                                rest = "[{\"result\":\"no\",\"info\":\"此二维码已失效！\"}]";
                                break;
                            }
                            //
                           
                         

                            
                                 
                           
                          
                            List<string> tl = new List<string>();
                            tl.Add(string.Format("update tbl_ad_appid_qr set isused=1 where encode='{0}';", code));
                         
                            int result = DbHelperMySQL.ExecuteSqlTran(tl);
                            tl.Clear();
                            tl = null;
                            if (result > 0)
                            {

                                rest = "[{'result':'ok',\"info\":\"测试通过！\"}]";
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
            Common.Scheduler.SaveExLog("testcode: " + ex.ToString());
            context.Response.Write("[{\"result\":\"no\",\"info\":\"参数不正确！\"}]");
            context.Response.End();
        }

    }
   
    public bool IsReusable {
        get {
            return false;
        }
    }

}