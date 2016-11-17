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

public partial class admin_ad_admin_mp_list : AdminPageBasead
{
    public string QrUrl = "";
    public string QRText = "";

    string exlist = ".jpg.jpeg.gif.png.bmp";
    protected void Page_Load(object sender, EventArgs e)
    {

       
        if (Request.QueryString["action"]!=null&&Request.QueryString["action"].ToString() == "testcode")
        {
            TestCode();
        }
        
        else
        {
           
            LoadMPs(true);
        }
    }
    public void TestCode()
    {
        try
        {
            string sid = Request.Form["id"];
            if (string.IsNullOrEmpty(sid))
            {
                Response.Write("[{\"result\":\"no\",\"info\":\"参数错误\"}]");
                Response.End();
            }
            byte[] bdata = Convert.FromBase64String(sid);

            int id = BitConverter.ToInt32(bdata, 4);

            string sqlstr = string.Format("select * from tbl_wx_xifenji where id={0};", id);
            string rest = "[{\"result\":\"nodata\",\"info\":\"NO Data\"}]";
            string connst = DBUtility.DbHelperMySQL.connectionString;
            using (MySqlConnection conn = new MySqlConnection(connst))
            {
                DateTime returntime = DateTime.Now.AddSeconds(5);  //10秒没数据直接返回
                conn.Open();
                MySqlCommand query = new MySqlCommand(sqlstr, conn);
                MySqlDataReader reader = null;
                bool ok = false;
                int status = 1;
                while (DateTime.Now < returntime)
                {
                    reader = query.ExecuteReader();
                    if (reader.Read())
                    {
                        //
                        status = int.Parse(reader["status"].ToString());

                        if (status == 0)
                            ok = true;
                    }
                    reader.Close();

                    if (ok)
                    {
                        rest = "[{\"result\":\"ok\",\"info\":\"OK\"}]";
                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                query.Dispose();
            }
            Response.Write(rest);
            Response.End();


        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("mp_list: " + ex.ToString());
            Response.Write("[{\"result\":\"no\",\"info\":\"参数错误\"}]");
            Response.End();
        }

    }
    private void LoadMPs(bool iscache)
    {
        if (iscache && !IsPostBack && ViewState["dt"] != null)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            gv.DataSource = dt;
            gv.DataBind();
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        else
        {
            string sqlstr = "select f.*,m.ad_name,m.ad_mobile,m.ad_id,a.mch_acc from tbl_wx_xifenji as f left join tbl_ad_mch as m on f.ad_id=m.ad_id left join tbl_mch_account as a on f.mch_id=a.mch_id";
            if (!string.IsNullOrEmpty(t_admch.Text) || !string.IsNullOrEmpty(TextBox1.Text))
                sqlstr += " where";
            if (!string.IsNullOrEmpty(t_admch.Text))
            {
                sqlstr += string.Format(" (m.ad_name like '%{0}%' or m.ad_mobile like '%{0}%') and", t_admch.Text);
            }
            if (!string.IsNullOrEmpty(TextBox1.Text))
            {
                sqlstr += string.Format(" f.appid like '%{0}%' and", TextBox1.Text);
            }
            if (sqlstr.EndsWith(" and"))
                sqlstr = sqlstr.Remove(sqlstr.Length - 4);

            //string sqlstr = string.Format("select * from tbl_wx_xifenji where ad_id={0};", ad_id);
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            gv.DataSource = dt;
            gv.DataBind();
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            ViewState["dt"] = dt;
        }
    }

    public string GetMpTypes()
    {
        int mp_types = int.Parse(Eval("mp_types").ToString());
        switch (mp_types)
        {
            case 0:
                return "服务号";
            case 1:
                return "订阅号";
            case 2:
                return "企业号";
        }
        return "未知";
    }

    public string GetEnable(object obj)
    {
        if (obj.ToString() == "1")
            return "可用";
        return "不可用";
    }
    public string GetStatus(object obj)
    {
        int status = int.Parse(obj.ToString());
        switch (status)
        {
            case 0:
                return "<label style=\"color:blue;\">已通过检测</label>";
            case 1:
                return "<label style=\"color:red;\">未检测</label>";
            case 2:
                return "<label style=\"color:black;\">正在检测</label>";
            case 3:
                return "<label style=\"color:green;\">状态异常，请重新检测</label>";
        }
        return "<label style=\"color:green;\">状态异常，请重新检测</label>";
    }
    public string GetAdName()
    {
        string name = Eval("ad_name").ToString();
        string mobile = Eval("ad_mobile").ToString();
        if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(mobile))
            return string.Empty;
        return string.Format("{0}({1})", name, mobile);
    }
    public string GetAmount(object obj)
    {
        float money = float.Parse(obj.ToString());
        money = money / 100f;
        return money.ToString("F2");
    }

    

  

    protected void b_openedit_Click(object sender, EventArgs e)
    {
        string id = h_id.Value;
        string sqlstr = string.Format("select * from tbl_wx_xifenji where id={0}", id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            Jscript.Alert2(Page, "待编辑的公众号不存在");
            return;
        }

        DataRow row = dt.Rows[0];
        p_list.Visible = false;
        pztable.Visible = false;
        p_addedit.Visible = true;
       
        b_edit.Visible = true;
        trid.Visible = false;
        t_tags.Text = row["tags"].ToString();
        t_desc.Text = row["desp"].ToString();
        t_appid.Text = row["appid"].ToString();
        t_appsecret.Text = row["appsecret"].ToString();
        t_originalid.Text = row["original_id"].ToString();
        t_title.Text = row["title"].ToString();
        c_enable.Checked = row["enable"].ToString() == "1";
        d_mptypes.SelectedValue = row["mp_types"].ToString();
        d_status.SelectedValue = row["status"].ToString();
        dt.Dispose();
        ds.Dispose();
    }

    protected void b_del_Click(object sender, EventArgs e)
    {
        string id = h_id.Value;
        string sqlstr = string.Format("delete from tbl_wx_xifenji where id={0}", id);
        int result = DbHelperMySQL.ExecuteSql(sqlstr);
        if (result > 0)
        {
            p_list.Visible = true;
            pztable.Visible = false;
            trid.Visible = true;
            p_addedit.Visible = false;
            LoadMPs(false);
        }
        else
        {
            Jscript.Alert2(Page, "删除公众号出错，请稍候重试");
        }
    }

    protected void b_edit_Click(object sender, EventArgs e)
    {
        try
        {
            string appid = t_appid.Text;
            string appsecret = t_appsecret.Text;

            if (!CheckAppID(appid, appsecret))
            {
                Jscript.Alert2(Page, "经检测公众号信息不正确，请重新填写");
                return;
            }

            string token = MchAgentIDEn.GetAgentToken(ad_id);
            string aeskey = MchAgentIDEn.GetAgentAESKey(ad_id);

            string sqlstr = "";
            string id = h_id.Value;
            if (f_upload.HasFile)
            {


                string filename = f_upload.PostedFile.FileName;
                string exp = filename.Substring(filename.LastIndexOf("."));
                if (exlist.IndexOf(exp) < 0)
                {
                    Jscript.Alert2(Page, "非法操作，你的IP已被记下！");
                    return;
                }
                string strName = string.Format("{0}_ad{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ad_id, exp);


                //设置图片存放路径
                string path = Server.MapPath("~/kindeditor/attached/WX_Image/");
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                string strUpPath = path + strName;

                f_upload.SaveAs(strUpPath);//上传图片
                                           //
                string imgurl = "/kindeditor/attached/WX_Image/" + strName;

                sqlstr = "update tbl_wx_xifenji set tags=@tags,desp=@desp,appid=@appid,appsecret=@appsecret,original_id=@original_id,token=@token,aeskey=@aeskey,title=@title,imgurl=@imgurl,enable=@enable,mp_types=@mp_types,status=@status where id=@id";
                MySqlParameter mtags = new MySqlParameter("@tags", MySqlDbType.VarChar, 50);
                mtags.Value = t_tags.Text;
                MySqlParameter mdesc = new MySqlParameter("@desp", MySqlDbType.VarChar, 250);
                mdesc.Value = t_desc.Text;
                MySqlParameter mappid = new MySqlParameter("@appid", MySqlDbType.VarChar, 100);
                mappid.Value = appid;
                MySqlParameter mappsecret = new MySqlParameter("@appsecret", MySqlDbType.VarChar, 100);
                mappsecret.Value = appsecret;
                MySqlParameter moriginal_id = new MySqlParameter("@original_id", MySqlDbType.VarChar, 100);
                moriginal_id.Value = t_originalid.Text;
                MySqlParameter mtoken = new MySqlParameter("@token", MySqlDbType.VarChar, 100);
                mtoken.Value = token;
                MySqlParameter maeskey = new MySqlParameter("@aeskey", MySqlDbType.VarChar, 100);
                maeskey.Value = aeskey;
                MySqlParameter mtitle = new MySqlParameter("@title", MySqlDbType.VarChar, 50);
                mtitle.Value = t_title.Text;
                MySqlParameter mimgurl = new MySqlParameter("@imgurl", MySqlDbType.VarChar, 256);
                mimgurl.Value = imgurl;
                MySqlParameter menable = new MySqlParameter("@enable", MySqlDbType.Int32);
                menable.Value = c_enable.Checked ? 1 : 0;
                MySqlParameter mmp_types = new MySqlParameter("@mp_types", MySqlDbType.Int32);
                mmp_types.Value = int.Parse(d_mptypes.SelectedValue);
                MySqlParameter mstatus = new MySqlParameter("@status", MySqlDbType.Int32);
                mstatus.Value = int.Parse(d_status.SelectedValue);

                MySqlParameter mid = new MySqlParameter("@id", MySqlDbType.Int32);
                mid.Value = int.Parse(id);

                int result = DbHelperMySQL.ExecuteSql(sqlstr, mtags, mdesc, mappid, mappsecret, moriginal_id, mtoken, maeskey, mtitle, mimgurl, menable, mmp_types, mstatus, mid);
                if (result == 0)
                {
                    Jscript.Alert2(Page, "更新公众号出错！");
                    return;
                }
                //

            }
            else
            {
                sqlstr = "update tbl_wx_xifenji set tags=@tags,desp=@desp,appid=@appid,appsecret=@appsecret,original_id=@original_id,token=@token,aeskey=@aeskey,title=@title,enable=@enable,mp_types=@mp_types,status=@status where id=@id";
                MySqlParameter mtags = new MySqlParameter("@tags", MySqlDbType.VarChar, 50);
                mtags.Value = t_tags.Text;
                MySqlParameter mdesc = new MySqlParameter("@desp", MySqlDbType.VarChar, 250);
                mdesc.Value = t_desc.Text;
                MySqlParameter mappid = new MySqlParameter("@appid", MySqlDbType.VarChar, 100);
                mappid.Value = appid;
                MySqlParameter mappsecret = new MySqlParameter("@appsecret", MySqlDbType.VarChar, 100);
                mappsecret.Value = appsecret;
                MySqlParameter moriginal_id = new MySqlParameter("@original_id", MySqlDbType.VarChar, 100);
                moriginal_id.Value = t_originalid.Text;
                MySqlParameter mtoken = new MySqlParameter("@token", MySqlDbType.VarChar, 100);
                mtoken.Value = token;
                MySqlParameter maeskey = new MySqlParameter("@aeskey", MySqlDbType.VarChar, 100);
                maeskey.Value = aeskey;
                MySqlParameter mtitle = new MySqlParameter("@title", MySqlDbType.VarChar, 50);
                mtitle.Value = t_title.Text;
                MySqlParameter menable = new MySqlParameter("@enable", MySqlDbType.Int32);
                menable.Value = c_enable.Checked ? 1 : 0;
                MySqlParameter mmp_types = new MySqlParameter("@mp_types", MySqlDbType.Int32);
                mmp_types.Value = int.Parse(d_mptypes.SelectedValue);
                MySqlParameter mstatus = new MySqlParameter("@status", MySqlDbType.Int32);
                mstatus.Value = int.Parse(d_status.SelectedValue);

                MySqlParameter mid = new MySqlParameter("@id", MySqlDbType.Int32);
                mid.Value = int.Parse(id);

                int result = DbHelperMySQL.ExecuteSql(sqlstr, mtags, mdesc, mappid, mappsecret, moriginal_id, mtoken, maeskey, mtitle, menable, mmp_types, mstatus, mid);
                if (result == 0)
                {
                    Jscript.Alert2(Page, "更新公众号出错！");
                    return;
                }
            }
            p_list.Visible = true;
            pztable.Visible = false;
            trid.Visible = true;
            p_addedit.Visible = false;
            LoadMPs(false);

        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            if (ex.Message.IndexOf("original_id") > 0)
            {
                Jscript.Alert2(Page, "此公众号已存在，不能重复添加");
            }
        }
        catch (System.IO.IOException ex)
        {
            Common.Scheduler.SaveExLog("xifen_list: " + ex.ToString());
            Jscript.Alert2(Page, "上传出错！");
        }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("xifen_list: " + ex.ToString());
            Jscript.Alert2(Page, "添加公众号出错！");
        }
    }
    protected void b_showpz_Click(object sender, EventArgs e)
    {
        int id =int.Parse(pz_id.Value);
        string name = (pz_name.Value);
        if (id <= 0)
        {
            Jscript.Alert2(Page, "参数错误");
    

        }
        thpz.InnerHtml = "广告商" + name + "的公众号配置";
        l_serverurl.Text = System.Configuration.ConfigurationManager.AppSettings["wxserver"] + "/alipay/WeiXin/ReceiveHandler.aspx?id=" + MchAgentIDEn.EncryptAgentID(id);
        l_token.Text = MchAgentIDEn.GetAgentToken(id);
        l_aeskey.Text = MchAgentIDEn.GetAgentAESKey(id);
        pztable.Visible = true;
        listtable.Visible = false;
        
     

    }
    protected void b_pzfanhui_Click(object sender, EventArgs e)
    {
       
        pztable.Visible = false;
        listtable.Visible = true;

    }
    protected void b_cancel_Click(object sender, EventArgs e)
    {
        p_list.Visible = true;
        pztable.Visible = false;
        trid.Visible = true;
        p_addedit.Visible = false;
    }

    private bool CheckAppID(string appid, string appsecret)
    {
        try
        {
            WebClient web = new WebClient();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, appsecret);
            string result = web.DownloadString(url);
            JsonData jd = JsonMapper.ToObject(result);
            if (JsonMapper.IsKey(jd, "access_token"))
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("xifen_list: " + ex.ToString());
        }
        return false;
    }

