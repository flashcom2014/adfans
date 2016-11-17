using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;

public partial class admin_ad_admin_txlist : AdminPageBasead
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadDatas();
    }

    private void LoadDatas()
    {
        string date = t_date.Text;
        string status = d_status.SelectedValue;
        string sqlstr = "select t.*,m.*,b.charge from tbl_ad_tx as t left join tbl_ad_mch as m on t.ad_id=m.ad_id left join tbl_ad_bank as b on t.pay_id=b.bank_id";
        if (!string.IsNullOrEmpty(date) || !string.IsNullOrEmpty(status))
            sqlstr += " where";
        if (!string.IsNullOrEmpty(date))
            sqlstr += string.Format(" create_time>='{0}' and create_time<='{0} 23:59:59.999'", date);
        if (!string.IsNullOrEmpty(status))
            sqlstr += string.Format(" status={0}", status);
        sqlstr += " order by tx_id desc;";
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        gv_tx.DataSource = dt;
        gv_tx.DataBind();
        if (gv_tx.HeaderRow != null)
            gv_tx.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    public string AdMchName()
    {
        string name = Eval("ad_name").ToString();
        string mobile = Eval("ad_mobile").ToString();
        return string.Format("{0}({1})", name, mobile);
    }

    public string GetStatus()
    {
        int status = int.Parse(Eval("status").ToString());
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

    public string RealAmount()
    {
        float amount = float.Parse(Eval("amount").ToString());
        int charge = int.Parse(Eval("charge").ToString());
        amount = amount * 100f;
        int realcharge = (int)amount * charge / 1000;
        float realamount = amount - realcharge;
        return (realamount / 100f).ToString("F2");
    }

    public string CommandTx()
    {
        string txid = Eval("tx_id").ToString();
        string status = Eval("status").ToString();
        string remittance_status = Eval("remittance_status").ToString();

        string tdstr = string.Format("<a href=\"#\" onclick=\"location.href = 'txedit.aspx?txid={0}';\">审核</a>", txid);
        if (status == "2" && string.IsNullOrEmpty(remittance_status))
        {
            tdstr += "<span style=\"margin-left:8px; margin-right:8px;\">|</span>";
            tdstr += string.Format("<a href=\"#\" onclick=\"remittance({0});\">汇款</a>", txid);
        }
        return tdstr;
    }

    public string GetRemittance_status()
    {
        string rs = Eval("remittance_status").ToString();
        if (string.IsNullOrEmpty(rs))
            return rs;
        int status = int.Parse(rs);
        switch (status)
        {
            case 1:
                return "线下汇款成功";
        }
        return "未知";
    }

    protected void b_remit_Click(object sender, EventArgs e)
    {
        string order = t_remittance_order.Text;
        string txid = h_txid.Value;
        string sqlstr = string.Format("update tbl_ad_tx set remittance_status=1,remittance_order='{0}',remittance_time=now() where tx_id={1};", order, txid);
        int result = DbHelperMySQL.ExecuteSql(sqlstr);
        LoadDatas();
    }
}