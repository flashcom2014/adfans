using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.Xml;

namespace WxPayAPI
{
    /// <summary>
    /// 微信转清&二清
    /// </summary>
    public class PayWxMch
    {
        private string appid;
        private string mch_id;
        private string key;
        private string certpath;
        private string certpaw;

        /// <summary>
        /// 初始化转清&二清类
        /// </summary>
        /// <param name="appid">微信分配的公众账号 ID</param>
        /// <param name="mchid">微信支付分配的商户号</param>
        /// <param name="key">商户支付密钥</param>
        /// <param name="certpath">商户证书路径</param>
        /// <param name="certpaw">商户证书密码</param>
        public PayWxMch(string appid, string mchid, string key, string certpath, string certpaw)
        {
            this.appid = appid;
            this.mch_id = mchid;
            this.key = key;
            this.certpath = certpath;
            this.certpaw = certpaw;
        }

        /// <summary>
        /// 微信分配的公众账号 ID
        /// </summary>
        public string APPID
        {
            get
            {
                return this.appid;
            }
            set
            {
                this.appid = value;
            }
        }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string Mch_ID
        {
            get
            {
                return this.mch_id;
            }
            set
            {
                this.mch_id = value;
            }
        }

        /// <summary>
        /// 商户支付密钥
        /// </summary>
        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }

        /// <summary>
        /// 商户证书路径
        /// </summary>
        public string CertPath
        {
            get
            {
                return this.certpath;
            }
            set
            {
                this.certpath = value;
            }
        }

        /// <summary>
        /// 商户证书密码
        /// </summary>
        public string CertPassWrod
        {
            get
            {
                return this.certpaw;
            }
            set
            {
                this.certpaw = value;
            }
        }

        /// <summary>
        /// 新增商户资料
        /// </summary>
        /// <param name="merchant_name">商户名称(须与营业执照的名称保持一致)</param>
        /// <param name="merchant_shortname">商户简称(该名称将于支付成功页向消费者进行展示)</param>
        /// <param name="service_phone">客服电话(必填，以方便微信在必要时能联系上商家)</param>
        /// <param name="business">经营类目(必填，须与实际售卖商品保持一致。如开通公 众号将会在公众号的 Profile 页面展示)</param>
        /// <param name="merchant_remark">商户备注(同一个受理机构，子商户“商户备注”唯一。不 同受理机构间，“商户备注”允许重复 商户备注重复时，生成商户识别码失败，并返 回提示信息“商户备注已存在，请修改后重新提 交”)</param>
        /// <param name="contact">联系人(选填，以方便微信在必要时能联系上商家。此字段也用于商户平台安装操作证书使用（转清二清商户暂用不上）)</param>
        /// <param name="contact_phone">联系电话(选填，以方便微信在必要时能联系上商家。此字段也用于发送开户邮件、重置密码使用（转清二清商户暂用不上）)</param>
        /// <param name="contact_email">联系邮箱(选填，以方便微信在必要时能联系上商家。此字段也用于发送开户邮件、重置密码使用（转清二清商户暂用不上）)</param>
        /// <returns></returns>
        public AddMchResult AddMch(string merchant_name, string merchant_shortname, string service_phone, string business, string merchant_remark, string contact = null, string contact_phone = null, string contact_email = null)
        {
            try
            {
                WxPayData wxdata = new WxPayData();
                wxdata.SetValue("appid", APPID);
                wxdata.SetValue("mch_id", Mch_ID);
                wxdata.SetValue("merchant_name", merchant_name);
                wxdata.SetValue("merchant_shortname", merchant_shortname);
                wxdata.SetValue("service_phone", service_phone);
                if (contact != null)
                    wxdata.SetValue("contact", contact);
                if (contact_phone != null)
                    wxdata.SetValue("contact_phone", contact_phone);
                if (contact_email != null)
                    wxdata.SetValue("contact_email", contact_email);
                wxdata.SetValue("business", business);
                wxdata.SetValue("merchant_remark", merchant_remark);
                string sign = wxdata.MakeSign(Key);
                wxdata.SetValue("sign", sign);
                string postdata = wxdata.ToXml();
                //
                string url = "https://api.mch.weixin.qq.com/secapi/mch/submchmanage?action=add";
                string xml = HttpService.Post(postdata, url, 60, CertPath, CertPassWrod);

                AddMchResult amr = new AddMchResult();
                amr.FromXML(xml);

                return amr;
            }
            catch (Exception ex)
            {
                AddMchResult amr = new AddMchResult();
                amr.return_code = "FAIL";
                amr.return_msg = ex.Message;
                return amr;
            }
        }

        /// <summary>
        /// 查询商户资料
        /// </summary>
        /// <param name="sub_mch_id">商户识别码（微信支付分配的商户识别码，转清或二清受理模式下使用）</param>
        /// <returns></returns>
        public QueryMchResult QueryMch(string sub_mch_id)
        {
            return QueryMch(sub_mch_id, "", 1, 100);
        }

