using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using LitJson;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Configuration;
public partial class admin_manage_main : AdminPageBasead
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsAdmin)
        {
            Load();
        }
    }
    protected string html = string.Empty;
    public string labels = "";
    public string datasets = "";
    private void Load()
    {


        if (Request.Form.Count > 0)
        {
            switch (Request.Form["action"])
            {
                case "Search_get": Search_Get(); break;
                case "Search_zhichu": Search_Out(); break;

            }

        }
        else
        {
          
                Label1.Text = (MyAdMch.agent_money/100.0).ToString("F4");
                Label6.Text = (MyAdMch.mch_money /100.0).ToString();
                Label7.Text = (MyAdMch.mch_points/100.0).ToString();

                if (!MyAdMch.IsAgent)
                {
                    div1.Style.Add("display", "none");
                    div3.Style.Add("display", "none");
                   

                }

            

           
        }
 
    }
    protected void Search_Out()
    {


        Dictionary<int, double> tl = new Dictionary<int, double>();
       
        Dictionary<string, object> dlist = new Dictionary<string, object>();

        List<string> list = new List<string>();
        for (int i = 0; i < 24; i++)
        {
            list.Add(i.ToString().PadLeft(2, '0') + "时");
            tl[i] = 0;
        }
        dlist["labels"] = list;
    
        float sum = 0f;
        DateTime subtime;
        float money = 0f;
        string sqlstr = string.Format("select * from tbl_wx_mp_fensi where ad_id={0} and subscribe_time>='{1}' and subscribe_time<'{2}';", ad_id, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            subtime = (DateTime)row["subscribe_time"];
            money = float.Parse(row["charge"].ToString());
            sum += money;
            tl[subtime.Hour] += money;
        }
        dt.Dispose();
        ds.Dispose();
        sum = sum / 100f;
      
       
        dlist["fillColor"] = "#87CEFA";
        dlist["type"] = "1";
        dlist["sum"] = sum.ToString();
        List<double> datalist = new List<double>();
        for (int i = 0; i < 24; i++)
        {
            double n = tl[i];
            n = n / 100d;
            datalist.Add(n);
        }
        dlist["data"] = datalist;
        List<Dictionary<string, object>> all = new List<Dictionary<string, object>>();
        all.Add(dlist);
       
        Response.Write(LitJson.JsonMapper.ToJson(all));
        list.Clear();
        list = null;
        dlist.Clear();
        datalist.Clear();
        all.Clear();
        Response.End();
       
      
      
        
    }
    protected void Search_Get()
    {
        Dictionary<int, double> tl = new Dictionary<int, double>();

        Dictionary<string, object> dlist = new Dictionary<string, object>();

        List<string> list = new List<string>();
        for (int i = 0; i < 24; i++)
        {
            list.Add(i.ToString().PadLeft(2, '0') + "时");
            tl[i] = 0;
        }
        dlist["labels"] = list;
       
        float sum = 0f;
        DateTime subtime;
        float money = 0f;




        string sqlstr = string.Format("select sum(amount)as amount ,create_time from tbl_ad_divide_into   where ad_id='{0}' and  create_time>='{1}' and create_time<'{2}' group by create_time;", ad_id, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            subtime = (DateTime)row["create_time"];
            money = float.Parse(row["amount"].ToString());
            sum += money;
            tl[subtime.Hour] += money;
        }
        dt.Dispose();
        ds.Dispose();
        sum = sum / 100f;

    
        dlist["fillColor"] = "#05CE3E";
        dlist["type"] = "2";
        dlist["sum"] = sum.ToString("F4");
        List<string> datalist = new List<string>();
        for (int i = 0; i < 24; i++)
        {
            double n = tl[i];
            n = n / 100d;
            datalist.Add(n.ToString("F4"));
        }
        dlist["data"] = datalist;
        List<Dictionary<string, object>> all = new List<Dictionary<string, object>>();
        all.Add(dlist);

        Response.Write(LitJson.JsonMapper.ToJson(all));
        list.Clear();
        list = null;
        dlist.Clear();
        datalist.Clear();
        all.Clear();
        Response.End();
    }
}