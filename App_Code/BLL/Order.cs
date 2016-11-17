using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Common;
using DBUtility;
using WxPayAPI;

/// <summary>
/// XiaDan 的摘要说明
/// </summary>
public class Order
{
	public enum PayType
	{
		None = 0,
		AliPay = 20000,
		WeiXin = 21000,
		API = 22000,
		PayPal = 23000,
		YskjPay = 24000,
        Wallet = 27000,
        TenPay = 28000
	};

	public Order()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
	/*public static bool IsOpenIdExists(string OpenId)
	{
		return (DbHelperMySQL.Query("SELECT user_id FROM tbl_consumer WHERE openid='" + OpenId + "';").Tables[0].Rows.Count > 0);
	}
	public static bool IsFirstOrder(string OpenId)
	{
		return (DbHelperMySQL.Query("SELECT user_id FROM tbl_consumer WHERE openid='" + OpenId + "' AND order_id<>'';").Tables[0].Rows.Count == 0);
	}

	public static bool IsOrderIdExists(string OrderId)
	{
		return (DbHelperMySQL.Query("SELECT order_id FROM tbl_order WHERE order_id='" + OrderId + "';").Tables[0].Rows.Count > 0);
	}
	public static string GetProductDetail(DataRow Row, out int CoinCount)
	{
		string rec_product_detail = "";
		string product_id = Row["product_id"].ToString();
		CoinCount = 0;
		if (Row["product_typeid"].ToString() == "1")
		{
			DataTable coin_dt = DbHelperMySQL.Query("select * from tbl_product_coin where product_id=" + product_id + "").Tables[0];
			CoinCount = int.Parse(coin_dt.Rows[0]["quantity"].ToString()) + int.Parse(coin_dt.Rows[0]["free_coin"].ToString());//出币数
			rec_product_detail += "游戏币数量：" + coin_dt.Rows[0]["quantity"];
			rec_product_detail += "\r\n赠送数量：" + coin_dt.Rows[0]["free_coin"];
		}
		else if (Row["product_typeid"].ToString() == "2")
		{
			DataTable foot_dt = DbHelperMySQL.Query("select * from tbl_product_food where product_id=" + product_id + "").Tables[0];
			rec_product_detail += "食品条形码：" + foot_dt.Rows[0]["barcode"];
		}
		else if (Row["product_typeid"].ToString() == "3")
		{
			DataTable tushu_dt = DbHelperMySQL.Query("select * from tbl_product_book where product_id=" + product_id + "").Tables[0];
			rec_product_detail += "图书条形码：" + tushu_dt.Rows[0]["barcode"];
			rec_product_detail += "\r\n国际标准号：" + tushu_dt.Rows[0]["ISBN"];
		}
		return rec_product_detail;
	}
	//扫码下单(停用)
	public static void XiaDan(string productId, ref string order_id, ref string d_id, ref string p_id,ref DataSet ds)
    {
        order_id = WxPayApi.GenerateOutTradeNo();
        #region 服务器下单处理
        string[] pds = productId.Split('_');
        d_id = pds[0];
        p_id = pds[1];
        string product_quantity = "1";
        ds = DbHelperMySQL.Query("select * from tbl_mch_payment where mch_id=(select mch_id from tbl_device where device_id=" + d_id + ") LIMIT 1,1;select * from tbl_product where product_id=" + p_id + "");
        string payment_id = ds.Tables[0].Rows[0]["payment_id"].ToString();
        string mch_id = ds.Tables[0].Rows[0]["mch_id"].ToString();
        string total_fee = ds.Tables[1].Rows[0]["total_fee"].ToString();
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        string rec_product_title = ds.Tables[1].Rows[0]["title"].ToString();
        string rec_product_des = ds.Tables[1].Rows[0]["des"].ToString();
        string rec_product_detail = "";
        #region 产品详细
        if(ds.Tables[1].Rows[0]["product_typeid"].ToString()=="1")
        {
           DataTable coin_dt=DbHelperMySQL.Query("select * from tbl_product_coin where product_id=" + p_id + "").Tables[0];
           rec_product_detail += "游戏币数量：" + coin_dt.Rows[0]["quantity"];
           rec_product_detail += "\r\n赠送数量：" + coin_dt.Rows[0]["free_coin"];
        }
        else if (ds.Tables[1].Rows[0]["product_typeid"].ToString() == "2")
        {
            DataTable foot_dt = DbHelperMySQL.Query("select * from tbl_product_food where product_id=" + p_id + "").Tables[0];
            rec_product_detail += "食品条形码：" + foot_dt.Rows[0]["barcode"];
        }
        else if (ds.Tables[1].Rows[0]["product_typeid"].ToString() == "3")
        {
            DataTable tushu_dt = DbHelperMySQL.Query("select * from tbl_product_book where product_id=" + p_id + "").Tables[0];
            rec_product_detail += "图书条形码：" + tushu_dt.Rows[0]["barcode"];
            rec_product_detail += "\r\n国际标准号：" + tushu_dt.Rows[0]["ISBN"];
        }
        #endregion
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create',"+p_id+",'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "');");
        //listsql.Add(string.Format("insert into tbl_device_order values({0},{1});", d_id, order_id));
        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            //Scheduler.SaveEventLog("order_id:" + order_id + "\r\nstatus:下单成功.");
        }
        else
        {
            //Scheduler.SaveEventLog("order_id:" + order_id + "\r\nstatus:下单失败.");
        }
        #endregion
    }
    //扫码下单(停用)
    public static void XiaDan(string productId, ref DataSet ds, ref decimal total_fee, ref string p_id, string order_id, ref string openId)
    {
        //string order_id = WxPayApi.GenerateOutTradeNo();
        #region 服务器下单处理
        string[] pds = productId.Split('_');
        string d_id = pds[0];
        p_id = pds[1];
        string product_quantity = "1";
        ds = DbHelperMySQL.Query("select * from tbl_mch_payment where mch_id=(select mch_id from tbl_device where device_id=" + d_id + ") LIMIT 1,1;select * from tbl_product where product_id=" + p_id + "");
        string payment_id = ds.Tables[0].Rows[0]["payment_id"].ToString();
        string mch_id = ds.Tables[0].Rows[0]["mch_id"].ToString();
        string t = ds.Tables[1].Rows[0]["total_fee"].ToString();
        total_fee = decimal.Parse(t);
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        string rec_product_title = ds.Tables[1].Rows[0]["title"].ToString();
        string rec_product_des = ds.Tables[1].Rows[0]["des"].ToString();
        string rec_product_detail = "";
        #region 产品详细
        if (ds.Tables[1].Rows[0]["product_typeid"].ToString() == "1")
        {
            DataTable coin_dt = DbHelperMySQL.Query("select * from tbl_product_coin where product_id=" + p_id + "").Tables[0];
            rec_product_detail += "游戏币数量：" + coin_dt.Rows[0]["quantity"];
            rec_product_detail += "\r\n赠送数量：" + coin_dt.Rows[0]["free_coin"];
        }
        else if (ds.Tables[1].Rows[0]["product_typeid"].ToString() == "2")
        {
            DataTable foot_dt = DbHelperMySQL.Query("select * from tbl_product_food where product_id=" + p_id + "").Tables[0];
            rec_product_detail += "食品条形码：" + foot_dt.Rows[0]["barcode"];
        }
        else if (ds.Tables[1].Rows[0]["product_typeid"].ToString() == "3")
        {
            DataTable tushu_dt = DbHelperMySQL.Query("select * from tbl_product_book where product_id=" + p_id + "").Tables[0];
            rec_product_detail += "图书条形码：" + tushu_dt.Rows[0]["barcode"];
            rec_product_detail += "\r\n国际标准号：" + tushu_dt.Rows[0]["ISBN"];
        }
        #endregion
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create'," + p_id + ",'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "');");
        //listsql.Add(string.Format("insert into tbl_device_order values({0},'{1}','','');", d_id, order_id));

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("order_id:" + order_id + "\r\nstatus:下单成功.");

            #region 我是土豪,第一单免费
            if (openId != "" && 
				!IsOpenIdExists(openId) && 
				(ds.Tables[1].Rows[0]["isfree"].ToString() == "1"))
            {
                Scheduler.SaveEventLog("第一单免费S1");
                listsql = new List<string>();
                listsql.Add("insert into tbl_consumer(openid) values('" + openId + "');");
                listsql.Add("update tbl_order set total_fee=0,time_pay='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',status='paid' where order_id='" + order_id + "';");//
                Scheduler.SaveEventLog("第一单免费S2");
                if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
                {
                    Scheduler.SaveEventLog("第一单免费S3");
                    MyClientSocket.Send0("P " + order_id, false, null);//发送订单号
                    Scheduler.SaveEventLog("第一单免费S4");

                    Static.LoadCache("tbl_consumer");
                    Scheduler.SaveEventLog("第一单免费SUCCESS");
                    //openId = "MIANFEI";
                    //手动修改参数
                    //total_fee = 0.01m;
                    ds.Tables[1].Rows[0]["des"] = "「請勿付款」" + ds.Tables[1].Rows[0]["des"].ToString();
                    //Scheduler.SaveEventLog("第一单免费SUCCESS_修改参数" + total_fee);
                }
                else
                {
                    Scheduler.SaveEventLog("第一单免费F4");
                }
            }
            #endregion
        }
        else
        {
            Scheduler.SaveEventLog("order_id:" + order_id + "\r\nstatus:下单失败.");
        }
        #endregion
    }
    
	//微信在线下单0:订单存在1:免单2:下单成功
    public static int XiaDanWeiXin(string productId, string order_id, string WxAppId, string openId, string extra)
    {
        int result = 2;
        //string order_id = WxPayApi.GenerateOutTradeNo();
        #region 服务器下单处理
        string[] pds = productId.Split('_');
        string d_id = pds[0];
        string p_id = pds[1];
        string product_quantity = "1";
        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_product  where product_id=" + p_id + ";select is_free from tbl_device where device_id=" + d_id + "");

        //if (ds.Tables[2].Rows.Count > 0) return 0;
        int order_id_count = int.Parse(DbHelperMySQL.GetSingle("select count(*) from tbl_order where order_id='" + order_id + "';").ToString());
        if (order_id_count > 0) return 0;

        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
		BLL.tbl_wx_config wx = ((BLL.tbl_wx_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.WeiXin)[0]);
        int payment_id =wx.payment_id;
        
        string t = ds.Tables[1].Rows[0]["total_fee"].ToString();
        string device_is_free = ds.Tables[2].Rows[0]["is_free"].ToString();
        int rec_product_coin = 0;//出币数
		int user_id = GetUserId(WxAppId, openId);
        decimal total_fee = decimal.Parse(t);
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string rec_product_title = ds.Tables[1].Rows[0]["title"].ToString();
        string rec_product_des = ds.Tables[1].Rows[0]["des"].ToString();
        string rec_product_detail = GetProductDetail(ds.Tables[1].Rows[0], out rec_product_coin);
 		string mch_ids = string.Empty;
		if (string.IsNullOrWhiteSpace(wx.W_SUB_MCHID))
			mch_ids = wx.W_MCHID;
		else
			mch_ids = wx.W_SUB_MCHID + "@" + wx.W_MCHID;

		//查询订单号是否重复
		order_id = IsOrderIdExists(order_id) ? WxPayApi.GenerateOutTradeNo() : order_id;
        List<string> listsql = new List<string>();
		listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host,user_id) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create'," + p_id + ",'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + mch_ids + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "'," + user_id + ");");
        //listsql.Add(string.Format("insert into tbl_device_order values({0},'{1}','','');", d_id, order_id));

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.openId:" + openId+"order_id:"+order_id);

            #region 我是土豪,第一单免费
            if ((device_is_free == "1") || (openId != "" && IsFirstOrder(openId) && (ds.Tables[1].Rows[0]["isfree"].ToString() == "1")))
            {
                Scheduler.SaveEventLog("第一单免费S1");
                listsql = new List<string>();
                listsql.Add("update tbl_order set total_fee=0,time_pay='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',status='paid' where order_id='" + order_id + "';");//
                Scheduler.SaveEventLog("第一单免费S2");
                if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
                {
                    Scheduler.SaveEventLog("第一单免费S3");
                    MyClientSocket.Send0("P " + order_id, false, null);//发送订单号
                    Scheduler.SaveEventLog("第一单免费S4");

                    Scheduler.SaveEventLog("第一单免费SUCCESS");
                    result = 1;
                }
                else
                {
                    Scheduler.SaveEventLog("第一单免费F4");
                }
            }
            #endregion
            listsql=new List<string>();
            if (!IsOpenIdExists(openId))
            {
                listsql.Add("insert into tbl_consumer(openid,order_id) values('" + openId + "','" + order_id + "');");
            }
            else
            {
                listsql.Add("update tbl_consumer set order_id='" + order_id + "' where openid='" + openId + "';");
            }
            DbHelperMySQL.ExecuteSqlTran(listsql);
            //Static.LoadCache("tbl_consumer");
        }
        else
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单失败.");
        }
        return result;
        #endregion
    }

    //微信在线下单0:订单存在1:免单2:下单成功
    public static int XiaDan_kb_wx(string productId, string order_id, string WxAppId, string openId, string extra)
    {
        int result = 2;
        //string order_id = WxPayApi.GenerateOutTradeNo();
        #region 服务器下单处理
        string[] pds = productId.Split('_');
        string d_id = pds[0];
        string p_id = pds[1];
        string product_quantity = "1";
        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_product  where product_id=" + p_id + ";select * from tbl_order where order_id='" + order_id + "';select is_free from tbl_device where device_id=" + d_id + "");
        if (ds.Tables[2].Rows.Count > 0) return 0;
        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
        BLL.tbl_kb_config kb = ((BLL.tbl_kb_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.YskjPay)[0]); 
        int payment_id = kb.payment_id;

        string t = ds.Tables[1].Rows[0]["total_fee"].ToString();
        string device_is_free = ds.Tables[3].Rows[0]["is_free"].ToString();
        int rec_product_coin = 0;//出币数

        decimal total_fee = decimal.Parse(t);
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		int user_id = GetUserId(WxAppId, openId);

        string rec_product_title = ds.Tables[1].Rows[0]["title"].ToString();
        string rec_product_des = ds.Tables[1].Rows[0]["des"].ToString();
		string rec_product_detail = GetProductDetail(ds.Tables[1].Rows[0], out rec_product_coin);

        int yskj_payment_id = int.Parse(System.Configuration.ConfigurationManager.AppSettings["yskj_payment_id"].ToString());
        BLL.tbl_wx_config wx = ((BLL.tbl_wx_config)BLL.tbl_agent.GetPayAcc(yskj_payment_id, (int)PayType.WeiXin)[0]); //用mch1=1,禁止移动
        //string mch_ids = string.Empty;
        //if (string.IsNullOrWhiteSpace(wx.W_SUB_MCHID))
        //    mch_ids = wx.W_MCHID;
        //else
        //    mch_ids = wx.W_SUB_MCHID + "@" + wx.W_MCHID;

		//查询订单号是否重复
        order_id = IsOrderIdExists(order_id) ? WxPayApi.GenerateOutTradeNo() : order_id;
        List<string> listsql = new List<string>();
		listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host,user_id) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create'," + p_id + ",'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'','" + extra + "','" + HttpContext.Current.Request.Url.Host + "'," + user_id + ");");
		if (!IsOpenIdExists(openId))
            listsql.Add("insert into tbl_consumer(openid,order_id) values('" + openId + "','" + order_id + "');");
        else
            listsql.Add("update tbl_consumer set order_id='" + order_id + "' where openid='" + openId + "';");

		bool ok_order = (DbHelperMySQL.ExecuteSqlTran(listsql) > 0);
		Scheduler.SaveEventLog(string.Format("IP: {0}, order_id: '{1}' (下单{2})",
			Tools.GetClientIP(), order_id, (ok_order ? "成功" : "失败")));
        return result;
        #endregion
    }

    //支付宝在线下单
    public static bool XiaDanAlipay(string productId, string order_id, ref DataSet ds, string extra)
    {
        bool IsMd = false;
        //string order_id = WxPayApi.GenerateOutTradeNo();
        #region 服务器下单处理
        string[] pds = productId.Split('_');
        string d_id = pds[0];
        string p_id = pds[1];
        string product_quantity = "1";

        ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_product  where product_id=" + p_id + ";select * from tbl_order where order_id='" + order_id + "';select is_free from tbl_device where device_id=" + d_id + "");
        if (ds.Tables[2].Rows.Count > 0) return false;

        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
		BLL.tbl_zfb_config zfb = ((BLL.tbl_zfb_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.AliPay)[0]);
        int payment_id = zfb.payment_id;
        
        string t = ds.Tables[1].Rows[0]["total_fee"].ToString();
        string device_is_free = ds.Tables[3].Rows[0]["is_free"].ToString();
        int rec_product_coin = 0;//出币数

        decimal total_fee = decimal.Parse(t);
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        string rec_product_title = ds.Tables[1].Rows[0]["title"].ToString();
        string rec_product_des = ds.Tables[1].Rows[0]["des"].ToString();
		string rec_product_detail = GetProductDetail(ds.Tables[1].Rows[0], out rec_product_coin);

        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create'," + p_id + ",'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + zfb.Z_PARTNER + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "');");

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.");
        }
        else
        {
            Scheduler.SaveEventLog("order_id:" + order_id + "\r\nstatus:下单失败.");
        }
        return IsMd;
        #endregion
    }

    //PayPal在线下单
    public static bool XiaDanPayPal(string productId, string order_id, string extra)
    {
        bool IsMd = false;
        //string order_id = WxPayApi.GenerateOutTradeNo();
        #region 服务器下单处理
        string[] pds = productId.Split('_');
        string d_id = pds[0];
        string p_id = pds[1];
        string product_quantity = "1";

        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_product  where product_id=" + p_id + ";select * from tbl_order where order_id='" + order_id + "';select is_free from tbl_device where device_id=" + d_id + "");
        if (ds.Tables[2].Rows.Count > 0) return false;

        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
		BLL.tbl_pp_config pp = ((BLL.tbl_pp_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.PayPal)[0]);
        int payment_id =pp.payment_id;

        string t = ds.Tables[1].Rows[0]["total_fee"].ToString();
        string device_is_free = ds.Tables[3].Rows[0]["is_free"].ToString();
        int rec_product_coin = 0;//出币数

        decimal total_fee = decimal.Parse(t);
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        string rec_product_title = ds.Tables[1].Rows[0]["title"].ToString();
        string rec_product_des = ds.Tables[1].Rows[0]["des"].ToString();
		string rec_product_detail = GetProductDetail(ds.Tables[1].Rows[0], out rec_product_coin);
        
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create'," + p_id + ",'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + pp.P_Email + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "');");

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.");
        }
        else
        {
            Scheduler.SaveEventLog("order_id:" + order_id + "\r\nstatus:下单失败.");
        }
        return IsMd;
        #endregion
    }

    //酷币在线下单
    //public static int XiaDan_kb(string productId, string order_id, string openId, ref decimal total_fee, ref BLL.tbl_kb_config kb, string extra)
    //{
    //    int result = 2;
    //    //string order_id = WxPayApi.GenerateOutTradeNo();
    //    #region 服务器下单处理
    //    string[] pds = productId.Split('_');
    //    string d_id = pds[0];
    //    string p_id = pds[1];
    //    string product_quantity = "1";
    //    DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_product  where product_id=" + p_id + ";select * from tbl_order where order_id='" + order_id + "';select is_free from tbl_device where device_id=" + d_id + "");
    //    if (ds.Tables[2].Rows.Count > 0) return 0;
    //    int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
    //    kb = ((BLL.tbl_kb_config)BLL.tbl_agent.PayAcc(mch_id, 24000)[0]);
    //    int payment_id = kb.payment_id;

    //    string t = ds.Tables[1].Rows[0]["total_fee"].ToString();
    //    string device_is_free = ds.Tables[3].Rows[0]["is_free"].ToString();
    //    int rec_product_coin = 0;//出币数

    //    total_fee = decimal.Parse(t);
    //    string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


    //    string rec_product_title = ds.Tables[1].Rows[0]["title"].ToString();
    //    string rec_product_des = ds.Tables[1].Rows[0]["des"].ToString();
    //    string rec_product_detail = "";
    //    #region 产品详细
    //    if (ds.Tables[1].Rows[0]["product_typeid"].ToString() == "1")
    //    {
    //        DataTable coin_dt = DbHelperMySQL.Query("select * from tbl_product_coin where product_id=" + p_id + "").Tables[0];
    //        rec_product_coin = int.Parse(coin_dt.Rows[0]["quantity"].ToString()) + int.Parse(coin_dt.Rows[0]["free_coin"].ToString());//出币数
    //        rec_product_detail += "游戏币数量：" + coin_dt.Rows[0]["quantity"];
    //        rec_product_detail += "\r\n赠送数量：" + coin_dt.Rows[0]["free_coin"];
    //    }
    //    else if (ds.Tables[1].Rows[0]["product_typeid"].ToString() == "2")
    //    {
    //        DataTable foot_dt = DbHelperMySQL.Query("select * from tbl_product_food where product_id=" + p_id + "").Tables[0];
    //        rec_product_detail += "食品条形码：" + foot_dt.Rows[0]["barcode"];
    //    }
    //    else if (ds.Tables[1].Rows[0]["product_typeid"].ToString() == "3")
    //    {
    //        DataTable tushu_dt = DbHelperMySQL.Query("select * from tbl_product_book where product_id=" + p_id + "").Tables[0];
    //        rec_product_detail += "图书条形码：" + tushu_dt.Rows[0]["barcode"];
    //        rec_product_detail += "\r\n国际标准号：" + tushu_dt.Rows[0]["ISBN"];
    //    }
    //    #endregion
    //    List<string> listsql = new List<string>();
    //    listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create'," + p_id + ",'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + kb.KB_JJH + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "');");
    //    //listsql.Add(string.Format("insert into tbl_device_order values({0},'{1}','','');", d_id, order_id));
    //    if (Static.tbl_consumer.Select("openid='" + openId + "'").Length == 0)
    //    {
    //        listsql.Add("insert into tbl_consumer(openid,order_id) values('" + openId + "','" + order_id + "');");
    //    }
    //    else
    //    {
    //        listsql.Add("update tbl_consumer set order_id='" + order_id + "' where openid='" + openId + "';");
    //    }

    //    if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
    //    {
    //        Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.");

    //        #region 我是土豪,第一单免费
    //        if (device_is_free == "1" || (openId != "" && Static.tbl_consumer.Select("openid='" + openId + "'").Length == 0 && ds.Tables[1].Rows[0]["isfree"].ToString() == "1"))
    //        {
    //            Scheduler.SaveEventLog("第一单免费S1");
    //            listsql = new List<string>();
    //            listsql.Add("update tbl_order set total_fee=0,time_pay='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',status='paid' where order_id='" + order_id + "';");//
    //            Scheduler.SaveEventLog("第一单免费S2");
    //            if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
    //            {
    //                Scheduler.SaveEventLog("第一单免费S3");
    //                MyClientSocket.Send0("P " + order_id, false, null);//发送订单号
    //                Scheduler.SaveEventLog("第一单免费S4");

    //                Scheduler.SaveEventLog("第一单免费SUCCESS");
    //                result = 1;
    //            }
    //            else
    //            {
    //                Scheduler.SaveEventLog("第一单免费F4");
    //            }
    //        }
    //        Static.LoadCache("tbl_consumer");
    //        #endregion
    //    }
    //    else
    //    {
    //        Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单失败.");
    //    }
    //    return result;
    //    #endregion
    //}
    //API下单
    public static bool XiaDanAPI(string order_id, string mch_id, string d_id, string total_fee, string extra)
    {
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        int rec_product_coin = int.Parse(total_fee);//实际出币数

		BLL.api_device0 api = ((BLL.api_device0)BLL.tbl_agent.PayAcc(int.Parse(mch_id), (int)PayType.API)[0]);
        int payment_id = api.payment_id;

        //查询订单号是否重复
        order_id = IsOrderIdExists(order_id) ? WxPayApi.GenerateOutTradeNo() : order_id;
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,total_fee,time_pay,time_create,time_change,status,payment_id,rec_product_coin,pay_mchacc,extra,web_host) values('" + order_id + "'," + mch_id + "," + d_id + ",'" + total_fee + "','" + time_create + "','" + time_create + "','" + time_create + "','paid'," + payment_id + "," + rec_product_coin + ",'" + api.ip + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "');");
        //listsql.Add(string.Format("insert into tbl_device_order values({0},'{1}','','');", d_id, order_id));
        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            return true;
        }
        return false;

    }
    //设备派币
    public static bool SheBeiPaiDan(string mch_acc, string mch_id, string d_id, string rec_product_coin, string extra)
    {
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        int payment_id = 10;

        //查询订单号是否重复
        string order_id = WxPayApi.GenerateOutTradeNo();
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,total_fee,time_pay,time_create,time_change,status,payment_id,rec_product_coin,pay_mchacc,extra,rec_product_title,web_host) values('" + order_id + "'," + mch_id + "," + d_id + ",'0','" + time_create + "','" + time_create + "','" + time_create + "','paid'," + payment_id + "," + rec_product_coin + ",'','" + extra + "','" + (mch_acc + " - " + Tools.GetClientIP()) + "','" + HttpContext.Current.Request.Url.Host + "');");
        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            MyClientSocket.Send0("P " + order_id, false, null);//发送订单号
            return true;
        }

        return false;
    }
    //吸粉机下单
    public static bool WxXiFenJi(int mch_id, int device_id, int get_amount, int rec_product_coin, string mp_tags, string appid, string openid, string extra)
    {
        string sqlstr = "select wx.* from tbl_settings as s join tbl_wx_config as wx on  cast(s.`values` as signed)=wx.payment_id where s.`keys`='ad_payment_id';";
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
            return false;
        string order_id = WxPayApi.GenerateOutTradeNo();
        int payment_id = int.Parse(dt.Rows[0]["payment_id"].ToString());
        string mch_ids = string.Empty;
        if (string.IsNullOrWhiteSpace(dt.Rows[0]["W_SUB_MCHID"].ToString()))
            mch_ids = dt.Rows[0]["W_MCHID"].ToString();
        else
            mch_ids = dt.Rows[0]["W_SUB_MCHID"].ToString() + "@" + dt.Rows[0]["W_MCHID"].ToString();

        int user_id = GetUserId(appid, openid);
        decimal total_fee = decimal.Parse((double.Parse(get_amount.ToString()) / 100d).ToString());
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string rec_product_title = "吸粉机";
        string rec_product_des = "吸粉机-" + mp_tags + "-" + openid;
        string rec_product_detail = GetProductDetail(ds.Tables[1].Rows[0], out rec_product_coin);
        string product_quantity = "1";

        
        sqlstr = "insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host,user_id) values('" + order_id + "'," + mch_id + "," + device_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create'," + openid + ",'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + mch_ids + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "'," + user_id + ");");

        if (DbHelperMySQL.ExecuteSql(sqlstr) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.openId:" + openid + "order_id:" + order_id);
            if (!IsOpenIdExists(openid))
            {
                sqlstr = "insert into tbl_consumer(openid,order_id) values('" + openid + "','" + order_id + "');";
            }
            else
            {
                sqlstr = "update tbl_consumer set order_id='" + order_id + "' where openid='" + openid + "';";
            }
            DbHelperMySQL.ExecuteSql(sqlstr);
            //Static.LoadCache("tbl_consumer");
        }
        else
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单失败.");
            return false;
        }
        return true;
    }
    //微信关注赠送
    public static bool WxGzZengS(string ToUserName, string mch_id, string d_id, string rec_product_coin, string extra)
    {
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        int payment_id = 11;

        //查询订单号是否重复
        string order_id = WxPayApi.GenerateOutTradeNo();
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,total_fee,time_pay,time_create,time_change,status,payment_id,rec_product_coin,pay_mchacc,extra,rec_product_title,web_host) values('" + order_id + "'," + mch_id + "," + d_id + ",'0','" + time_create + "','" + time_create + "','" + time_create + "','paid'," + payment_id + "," + rec_product_coin + ",'','" + extra + "','" + (ToUserName + " - " + Tools.GetClientIP()) + "','" + HttpContext.Current.Request.Url.Host + "');");
        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            MyClientSocket.Send0("P " + order_id, false, null);//发送订单号
            return true;
        }

        return false;
    }
    //POS机微信下单
    public static int POS_XiaDan_wx(string total, string d_id, string order_id, string WxAppId, string openId, string extra)
    {
        string product_quantity = "0";
        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_order where order_id='" + order_id + "';");
        if (ds.Tables[1].Rows.Count > 0) return 1;
        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
        BLL.tbl_wx_config wx = ((BLL.tbl_wx_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.WeiXin)[0]);
        int payment_id = wx.payment_id;
		int user_id = GetUserId(WxAppId, openId);
        int rec_product_coin = (int)(decimal.Parse(total) * 100);//出币数

        decimal total_fee = decimal.Parse(total);
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        string rec_product_title = "POS机充值" + total_fee+"元";
        string rec_product_des = "POS机充值" + total_fee+"元";
        string rec_product_detail = "POS机充值" + total_fee+"元";

		string mch_ids = string.Empty;
		if (string.IsNullOrWhiteSpace(wx.W_SUB_MCHID))
			mch_ids = wx.W_MCHID;
		else
			mch_ids = wx.W_SUB_MCHID + "@" + wx.W_MCHID;

		List<string> listsql = new List<string>();
		listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host,user_id) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create',0,'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + mch_ids + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "'," + user_id + ");");
        if (!IsOpenIdExists(openId))
        {
            listsql.Add("insert into tbl_consumer(openid,order_id) values('" + openId + "','" + order_id + "');");
        }
        else
        {
            listsql.Add("update tbl_consumer set order_id='" + order_id + "' where openid='" + openId + "';");
        }

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.");
        }
        else
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单失败.");
            return 2;
        }
        return 0;
    }
    //POS机酷贝下单
    //public static int POS_XiaDan_kb(string d_id, string order_id, string openId, string total_fee, ref BLL.tbl_kb_config kb, string extra)
    //{
    //    string product_quantity = "0";
    //    DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_order where order_id='" + order_id + "';");
    //    if (ds.Tables[1].Rows.Count > 0) return 1;
    //    int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
    //    kb = ((BLL.tbl_kb_config)BLL.tbl_agent.PayAcc(mch_id, 24000)[0]);
    //    int payment_id = kb.payment_id;

    //    int rec_product_coin = (int)(decimal.Parse(total_fee) * 100);//出币数

    //    string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


    //    string rec_product_title = "POS机充值" + total_fee + "元";
    //    string rec_product_des = "POS机充值" + total_fee + "元";
    //    string rec_product_detail = "POS机充值" + total_fee + "元";
    //    List<string> listsql = new List<string>();
    //    listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create',0,'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + kb.KB_JJH + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "');");
    //    //listsql.Add(string.Format("insert into tbl_device_order values({0},'{1}','','');", d_id, order_id));
    //    if (Static.tbl_consumer.Select("openid='" + openId + "'").Length == 0)
    //    {
    //        listsql.Add("insert into tbl_consumer(openid,order_id) values('" + openId + "','" + order_id + "');");
    //    }
    //    else
    //    {
    //        listsql.Add("update tbl_consumer set order_id='" + order_id + "' where openid='" + openId + "';");
    //    }

    //    if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
    //    {
    //        Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.");
    //    }
    //    else
    //    {
    //        Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单失败.");
    //        return 2;
    //    }
    //    return 0;
    //}
    //POS机支付宝下单
    public static int POS_XiaDan_zfb(string d_id, string total_fee, string extra, string order_id)
    {
        string product_quantity = "0";

        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_order where order_id='" + order_id + "';");
        if (ds.Tables[1].Rows.Count > 0) return 1;

        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
		BLL.tbl_zfb_config zfb = ((BLL.tbl_zfb_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.AliPay)[0]);
        int payment_id = zfb.payment_id;

        int rec_product_coin = (int)(decimal.Parse(total_fee) * 100);//出币数

        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        string rec_product_title = "POS机充值" + total_fee + "元";
        string rec_product_des = "POS机充值" + total_fee + "元";
        string rec_product_detail = "POS机充值" + total_fee + "元";
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create',0,'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + zfb.Z_PARTNER + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "');");

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.");
        }
        else
        {
            Scheduler.SaveEventLog("order_id:" + order_id + "\r\nstatus:下单失败.");
            return 2;
        }
        return 0;
    }
    //POS机PayPal下单
    public static int POS_XiaDan_pp(string d_id, string total_fee, string order_id, string extra)
    {
        string product_quantity = "0";

        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_order where order_id='" + order_id + "';");
        if (ds.Tables[1].Rows.Count > 0) return 1;

        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
		BLL.tbl_pp_config pp = ((BLL.tbl_pp_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.PayPal)[0]);
        int payment_id = pp.payment_id;

        int rec_product_coin = (int)(decimal.Parse(total_fee) * 100);//出币数

        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        string rec_product_title = "POS机充值" + total_fee + "元";
        string rec_product_des = "POS机充值" + total_fee + "元";
        string rec_product_detail = "POS机充值" + total_fee + "元";
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create',0,'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + pp.P_Email + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "');");

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.");
        }
        else
        {
            Scheduler.SaveEventLog("order_id:" + order_id + "\r\nstatus:下单失败.");
            return 2;
        }
        return 0;
    }
    public static string GetMch_Id(string orderid)
    {
        return DBUtility.DbHelperMySQL.Query_Main("select * from tbl_order where order_id='" + orderid + "'").Tables[0].Rows[0]["mch_id"].ToString();
    }
    public static bool IsYsPay(string orderid)
    {
        return DBUtility.DbHelperMySQL.Query_Main("select * from tbl_kb_config as a  LEFT JOIN tbl_order as b on a.payment_id=b.payment_id where b.order_id='" + orderid + "'").Tables[0].Rows.Count > 0;
    }
    public static string GetOrder_Id(string trade_no)
    {
        return DBUtility.DbHelperMySQL.Query("select order_id from tbl_order where trade_no='" + trade_no + "'").Tables[0].Rows[0]["order_id"].ToString();
    }
	protected static int GetWalletPaymentId(int MchId)
	{
		int payment_id = 0;
		string sqlstr = string.Format("SELECT payment_id FROM tbl_payment WHERE m_id={0} AND permission_id={1}", MchId, (int)PayType.Wallet);
		DataSet ds = DbHelperMySQL.Query(sqlstr);
		if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
		{
			DataRow dr = ds.Tables[0].Rows[0];
			int.TryParse(dr["payment_id"].ToString(), out payment_id);
		}
		return payment_id;
	}
	public static int GetUserId(string WxAppId, string OpenId)
	{
		int user_id = 0;
		DataSet ds_consumer = DbHelperMySQL.Query(string.Format("SELECT user_id FROM tbl_consumer WHERE wx_appid='{0}' AND openid='{1}';", WxAppId, OpenId));
		if ((ds_consumer.Tables.Count > 0) && (ds_consumer.Tables[0].Rows.Count > 0))
			int.TryParse(ds_consumer.Tables[0].Rows[0]["user_id"].ToString(), out user_id);
		return user_id;
	}
	//钱包下单0:订单存在1:免单2:下单成功
	public static int XiaDan_Wallet(string productId, string order_id, string WxAppId, string OpenId, string extra)
	{
		int result = 2;

		#region 服务器下单处理
		string[] pds = productId.Split('_');
		string d_id = pds[0];
		string p_id = pds[1];
		string product_quantity = "1";
		DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_product  where product_id=" + p_id + ";select * from tbl_order where order_id='" + order_id + "';select is_free from tbl_device where device_id=" + d_id + "");
		if (ds.Tables[2].Rows.Count > 0)
			return 0;

		int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
		int payment_id = GetWalletPaymentId(mch_id);


		string t = ds.Tables[1].Rows[0]["total_fee"].ToString();
		string device_is_free = ds.Tables[3].Rows[0]["is_free"].ToString();
		int rec_product_coin = 0;//出币数

		decimal total_fee = decimal.Parse(t);
        int total_fee_cent = (int)(total_fee * 100);
		string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


		#region 检查钱包
		int user_id = GetUserId(WxAppId, OpenId);
		int wallet_balance_cent = 0;
		DataSet ds_wallet = DbHelperMySQL.Query(string.Format("SELECT balance FROM tbl_consumer_wallet WHERE user_id={0} AND mch_id={1};", user_id, mch_id));
		if ((ds_wallet.Tables.Count > 0) && (ds_wallet.Tables[0].Rows.Count > 0))
			int.TryParse(ds_wallet.Tables[0].Rows[0]["balance"].ToString(), out wallet_balance_cent);

		if (wallet_balance_cent < total_fee_cent)
		{
			Scheduler.SaveEventLog(string.Format("Wallet {0} {1} {2} {3}", WxAppId, OpenId, user_id, mch_id));
			return 0;
		}
		#endregion

		string rec_product_title = ds.Tables[1].Rows[0]["title"].ToString();
		string rec_product_des = ds.Tables[1].Rows[0]["des"].ToString();
		string rec_product_detail = GetProductDetail(ds.Tables[1].Rows[0], out rec_product_coin);

		//查询订单号是否重复
		order_id = Order.IsOrderIdExists(order_id) ? WxPayApi.GenerateOutTradeNo() : order_id;
		List<string> listsql = new List<string>();
		listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host,user_id) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create'," + p_id + ",'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + user_id + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "'," + user_id + ");");
		//listsql.Add(string.Format("insert into tbl_device_order values({0},'{1}','','');", d_id, order_id));
		Scheduler.SaveEventLog(listsql[0]);

		//if (Static.tbl_consumer.Select("openid='" + OpenId + "'").Length == 0)
		if (DbHelperMySQL.Query("SELECT user_id FROM tbl_consumer WHERE openid='" + OpenId + "';").Tables[0].Rows.Count == 0)
		{
			listsql.Add("insert into tbl_consumer(openid,order_id) values('" + OpenId + "','" + order_id + "');");
		}
		else
		{
			listsql.Add("update tbl_consumer set order_id='" + order_id + "' where openid='" + OpenId + "';");
		}

		listsql.Add(string.Format("UPDATE tbl_consumer_wallet SET balance=(balance-{2}) WHERE user_id='{0}' AND mch_id='{1}';", user_id, mch_id, total_fee_cent));
		listsql.Add(string.Format("INSERT INTO tbl_consumer_wallet_log(user_id,mch_id,amount,comment) VALUES({0},{1},{2},'{3}');",
			user_id, mch_id, -total_fee_cent, rec_product_title));

		listsql.Add("update tbl_order set status='paid' where order_id='" + order_id + "';");

		foreach (string sql in listsql)
			Scheduler.SaveEventLog("SQL：'" + sql + "'");

		if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
		{
			Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.");

			MyClientSocket.Send0("P " + order_id, false, null);//发送订单号
			Static.LoadCache("tbl_consumer");
		}
		else
		{
			Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单失败.");
		}
		return result;
		#endregion
	}

    public static int XiaDanWeiXin_ZDY(string total, string device_id, string order_id, string WxAppId, string openId, string extra)
    {
        int result = 2;
        //string order_id = WxPayApi.GenerateOutTradeNo();
        #region 服务器下单处理
        string d_id = device_id;
        string product_quantity = "1";
        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_order where order_id='" + order_id + "';");
        if (ds.Tables[1].Rows.Count > 0) return 0;
        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
        BLL.tbl_wx_config wx = ((BLL.tbl_wx_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.WeiXin)[0]);
        int payment_id = wx.payment_id;

        int rec_product_coin = int.Parse(total);//出币数
        int user_id = GetUserId(WxAppId, openId);
        decimal total_fee = decimal.Parse(total);
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string rec_product_title = "其他金额";
        string rec_product_des = "其他金额";
        string rec_product_detail = "其他金额";
        string mch_ids = string.Empty;
        if (string.IsNullOrWhiteSpace(wx.W_SUB_MCHID))
            mch_ids = wx.W_MCHID;
        else
            mch_ids = wx.W_SUB_MCHID + "@" + wx.W_MCHID;

        //查询订单号是否重复
        order_id = IsOrderIdExists(order_id) ? WxPayApi.GenerateOutTradeNo() : order_id;
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host,user_id) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create',0,'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + mch_ids + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "'," + user_id + ");");
        //listsql.Add(string.Format("insert into tbl_device_order values({0},'{1}','','');", d_id, order_id));

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.openId:" + openId + "order_id:" + order_id);
            listsql = new List<string>();
            if (!IsOpenIdExists(openId))
            {
                listsql.Add("insert into tbl_consumer(openid,order_id) values('" + openId + "','" + order_id + "');");
            }
            else
            {
                listsql.Add("update tbl_consumer set order_id='" + order_id + "' where openid='" + openId + "';");
            }
            DbHelperMySQL.ExecuteSqlTran(listsql);
            //Static.LoadCache("tbl_consumer");
        }
        else
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单失败.");
        }
        return result;
        #endregion
    }

    //微信在线下单0:订单存在1:免单2:下单成功
    public static int XiaDan_kb_wx_ZDY(string total,string device_id, string order_id, string WxAppId, string openId, string extra)
    {
        int result = 2;
        //string order_id = WxPayApi.GenerateOutTradeNo();
        #region 服务器下单处理
        string d_id = device_id;
        string product_quantity = "1";
        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_order where order_id='" + order_id + "';");
        if (ds.Tables[1].Rows.Count > 0) return 0;
        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
        BLL.tbl_kb_config kb = ((BLL.tbl_kb_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.YskjPay)[0]);
        int payment_id = kb.payment_id;

        //string device_is_free = ds.Tables[3].Rows[0]["is_free"].ToString();
        int rec_product_coin = int.Parse(total);//出币数

        decimal total_fee = decimal.Parse(total);
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        int user_id = GetUserId(WxAppId, openId);

        string rec_product_title = "其他金额";
        string rec_product_des = "其他金额";
        string rec_product_detail = "其他金额";

        int yskj_payment_id = int.Parse(System.Configuration.ConfigurationManager.AppSettings["yskj_payment_id"].ToString());
        BLL.tbl_wx_config wx = ((BLL.tbl_wx_config)BLL.tbl_agent.GetPayAcc(yskj_payment_id, (int)PayType.WeiXin)[0]); //用mch1=1,禁止移动
        //string mch_ids = string.Empty;
        //if (string.IsNullOrWhiteSpace(wx.W_SUB_MCHID))
        //    mch_ids = wx.W_MCHID;
        //else
        //    mch_ids = wx.W_SUB_MCHID + "@" + wx.W_MCHID;

        //查询订单号是否重复
        order_id = IsOrderIdExists(order_id) ? WxPayApi.GenerateOutTradeNo() : order_id;
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host,user_id) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create',0,'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'','" + extra + "','" + HttpContext.Current.Request.Url.Host + "'," + user_id + ");");
        if (!IsOpenIdExists(openId))
            listsql.Add("insert into tbl_consumer(openid,order_id) values('" + openId + "','" + order_id + "');");
        else
            listsql.Add("update tbl_consumer set order_id='" + order_id + "' where openid='" + openId + "';");

        bool ok_order = (DbHelperMySQL.ExecuteSqlTran(listsql) > 0);
        Scheduler.SaveEventLog(string.Format("IP: {0}, order_id: '{1}' (下单{2})",
            Tools.GetClientIP(), order_id, (ok_order ? "成功" : "失败")));
        return result;
        #endregion
    }
    public static int XiaDan_Wallet_ZDY(string total, string device_id, string order_id, string WxAppId, string OpenId, string extra)
    {
        int result = 2;

        #region 服务器下单处理
        string d_id = device_id;
        string product_quantity = "1";
        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_order where order_id='" + order_id + "';");
        if (ds.Tables[1].Rows.Count > 0)
            return 0;

        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
        int payment_id = GetWalletPaymentId(mch_id);


        int rec_product_coin = int.Parse(total);//出币数

        decimal total_fee = decimal.Parse(total);
        int total_fee_cent = (int)(total_fee * 100);
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        #region 检查钱包
        int user_id = GetUserId(WxAppId, OpenId);
        int wallet_balance_cent = 0;
        DataSet ds_wallet = DbHelperMySQL.Query(string.Format("SELECT balance FROM tbl_consumer_wallet WHERE user_id={0} AND mch_id={1};", user_id, mch_id));
        if ((ds_wallet.Tables.Count > 0) && (ds_wallet.Tables[0].Rows.Count > 0))
            int.TryParse(ds_wallet.Tables[0].Rows[0]["balance"].ToString(), out wallet_balance_cent);

        if (wallet_balance_cent < total_fee_cent)
        {
            Scheduler.SaveEventLog(string.Format("Wallet {0} {1} {2} {3}", WxAppId, OpenId, user_id, mch_id));
            return 0;
        }
        #endregion

        string rec_product_title = "其他金额";
        string rec_product_des = "其他金额";
        string rec_product_detail = "其他金额";

        //查询订单号是否重复
        order_id = Order.IsOrderIdExists(order_id) ? WxPayApi.GenerateOutTradeNo() : order_id;
        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host,user_id) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create',0,'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + user_id + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "'," + user_id + ");");
        //listsql.Add(string.Format("insert into tbl_device_order values({0},'{1}','','');", d_id, order_id));
        Scheduler.SaveEventLog(listsql[0]);

        //if (Static.tbl_consumer.Select("openid='" + OpenId + "'").Length == 0)
        if (DbHelperMySQL.Query("SELECT user_id FROM tbl_consumer WHERE openid='" + OpenId + "';").Tables[0].Rows.Count == 0)
        {
            listsql.Add("insert into tbl_consumer(openid,order_id) values('" + OpenId + "','" + order_id + "');");
        }
        else
        {
            listsql.Add("update tbl_consumer set order_id='" + order_id + "' where openid='" + OpenId + "';");
        }

        listsql.Add(string.Format("UPDATE tbl_consumer_wallet SET balance=(balance-{2}) WHERE user_id='{0}' AND mch_id='{1}';", user_id, mch_id, total_fee_cent));
        listsql.Add(string.Format("INSERT INTO tbl_consumer_wallet_log(user_id,mch_id,amount,comment) VALUES({0},{1},{2},'{3}');",
            user_id, mch_id, -total_fee_cent, rec_product_title));

        listsql.Add("update tbl_order set status='paid' where order_id='" + order_id + "';");

        foreach (string sql in listsql)
            Scheduler.SaveEventLog("SQL：'" + sql + "'");

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.");

            MyClientSocket.Send0("P " + order_id, false, null);//发送订单号
            Static.LoadCache("tbl_consumer");
        }
        else
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单失败.");
        }
        return result;
        #endregion
    }
    public static bool XiaDanAlipay_ZDY(string total, string device_id, string order_id, string extra)
    {
        bool IsMd = false;
        //string order_id = WxPayApi.GenerateOutTradeNo();
        #region 服务器下单处理
        string d_id = device_id;
        string product_quantity = "1";

        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + d_id + ";select * from tbl_order where order_id='" + order_id + "';");
        if (ds.Tables[1].Rows.Count > 0) return false;

        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
        BLL.tbl_zfb_config zfb = ((BLL.tbl_zfb_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.AliPay)[0]);
        int payment_id = zfb.payment_id;

        int rec_product_coin = int.Parse(total);//出币数

        decimal total_fee = decimal.Parse(total);
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        string rec_product_title = "其它金额";
        string rec_product_des = "其它金额";
        string rec_product_detail = "其它金额";

        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host) values('" + order_id + "'," + mch_id + "," + d_id + "," + payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create',0,'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + zfb.Z_PARTNER + "','" + extra + "','" + HttpContext.Current.Request.Url.Host + "');");

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + order_id + "\r\nstatus:下单成功.");
        }
        else
        {
            Scheduler.SaveEventLog("order_id:" + order_id + "\r\nstatus:下单失败.");
        }
        return IsMd;
        #endregion
    }


    public static bool XiaDanTenPay(ref Dictionary<string, object> dict)
    {
        #region 服务器下单处理
        string product_quantity = "1";

        DataSet ds = DbHelperMySQL.Query("select mch_id from tbl_device where device_id=" + dict["device_id"].ToString() + ";select * from tbl_product  where product_id=" + dict["product_id"].ToString() + ";select * from tbl_order where order_id='" + dict["order_id"].ToString() + "';select is_free from tbl_device where device_id=" + dict["device_id"].ToString() + "");
        if (ds.Tables[2].Rows.Count > 0) return false;

        int mch_id = int.Parse(ds.Tables[0].Rows[0]["mch_id"].ToString());
        BLL.tbl_tenpay_config tp = ((BLL.tbl_tenpay_config)BLL.tbl_agent.PayAcc(mch_id, (int)PayType.TenPay)[0]);
        dict.Add("partner", tp.partner);
        dict.Add("key", tp.key);
        dict.Add("payment_id", tp.payment_id);
        dict.Add("total_fee", ds.Tables[1].Rows[0]["total_fee"].ToString());
        dict.Add("title", ds.Tables[1].Rows[0]["title"].ToString());

        string device_is_free = ds.Tables[3].Rows[0]["is_free"].ToString();
        int rec_product_coin = 0;//出币数

        decimal total_fee = decimal.Parse(dict["total_fee"].ToString());
        string time_create = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        string rec_product_title = dict["title"].ToString();
        string rec_product_des = ds.Tables[1].Rows[0]["des"].ToString();
        string rec_product_detail = GetProductDetail(ds.Tables[1].Rows[0], out rec_product_coin);

        List<string> listsql = new List<string>();
        listsql.Add("insert into tbl_order(order_id,mch_id,device_id,payment_id,product_quantity,total_fee,time_create,time_change,status,rec_product_id,rec_product_title,rec_product_des,rec_product_detail,rec_product_coin,pay_mchacc,extra,web_host) values('" + dict["order_id"].ToString() + "'," + mch_id + "," + dict["device_id"].ToString() + "," + tp.payment_id + ",'" + product_quantity + "','" + total_fee + "','" + time_create + "','" + time_create + "','create'," + dict["product_id"].ToString() + ",'" + rec_product_title + "','" + rec_product_des + "','" + rec_product_detail + "'," + rec_product_coin + ",'" + tp.partner + "','" + dict["extra"].ToString() + "','" + HttpContext.Current.Request.Url.Host + "');");

        if (DbHelperMySQL.ExecuteSqlTran(listsql) > 0)
        {
            Scheduler.SaveEventLog("IP：" + Tools.GetClientIP() + "order_id:" + dict["order_id"].ToString() + "\r\nstatus:下单成功.");
        }
        else
        {
            Scheduler.SaveEventLog("order_id:" + dict["order_id"].ToString() + "\r\nstatus:下单失败.");
        }
        #endregion
        return true;
    }*/
}