using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DBUtility;

namespace WxPayAPI
{
    /// <summary>
    /// 扫码支付模式一回调处理类
    /// 接收微信支付后台发送的扫码结果，调用统一下单接口并将下单结果返回给微信支付后台
    /// </summary>
    public class NativeNotify : Notify
    {
        public NativeNotify(Page page)
            : base(page)
        {

        }
        WxPayConfig wx = new WxPayConfig("0");
        public override void ProcessNotify()
        {
            /*Scheduler.SaveEventLog("0");
            WxPayData notifyData = GetNotifyData(wx);

            //检查openid和product_id是否返回
            if (!notifyData.IsSet("openid") || !notifyData.IsSet("product_id"))
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "回调数据异常");
                Log.Info(this.GetType().ToString(), "The data WeChat post is error : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            //调统一下单接口，获得下单结果
            string openid = notifyData.GetValue("openid").ToString();
            string product_id = notifyData.GetValue("product_id").ToString();
            Scheduler.SaveEventLog("product_id:" + product_id);
            
            WxPayData unifiedOrderResult = new WxPayData();
            try
            {
                unifiedOrderResult = UnifiedOrder(openid, product_id);
                //if (unifiedOrderResult == null) return;
            }
            catch (Exception ex)//若在调统一下单接口时抛异常，立即返回结果给微信支付后台
            {
                Scheduler.SaveEventLog("Error:1" + ex.ToString());
                update_order("FAIL", "统一下单失败");
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "统一下单失败");
                Log.Error(this.GetType().ToString(), "UnifiedOrder failure : " + res.ToXml() + ex);
                page.Response.Write(ex.ToString() + res.ToXml());
                page.Response.End();

            } Scheduler.SaveEventLog("Normal:2");

            //若下单失败，则立即返回结果给微信支付后台
            if (!unifiedOrderResult.IsSet("appid") || !unifiedOrderResult.IsSet("mch_id") || !unifiedOrderResult.IsSet("prepay_id"))
            {
                update_order("FAIL", "统一下单失败");
                WxPayData res = new WxPayData();
                //res.SetValue("return_code", "FAIL");
                //res.SetValue("return_msg", "统一下单失败");
                res.SetValue("return_code", unifiedOrderResult.GetValue("return_code"));
                res.SetValue("return_msg", unifiedOrderResult.GetValue("return_msg"));
                res.SetValue("Is_mch_id", unifiedOrderResult.IsSet("mch_id"));
                res.SetValue("Is_appid", unifiedOrderResult.IsSet("appid"));
                res.SetValue("Is_prepay_id", unifiedOrderResult.IsSet("prepay_id"));
                if (unifiedOrderResult.GetValue("return_code").ToString() == "SUCCESS")
                {
                    res.SetValue("err_code", unifiedOrderResult.GetValue("err_code"));
                    res.SetValue("err_code_des", unifiedOrderResult.GetValue("err_code_des"));
                }
                Log.Error(this.GetType().ToString(), "UnifiedOrder failure : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }
            Scheduler.SaveEventLog("Normal:3");
            //统一下单成功,则返回成功结果给微信支付后台
            WxPayData data = new WxPayData();
            data.SetValue("return_code", "SUCCESS");
            data.SetValue("return_msg", "OK");
            data.SetValue("appid", wx.APPID);
            data.SetValue("mch_id", wx.MCHID);
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
            data.SetValue("prepay_id", unifiedOrderResult.GetValue("prepay_id"));
            data.SetValue("result_code", "SUCCESS");
            data.SetValue("err_code_des", "OK");
            data.SetValue("sign", data.MakeSign(wx));
            Scheduler.SaveEventLog("Normal:4");
            Log.Info(this.GetType().ToString(), "UnifiedOrder success , send data to WeChat : " + data.ToXml());
            Scheduler.SaveEventLog("Normal:5");
            page.Response.Write(data.ToXml());
            Scheduler.SaveEventLog("Normal:6" + data.ToXml());
            page.Response.End();*/
        }
        private string order_id = "";
        /*private WxPayData UnifiedOrder(string openId, string productId)
        {
            order_id = WxPayApi.GenerateOutTradeNo();
            #region 服务器下单处理
            DataSet ds = null; decimal total_fee = 0; string p_id = "";
            Order.XiaDan(productId, ref ds, ref total_fee, ref p_id, order_id,ref openId);
            Log.Info(this.GetType().ToString(), "本地服务器下单成功!");
            //if (openId == "MIANFEI") total_fee=0.01m;
            #endregion
            
            //统一下单
            WxPayData req = new WxPayData();
            req.SetValue("body", ds.Tables[1].Rows[0]["des"].ToString());
            req.SetValue("attach", order_id);
            req.SetValue("out_trade_no", order_id);
            req.SetValue("total_fee", (total_fee * 100).ToString("f0"));//单位：分
            req.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            req.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            req.SetValue("goods_tag", p_id);
            req.SetValue("trade_type", "NATIVE");
            req.SetValue("openid", openId);
            req.SetValue("product_id", p_id);
            req.SetValue("device_info", order_id);
            WxPayData result = WxPayApi.UnifiedOrder(req, wx);
            Scheduler.SaveEventLog("发送给微信的价格:"+(total_fee * 100).ToString("f0"));
            return result;
        }
        //更新统一下单错误码
        private void update_order(string err_code, string err_desc)
        {
            string sql = "update tbl_order set status='failed',err_code='" + err_code + "',err_desc='" + err_desc + "' where order_id='" + order_id + "';";
            Scheduler.SaveEventLog(sql);
            DbHelperMySQL.ExecuteSql(sql);
        }*/
    }
}