    protected void b_checkstatus_Command(object sender, CommandEventArgs e)
    {
        string xid = e.CommandName;
        string sqlstr = string.Format("select * from tbl_wx_xifenji where id={0};", xid);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            Jscript.Alert2(Page, "系统错误，请刷新后重试！");
            return;
        }
        string appid = dt.Rows[0]["appid"].ToString();
        string appsecret = dt.Rows[0]["appsecret"].ToString();
        string original_id = dt.Rows[0]["original_id"].ToString();
        int mp_types = int.Parse(dt.Rows[0]["mp_types"].ToString());
        dt.Dispose();
        ds.Dispose();
        if (mp_types == 1)
        {
            QRText = "请扫码进入订阅号后发送“517”到订阅号进行检测";
            QrUrl = string.Format("http://open.weixin.qq.com/qr/code/?username={0}", original_id);
        }
        else
        {
            QRText = "检测完，请手动关闭窗口";
            WxPayAPI.WX_config gwx = WxPayAPI.AccessToken_Manage.GetWxConfig(appid, appsecret);
            JsonData jsdata = gwx.CreateQrcode(86400, 88889);
            if (jsdata == null)
            {
                Jscript.Alert2(Page, "网络错误，请稍候再试");
                return;
            }
            if (JsonMapper.IsKey(jsdata, "ticket"))
            {
                string ticket = jsdata["ticket"].ToString();
                QrUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(ticket);
            }
        }
    }

}