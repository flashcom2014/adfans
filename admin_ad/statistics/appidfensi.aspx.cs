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
namespace buildtablelf
{
    public class bulidtable_ad
    {
        public string html = string.Empty;
        public string SQL = string.Empty;
        public List<string> Columns = null;
        public string Title = "Untitle";
        public string Xdes = "X";
        public string Ydes = "Y";
        public int Width = 850;
        public int Height = 320;
        public Page WebPage = null;
        public string TableCSS = "adtable";
        public string TableId = "StatisticView1";
        public Dictionary<string, string> mapDeviceTitle = new Dictionary<string, string>();
        public int CalcBase = 0;
        int rowCount = 10;
        int pageNumber = 1;
        int pageCount = 1;
        public bulidtable_ad(Page _WebPage)
        {
            WebPage = _WebPage;
        }

        private DataTable GetStatisticData(string SQL, List<string> Columns)
        {
            HashSet<string> setDevice = new HashSet<string>();
            Dictionary<string, string> mapDeviceCoin = new Dictionary<string, string>();
            DataTable dt = DbHelperMySQL.Query(SQL).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string device_name = dr["appid"].ToString();
                string date = dr["xftime"].ToString();
                string coin = dr["total"].ToString();


                if (dt.Columns.Contains("tags"))
                {
                    string des = dr["tags"].ToString();
                    if (des.Length > 0 && !mapDeviceTitle.ContainsKey(device_name))
                        mapDeviceTitle.Add(device_name, des);
                }

                string key = device_name + "_" + date;
                mapDeviceCoin.Add(key, coin);
                setDevice.Add(device_name);
            }

            #region 新建DataTable
            DataTable tblDatas = new DataTable("Datas");
            tblDatas.Columns.Add("appid", Type.GetType("System.String"));
            foreach (string date in Columns)
            {
                tblDatas.Columns.Add(date, Type.GetType("System.String"));
            }
            tblDatas.Columns.Add("sum", Type.GetType("System.String"));


            foreach (string device_name in setDevice)
            {
                DataRow rr = tblDatas.NewRow();
                rr["appid"] = device_name;

                decimal sum = 0m;
                foreach (string date in Columns)
                {
                    string key = device_name + "_" + date;
                    string count = string.Empty;
                    if (mapDeviceCoin.ContainsKey(key))
                    {
                        count = mapDeviceCoin[key];
                        sum += decimal.Parse(count);
                    }
                    else
                        count = "-";

                    rr[date] = count;
                }

                rr["sum"] = sum.ToString("#.##");
                tblDatas.Rows.Add(rr);
            }

            #endregion
            return tblDatas;
        }


        public string Build(out List<decimal> DataList)
        {
            DataTable dt = GetStatisticData(SQL, Columns);

            html = string.Format("<table id=\"{1}\" class=\"{0}\" style='width:100%' cellpadding='3' cellspacing='1' ><thead>", TableCSS, TableId);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string dd = dt.Columns[i].ToString();
                if (dd == "appid")
                    dd = "公众号";
                else if (dd == "sum")
                    dd = "小计";
                html += "<th scope='col'>" + dd + "</th>";
            }
            html += "</thead><tbody>";

            decimal[] sum_list = new decimal[dt.Columns.Count];
        
           
            //if (rowCount < 0)
            //    rowCount = 10;
            //if (pageNumber < 1)
            //    pageNumber = 1;

            //int count = dt.Rows.Count;
            //if (count <= rowCount)
            //    pageCount = 1;
            //else
            //{
            //    pageCount = count / rowCount;
            //    if (count % rowCount > 0)
            //        pageCount++;
            //}

            //DataTable dtCopy = dt.Copy();
            //DataView dv = dt.DefaultView;
            //dtCopy = dv.ToTable();
            //DataTable NewTable=null;
            //int TopItem = 0;
            //TopItem=(pageNumber-1)*rowCount;
            //NewTable = dt.Clone();
            //DataRow[] rows = dt.Select("1=1");
            //for (int i = TopItem; i < TopItem + rowCount; i++)
            //{
            //    if (dt.Rows.Count > i)
            //        NewTable.ImportRow((DataRow)rows[i]);
            //}



