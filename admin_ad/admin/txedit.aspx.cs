using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;

public partial class admin_ad_admin_txedit : AdminPageBasead
{
    int tx_id = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = Request.QueryString["txid"];
        if (string.IsNullOrEmpty(id))
        {
            Response.Write("参数错误！");
            Response.End();
        }

        int.TryParse(id, out tx_id);
        if (!IsPostBack)
            LoadDatas();
    }

    private void LoadDatas()
    {
        string sqlstr = string.Format("select t.*,m.*,b.charge,b.amount as no_amount,b.account_no,b.account_name,g.bank_name from tbl_ad_tx as t left join tbl_ad_mch as m on t.ad_id=m.ad_id left join tbl_ad_bank as b on t.pay_id=b.bank_id left join tbl_ght_bank as g on b.bank_code=g.bank_code where t.tx_id={0} limit 1;", tx_id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            Response.Write("参数错误！");
            Response.End();
        }
        DataRow row = dt.Rows[0];
        ViewState["ad_id"] = row["ad_id"].ToString();
        l_acc.Text = row["ad_acc"].ToString();
        l_name.Text = row["ad_name"].ToString();
        l_mobile.Text = row["ad_mobile"].ToString();
        l_time.Text = row["create_time"].ToString();
        float amount = float.Parse(row["amount"].ToString());
        int charge = int.Parse(row["charge"].ToString());
        int no_amount = int.Parse(row["no_amount"].ToString());
        l_amount.Text = amount.ToString("F2");
        amount = amount * 100f;
        ViewState["amount"] = amount;
        int allamount = (int)amount/* + no_amount*/;   //加上之前没有结手续费的金额
        //int no_amount_now = (int)(allamount % 1000);   //这次未结手续费的金额
        //ViewState["no_amount_now"] = no_amount_now;
        int real_service_charge = allamount / 1000 * charge;
        float realamount = amount - real_service_charge;

        l_realamount.Text = (realamount / 100f).ToString("F2");
        l_charge.Text = string.Format("{0}‰", charge);

        l_txmin.Text = float.Parse(row["tx_min"].ToString()).ToString("F2");
        string account_no = row["account_no"].ToString();
        string account_name = row["account_name"].ToString();
        string bank_name = row["bank_name"].ToString();
        l_bank.Text = string.Format("收款人：{0}，\t开户行：{1}，\t银行卡：{2}", account_name, bank_name, account_no);
        int status = int.Parse(row["status"].ToString());
        switch (status)
        {
            case 0:
                r0.Checked = true;
                break;
            case 2:
                r2.Checked = true;
                break;
            case 3:
                r3.Checked = true;
                break;
        }
        //
        string rs = row["remittance_status"].ToString();
        if (!string.IsNullOrEmpty(rs))
        {
            int nrs = int.Parse(rs);
            if (nrs == 1)
                rs = "线下转款成功";
        }
        l_remittance_status.Text = rs;
        l_remittance_order.Text = row["remittance_order"].ToString();
        l_remittance_time.Text = row["remittance_time"].ToString();
        t_remark.Text = row["remark"].ToString();

        dt.Dispose();
        ds.Dispose();
        if (status > 0)
            b_submit.Enabled = false;
    }

    protected void b_submit_Click(object sender, EventArgs e)
    {
        int status = 0;
        if (r2.Checked)
        {
            status = 2;
        }
        else if (r3.Checked)
            status = 3;
        string remark = t_remark.Text;
        List<string> list = new List<string>();
        string sqlstr = string.Format("update tbl_ad_tx set status={0},admin_id={1},remark='{2}' where tx_id={3};", status, Admin_id, remark, tx_id);
        list.Add(sqlstr);
        if (status == 3)
        {
            if (ViewState["amount"] == null || ViewState["ad_id"] == null)
            {
                Jscript.Alert2(Page, "数据有误，请返回后重试！");
                return;
            }
            float amount = float.Parse(ViewState["amount"].ToString());
            string ad_id = ViewState["ad_id"].ToString();
            list.Add(string.Format("update tbl_ad_mch set mch_money=mch_money+{0} where ad_id={1};", amount, ad_id));
        }
        /*else if (status == 2)
        {
            if (ViewState["no_amount_now"] == null || ViewState["ad_id"] == null)
            {
                Jscript.Alert2(Page, "数据有误，请返回后重试！");
                return;
            }
            int no_amount_now = int.Parse(ViewState["no_amount_now"].ToString());
            string ad_id = ViewState["ad_id"].ToString();
            list.Add(string.Format("update tbl_ad_bank set amount={0} where ad_id={1};", no_amount_now, ad_id));
        }*/
        int result = DbHelperMySQL.ExecuteSqlTran(list);
        list.Clear();
        list = null;
        Response.Redirect("txlist.aspx");
    }
}