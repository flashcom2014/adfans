using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;

/// <summary>
/// 广告商家类
/// </summary>
public class tbl_ad_mch
{
    public int ad_id = 0;
    public string ad_mobile = "";
    public string ad_acc = "";
    public bool isset_acc = false;
    public string ad_name = "";
    public bool enable = true;
    public int mch_money = 0;
    public int mch_points = 0;
    public float agent_money = 0;
    public float scale_points = 0;
    public int parent_id = 0;
    public string telephone = "";
    public string email = "";
    public string address = "";
    public string comment_ = "";
    public string share_code = "";
    public long areaid = 0;
    public string fullid = "";
    public float tx_min = 100f;
    public int sub_amount = 150;
    public DateTime reg_time;

    public tbl_ad_mch(int ad_id)
    {
        LoadDatanew(ad_id);
    }
    public tbl_ad_mch()
    {
    }
    public void LoadData(int ad_id)
    {
        string sqlstr = string.Format("select * from tbl_ad_mch where ad_id={0};", ad_id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            this.ad_id = ad_id;
            this.ad_mobile = row["ad_mobile"].ToString();
            this.ad_acc = row["ad_acc"].ToString();
            this.isset_acc = row["isset_acc"].ToString() == "1" ? true : false;
            this.ad_name = row["ad_name"].ToString();
            this.enable = row["enable"].ToString() == "1" ? true : false;
            this.mch_money = int.Parse(row["mch_money"].ToString());
            this.mch_points = int.Parse(row["mch_points"].ToString());
            this.agent_money = float.Parse(row["agent_money"].ToString());
            this.scale_points = float.Parse(row["scale_points"].ToString());
            this.parent_id = int.Parse(row["parent_id"].ToString());
            if(row["area_id"].ToString()!="")
                this.areaid = long.Parse(row["area_id"].ToString()); 
            this.telephone = row["telephone"].ToString();
            this.email = row["email"].ToString();
            this.address = row["address"].ToString();
            this.comment_ = row["comment_"].ToString();
            this.share_code = row["share_code"].ToString();
            this.reg_time = (DateTime)row["reg_time"];
            this.tx_min = float.Parse(row["tx_min"].ToString());
            this.sub_amount = int.Parse(row["sub_amount"].ToString());
        }
        dt.Dispose();
        ds.Dispose();
    }

    public void LoadDatanew(int ad_id)
    {
        string sqlstr = string.Format("select a.*,p.fullid as fullid from tbl_ad_mch as a left join tbl_position as p on a.area_id=p.id where ad_id={0}", ad_id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            this.ad_id = ad_id;
            this.ad_mobile = row["ad_mobile"].ToString();
            this.ad_acc = row["ad_acc"].ToString();
            this.isset_acc = row["isset_acc"].ToString() == "1" ? true : false;
            this.ad_name = row["ad_name"].ToString();
            this.enable = row["enable"].ToString() == "1" ? true : false;
            this.mch_money = int.Parse(row["mch_money"].ToString());
            this.mch_points = int.Parse(row["mch_points"].ToString());
            this.agent_money = float.Parse(row["agent_money"].ToString());
            this.scale_points = float.Parse(row["scale_points"].ToString());
            this.parent_id = int.Parse(row["parent_id"].ToString());
            if (row["area_id"].ToString() != "")
                this.areaid = long.Parse(row["area_id"].ToString()); 
            this.fullid = row["fullid"].ToString();
            this.telephone = row["telephone"].ToString();
            this.email = row["email"].ToString();
            this.address = row["address"].ToString();
            this.comment_ = row["comment_"].ToString();
            this.share_code = row["share_code"].ToString();
            this.reg_time = (DateTime)row["reg_time"];
        }
        dt.Dispose();
        ds.Dispose();
    }
    public bool IsAgent
    {
        get
        {
            if (scale_points > 0)
                return true;
            return false;
        }
    }

    public static tbl_ad_mch GetAdMch(int ad_id)
    {
        tbl_ad_mch adm = new tbl_ad_mch(ad_id);
        return adm;
    }
}