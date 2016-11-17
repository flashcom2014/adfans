using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class admin_ad_admin_history_list : AdminPageBasead
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadDatas();
    }

    private void LoadDatas()
    {
        string sqlstr = "select f.*,d.device_name,d.des,m.ad_name,m.ad_mobile,acc.mch_acc from tbl_wx_mp_fensi as f left join tbl_ad_mch as m on f.ad_id=m.ad_id left join tbl_device as d on d.device_id=f.device_id join tbl_mch_account as acc on d.mch_id=acc.mch_id";
        if (!string.IsNullOrEmpty(t_admch.Text) || !string.IsNullOrEmpty(t_appid.Text) || !string.IsNullOrEmpty(t_date.Text))
            sqlstr += " where";
        if (!string.IsNullOrEmpty(t_admch.Text))
        {
            sqlstr += string.Format(" (m.ad_name like '%{0}%' or m.ad_mobile like '%{0}%') and", t_admch.Text);
        }
        if (!string.IsNullOrEmpty(t_appid.Text))
        {
            sqlstr += string.Format(" f.appid like '%{0}%' and", t_appid.Text);
        }
        if (!string.IsNullOrEmpty(t_date.Text))
        {
            sqlstr += string.Format(" f.subscribe_time>='{0}' and f.subscribe_time<='{0} 23:59:59.999'", t_date.Text);
        }
        if (sqlstr.EndsWith(" and"))
            sqlstr = sqlstr.Remove(sqlstr.Length - 4);
        sqlstr += " order by f.sn desc limit 100;";
        DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
        gv_list.DataSource = dt;
        gv_list.DataBind();
        if (gv_list.HeaderRow != null)
            gv_list.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    public string GetMchAcc()
    {
        string acc = Eval("mch_acc").ToString();
        string mch_id = Eval("ad_id").ToString();
        string color = mch_id == "0" ? "red" : "blue";
        return string.Format("<span style=\"color:{0}\">{1}</span>", color, acc);
    }

    public string GetDeciveName()
    {
        string name = Eval("device_name").ToString();
        string des = Eval("des").ToString();

        string restr = name;
        if (!string.IsNullOrEmpty(des))
        {
            restr += "(" + des + ")";
        }
        return restr;
    }

    public string GetAdName()
    {
        string name = Eval("ad_name").ToString();
        string mobile = Eval("ad_mobile").ToString();
        if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(mobile))
            return string.Empty;
        return string.Format("{0}({1})", name, mobile);
    }
}