            foreach (DataRow dr in dt.Rows)
            {
                html += "<tr align='center'>";

                string device_name = dr["appid"].ToString();
                string des = string.Empty;
                string html_desc = "";
                if (mapDeviceTitle.ContainsKey(device_name))
                {
                    des = mapDeviceTitle[device_name];
                    html_desc = string.Format("<span class=\"appid_desc\"> - {0}</span>", des);
                }

                html += string.Format("<td align='center' title='{1}'><span class=\"appid_name\">{0}</span>{2}</td>", device_name, des, html_desc);
                decimal sum = 0m;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    string dd = dt.Columns[i].ToString();
                    decimal val = 0m;
                    if (decimal.TryParse(dr[dd].ToString(), out val))
                    {
                        if (CalcBase > 0)
                            val /= CalcBase;

                        sum += val;
                        sum_list[i] += val;
                        html += "<td>" + val + "</td>";
                    }
                    else
                    {
                        html += "<td>" + dr[dd] + "</td>";
                    }
                }
                html += "</tr>";
            }
            html += "</tbody><tfoot><tr align='center'>";
            html += "<th align='center' class='td_sum'>合计</th>";
            for (int i = 1; i < sum_list.Length; i++)
            {
                decimal sum = sum_list[i];
                html += "<th align='center' class='td_sum'>";
                html += (sum == 0) ? "-" : sum.ToString("#.##");
                html += "</th>";
            }
            html += "</tr></tfoot></table><br/>";


            DataList = new List<decimal>();
            DataTable tblDatas = new DataTable();
            tblDatas.Columns.Add();
            tblDatas.Columns.Add();
            for (int i = 1; i < (dt.Columns.Count - 1); i++)
            {
                DataRow rr = tblDatas.NewRow();
                decimal sum = sum_list[i];
                rr[0] = dt.Columns[i].ToString();
                rr[1] = sum.ToString("0.##");
                tblDatas.Rows.Add(rr);

                DataList.Add(sum);
            }

            return html;
        }
    }
}

public partial class appidfensi_statistics : AdminPageBasead
{
    public string labels = "";
    public string datasets = "";
    public string title = "";

