using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;

public partial class admin_ad_agent_mch_manage : AdminPageBasead
{
    public bool iseditscale = false;
    public string max = "0";
    public string myscale = "0";
    public string mchscalevalue = "0";
    public string minamount = "1.5";

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadData(true);
    }

    private void LoadData(bool iscache)
    {
        if (IsPostBack && iscache)
        {
            if (ViewState["dt"] == null)
            {
                LoadData(false);
                return;
            }
            DataTable dt = (DataTable)ViewState["dt"];
            gv_mch.DataSource = dt;
            gv_mch.DataBind();
            if (gv_mch.HeaderRow != null)
                gv_mch.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        else
        {
            string sqlstr = string.Format("select m.*,sum(d.amount) as amount from tbl_ad_mch as m left join (select * from tbl_ad_divide_into where create_time>='{0}') as d on m.parent_id=d.ad_id and m.ad_id=d.child_id where m.parent_id={1} group by d.ad_id;", DateTime.Now.AddDays(-30), ad_id);
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            gv_mch.DataSource = dt;
            gv_mch.DataBind();
            if (gv_mch.HeaderRow != null)
                gv_mch.HeaderRow.TableSection = TableRowSection.TableHeader;
            ViewState["dt"] = dt;
        }
    }

    public string MyScale()
    {
        if (MyAdMch == null)
            return "0%";
        float scale = float.Parse(Eval("scale_points").ToString());
        float real_scale = MyAdMch.scale_points - scale;
        return real_scale.ToString() + "%";
    }

    protected void b_edit_Command(object sender, CommandEventArgs e)
    {
        string theid = e.CommandName.ToString();
        string sqlstr = string.Format("select * from tbl_ad_mch where ad_id={0};", theid);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            l_acc.Text = string.Format("{0}({1})", dt.Rows[0]["ad_name"], dt.Rows[0]["ad_mobile"]);
            mchscalevalue = dt.Rows[0]["scale_points"].ToString();
            l_scale.Text = mchscalevalue + "%";
            mchscalevalue = (float.Parse(mchscalevalue) * 100f).ToString();
            float scale = MyAdMch.scale_points;
            myscale = scale.ToString() + "%";
            max = (scale * 100f).ToString();
            t_amount.Value = (float.Parse(dt.Rows[0]["sub_amount"].ToString()) / 100f).ToString("F2");
            minamount = ((float)MyAdMch.sub_amount / 100f).ToString("F2");
            l_minamount.Text = string.Format(" (不能低于{0}元/个)", minamount);
            h_id.Value = theid;
        }
        dt.Dispose();
        ds.Dispose();
        iseditscale = true;
    }

    protected void b_edit_Click(object sender, EventArgs e)
    {
        string sscale = Request.Form["scale"];
        if (string.IsNullOrEmpty(sscale))
        {
            Jscript.Alert2(Page, "参数错误！");
            return;
        }
        string samount = Request.Form["t_amount"];
        if (string.IsNullOrEmpty(samount))
        {
            Jscript.Alert2(Page, "参数错误！");
            return;
        }
        ViewState["amount"] = samount;
        //
        float amount = float.Parse(samount) * 100f;
        if (amount < MyAdMch.sub_amount)
        {
            Jscript.Alert2(Page, "非法操作！你的IP已被记录！");
            return;
        }

        float scale = float.Parse(sscale);
        if (scale > MyAdMch.scale_points)
        {
            Jscript.Alert2(Page, "非法操作！你的IP已被记录！");
            return;
        }

        string id = h_id.Value;

        string sqlstr = string.Format("select * from tbl_ad_mch where ad_id={0} and parent_id={1};", id, ad_id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            Jscript.Alert2(Page, "非法操作！你的IP已被记录！");
            return;
        }
        float child_scale = float.Parse(dt.Rows[0]["scale_points"].ToString());
        dt.Dispose();
        ds.Dispose();
        if (scale < child_scale)
        {
            Jscript.Alert2(Page, "非法操作！你的IP已被记录！");
            return;
        }
        sqlstr = string.Format("update tbl_ad_mch set scale_points={0},sub_amount={2} where ad_id={1};", scale, id, amount);
        int result = DbHelperMySQL.ExecuteSql(sqlstr);

        LoadData(false);
    }
}