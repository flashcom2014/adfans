using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;

public partial class admin_ad_register : System.Web.UI.Page
{
    public string scripts = "";

    private string share_code = "";
    private float scale = 0f;
    private int parent_id = 0;
    private int codeid = 0;
    private int amount = 150;
    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request.QueryString["action"];
        if (!string.IsNullOrEmpty(action) && Request.Form.Count > 0)
        {
            GetCode();
        }

        if (!IsPostBack)
        {
            string sqlstr = "call GetAdName;";
            DataSet ds = DbHelperMySQL.Query_Main(sqlstr);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                t_acc.Value = dt.Rows[0]["ad_acc"].ToString();
            }
            string code = Request.QueryString["code"];
            sqlstr = string.Format("select * from tbl_ad_share_code where share_code='{0}'", code);
            ds = DbHelperMySQL.Query(sqlstr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                share_code = dt.Rows[0]["share_code"].ToString();
                string scales = dt.Rows[0]["scale"].ToString();
                scale = float.Parse(scales);
                l_scale.InnerText = scales + "%";
                parent_id = int.Parse(dt.Rows[0]["ad_id"].ToString());
                parent_id = Math.Max(0, parent_id);
                amount = int.Parse(dt.Rows[0]["amount"].ToString());
            }

            dt.Dispose();
            ds.Dispose();

