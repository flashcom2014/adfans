using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Common;
using DBUtility;
using MySql.Data.MySqlClient;

public partial class admin_login: System.Web.UI.Page
{
    bool checkadmin = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            return;
         but_Click();
    }

    protected void but_Click()
    {
        try
        {
            if (Session["CheckCodes"] == null || Request.Form["checkcode"].ToString().Trim().ToUpper() != Session["CheckCodes"].ToString())
            {
                Jscript.Alert(this.Page, "验证码不正确");
                return;
            }

            string uid = Request.Form["uid"].ToString();
            string pwd = Request.Form["pwd"].ToString();
            string sqlstr = "select * from tbl_ad_mch where ad_mobile=@mobile or ad_acc=@ad_acc";
            MySqlParameter mmobile = new MySqlParameter("@mobile", MySqlDbType.VarChar, 16);
            mmobile.Value = uid;
            MySqlParameter macc = new MySqlParameter("@ad_acc", MySqlDbType.VarChar, 32);
            macc.Value = uid;
            DataSet ds = DbHelperMySQL.Query(sqlstr, mmobile, macc);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                if (AdminLogin(uid, pwd) == 1)
                    Jscript.Alert(this.Page, "用户名不存在！");
            }
            else
            {
                string ad_paw = dt.Rows[0]["ad_paw"].ToString();
                string enable = dt.Rows[0]["enable"].ToString();
                if (enable == "0")
                {
                    if (AdminLogin(uid, pwd) == 1)
                        Jscript.Alert("该用户已被禁用！");
                }
                else if (ad_paw != Tools.MD5(pwd))
                {
                    if (AdminLogin(uid, pwd) == 1)
                        Jscript.Alert(this.Page, "用户名或密码不正确！");
                }
                else
                {
                    int ad_id = (int)dt.Rows[0]["ad_id"];
                    string ad_acc = dt.Rows[0]["ad_acc"].ToString();
                    Session["ad_id"] = ad_id;
                    Session["ad_acc"] = ad_acc;


                    Jscript.Alert("登录成功");
                    Response.Redirect("Default.aspx");
                }
            }
            dt.Dispose();
            ds.Dispose();
        }
        catch (Exception ex)
        {
            Jscript.Alert(this.Page, "验证码超时，请刷新页面");
        }
    }

    private int AdminLogin(string uid, string pwd)
    {
        if (checkadmin)     //已经检查过管理员，不通过下次不再检查
            return 1;

        checkadmin = true;
        string sqlstr = "select * from tbl_ad_admin where admin_acc=@admin_acc;";
        MySqlParameter macc = new MySqlParameter("@admin_acc", MySqlDbType.VarChar, 32);
        macc.Value = uid;
        DataSet ds = DbHelperMySQL.Query(sqlstr, macc);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            return 1;
        }
        int admin_id = int.Parse(dt.Rows[0]["admin_id"].ToString());
        string admin_paw = dt.Rows[0]["admin_paw"].ToString();
        string appid = dt.Rows[0]["appid"].ToString();
        string openid = dt.Rows[0]["openid"].ToString();
        string flag = dt.Rows[0]["flag"].ToString();
        dt.Dispose();
        ds.Dispose();
        if (admin_paw != Tools.MD5(pwd))
        {
            Jscript.Alert2(Page, "用户名或密码不正确！");
            return 2;
        }
        Session["Admin_id"] = admin_id;
        Session["Admin_acc"] = uid;
        Session["admin_ad_flag"] = flag;
        if (string.IsNullOrEmpty(openid))
        {
            sqlstr = string.Format("update tbl_ad_admin set login_time=now() where admin_id={0};", admin_id);
            int result = DbHelperMySQL.ExecuteSql(sqlstr);
            Session["ad_id"] = 0;
            Session["ad_acc"] = "Admin";
            Response.Redirect("Default.aspx");
            return 0;
        }
        //进行微信账号验证
        Response.Redirect("Admin_Login.aspx");
        return 0;
    }
}
