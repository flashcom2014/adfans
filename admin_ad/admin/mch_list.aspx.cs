using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;

public partial class admin_ad_admin_mch_list : AdminPageBasead
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadDatas(true);
    }

    private void LoadDatas(bool iscache)
    {
        if (ViewState["gv"] != null && iscache)
        {
            DataTable dt = (DataTable)ViewState["gv"];
            gv_mch.DataSource = dt;
            gv_mch.DataBind();
            gv_mch.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        else
        {
            string sqlstr = "select * from tbl_ad_mch";
            bool b1 = t_acc.Text == "" ? false : true;
            bool b2 = t_mobile.Text == "" ? false : true;
            if (b1 || b2)
                sqlstr += " where";
            if (b1)
                sqlstr += string.Format(" ad_acc like '%{0}%' and", t_acc.Text);
            if (b2)
                sqlstr += string.Format(" ad_mobile like '%{0}%'", t_mobile.Text);
            if (sqlstr.EndsWith("and"))
                sqlstr = sqlstr.Remove(sqlstr.Length - 3);
            sqlstr += " order by ad_id desc limit 100;";
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            gv_mch.DataSource = dt;
            gv_mch.DataBind();
            gv_mch.HeaderRow.TableSection = TableRowSection.TableHeader;
            ViewState["gv"] = dt;
        }
    }

    public string IsAgent()
    {
        float scale_points = float.Parse(Eval("scale_points").ToString());
        if (scale_points > 0)
            return "<label class='agent'>代理</label>";
        return "<label class='mch'>商家</label>";
    }

    protected void b_recharge_Command(object sender, CommandEventArgs e)
    {
        p_list.Visible = false;
        p_recharge.Visible = true;

        t_money.Text = t_points.Text = "0";

        string id = e.CommandName;
        string sqlstr = string.Format("select * from tbl_ad_mch where ad_id={0};", id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            return;
        }
        l_acc.Text = string.Format("{0}({1})", dt.Rows[0]["ad_name"], dt.Rows[0]["ad_mobile"]);
        l_mch_money.Text = string.Format("{0}元", ((float.Parse(dt.Rows[0]["mch_money"].ToString())/100f).ToString("F2")));
        l_points.Text = string.Format("{0}元", ((float.Parse(dt.Rows[0]["mch_points"].ToString()))/100f).ToString("F2"));
        ViewState["id"] = dt.Rows[0]["ad_id"].ToString();
        dt.Dispose();
        ds.Dispose();
    }

    protected void b_find_Click(object sender, EventArgs e)
    {
        LoadDatas(false);
        p_list.Visible = true;
        p_recharge.Visible = false;
    }

    protected void b_money_Click(object sender, EventArgs e)
    {
        try
        {
            float money = 0f;
            if (!float.TryParse(t_money.Text, out money))
            {
                Jscript.Alert2(Page, "请输入正确的金额！");
                return;
            }
            if (money <= 0)
            {
                Jscript.Alert2(Page, "请输入正确的金额！");
                return;
            }
            money = money * 100f;
            string id = ViewState["id"].ToString();
            List<string> tl = new List<string>();
            tl.Add(string.Format("update tbl_ad_mch set mch_money=mch_money+{0} where ad_id={1};", money, id));
            tl.Add(string.Format("insert into tbl_ad_recharge_log(ad_id,amount,types,admin_id) values({0},{1},0,{2});", id, money, Admin_id));
            int result = DbHelperMySQL.ExecuteSqlTran(tl);
            p_list.Visible = true;
            p_recharge.Visible = false;
            LoadDatas(false);
        }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("mch_list: " + ex.ToString());
            Jscript.Alert2(Page, "系统错，请稍候再试！");
        }
    }

    protected void b_points_Click(object sender, EventArgs e)
    {
        try
        {
            float points = 0f;
            if (!float.TryParse(t_points.Text, out points))
            {
                Jscript.Alert2(Page, "请输入正确的金额！");
                return;
            }
            if (points <= 0)
            {
                Jscript.Alert2(Page, "请输入正确的金额！");
                return;
            }
            points = points * 100f;
            string id = ViewState["id"].ToString();
            List<string> tl = new List<string>();
            tl.Add(string.Format("update tbl_ad_mch set mch_points=mch_points+{0} where ad_id={1};", points, id));
            tl.Add(string.Format("insert into tbl_ad_recharge_log(ad_id,amount,types,admin_id) values({0},{1},3,{2});", id, points, Admin_id));
            int result = DbHelperMySQL.ExecuteSqlTran(tl);
            p_list.Visible = true;
            p_recharge.Visible = false;
            LoadDatas(false);
        }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("mch_list: " + ex.ToString());
            Jscript.Alert2(Page, "系统错，请稍候再试！");
        }
    }

    protected void b_cancel_Click(object sender, EventArgs e)
    {
        p_list.Visible = true;
        p_recharge.Visible = false;
    }
}