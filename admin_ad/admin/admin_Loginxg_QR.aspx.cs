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

public partial class admin_loginxg_QR : System.Web.UI.Page
{

    bool isbase = false;
    WxPayConfig wx = null;
    string openid = "";
    string code = "";
    int admin_id = 0;
    string nickname = "";
    string headimgurl = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string action = "";
        action = Request.QueryString["action"];
        code = Request.QueryString["id"];
        if (string.IsNullOrEmpty(code))
        {
            p_error.Visible = true;
            l_error.Text = "参数错误";
            return;
        }
      
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
          
            admin_id = BitConverter.ToInt32(endata, 4);
            //

            JsApiPay js = new JsApiPay(Page);
            js.GetOpenidAndAccessTokenHost(wx.APPID, wx.APPSECRET, wx.Host, isbase);
            if (js.openid == "")
            {
                p_error.Visible = true;
                l_error.Text = "获得数据出错";
                return;
            }
            ViewState["openid"] = openid = js.openid;
           
            

           
         

           
             
                 
                
           
            b_login_Click();
            
        }
        catch (Exception ex)
        {
            p_error.Visible = true;
            l_error.Text = "非法二维码";
        }
    }

    private void InsertData()
    {
        string ipadd = Jscript.GetIp();
        string sqlstr = string.Format("insert into tbl_ad_admin_qr(encode,ad_admin_id,appid,openid,wx_nickname,wx_headimgurl,ip) values('{0}',{1},'{2}','{3}','{4}','{5}','{6}');", code, admin_id, wx.APPID, openid,nickname, headimgurl, ipadd);
        int result = DbHelperMySQL.ExecuteSql(sqlstr);
    }

    protected void b_login_Click()
    {
        InsertData();
        p_login.Visible = false;
        p_error.Visible = true;
        l_error.Text = "测试公众号成功！";
        l_error.ForeColor = System.Drawing.Color.Green;
    }
}