            ViewState["share_code"] = share_code;
            ViewState["scale"] = scale;
            ViewState["parent_id"] = parent_id;
            ViewState["amount"] = amount;
        }
        else
        {
            share_code = ViewState["share_code"].ToString();
            scale = (float)ViewState["scale"];
            parent_id = (int)ViewState["parent_id"];
            amount = (int)ViewState["amount"];
            b_reg_Click();
        }
    }

    public bool HasCode
    {
        get
        {
            return (!string.IsNullOrEmpty(share_code) && scale > 0f);
        }
    }

    private void GetCode()
    {
        string mobile = Request.Form["mobile"];
        if (string.IsNullOrEmpty(mobile))
        {
            Response.Write("[{\"result\":\"no\",\"info\":\"请先输入手机号码\"}]");
            Response.End();
        }
        string sqlstr = string.Format("select * from tbl_sms_code where mobile='{0}' order by sn desc limit 1", mobile);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            DateTime logtime = (DateTime)dt.Rows[0]["log_time"];
            if (logtime.AddSeconds(59) >= DateTime.Now)
            {
                dt.Dispose();
                ds.Dispose();
                Response.Write("[{\"result\":\"no\",\"info\":\"获取验证码太频繁\"}]");
                Response.End();
            }
        }
        //
        sqlstr = string.Format("select ad_id from tbl_ad_mch where ad_mobile='{0}' or ad_acc='{0}'", mobile);
        ds = DbHelperMySQL.Query(sqlstr);
        dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            dt.Dispose();
            ds.Dispose();
            Response.Write("[{\"result\":\"no\",\"info\":\"该手机号码已注册\"}]");
            Response.End();
        }
        //
        dt.Dispose();
        ds.Dispose();
        //
        string tkey = "regcode";
        inform.ISmsSend sms = inform.SmsSend.GetSmsTemplate(tkey);
        if (sms != null)
        {
            Random ra = new Random(Guid.NewGuid().GetHashCode());
            string code = "";
            for (int i = 0; i < 6; i++) //生成6位验证码
            {
                int n = ra.Next(10);
                code += n.ToString();
            }
            //
            string ip = Jscript.GetIp();
            string restr = "";
            if (sms.SendSms(mobile, code, ip, ref restr))
            {
                Response.Write("[{\"result\":\"ok\"}]");
                Response.End();
            }
            else
            {
                Response.Write("[{\"result\":\"no\",\"info\":\"网络繁忙，请稍候再试\"}]");
                Response.End();
            }
        }
        Response.Write("[{\"result\":\"no\",\"info\":\"短信功能有错，请联系管理员！\"}]");
        Response.End();
    }

    protected void b_reg_Click()
    {
        try
        {
            string mobile = t_mobile.Value;
            string code = t_code.Value;
            //检查验证码
            if (!CheckSmsCode(mobile, code))
            {
                Jscript.Alert2(Page, "验证码错误！");
                return;
            }
            //
            string sqlstr = string.Format("select ad_id from tbl_ad_mch where ad_mobile='{0}' or ad_acc='{0}'", mobile);
            DataSet ds = DbHelperMySQL.Query(sqlstr);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                ds.Dispose();
                Jscript.Alert2(Page, "该手机号码已存在，请用别的手机号码注册");
                return;
            }

            string acc = t_acc.Value;
            sqlstr = string.Format("select ad_id from tbl_ad_mch where ad_mobile='{0}' or ad_acc='{0}'", acc);
            ds = DbHelperMySQL.Query(sqlstr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                ds.Dispose();
                Jscript.Alert2(Page, "该用户名已存在，请用别的用户名注册");
                return;
            }

            bool issetacc = CheckIsetAcc(acc);

            sqlstr = "insert into tbl_ad_mch(ad_mobile,ad_acc,ad_paw,isset_acc,ad_name,scale_points,parent_id,share_code,sub_amount) values(@ad_mobile,@ad_acc,@ad_paw,@isset_acc,@ad_name,@scale_points,@parent_id,@share_code,@sub_amount);";
            MySqlParameter mad_mobile = new MySqlParameter("@ad_mobile", MySqlDbType.VarChar, 16);
            mad_mobile.Value = mobile;
            MySqlParameter mad_acc = new MySqlParameter("@ad_acc", MySqlDbType.VarChar, 32);
            mad_acc.Value = acc;
            MySqlParameter mad_paw = new MySqlParameter("@ad_paw", MySqlDbType.VarChar, 56);
            mad_paw.Value = Tools.MD5(t_paw.Value);
            MySqlParameter misset_acc = new MySqlParameter("@isset_acc", MySqlDbType.Int32);
            misset_acc.Value = issetacc ? 1 : 0;
            MySqlParameter mad_name = new MySqlParameter("@ad_name", MySqlDbType.VarChar, 32);
            mad_name.Value = t_name.Value;
            MySqlParameter mscale_points = new MySqlParameter("@scale_points", MySqlDbType.Float);
            mscale_points.Value = scale;
            MySqlParameter mparent_id = new MySqlParameter("@parent_id", MySqlDbType.Int32);
            mparent_id.Value = parent_id;
            MySqlParameter mshare_code = new MySqlParameter("@share_code", MySqlDbType.VarChar, 25);
            mshare_code.Value = share_code;
            MySqlParameter msub_amount = new MySqlParameter("@sub_amount", MySqlDbType.Int32);
            msub_amount.Value = amount;
            //
            int result = DbHelperMySQL.ExecuteSql(sqlstr, mad_mobile, mad_acc, mad_paw, misset_acc, mad_name, mscale_points, mparent_id, mshare_code, msub_amount);

            List<string> list = new List<string>();
            sqlstr = string.Format("update tbl_sms_code set is_used=1 where sn={0};", codeid);
            list.Add(sqlstr);
            sqlstr = string.Format("update tbl_ad_share_code set reg_count=reg_count+1 where share_code='{0}';", share_code);
            list.Add(sqlstr);
            sqlstr = string.Format("insert into tbl_ad_mch_log(ad_mobile,share_code,scale_points,parent_id) values('{0}','{1}',{2},{3});", mobile, share_code, scale, parent_id);
            list.Add(sqlstr);
            DbHelperMySQL.ExecuteSqlTran(list);
            list.Clear();
            list = null;

            if (result > 0)
            {
                scripts = "alert('注册成功，现在登陆吧！');\r\nlocation.href = 'login.aspx';";
            }
            else
            {
                Jscript.Alert2(Page, "注册失败，请稍候再试！");
            }
        }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("register: " + ex.ToString());
            Jscript.Alert2(Page, "系统错误");
        }
    }

    private bool CheckIsetAcc(string acc)
    {
        if (!acc.StartsWith("广告商"))
            return true;
        long n = 0;
        if (!long.TryParse(acc.Substring(3), out n))
            return true;
        return false;
    }

    private bool CheckSmsCode(string mobile, string code)
    {
        string sqlstr = string.Format("select * from tbl_sms_code where mobile='{0}' and is_used=0 order by sn desc limit 1", mobile);
        DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
        if (dt.Rows.Count > 0)
        {
            codeid = (int)dt.Rows[0]["sn"];
            string dcode = dt.Rows[0]["code"].ToString();
            DateTime exptime = (DateTime)dt.Rows[0]["expires_time"];
            if (DateTime.Now < exptime && dcode == code)
                return true;
        }
        return false;
    }
}