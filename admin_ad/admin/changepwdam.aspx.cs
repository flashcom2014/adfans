using DBUtility;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class changepwdam : AdminPageBasead
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                this.tbmch_acc.InnerText = Admin_acc;
            }
            catch
            {
                Jscript.Alert(this.Page, "可能超时，请刷新页面");
            }
        }
        else
        {
            ok_Click();
        }
    }
    protected void ok_Click()
    {

        if (this.tbpwdold.Value == "") { Jscript.Alert(this.Page, "原密码不能为空"); this.tbpwdold.Focus(); }
        else if (Tools.MD5(this.tbpwdold.Value.Trim()) != _pwd()) { Jscript.Alert(this.Page, "原密码不正确"); this.tbpwdold.Focus(); }
        else if (this.tbpwd.Value == "") { Jscript.Alert(this.Page, "新密码不能为空"); this.tbpwd.Focus(); }
        else if (this.tbpwd2.Value != this.tbpwd.Value) { Jscript.Alert(this.Page, "两次输入的密码必须一致"); this.tbpwd2.Focus(); }
        else
        {
            UpdatePWD(Tools.MD5(this.tbpwd.Value));
            Jscript.RepalceLocation("修改密码成功，请重新登录", UrlHelper.GetNLBPathUrl("/admin_ad/login.aspx"));
        }
    }
    private string _pwd()
    {
        string s = "";
        string sql = "select admin_paw from tbl_ad_admin where admin_acc='" + Admin_acc + "'";
        MySqlDataReader dr = DbHelperMySQL.ExecuteReader(sql);
        if (dr.Read()) s = dr.GetString(0);
        dr.Close();
        return s;
    }
    private void UpdatePWD(string pp)
    {
        string sql = "update tbl_ad_admin set admin_paw='" + pp + "' where admin_acc='" + Admin_acc + "'";
        DbHelperMySQL.ExecuteSql(sql);

    }
}