using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;
using System.Net;
using LitJson;
using System.Collections;
using pagetablead;

public partial class focusrecord_list : AdminPageBasead
{
    protected string html = "";
    protected string condition = " ";

    DataTable devices = new DataTable();
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Form.Count > 0)
        {
            switch (Request.Form["action"])
            {
                case "Search_Click":Data(); break;
                case "pre_Click": Data(); break;
                case "next_Click": Data(); break;
                case "getappid": GetappidList(); break;
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
        int pPageSize =20;
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
            condition += " and f.appid='" + choose + "'";
        }

        int pTop = (pPageIndex - 1) * pPageSize;
        string fmt = "select d.tags,d.appid,f.wx_nickname ,f.wx_headimgurl,f.subscribe_time,f.charge from tbl_wx_xifenji as d";
        fmt += " right join  (";
        fmt += " select appid, subscribe_time ,wx_nickname,wx_headimgurl,charge from tbl_wx_mp_fensi where subscribe_time>='{0}' and subscribe_time<'{1}'";
        fmt += ") As f";
        fmt += " ON d.appid = f.appid where enable=1 and ad_id={2}{3} order by f.subscribe_time desc";
         
        string strSel = string.Format(fmt, theday.ToString("yyyy-MM-dd"), theday2.AddDays(1).ToString("yyyy-MM-dd"), ad_id, condition);
        string sql = strSel + " limit " + pTop + "," + pPageSize;
        sql += ";select count(1) as total from (" + strSel + ") as c";
        DataSet ds= DbHelperMySQL.Query(sql);
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
    protected void GetappidList()
    {
        string sqlstr = string.Format("select appid,tags from tbl_wx_xifenji where ad_id={0}", ad_id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            Response.Write("{\"err\":\"你还没有添加公众号\"}");
            Response.End();
            return;
        }
        PageDataTable pd = new PageDataTable(0, 0, 0, 0, dt);
        Response.Write(LitJson.JsonMapper.ToJson(pd));
        Response.End();

    }
   
}