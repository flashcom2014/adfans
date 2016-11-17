using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using LitJson;
using MySql.Data.MySqlClient;

public partial class payamount_statistics : AdminPageBasead
{
    public string labels = "";
    public string datasets = "";
    public string title = "";
   
    Dictionary<int, Dictionary<int, int>> list = new Dictionary<int, Dictionary<int, int>>();
    Dictionary<int, Dictionary<string, int>> list_day = new Dictionary<int, Dictionary<string, int>>();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (ad_id == 0)
        {
            Jscript.Alert2(this.Page, "参数错误");
        }
        if (!IsPostBack)
        {
            t_date.Value = DateTime.Now.ToString("yyyy-MM-dd");
            ToDay();
        }
        
    }

    protected void d_list_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = d_list.SelectedIndex;
        if (index >= 0)
        {
            t_date.Value = DateTime.Now.ToString("yyyy-MM-dd");
            SetDatas(index);
        }
    }

    private void SetDatas(int index)
    {
        p_mchid.Visible = true; 
        d_yang.Visible = false;
        switch (index)
        {
            case 0:
                l_date.Text = "查询日期：";
                ToDay();
                break;
            case 1:
                l_date.Text = "结束日期：";
                Days30();
                break;
          
        }
    }



 

  
   



   


    private void Days30()
    {
        DateTime theday = DateTime.Now;
        if (!DateTime.TryParse(t_date.Value, out theday))
        {
            theday = DateTime.Now;
            t_date.Value =theday.ToString("yyyy-MM-dd");
        }
        int days = 29;  //30天相减少1天，用29
        DateTime day30 = theday.Date.AddDays(-days);
        Dictionary<string, double> tl = new Dictionary<string, double>();
        List<string> list = new List<string>();
        string sday = "";
        for (int i = 0; i <= days; i++)
        {
            sday = day30.AddDays(i).ToString("MM-dd");
            list.Add(sday);
            sday = day30.AddDays(i).ToString("yyyy-MM-dd");
            tl[sday] = 0d;
        }
        labels = JsonMapper.ToJson(list);
        list.Clear();
        list = null;
        //
        double sum = 0d;
        DateTime day_time;
        double money = 0d;
        
        string sqlstr = string.Format("select * from tbl_wx_mp_fensi_day where ad_id={0} and day_time>='{1}' and day_time<='{2}';", ad_id, day30.ToString("yyyy-MM-dd"), theday.ToString("yyyy-MM-dd"));
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            day_time = (DateTime)row["day_time"];
            money = double.Parse(row["ad_money"].ToString());
            sum += money;
            sday = day_time.ToString("yyyy-MM-dd");
            if (tl.ContainsKey(sday))
                tl[sday] += money;
        }
        dt.Dispose();
        ds.Dispose();
        sum = sum / 100d;
        title = "[" + day30.ToString("yyyy-MM-dd") + "]到[" + theday.ToString("yyyy-MM-dd") + "]总共支出：<span style=\"color:red;\">" + sum.ToString("F2") + "</span>元";
        //
        Dictionary<string, object> dlist = new Dictionary<string, object>();
        dlist["fillColor"] = "#05CE3E";
        List<double> datalist = new List<double>();
        for (int i = 0; i <= days; i++)
        {
            double n = tl[day30.AddDays(i).ToString("yyyy-MM-dd")];
            n = n / 100d;
            datalist.Add(n);
        }
        dlist["data"] = datalist;
        List<Dictionary<string, object>> all = new List<Dictionary<string, object>>();
        all.Add(dlist);
        datasets = JsonMapper.ToJson(all);
        dlist.Clear();
        datalist.Clear();
        all.Clear();
    }

    private void ToDay()
    {
        DateTime theday = DateTime.Now;
        if (!DateTime.TryParse(t_date.Value, out theday))
        {
            theday = DateTime.Now;
            t_date.Value = theday.ToString("yyyy-MM-dd");
        }

        Dictionary<int, double> tl = new Dictionary<int, double>();

        List<string> list = new List<string>();
        for (int i = 0; i < 24; i++)
        {
            list.Add(i.ToString().PadLeft(2, '0') + "时");
            tl[i] = 0;
        }
        labels = JsonMapper.ToJson(list);
        list.Clear();
        list = null;
        //
        float sum = 0f;
        DateTime subtime;
        float money = 0f;
        string sqlstr = string.Format("select * from tbl_wx_mp_fensi where ad_id={0} and subscribe_time>='{1}' and subscribe_time<'{2}';", ad_id, theday.ToString("yyyy-MM-dd"), theday.AddDays(1).ToString("yyyy-MM-dd"));
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
        title = "[" + theday.ToString("yyyy-MM-dd") + "]总共支出：<span style=\"color:red;\">" + sum.ToString("F2") + "</span>元";
        //
        Dictionary<string, object> dlist = new Dictionary<string, object>();
        dlist["fillColor"] = "#05CE3E";
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
        datasets = JsonMapper.ToJson(all);
        dlist.Clear();
        datalist.Clear();
        all.Clear();
    }

    protected void b_search_Click(object sender, EventArgs e)
    {
        SetDatas(d_list.SelectedIndex);
    }

    protected void b_prv_Click(object sender, EventArgs e)
    {
        DateTime theday = DateTime.Now;
        if (!DateTime.TryParse(t_date.Value, out theday))
        {
            theday = DateTime.Now;
           
        }
        t_date.Value = theday.AddDays(-1).ToString("yyyy-MM-dd");
        SetDatas(d_list.SelectedIndex);
    }

    protected void b_next_Click(object sender, EventArgs e)
    {
        DateTime theday = DateTime.Now;
        if (!DateTime.TryParse(t_date.Value, out theday))
            theday = DateTime.Now;
        t_date.Value = theday.AddDays(1).ToString("yyyy-MM-dd");
        SetDatas(d_list.SelectedIndex);
    }

   

   


   
}