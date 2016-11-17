using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;
using WxPayAPI;

public partial class admin_ad_Admin_Login_QR : System.Web.UI.Page
{

    bool isbase = true;
    WxPayConfig wx = null;
    string openid = "";
    string code = "";
    int admin_id = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        code = Request.QueryString["id"];
        if (string.IsNullOrEmpty(code))
        {
            p_error.Visible = true;
            l_error.Text = "参数错误";
            return;
        }
        string t = Request.QueryString["t"];
        if (t == "1")
            isbase = false;

        //
        try
        {
            string sqlstr = string.Format("select openid from tbl_ad_admin_qr where encode='{0}'", code);
            object obj = DbHelperMySQL.GetSingle(sqlstr);
            if (obj != null)
            {
                p_error.Visible = true;
                l_error.Text = "此二维码已失效";
                return;
            }

            byte[] endata = Convert.FromBase64String(code);
            if (endata.Length < 8)
            {
                p_error.Visible = true;
                l_error.Text = "非法二维码";
                return;
            }
            wx = inform.Message_System.GetWxConfig();
            admin_id = BitConverter.ToInt32(endata, 4);
            //
            if (ViewState["openid"] == null)
            {
                JsApiPay js = new JsApiPay(Page);
                js.GetOpenidAndAccessTokenHost(wx.APPID, wx.APPSECRET, wx.Host, isbase);
                if (js.openid == "")
                {
                    p_error.Visible = true;
                    l_error.Text = "获得数据出错";
                    return;
                }
                
                ViewState["openid"] = openid = js.openid;
            }
            else
            {
                openid = ViewState["openid"].ToString();
            }
            

            sqlstr = string.Format("select * from tbl_ad_admin where admin_id={0};", admin_id);
            DataSet ds = DbHelperMySQL.Query(sqlstr);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                ds.Dispose();
                p_error.Visible = true;
                l_error.Text = "非法二维码";
                return;
            }
            string acc = dt.Rows[0]["admin_acc"].ToString();
            string dappid = dt.Rows[0]["appid"].ToString();
            string dopenid = dt.Rows[0]["openid"].ToString();
            dt.Dispose();
            ds.Dispose();

            if (dappid != wx.APPID || dopenid != openid)
            {
                InsertData(true);
                p_error.Visible = true;
                l_error.Text = "你无权登录，你的微信账号和IP地址已被记录";
                return;
            }
            p_login.Visible = true;
            l_login.Text = string.Format("是否确认登录【{0}】管理员账号？", acc);
        }
        catch (Exception ex)
        {
            p_error.Visible = true;
            l_error.Text = "非法二维码";
        }
    }

    private void InsertData(bool isillegal)
    {

        string sqlstr = string.Format("insert into tbl_ad_admin_qr(encode,ad_admin_id,appid,openid,illegal,ip) values('{0}',{1},'{2}','{3}',{4},'{5}');", code, admin_id, wx.APPID, openid, isillegal ? 1 : 0, Jscript.GetIp());
        int result = DbHelperMySQL.ExecuteSql(sqlstr);
    }

    protected void b_login_Click(object sender, EventArgs e)
    {
        InsertData(false);
        p_login.Visible = false;
        p_error.Visible = true;
        l_error.Text = "你已成功授权登录！";
        l_error.ForeColor = System.Drawing.Color.Green;
    }
}