        /// <summary>
        /// 查询商户资料
        /// </summary>
        /// <param name="sub_mch_id">商户识别码（微信支付分配的商户识别码，转清或二清受理模式下使用）</param>
        /// <param name="merchant_name">商户名称（公司名称需与营业执照登记公司名称一致）</param>
        /// <param name="page_index">页码（当前查询的具体分页页面）</param>
        /// <param name="page_size">展示资料个数（每一个分页，可展示多少条资料信息）</param>
        /// <returns></returns>
        public QueryMchResult QueryMch(string sub_mch_id, string merchant_name, int page_index, int page_size)
        {
            try
            {
                WxPayData wxdata = new WxPayData();
                wxdata.SetValue("appid", APPID);
                wxdata.SetValue("mch_id", Mch_ID);
                wxdata.SetValue("sub_mch_id", sub_mch_id);
                wxdata.SetValue("merchant_name", merchant_name);
                wxdata.SetValue("page_index", page_index.ToString());
                wxdata.SetValue("page_size", page_size.ToString());
                string sign = wxdata.MakeSign(Key);
                wxdata.SetValue("sign", sign);
                string postdata = wxdata.ToXml();

                //
                string url = "https://api.mch.weixin.qq.com/secapi/mch/submchmanage?action=query";
                string xml = HttpService.Post(postdata, url, 60, CertPath, CertPassWrod);

                QueryMchResult qmr = new QueryMchResult();
                qmr.FromXML(xml);

                return qmr;
                
            }
            catch (Exception ex)
            {
                QueryMchResult qmr = new QueryMchResult();
                qmr.result_code = "FAIL";
                qmr.result_msg = ex.Message;
                return qmr;
            }
        }

        /// <summary>
        /// 修改商户资料
        /// </summary>
        /// <param name="sub_mch_id">商户识别码</param>
        /// <param name="merchant_shortname">商户简称</param>
        /// <param name="service_phone">客服电话</param>
        /// <param name="contact">联系人</param>
        /// <returns></returns>
        public AddMchResult MdifyMch(string sub_mch_id, string merchant_shortname, string service_phone, string contact = null)
        {
            try
            {
                WxPayData wxdata = new WxPayData();
                wxdata.SetValue("appid", APPID);
                wxdata.SetValue("mch_id", Mch_ID);
                wxdata.SetValue("sub_mch_id", sub_mch_id);
                wxdata.SetValue("merchant_shortname", merchant_shortname);
                wxdata.SetValue("service_phone", service_phone);
                if (contact != null)
                    wxdata.SetValue("contact", contact);
                string sign = wxdata.MakeSign(Key);
                wxdata.SetValue("sign", sign);
                string postdata = wxdata.ToXml();
                //
                string url = "https://api.mch.weixin.qq.com/secapi/mch/submchmanage?action=modify";
                string xml = HttpService.Post(postdata, url, 60, CertPath, CertPassWrod);

                AddMchResult amr = new AddMchResult();
                amr.FromXML(xml);

                return amr;
            }
            catch (Exception ex)
            {
                AddMchResult amr = new AddMchResult();
                amr.return_code = "FAIL";
                amr.return_msg = ex.Message;
                return amr;
            }
        }

        /// <summary>
        /// 删除商户资料
        /// </summary>
        /// <param name="sub_mch_id">商户识别码</param>
        /// <returns></returns>
        public Dictionary<string,string> DelMch(string sub_mch_id)
        {
            try
            {
                WxPayData wxdata = new WxPayData();
                wxdata.SetValue("appid", APPID);
                wxdata.SetValue("mch_id", Mch_ID);
                wxdata.SetValue("sub_mch_id", sub_mch_id);
                string sign = wxdata.MakeSign(Key);
                wxdata.SetValue("sign", sign);
                string postdata = wxdata.ToXml();
                //
                string url = "https://api.mch.weixin.qq.com/secapi/mch/submchmanage?action=del";
                string xml = HttpService.Post(postdata, url, 60, CertPath, CertPassWrod);

                Dictionary<string, string> tl = FromXML(xml);

                return tl;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> list = new Dictionary<string, string>();
                list["return_code"] = "FAIL";
                list["return_msg"] = ex.Message;
                return list;
            }
        }

