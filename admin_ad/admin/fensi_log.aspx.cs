using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class admin_ad_admin_fensi_log : AdminPageBasead
{
    public string admch_html = "";
    public string adagent_html = "";
    public string mch_html = "";

    private int sn = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = Request.QueryString["id"];
        if (string.IsNullOrEmpty(id))
        {
            Response.Write("参数错误！");
            Response.End();
        }
        if (!int.TryParse(id, out sn))
        {
            Response.Write("参数错误！");
            Response.End();
        }

        LoadDatas();
    }

    private void LoadDatas()
    {
        string sqlstr = string.Format("select f.*,d.device_name,d.des,a.mch_acc,m.ad_mobile,m.ad_name from tbl_wx_mp_fensi as f left join tbl_device as d on f.device_id=d.device_id left join tbl_mch_account as a on d.mch_id=a.mch_id left join tbl_ad_mch as m on f.ad_id=m.ad_id where sn={0};", sn);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            Response.Write("参数错误！");
            Response.End();
        }
        string mch_name = dt.Rows[0]["mch_acc"].ToString();
        string dname = dt.Rows[0]["device_name"].ToString();
        string ddes = dt.Rows[0]["des"].ToString();
        string device_name = dname;
        if (!string.IsNullOrEmpty(ddes))
            device_name += "(" + ddes + ")";
        float get_amount = float.Parse(dt.Rows[0]["get_amount"].ToString()) / 100f;
        string subscribe_time = dt.Rows[0]["subscribe_time"].ToString();
        string appid = dt.Rows[0]["appid"].ToString();
        string nickname = dt.Rows[0]["wx_nickname"].ToString();
        nickname = HttpUtility.UrlDecode(nickname);
        string headimgurl = dt.Rows[0]["wx_headimgurl"].ToString();
        float charge = float.Parse(dt.Rows[0]["charge"].ToString()) / 100f;
        string ad_mobile = dt.Rows[0]["ad_mobile"].ToString();
        string ad_name = dt.Rows[0]["ad_name"].ToString();
        dt.Dispose();
        ds.Dispose();
        mch_html = string.Format("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>", mch_name, device_name, get_amount.ToString("F2"), subscribe_time);
        string admch_name = ad_name;
        if (!string.IsNullOrEmpty(admch_name))
            admch_name += "(" + ad_mobile + ")";
        admch_html = string.Format("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td><a href=\"#\" class=\"link\" ><img src='{4}' alt=\"引流图片\" class=\"wximg\" /><img src = '{4}' alt=\"引流图片\" class=\"wximgbig\" /></a></td><td>{5}</td>", admch_name, appid, charge.ToString("F2"), nickname, headimgurl, subscribe_time);

        sqlstr = string.Format("select adm.ad_mobile as admobile,adm.ad_name as adname,d.amount,d.actual_scale,cm.ad_mobile as cmobile,cm.ad_name as cname,d.create_time from tbl_ad_divide_into as d join tbl_ad_mch as adm on d.ad_id=adm.ad_id join tbl_ad_mch as cm on cm.ad_id=d.child_id where d.fensi_id={0} order by d.sn;", sn);
        ds = DbHelperMySQL.Query(sqlstr);
        dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            return;
        }
        float amount = 0f;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<tr>");
            sb.Append("<td>" + row["adname"].ToString() + "(" + row["admobile"].ToString() + ")</td>");
            amount = float.Parse(row["amount"].ToString()) / 100f;
            sb.Append("<td>" + amount.ToString("F4") + "</td>");
            sb.Append("<td>" + row["actual_scale"].ToString() + "%</td>");
            sb.Append("<td>" + row["cname"].ToString() + "(" + row["cmobile"].ToString() + ")</td>");
            sb.Append("<td>" + row["create_time"].ToString() + "</td></tr>");
        }
        dt.Dispose();
        ds.Dispose();
        adagent_html = sb.ToString();
        sb.Clear();
    }
}