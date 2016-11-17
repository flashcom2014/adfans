using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DBUtility;

/// <summary>
/// 基本设置类
/// </summary>
public class Settings
{
    /// <summary>
    /// 当公众号有没停用的情况必须保留的余额
    /// </summary>
    public int ad_amount = 10000;
    /// <summary>
    /// 广告商公众号关注所需基本费用
    /// </summary>
    public int ad_charge = 150;
    /// <summary>
    /// 广告平台代理分成，公司成本价，代理分成费用为关注金额减去成本价再乘以分成比例
    /// </summary>
    public int ad_cost = 110;
    /// <summary>
    /// 吸粉机关注分给商家的费用
    /// </summary>
    public int ad_get_amount = 100;
    /// <summary>
    /// 停用所有公众号后等等分钟，之后可提取所有余额
    /// </summary>
    public int ad_waittime = 10;

    public Settings()
    {
        string keys = "", values = "";
        string sqlstr = "select * from tbl_settings";
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            keys = row["keys"].ToString();
            values = row["values"].ToString();
            switch (keys)
            {
                case "ad_amount":
                    ad_amount = int.Parse(values);
                    break;
                case "ad_charge":
                    ad_charge = int.Parse(values);
                    break;
                case "ad_cost":
                    ad_cost = int.Parse(values);
                    break;
                case "ad_get_amount":
                    ad_get_amount = int.Parse(values);
                    break;
                case "ad_waittime":
                    ad_waittime = int.Parse(values);
                    break;
            }
        }
        dt.Dispose();
        ds.Dispose();
    }

    private static Settings AdSettings = null;
    private static DateTime loadtime = DateTime.Now;

    public static Settings GetSettings()
    {
        if (AdSettings == null || DateTime.Now >= loadtime)
        {
            AdSettings = new Settings();
            loadtime = DateTime.Now.AddMinutes(1);
        }
        return AdSettings;
    }
}