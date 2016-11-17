using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DBUtility;
using System.Data;

namespace WxPayAPI
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify:Notify
    {
        public ResultNotify(Page page)
            : base(page)
        {
        }
        WxPayConfig wx = null; WxPayData notifyData;
        public override void ProcessNotify()
        {
            /*Scheduler.SaveEventLog("微信回调：1"+HttpContext.Current.Request.UserHostAddress);
            notifyData = GetNotifyData(ref wx);
            Scheduler.SaveEventLog("微信回调：2" + notifyData.ToJson());

            string transaction_id = "";

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                page.Response.Write(res.ToXml());
               // page.Response.End();
            }

            #region 订单查询结果,更新数据库
            if (notifyData.GetValue("return_code").ToString() == "SUCCESS")
            {
                string order_id = notifyData.GetValue("out_trade_no").ToString();
                string trade_state = notifyData.GetValue("result_code").ToString();
                transaction_id = notifyData.GetValue("transaction_id").ToString();
                string time_pay = "";
                switch (trade_state)
                {
                    case "SUCCESS": // "支付成功"; 
                        trade_state = "paid";
                        time_pay = DateTime.ParseExact(notifyData.GetValue("time_end").ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case "FALL": // "支付失败"; 
                        trade_state = "failed";
                        break;
                }
                Scheduler.SaveEventLog("支付状态：" + trade_state);
                string sql = "update tbl_order set time_pay='" + time_pay + "',status='" + trade_state + "',trade_no='" + transaction_id + "' where order_id='" + order_id + "' and status='create';";
                int result = DbHelperMySQL.ExecuteSql(sql);

                if (result > 0 && trade_state == "paid")
                {
                    #region 添加收款账号日志
                    string mch_ids = string.Empty;
                    if (string.IsNullOrWhiteSpace(wx.SUB_MCHID))
                        mch_ids = wx.MCHID;
                    else
                        mch_ids = wx.SUB_MCHID + "@" + wx.MCHID;

                    BLL.tbl_payment payment = new BLL.tbl_payment(wx.payment_id);
                    sql = "INSERT INTO tbl_last_sk_log (alipay_acc,a_id,m_id,order_id,update_time) VALUES ('{0}',{1},{3},'{4}','{2}');";
                    if (payment.m_id == 0)
                    {
                        sql += "INSERT INTO tbl_last_sk(alipay_acc,a_id,update_time) VALUES ('{0}',{1},'{2}') on duplicate key update alipay_acc='{0}',a_id={1},update_time='{2}'";

                    }
                    sql = string.Format(sql, mch_ids, payment.a_id, DateTime.Now, payment.m_id, order_id);

                    //
                    result = DbHelperMySQL.ExecuteSql(sql);
                    #endregion

                    DataSet ds = DbHelperMySQL.Query("select ex_device_type from tbl_device where device_id=(select device_id from tbl_order where order_id='" + order_id + "');");
                    DataTable device = ds.Tables[0];
                    //DataTable device1 = ds.Tables[1];
                    //int coin_sum = int.Parse(device1.Rows[0]["quantity"].ToString()) + int.Parse(device1.Rows[0]["free_coin"].ToString());
                    string ex_device_type = device.Rows[0]["ex_device_type"].ToString();
                    if (ex_device_type == "6")//POST机类型的订单不发送给代理,直接关闭订单
                    {
                        sql = "update `tbl_order` SET `status` = 'closed', `ship_code` = 100,`ship_desc` = '', `rec_product_payout` =rec_product_coin WHERE `status` = 'paid' AND `order_id` = '" + order_id + "'";
                        result = DbHelperMySQL.ExecuteSql(sql);
                        if (result > 0)
                            MyClientSocket.SendWeb("P " + order_id);//模拟代理通知web8080,让其通过API回调商家
                    }
					else if (ProcessWalletPay(order_id))
					{
						Scheduler.SaveEventLog(string.Format("錢包充值訂單 '{0}' Process", order_id));
					}
					else
                    {
                        MyClientSocket.Send0("P " + order_id, false, null);//发送订单号
                    }
                    //string mch_id = Order.GetMch_Id(order_id);
                    //new EnterprisePay().StartPay(mch_id);//打款
                }
                Scheduler.SaveEventLog((result > 0 && trade_state == "paid").ToString() + "数据操作：" + sql);
            }
            #endregion

           

            try
            {
                //查询订单，判断订单真实性
                QueryOrder(transaction_id);
                //if (!QueryOrder(transaction_id))
                //{
                //    //若订单查询失败，则立即返回结果给微信支付后台
                //    WxPayData res = new WxPayData();
                //    res.SetValue("return_code", "FAIL");
                //    res.SetValue("return_msg", "订单查询失败");
                //    Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                //    page.Response.Write(res.ToXml());
                //    //page.Response.End();
                //}
                ////查询订单成功
                //else
                //{
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                    page.Response.Write(res.ToXml());
                   //page.Response.End();
                //}
            }
            catch (Exception ex)
            {
                Scheduler.SaveEventLog(ex.ToString());
            }*/
        }

        /// <summary>
        /// 增值服务充值回调
        /// </summary>
        public override void ProcessNotify2()
        {
            Scheduler.SaveEventLog("微信回调：1" + HttpContext.Current.Request.UserHostAddress);
            notifyData = GetNotifyData();
            Scheduler.SaveEventLog("微信回调：2" + notifyData.ToJson());

            string transaction_id = "";

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            #region 订单查询结果,更新数据库
            if (notifyData.GetValue("return_code").ToString() == "SUCCESS")
            {
                string order_id = notifyData.GetValue("out_trade_no").ToString();
                string trade_state = notifyData.GetValue("result_code").ToString();
                transaction_id = notifyData.GetValue("transaction_id").ToString();
                string time_pay = "";
                switch (trade_state)
                {
                    case "SUCCESS": // "支付成功"; 
                        trade_state = "paid";
                        time_pay = DateTime.ParseExact(notifyData.GetValue("time_end").ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case "FALL": // "支付失败"; 
                        trade_state = "failed";
                        break;
                }
                Scheduler.SaveEventLog("支付状态：" + trade_state);
                string sql = "update tbl_value_add_pay set time_pay='" + time_pay + "',status='" + trade_state + "',trade_no='" + transaction_id + "' where order_id='" + order_id + "' and status='create';";
                int result = DbHelperMySQL.ExecuteSql(sql);

                if (result > 0 && trade_state == "paid")
                {
                    
                }
                
            }
            #endregion



            try
            {
                //查询订单，判断订单真实性
                QueryOrder(transaction_id);
                //if (!QueryOrder(transaction_id))
                //{
                //    //若订单查询失败，则立即返回结果给微信支付后台
                //    WxPayData res = new WxPayData();
                //    res.SetValue("return_code", "FAIL");
                //    res.SetValue("return_msg", "订单查询失败");
                //    Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                //    page.Response.Write(res.ToXml());
                //    //page.Response.End();
                //}
                ////查询订单成功
                //else
                //{
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
                //}
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                Scheduler.SaveEventLog(ex.ToString());
            }
        }

        /// <summary>
        /// 跟游戏中心连接的回调
        /// </summary>
        public override void ProcessNotify3()
        {
            Scheduler.SaveEventLog("微信回调：1" + HttpContext.Current.Request.UserHostAddress);
            notifyData = GetNotifyData();
            Scheduler.SaveEventLog("微信回调：2" + notifyData.ToJson());

            string transaction_id = "";

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            #region 订单查询结果,更新数据库
            if (notifyData.GetValue("return_code").ToString() == "SUCCESS")
            {
                string order_id = notifyData.GetValue("out_trade_no").ToString();
                string trade_state = notifyData.GetValue("result_code").ToString();
                transaction_id = notifyData.GetValue("transaction_id").ToString();
                string time_pay = "";
                switch (trade_state)
                {
                    case "SUCCESS": // "支付成功"; 
                        trade_state = "paid";
                        time_pay = DateTime.ParseExact(notifyData.GetValue("time_end").ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case "FALL": // "支付失败"; 
                        trade_state = "failed";
                        break;
                }
                Scheduler.SaveEventLog("支付状态：" + trade_state);
                string sql = "update tbl_wxgame_pay set time_pay='" + time_pay + "',status='" + trade_state + "',trade_no='" + transaction_id + "' where order_id='" + order_id + "' and status='waitpay';";
                int result = DbHelperMySQL.ExecuteSql(sql);

                if (result > 0 && trade_state == "paid")
                {

                }

            }
            #endregion



            try
            {
                //查询订单，判断订单真实性
                QueryOrder(transaction_id);
                //if (!QueryOrder(transaction_id))
                //{
                //    //若订单查询失败，则立即返回结果给微信支付后台
                //    WxPayData res = new WxPayData();
                //    res.SetValue("return_code", "FAIL");
                //    res.SetValue("return_msg", "订单查询失败");
                //    Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                //    page.Response.Write(res.ToXml());
                //    //page.Response.End();
                //}
                ////查询订单成功
                //else
                //{
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
                //}
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                Scheduler.SaveEventLog(ex.ToString());
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            /*
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req,wx);
            
            if (res.GetValue("return_code").ToString() == "SUCCESS" && res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                BLL.tbl_wx_pay_notify_error error = new BLL.tbl_wx_pay_notify_error();
                error.ip = HttpContext.Current.Request.UserHostAddress;
                error.notify_xml = notifyData.ToJson();
                req.SetValue("appid",wx.APPID);
                error.query_xml = req.ToJson();
                error.query_return = res.ToJson();
                error.Add();
                Scheduler.SaveEventLog("查询订单失败：" + res.ToJson());
                return false;
            }*/
            return false;
        }

		protected bool ProcessWalletPay(string OrderId)
		{
			int mch_id = 0, order_type = 0, user_id = 0;
			decimal total_fee = 0;
			int rec_product_coin = 0;
			DataSet ds_order = DbHelperMySQL.Query("SELECT `mch_id`,`type`,`user_id`,`total_fee`,`rec_product_coin` FROM `tbl_order` WHERE `order_id`='" + OrderId + "';");
			if ((ds_order.Tables.Count > 0) && (ds_order.Tables[0].Rows.Count > 0))
			{
				DataTable dt_order = ds_order.Tables[0];
				DataRow dr_order = dt_order.Rows[0];
				int.TryParse(dr_order["mch_id"].ToString(), out mch_id);
				int.TryParse(dr_order["type"].ToString(), out order_type);
				int.TryParse(dr_order["user_id"].ToString(), out user_id);
				decimal.TryParse(dr_order["total_fee"].ToString(), out total_fee);
				int.TryParse(dr_order["rec_product_coin"].ToString(), out rec_product_coin);
			}

			if (order_type == 1) // 錢包充值訂單
			{
				Scheduler.SaveEventLog(string.Format("錢包充值訂單 '{0}',mchid={1},type={2},user_id={3},total_fee={4}", OrderId, mch_id, order_type, user_id, total_fee));

				string sql = "UPDATE `tbl_order` SET `status`='closed',`ship_code`=100,`ship_desc`='',`rec_product_payout`=0 WHERE `status` = 'paid' AND `order_id` = '" + OrderId + "';";
				bool ok = (DbHelperMySQL.ExecuteSql(sql) > 0);
				Scheduler.SaveEventLog(string.Format("錢包充值訂單 '{0}' {1}", OrderId, (ok ? "OK" : "failed")));

				int pay_amount = rec_product_coin; // (int)(total_fee * 100);

				List<string> tran_wallet = new List<string>();
				string sql_update_wallet = string.Format(
					"INSERT INTO `tbl_consumer_wallet`(`user_id`,`mch_id`,`balance`)" +
					" VALUES({0},{1},{2})" +
					" ON DUPLICATE KEY" +
					" UPDATE `balance`=`balance`+{2};",
					user_id, mch_id, pay_amount);
				tran_wallet.Add(sql_update_wallet);

				string sql_wallet_log = string.Format(
					"INSERT INTO `tbl_consumer_wallet_log`(`user_id`,`mch_id`,`amount`,`comment`)" +
					" VALUES({0},{1},{2},'{3}');",
					user_id, mch_id, pay_amount, "钱包充值");
				tran_wallet.Add(sql_wallet_log);

				bool ok_update_wallet = (DbHelperMySQL.ExecuteSqlTran(tran_wallet) > 0);
				Scheduler.SaveEventLog(string.Format("錢包充值訂單 '{0}' {1} 添加充值金额至钱包", OrderId, (ok_update_wallet ? "OK" : "failed")));

				return true;
			}
			return false;
		}
	}
    
}