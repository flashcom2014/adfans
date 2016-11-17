using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;
using pagetablead;

public partial class profit_share : AdminPageBasead
{
    protected string html = "";
    protected string condition = " ";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Form.Count > 0)
        {
            switch (Request.Form["action"])
            {
                case "Search_Click": Data(); break;
                case "pre_Click": Data(); break;
                case "next_Click": Data(); break;
                case "getchild": GetchildList(); break;
            }

        }
        else
        {
            t_date1.Value = DateTime.Now.ToString("yyyy-MM-dd");
            t_date2.Value = DateTime.Now.ToString("yyyy-MM-dd");

        }



    }
    protected void Data()
    {
        string date = Request.Form["fdate"];
        string date2 = Request.Form["edate"];
        string choose = Request.Form["choose"];
        DateTime theday = DateTime.Now;
        DateTime theday2 = DateTime.Now;

        int pPageIndex = 1;
        int pPageSize = 20;
        if (!int.TryParse(Request.Form["PageIndex"], out pPageIndex))//每页记录数
        {
            Response.Write("{\"err\":\"页数不正确\"}");
            Response.End();
            return;
        }
        pPageSize = int.Parse(Request.Form["PageSize"]);


        if (!DateTime.TryParse(date, out theday))
        {
            Response.Write("{\"err\":\"起始时间格式不正确\"}");
            Response.End();
            return;

        }
        if (!DateTime.TryParse(date2, out theday2))
        {
            Response.Write("{\"err\":\"结束时间格式不正确\"}");
            Response.End();
            return;

        }

        if (choose != "")
        {
            condition += " and f.child_id='" + choose + "'";
        }

        int pTop = (pPageIndex - 1) * pPageSize;
        string fmt = "select d.ad_id,d.ad_mobile,d.ad_name,f.amount ,f.actual_scale,f.scale,f.child_id,f.child_scale,f.money,f.create_time from tbl_ad_mch as d";
        fmt += " right join  (";
        fmt += " select ad_id, amount ,actual_scale,scale,child_id,child_scale,money,create_time from tbl_ad_divide_into where create_time>='{0}' and create_time<'{1}'";
        fmt += ") As f";
        fmt += " ON f.child_id=d.ad_id  where f.ad_id={2}{3} order by f.create_time desc";

        string strSel = string.Format(fmt, theday.ToString("yyyy-MM-dd"), theday2.AddDays(1).ToString("yyyy-MM-dd"), ad_id, condition);
        string sql = strSel + " limit " + pTop + "," + pPageSize;
        sql += ";select count(1) as total from (" + strSel + ") as c";
        DataSet ds = DbHelperMySQL.Query(sql);
        DataTable dtcount = ds.Tables[1];
        int count = 0;
        if (dtcount.Rows.Count > 0)
        {
            count = int.Parse(dtcount.Rows[0]["total"].ToString());
        }

        int pPageCount = 0;//总页数
        PageDataTable pd = null;
        if (count == 0 || (pPageIndex - 1) * pPageSize >= count)
        {
            pd = new PageDataTable(pPageIndex, pPageSize, 0, 1, new DataTable());
            Response.Write(LitJson.JsonMapper.ToJson(pd));
            Response.End();
            return;
        }

        if (count % pPageSize == 0)
            pPageCount = count / pPageSize;
        else
            pPageCount = count / pPageSize + 1;



        dtcount.Dispose();
        DataTable dt = ds.Tables[0];
        pd = new PageDataTable(pPageIndex, pPageSize, count, pPageCount, dt);
        Response.Write(LitJson.JsonMapper.ToJson(pd));
        Response.End();
        return;
    }
    protected void GetchildList()
    {
        string sqlstr = string.Format("select ad_id,ad_name,ad_mobile from tbl_ad_mch where parent_id={0}", ad_id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            Response.Write("{\"err\":\"你还没有子广告商\"}");
            Response.End();
            return;
        }
        PageDataTable pd = new PageDataTable(0, 0, 0, 0, dt);
        Response.Write(LitJson.JsonMapper.ToJson(pd));
        Response.End();

    }
   


    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    LoadData(true);
    //}

    //private void LoadData(bool iscache)
    //{
    //    if (IsPostBack && iscache)
    //    {
    //        if (ViewState["dt"] == null)
    //        {
    //            LoadData(false);
    //            return;
    //        }
    //        DataTable dt = (DataTable)ViewState["dt"];
    //        gv_mch.DataSource = dt;
    //        gv_mch.DataBind();
    //        if (gv_mch.HeaderRow != null)
    //            gv_mch.HeaderRow.TableSection = TableRowSection.TableHeader;
    //    }
    //    else
    //    {
    //        string sqlstr = string.Format("select m.*,sum(d.amount) as amount from tbl_ad_mch as m left join (select * from tbl_ad_divide_into where create_time>='{0}') as d on m.parent_id=d.ad_id and m.ad_id=d.child_id where m.parent_id={1} group by d.ad_id;", DateTime.Now.AddDays(-30), ad_id);
    //        DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
    //        gv_mch.DataSource = dt;
    //        gv_mch.DataBind();
    //        if (gv_mch.HeaderRow != null)
    //            gv_mch.HeaderRow.TableSection = TableRowSection.TableHeader;
    //        ViewState["dt"] = dt;
    //    }
    //}

  
}