        private Dictionary<string,string> FromXML(string xml)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc["root"];//获取到根节点<root>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                list[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }
            return list;
        }
    }

    /// <summary>
    /// 查询商户资料返回类
    /// </summary>
    public class QueryMchResult
    {
        /// <summary>
        /// 回状态码(SUCCESS/FAIL)
        /// </summary>
        public string return_code;
        /// <summary>
        /// 返回信息(查询成功，直接展示商户信息，返回信息值为空，查询失败，返回信息暂未数据)
        /// </summary>
        public string return_msg;
        /// <summary>
        /// 处理结果(SUCCESS/FAIL)
        /// </summary>
        public string result_code;
        /// <summary>
        /// 微信接口返回的XML包
        /// </summary>
        public string result_msg;
        /// <summary>
        /// 总记录数(符合条件的有多少条记录)
        /// </summary>
        public int total = 0;
        /// <summary>
        /// 返回子商户列表
        /// </summary>
        public List<MchInfo> list = new List<MchInfo>();
        /// <summary>
        /// 微信接口返回XML包
        /// </summary>
        public string XML;

        public void FromXML(string xml)
        {
            this.XML = xml;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc["root"];//获取到根节点<root>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                switch (xe.Name)
                {
                    case "return_code":
                        return_code = xe.InnerText;
                        break;
                    case "return_msg":
                        return_msg = xe.InnerText;
                        break;
                    case "result_code":
                        result_code = xe.InnerText;
                        break;
                    case "result_msg":
                        result_msg = xe.InnerText;
                        break;
                    case "total":
                        total = int.Parse(xe.InnerText);
                        break;
                    case "mchinfo":
                        XmlNodeList cnodes = xe.ChildNodes;
                        MchInfo minfo = new MchInfo();
                        minfo.FromXMLNode(cnodes);
                        list.Add(minfo);
                        break;
                }
            }
        }

        /// <summary>
        /// 返回指定子商户号的信息
        /// </summary>
        /// <param name="mch_id">子商户号</param>
        /// <returns></returns>
        public MchInfo GetMchInfo(string mch_id)
        {
            foreach (MchInfo minfo in list)
            {
                if (minfo.mch_id == mch_id)
                    return minfo;
            }
            return null;
        }

        public bool QueryOK
        {
            get
            {
                return list.Count > 0;
            }
        }
    }

    public class MchInfo
    {
        /// <summary>
        /// 商户识别码
        /// </summary>
        public string mch_id;
        /// <summary>
        /// 商户名称
        /// </summary>
        public string merchant_name;
        /// <summary>
        /// 商户简称
        /// </summary>
        public string merchant_shortname;
        /// <summary>
        /// 客服电话
        /// </summary>
        public string service_phone;
        /// <summary>
        /// 联系人
        /// </summary>
        public string contact;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string contact_phone;
        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string contact_email;
        /// <summary>
        /// 经营类目
        /// </summary>
        public string business;
        /// <summary>
        /// 商户备注
        /// </summary>
        public string merchant_remark;

        public void FromXMLNode(XmlNodeList nodes)
        {
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                switch (xe.Name)
                {
                    case "mch_id":
                        mch_id = xe.InnerText;
                        break;
                    case "merchant_name":
                        merchant_name = xe.InnerText;
                        break;
                    case "merchant_shortname":
                        merchant_shortname = xe.InnerText;
                        break;
                    case "service_phone":
                        service_phone = xe.InnerText;
                        break;
                    case "contact":
                        contact = xe.InnerText;
                        break;
                    case "contact_phone":
                        contact_phone = xe.InnerText;
                        break;
                    case "contact_email":
                        contact_email = xe.InnerText;
                        break;
                    case "business":
                        business = xe.InnerText;
                        break;
                    case "merchant_remark":
                        merchant_remark = xe.InnerText;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 添加子商户返回类
    /// </summary>
    public class AddMchResult
    {
        /// <summary>
        /// 返回状态码(SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成 功需要查看 result_code 来判断)
        /// </summary>
        public string return_code;
        /// <summary>
        /// 返回信息(返回信息，如非空，为错误原因签名失败)
        /// </summary>
        public string return_msg;
        /// <summary>
        /// 商户号(微信支付分配的商户号)
        /// </summary>
        public string mch_id;
        /// <summary>
        /// 商户识别码(微信支付分配的商户识别码，转清或二清受理模式下使用)
        /// </summary>
        public string sub_mch_id;
        /// <summary>
        /// 处理结果(SUCCESS/FAIL)
        /// </summary>
        public string result_code;
        /// <summary>
        /// 处理信息(结果信息描述。处理成功，错误原因提示等)
        /// </summary>
        public string result_msg;
        /// <summary>
        /// 微信接口返回的XML包
        /// </summary>
        public string XML;

        public void FromXML(string xml)
        {
            this.XML = xml;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc["root"];//获取到根节点<root>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                switch (xe.Name)
                {
                    case "return_code":
                        return_code = xe.InnerText;
                        break;
                    case "return_msg":
                        return_msg = xe.InnerText;
                        break;
                    case "mch_id":
                        mch_id = xe.InnerText;
                        break;
                    case "sub_mch_id":
                        sub_mch_id = xe.InnerText;
                        break;
                    case "result_code":
                        result_code = xe.InnerText;
                        break;
                    case "result_msg":
                        result_msg = xe.InnerText;
                        break;
                }
            }
        }

        /// <summary>
        /// 是否成功添加了子商户
        /// </summary>
        public bool AddOK
        {
            get
            {
                return !string.IsNullOrEmpty(mch_id);
            }
        }
    }
}