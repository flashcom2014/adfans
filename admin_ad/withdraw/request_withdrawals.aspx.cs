using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;

public partial class admin_ad_withdraw_request_withdrawals : AdminPageBasead
{
    public string html = "";
    public bool showtx = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadDatas();
        if (ViewState["showtx"] != null)
        {
            showtx = (bool)ViewState["showtx"];
        }
    }

    private string GetMoney(float money)
    {
        int n = (int)money;
        float m = (float)n;
        return (m / 100f).ToString("F2") + "元";
    }

    private void LoadDatas()
    {
        l_agentmoney.Text = GetMoney(MyAdMch.agent_money);
        l_mchmoney.Text = GetMoney(MyAdMch.mch_money);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string sqlstr = string.Format("select tx.*,b.account_name,b.account_no from tbl_ad_tx as tx left join tbl_ad_bank as b on tx.pay_id=b.bank_id where tx.ad_id={0} and status<2 order by tx_id desc;", ad_id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<tr><td>");
            sb.Append(row["account_name"].ToString() + "(" + row["account_no"].ToString() + ")</td>");
            sb.Append("<td>" + row["create_time"].ToString() + "</td>");
            sb.Append("<td>" + float.Parse(row["amount"].ToString()).ToString("F2") + "</td>");
            int status = int.Parse(row["status"].ToString());
            sb.Append("<td>" + GetStatus(status) + "</td></tr>");
        }
        html = sb.ToString();
        sb.Clear();
        sb = null;
    }

    private string GetStatus(int status)
    {
        switch (status)
        {
            case 0:
                return "未审核";
            case 1:
                return "审核中";
            case 2:
                return "审核通过";
            case 3:
                return "拒绝提现";
        }
        return "未知";
    }

    protected void b_agent_Click(object sender, EventArgs e)
    {
        if (MyAdMch.agent_money == 0f)
        {
            Jscript.Alert2(Page, "您没有可转移的金额");
            return;
        }
        List<string> tl = new List<string>();
        tl.Add(string.Format("update tbl_ad_mch set mch_money=mch_money+agent_money where ad_id={0};", ad_id));
        tl.Add(string.Format("insert into tbl_ad_agent_tx_log(ad_id,amount) select ad_id,agent_money as amount from tbl_ad_mch where ad_id={0};", ad_id));
        tl.Add(string.Format("update tbl_ad_mch set agent_money=0 where ad_id={0};", ad_id));
        int result = DbHelperMySQL.ExecuteSqlTran(tl);
        MyAdMch_Referer();
        tl.Clear();
        tl = null;
        LoadDatas();
    }

    protected void b_mch_Click(object sender, EventArgs e)
    {
        float money = 0f;
        if (!float.TryParse(t_money.Text, out money))
        {
            Jscript.Alert2(Page, "请输入正确的金额!");
            return;
        }
        if (money == 0f)
        {
            Jscript.Alert2(Page, "不能提取0元！");
            return;
        }
        money = money * 100f;
        MyAdMch_Referer();
        if (money > MyAdMch.mch_money)
        {
            Jscript.Alert2(Page, "提取金额超实际余额！");
            return;
        }
        float hasmoney = MyAdMch.mch_money - money;

        float savemoeny = 10000f;
        int waitmin = 10;
        float mintx = MyAdMch.tx_min * 100f;

        if (money < mintx)
        {
            Jscript.Alert2(Page, string.Format("提款金额必须大于{0}元", (mintx / 100f).ToString("F0")));
            return;
        }

        Settings settings = Settings.GetSettings();
        savemoeny = settings.ad_amount;
        waitmin = settings.ad_waittime;
        /*string sqlstr = "select * from tbl_settings where `keys`='ad_amount' or `keys`='ad_waittime';";
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        string key = "";
        foreach(DataRow row in dt.Rows)
        {
            key = row["keys"].ToString();
            switch (key)
            {
                case "ad_amount":
                    savemoeny = float.Parse(row["values"].ToString());
                    break;
                case "ad_waittime":
                    waitmin = int.Parse(row["values"].ToString());
                    break;
            }            
        }
        dt.Dispose();
        ds.Dispose();*/
        string sqlstr = "";
        DataSet ds = null;
        DataTable dt = null;

        if (hasmoney < savemoeny)
        {
            int n = 0;
            DateTime lasttime = DateTime.Now;
            sqlstr = string.Format("select sum(`enable`) as `enable`,max(edit_time) as edit_time from tbl_wx_xifenji where ad_id={0};", ad_id);
            ds = DbHelperMySQL.Query(sqlstr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                n = int.Parse(dt.Rows[0]["enable"].ToString());
                lasttime = (DateTime)dt.Rows[0]["edit_time"];
            }
            dt.Dispose();
            ds.Dispose();
            if (n > 0)
            {
                Jscript.Alert2(Page, string.Format("申请提现后余额小于{0}元，需要把所有公众号停用再等待{1}分钟后才可提现！", (savemoeny / 100f).ToString("F2"), waitmin));
                return;
            }
            else
            {
                DateTime thetime = lasttime.AddMinutes(waitmin);
                Jscript.Alert2(Page, string.Format("请在停用所有公众号的{0}分钟后再申请提现！", waitmin));
                return;
            }
        }
        //
        if (bankcode.DataSource == null)
            LoadBankCode();
        //
        sqlstr = string.Format("select * from tbl_ad_bank where ad_id={0} order by bank_id desc limit 1;", ad_id);
        ds = DbHelperMySQL.Query(sqlstr);
        dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            bankcode.Value = dt.Rows[0]["bank_code"].ToString();
            t_account_name.Text = dt.Rows[0]["account_name"].ToString();
            t_account_no.Text = dt.Rows[0]["account_no"].ToString();
        }
        dt.Dispose();
        ds.Dispose();

        l_txmoney.Text = string.Format("{0}元", (money / 100f).ToString());

        ViewState["showtx"] = showtx = false;
        ViewState["money"] = money;
    }

    private void LoadBankCode()
    {
        string sqlstr = "select * from tbl_ght_bank";
        DataSet ds = DBUtility.DbHelperMySQL.Query_(sqlstr);
        DataTable dt = ds.Tables[0];
        bankcode.DataTextField = "bank_name";
        bankcode.DataValueField = "bank_code";
        bankcode.DataSource = dt;
        bankcode.DataBind();
    }

    protected void b_tx_Click(object sender, EventArgs e)
    {
        if (ViewState["money"] == null)
        {
            ViewState["showtx"] = showtx = true;
            Jscript.Alert2(Page, "网络错误，请稍候再试！");
            return;
        }
        float money = (float)ViewState["money"];
        MyAdMch_Referer();
        if (MyAdMch.mch_money < money)
        {
            ViewState["showtx"] = showtx = true;
            Jscript.Alert2(Page, "提款余额有误，请重试！");
            return;
        }

        string name = t_account_name.Text;
        string no = t_account_no.Text;
        string bankcode = Request.Form["bankcode"];

        bool hassettings = false;
        int bankid = 0;

        string sqlstr = string.Format("select * from tbl_ad_bank where ad_id={0};", ad_id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            if (row["bank_code"].ToString() == bankcode && row["account_no"].ToString() == no && row["account_name"].ToString() == name)
            {
                bankid = int.Parse(row["bank_id"].ToString());
                hassettings = true;
                break;
            }
        }
        dt.Dispose();
        ds.Dispose();

        if (!hassettings)
        {
            sqlstr = string.Format("call AddADBank({0},'{1}','{2}','{3}');", ad_id, bankcode, no, name);
            object obj = DbHelperMySQL.GetSingle(sqlstr);
            if (obj != null)
            {
                bankid = int.Parse(obj.ToString());
            }
        }


        float txamount = money / 100f;

        List<string> tl = new List<string>();
        tl.Add(string.Format("update tbl_ad_mch set mch_money=mch_money-{0} where ad_id={1};", money, ad_id));
        tl.Add(string.Format("insert into tbl_ad_tx(ad_id,amount,des,pay_id) values({0},{1},'申请提现',{2});", ad_id, txamount, bankid));
        /*if (!hassettings)
        {
            if (charge > 0)
            {
                tl.Add(string.Format("insert into tbl_ad_bank(ad_id,bank_code,account_no,account_name,charge,entpay_low,amount) values({0},'{1}','{2}','{3}',{4},{5},{6})", ad_id, bankcode, no, name, charge, entpay_low, amount));
            }
            else
            {
                tl.Add(string.Format("insert into tbl_ad_bank(ad_id,bank_code,account_no,account_name) values({0},'{1}','{2}','{3}');", ad_id, bankcode, no, name));
            }
        }*/
        int result = DbHelperMySQL.ExecuteSqlTran(tl);
        if (result > 0)
        {
            MyAdMch_Referer();
            LoadDatas();
            ViewState["showtx"] = showtx = true;
            //Jscript.Alert2(Page, "提现申请成功！");
        }
        else
        {
            Jscript.Alert2(Page, "提现申请失败！");
        }
    }

    protected void b_cancel_Click(object sender, EventArgs e)
    {
        ViewState["showtx"] = showtx = true;
    }
}