    public string html = "";
    int curpage = 0;
    int pagecount = 16;
    public string appidlist = "";
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
            GetappidList();
            MP_Today();
        }

        if (ViewState["page"] != null)
            curpage = (int)ViewState["page"];
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
    protected void appid_list_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = d_list.SelectedIndex;
        if (index >= 0)
        {
            t_date.Value = DateTime.Now.ToString("yyyy-MM-dd");
            SetDatas(index);
        }
    }
    protected void GetappidList()
    {
        string sqlstr = string.Format("select * from tbl_wx_xifenji where ad_id={0}",ad_id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        this.appid_list.Items.Clear();
        this.appid_list.Items.Add(new ListItem("全部", ""));
        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            //title = "<span style=\"color:red;\">您还没有添加公众号</span>";
            Jscript.Alert("您还没有添加公众号");
            return;
        }

        foreach (DataRow row in dt.Rows)
        {

            this.appid_list.Items.Add(new ListItem(row["appid"].ToString() + "-" + row["tags"].ToString(), row["appid"].ToString()));
            //appidlist += string.Format("<asp:ListItem  Value='{0}' />&nbsp;{1}&nbsp;", row["appid"], row["appid"] + "(" + row["tags"]+")");
        }
 
    }
    private void SetDatas(int index)
    {
        p_mchid.Visible = true;
        p_device.Visible = false;
        l_title.Visible = true;
        d_yang.Visible = false;
        switch (index)
        {       
            case 0:
                l_date.Text = "查询日期：";
                MP_Today();
                break;
             case 1:
                l_date.Text = "结束日期：";
                MP_Days30();
                break;
          
        }
    }



  

   



  

    private void MP_Days30()
    {
        p_device.Visible = true;
      
        d_yang.Visible = false;
        DateTime theday = DateTime.Now;
        if (!DateTime.TryParse(t_date.Value, out theday))
        {
            theday = DateTime.Now;
            t_date.Value = theday.ToString("yyyy-MM-dd");
        }

        string where = "";
        if (appid_list.SelectedValue != "")
            where = string.Format(" and d.appid='{0}' ", appid_list.SelectedValue);

        buildtablelf.bulidtable_ad stat = new buildtablelf.bulidtable_ad(this);
        stat.Ydes = "吸粉数";
        stat.Columns = new List<string>();


        int days = 29;  //30天相减少1天，用29
        DateTime day30 = theday.Date.AddDays(-days);
    
        string sday = "";
        for (int i = 0; i <= days; i++)
        {
            sday = day30.AddDays(i).ToString("MM-dd");
            stat.Columns.Add(sday);

           
        }





        string fmt = "select d.tags,d.appid,case when f.total is null then 0 else f.total end as total ,f.day_time as xftime from tbl_wx_xifenji as d";
        fmt += " left join  (";
        fmt += " select appid, DATE_FORMAT(day_time,'%m-%d')as day_time ,sum(appid_count) as total from tbl_wx_mp_fensi_day where day_time>='{0}' and day_time<='{1}' group by appid,day_time";
        fmt += ") As f";
        fmt += " ON d.appid = f.appid where enable=1 and ad_id={2}{3} order by f.total ";
        stat.SQL = string.Format(fmt, day30.ToString("yyyy-MM-dd"), theday.ToString("yyyy-MM-dd"), ad_id, where);
        stat.Xdes = "天数";
        stat.Title = stat.Ydes + " - " + theday.Month + "月";
        List<decimal> data_list;
        html = stat.Build(out data_list);

        List<string> list = new List<string>();
        List<decimal> datalist = new List<decimal>();
        decimal sum = 0;
        for (int i = 0; i < data_list.Count; i++)
        {
            decimal value = data_list[i];
            list.Add(stat.Columns[i]);
            sum += value;
            datalist.Add(value);
        }

        labels = JsonMapper.ToJson(list);
        list.Clear();
        list = null;

        if (appid_list.SelectedValue != "")
            title = "[" + day30.ToString("yyyy-MM-dd") + "]到[" + theday.ToString("yyyy-MM-dd") + "]此公众号获得粉丝：<span style=\"color:red;\">" + sum.ToString() + "</span>个";
        else
            title = "[" + day30.ToString("yyyy-MM-dd") + "]到[" + theday.ToString("yyyy-MM-dd") + "]所有公众号获得粉丝：<span style=\"color:red;\">" + sum.ToString() + "</span>个";

       
        Dictionary<string, object> dlist = new Dictionary<string, object>();
        dlist["fillColor"] = "#05CE3E";

        dlist["data"] = datalist;
        List<Dictionary<string, object>> all = new List<Dictionary<string, object>>();
        all.Add(dlist);
        datasets = JsonMapper.ToJson(all);
        dlist.Clear();
        datalist.Clear();
        all.Clear();
    }

    private void MP_Today()
    {
        p_device.Visible = true; 
        
        d_yang.Visible = false;
         DateTime theday = DateTime.Now;
        if (!DateTime.TryParse(t_date.Value, out theday))
        {
            theday = DateTime.Now;
            t_date.Value = theday.ToString("yyyy-MM-dd");
        }

         string where = "";
        if (appid_list.SelectedValue != "")
            where = string.Format(" and d.appid='{0}' ", appid_list.SelectedValue);

        buildtablelf.bulidtable_ad stat = new buildtablelf.bulidtable_ad(this);
        stat.Ydes = "吸粉数";
      
       


        stat.Columns = new List<string>();
        for (int i = 0; i < 24; i++)
        {
            stat.Columns.Add(i.ToString());
        }


        string fmt = "select d.tags,d.appid,case when f.total is null then 0 else f.total end as total ,f.xftime from tbl_wx_xifenji as d";
        fmt += " left join  (";
        fmt += " select appid, Hour(subscribe_time) as xftime ,count(appid) as total from tbl_wx_mp_fensi where subscribe_time>='{0}' and subscribe_time<'{1}' group by appid,xftime";
        fmt += ") As f";
        fmt += " ON d.appid = f.appid where enable=1 and ad_id={2}{3} order by f.total ";
        stat.SQL = string.Format(fmt,theday.ToString("yyyy-MM-dd"),theday.AddDays(1).ToString("yyyy-MM-dd"),ad_id,where);
        stat.Xdes = "小時";
        stat.Title = stat.Ydes + " - " + theday.ToString("yyyy-MM-dd (ddd)");
        List<decimal> data_list;
        html = stat.Build(out data_list);




        



       
      

       



        List<string> list = new List<string>();
        List<decimal> datalist = new List<decimal>();
        decimal sum = 0; 
        for (int i = 0; i < data_list.Count; i++)
        {
            decimal value = data_list[i];
            list.Add(stat.Columns[i]+"时");
            sum += value;
            datalist.Add(value);
        }

        labels = JsonMapper.ToJson(list);
        list.Clear();
        list = null;
      
        if (appid_list.SelectedValue != "")
            title = "[" + theday.ToString("yyyy-MM-dd") + "]此公众号获得粉丝：<span style=\"color:red;\">" + sum.ToString() + "</span>个";
        else
            title = "[" + theday.ToString("yyyy-MM-dd") + "]所有公众号获得粉丝：<span style=\"color:red;\">" + sum.ToString() + "</span>个";

        //
        Dictionary<string, object> dlist = new Dictionary<string, object>();
        dlist["fillColor"] = "#05CE3E";
        
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


    protected void LinkButton_Command(object sender, CommandEventArgs e)
    {
        string sArg = ((Button)sender).CommandArgument;
        switch (sArg)
        {
            case "First"://首页
                curpage = 0;
                break;
            case "Prev"://前一页
                curpage-= 1;
                break;
            case "Next"://下一页
                curpage += 1;
                break;
            case "Last"://尾页
                curpage = GetMaxPage();
                break;
        }
       // Device_Show(curpage);
    }
    private int GetMaxPage()
    {
        return (int.Parse(showPage.Text.Substring(1, showPage.Text.IndexOf("条") - 1)) - 1) / pagecount;
    }
    protected void btnPage_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.txtPage.Text.Length <= 0)
            {
                Jscript.Alert(this.Page, "你没有输入页数");
            }
            else
            {
                if (Convert.ToInt32(txtPage.Text.Trim()) > 0 && Convert.ToInt32(txtPage.Text.Trim()) <= GetMaxPage() + 1)
                {
                    ViewState["page"] = curpage = Convert.ToInt32(txtPage.Text.Trim()) - 1;
                }
                else
                {
                    txtPage.Text = "";
                    Jscript.Alert(this.Page, "你没有输入页数");
                }
            }
            //Device_Show(curpage);
        }
        catch
        {
            Jscript.Alert(this.Page, "你输入的页码不正确！");
        }
    }
}