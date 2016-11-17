<%@ WebHandler Language="C#" Class="loginadd" %>

using System;
using System.Web;
using DBUtility;
using System.Data;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
public class loginadd : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string f_action = context.Request.QueryString["action"].Trim();
        switch (f_action)
        {
         
            case "addadmin": Addadmin(context); break;
                
        }
    }
    public void Addadmin(HttpContext context)
    {
        if (context.Session["Admin_id"] == null || context.Session["Admin_acc"] == null || context.Session["admin_ad_flag"] == null || context.Session["admin_ad_flag"].ToString()=="0")
        {
            context.Session.Clear();

            context.Response.Write("[{\"result\":\"no\",\"info\":\"身份过期或者无权限,重新登陆\",\"url\":\"" + UrlHelper.GetNLBPathUrl("/admin_ad/login.aspx") + "\"}]");
            context.Response.End();
        }
        string openid = context.Request.Form["wx_openid"].Trim();
        string acc = context.Request.Form["tbmch_acc"].Trim();
        string pwd = context.Request.Form["tbmch_pwd"].Trim();
        string pwdnew=Tools.MD5(pwd);
        int enable=int.Parse(context.Request.Form["enable"].Trim());
        int pid=int.Parse(context.Session["Admin_id"].ToString());
        string appid = context.Request.Form["wx_appid"].Trim();
        string sqlstr = "";
        sqlstr = string.Format("select admin_id from tbl_ad_admin where  admin_acc='{0}'", acc);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count > 0)
        {
            dt.Dispose();
            ds.Dispose();
            context.Response.Write("[{'result':'no','info':'用户名已存在'}]");
            context.Response.End();
            return;
        }
        sqlstr = string.Format("insert into tbl_ad_admin(admin_acc,admin_paw,flag,appid,openid,login_time,upadminid) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", acc, pwdnew, enable, appid, openid, DateTime.Now.ToString(), pid);
        int result = DbHelperMySQL.ExecuteSql(sqlstr);
        if (result > 0)
        {
            context.Response.Write("[{'result':'ok','info':'添加管理员成功'}]");
            context.Response.End();
        }
        else
        {
            context.Response.Write("[{'result':'no','info':'添加管理员失败'}]");
            context.Response.End();
        }
    }
    

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }


}