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

public partial class admin_ad_mp_mp_list : AdminPageBasead
{
    public string QrUrl = "";
    public string QRText = "";
    public string Xid = "";
    public string Areas = "";

    string exlist = ".jpg.jpeg.gif.png.bmp";
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Request.QueryString["action"] != null && Request.QueryString["action"].ToString() == "testcode")
        {
            TestCode();
        }

        else
        {
            l_serverurl.Text = System.Configuration.ConfigurationManager.AppSettings["wxserver"] + "/alipay/WeiXin/ReceiveHandler.aspx?id=" + MchAgentIDEn.EncryptAgentID(ad_id);
            l_token.Text = MchAgentIDEn.GetAgentToken(ad_id);
            l_aeskey.Text = MchAgentIDEn.GetAgentAESKey(ad_id);

            LoadMPs(true);

            if(IsPostBack)
                GetAreaCheckboxs();
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
            string sqlstr = string.Format("select * from tbl_wx_xifenji where ad_id={0};", ad_id);
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            gv.DataSource = dt;
            gv.DataBind();
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            ViewState["dt"] = dt;
        }
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
    public string GetAmount(object obj)
    {
        float money = float.Parse(obj.ToString());
        money = money / 100f;
        return money.ToString("F2");
    }

    public string GetMpTypes()
    {
        int mp_types = int.Parse(Eval("mp_types").ToString());
        switch(mp_types)
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

    private bool CheckOriginalID(string original_id)
    {
        if (!original_id.StartsWith("gh_"))
            return false;
        if (original_id.Length != 15)
            return false;
        if (original_id != original_id.ToLower())
            return false;
        return true;
    }

    protected void b_addmp_Click(object sender, EventArgs e)
    {
        p_list.Visible = false;
        p_addedit.Visible = true;
        d_position2.Visible = false;
        ViewState.Remove("edit_xid");
        AddPosition();
        GetAreaCheckboxs();
        //
        t_tags.Text = t_desc.Text = t_appid.Text = t_appsecret.Text = t_originalid.Text = t_title.Text = "";
        c_enable.Checked = true;
        b_add.Visible = true;
        b_edit.Visible = false;
    }

    private void AddPosition()
    {
        d_position2.Visible = false;
        string fullid = MyAdMch.fullid;
        //
        string sqlstr = "";
        DataSet ds = null;
        DataTable dt = null;
        if (ViewState["edit_xid"] != null)
        {
            string xid = ViewState["edit_xid"].ToString();
            sqlstr = string.Format("select p.fullid from tbl_wx_xifenji_position as xi left join tbl_position as p on xi.area_id=p.id where xifenji_id={0} limit 1;", xid);
            ds = DbHelperMySQL.Query(sqlstr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
                fullid = dt.Rows[0]["fullid"].ToString();
        }

        string[] ss = fullid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        ListItem item = null;
        int index = 0;
        string id = "";
        d_position.Items.Clear();
        d_position.Items.Add(new ListItem("全国", "0"));
        index++;
        sqlstr = "select * from tbl_position where parent_id=0";
        ds = DbHelperMySQL.Query(sqlstr);
        dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            id = row["id"].ToString();
            item = new ListItem(row["name"].ToString(), id);
            d_position.Items.Add(item);
            if (ss.Length > 0)
            {
                if (id == ss[0])
                    d_position.SelectedIndex = index;
            }
            index++;
        }
        if (ss.Length > 0)
        {
            d_position2.Visible = true;
            d_position2.Items.Clear();
            //
            d_position2.Items.Add(new ListItem("全省", ss[0]));
            index = 1;

            sqlstr = string.Format("select * from tbl_position where parent_id={0};", ss[0]);
            ds = DbHelperMySQL.Query(sqlstr);
            dt = ds.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                id = row["id"].ToString();
                item = new ListItem(row["name"].ToString(), id);
                d_position2.Items.Add(item);
                if (ss.Length > 1 && id == ss[1])
                    d_position2.SelectedIndex = index;
                index++;
            }
        }

        dt.Dispose();
        ds.Dispose();
    }

    protected void b_add_Click(object sender, EventArgs e)
    {
        try
        {
            string appid = t_appid.Text;
            string appsecret = t_appsecret.Text;

            string ps = Request.Form["c_position"];
            if (d_position.SelectedIndex > 0 && d_position2.SelectedIndex > 0 && string.IsNullOrEmpty(ps))
            {
                Jscript.Alert2(Page, "请选择最少一个区，或选择全省");
                return;
            }

            if (!CheckAppID(appid, appsecret))
            {
                Jscript.Alert2(Page, "经检测公众号信息不正确，请重新填写");
                return;
            }

            if (!CheckOriginalID(t_originalid.Text))
            {
                Jscript.Alert2(Page, "原始ID不合法，请重新填写");
                return;
            }

            if (f_upload.HasFile)
            {
                string token = MchAgentIDEn.GetAgentToken(ad_id);
                string aeskey = MchAgentIDEn.GetAgentAESKey(ad_id);

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

                int get_amount = Settings.GetSettings().ad_get_amount;
                /*string sqlstr = "select * from tbl_settings where `keys`='ad_get_amount';";
                DataSet ds = DbHelperMySQL.Query(sqlstr);
                DataTable dt = ds.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    if (row["keys"].ToString() == "ad_get_amount")
                    {
                        get_amount = int.Parse(row["values"].ToString());
                    }
                }
                dt.Dispose();
                ds.Dispose();*/

                string sqlstr = "insert into tbl_wx_xifenji(ad_id,tags,desp,appid,appsecret,original_id,token,aeskey,title,imgurl,charge,get_amount,enable,mp_types) values(@ad_id,@tags,@desp,@appid,@appsecret,@original_id,@token,@aeskey,@title,@imgurl,@charge,@get_amount,@enable,@mp_types);";
                MySqlParameter madid = new MySqlParameter("@ad_id", MySqlDbType.Int32);
                madid.Value = ad_id;
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
                MySqlParameter mcharge = new MySqlParameter("@charge", MySqlDbType.Int32);
                mcharge.Value = MyAdMch.sub_amount;
                MySqlParameter mget_amount = new MySqlParameter("@get_amount", MySqlDbType.Int32);
                mget_amount.Value = get_amount;
                MySqlParameter menable = new MySqlParameter("@enable", MySqlDbType.Int32);
                menable.Value = c_enable.Checked ? 1 : 0;

                MySqlParameter mmp_types = new MySqlParameter("@mp_types", MySqlDbType.Int32);
                mmp_types.Value = int.Parse(d_mptypes.SelectedValue);

                int result = DbHelperMySQL.ExecuteSql(sqlstr, madid,mtags, mdesc, mappid, mappsecret, moriginal_id, mtoken, maeskey, mtitle, mimgurl, mcharge, mget_amount, menable, mmp_types);

                if (result > 0)
                {
                    int xifenji_id = 0;
                    sqlstr = string.Format("select id from tbl_wx_xifenji where ad_id={0} order by id desc limit 1", ad_id);
                    DataSet ds = DbHelperMySQL.Query_Main(sqlstr);
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        xifenji_id = int.Parse(dt.Rows[0]["id"].ToString());
                    }
                    dt.Dispose();
                    ds.Dispose();
                    if (xifenji_id > 0)
                    {
                        List<string> tl = new List<string>();
                        //
                        tl.Add(string.Format("insert into tbl_wx_xifenji_site(xifenji_id,site_id) values({0},0);", xifenji_id));
                        tl.Add(string.Format("insert into tbl_wx_xifenji_genre(xifenji_id,genre_id) values({0},0);", xifenji_id));
                        if (d_position.SelectedValue == "0")
                        {
                            tl.Add(string.Format("insert into tbl_wx_xifenji_position(xifenji_id,area_id) values({0},0);", xifenji_id));
                        }
                        else if (d_position2.SelectedIndex == 0)
                        {

                            tl.Add(string.Format("insert into tbl_wx_xifenji_position(xifenji_id,area_id) values({0},{1});", xifenji_id, d_position2.SelectedValue));
                        }
                        else
                        {
                            string[] cps = ps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string cid in cps)
                            {
                                tl.Add(string.Format("insert into tbl_wx_xifenji_position(xifenji_id,area_id) values({0},{1});", xifenji_id, cid));
                            }
                            
                        }
                        result = DbHelperMySQL.ExecuteSqlTran(tl);
                        tl.Clear();
                        tl = null;
                    }

                    p_list.Visible = true;
                    p_addedit.Visible = false;
                    LoadMPs(false);
                }
                else
                {
                    Jscript.Alert2(Page, "添加公众号失败，请稍候重试！");
                }
            }
            else
            {
                Jscript.Alert2(Page, "未获取到上传文件！");
            }
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

    protected void b_openedit_Click(object sender, EventArgs e)
    {
        string id = h_id.Value;
        string sqlstr = string.Format("select * from tbl_wx_xifenji where id={0} and ad_id={1};", id, ad_id);
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
        p_addedit.Visible = true;
        b_add.Visible = false;
        b_edit.Visible = true;

        ViewState["edit_xid"] = id;
        AddPosition();
        GetAreaCheckboxs();

        t_tags.Text = row["tags"].ToString();
        t_desc.Text = row["desp"].ToString();
        t_appid.Text = row["appid"].ToString();
        t_appsecret.Text = row["appsecret"].ToString();
        t_originalid.Text = row["original_id"].ToString();
        t_title.Text = row["title"].ToString();
        c_enable.Checked = row["enable"].ToString() == "1";
        d_mptypes.SelectedValue = row["mp_types"].ToString();

        dt.Dispose();
        ds.Dispose();
    }

    protected void b_del_Click(object sender, EventArgs e)
    {
        string id = h_id.Value;
        //string sqlstr = string.Format("delete from tbl_wx_xifenji where id={0}", id);
        List<string> list = new List<string>();
        list.Add(string.Format("delete from tbl_wx_xifenji where id={0}", id));
        list.Add(string.Format("delete from tbl_wx_xifenji_position where xifenji_id={0}", id));
        list.Add(string.Format("delete from tbl_wx_xifenji_genre where xifenji_id={0}", id));
        list.Add(string.Format("delete from tbl_wx_xifenji_site where xifenji_id={0}", id));
        int result = DbHelperMySQL.ExecuteSqlTran(list);
        if (result > 0)
        {
            p_list.Visible = true;
            p_addedit.Visible = false;
            LoadMPs(false);
        }
        else
        {
            Jscript.Alert2(Page, "删除公众号出错，请稍候重试");
        }
        list.Clear();
        list = null;
    }

    protected void b_edit_Click(object sender, EventArgs e)
    {
        try
        {
            string appid = t_appid.Text;
            string appsecret = t_appsecret.Text;

            string ps = Request.Form["c_position"];
            if (d_position.SelectedIndex > 0 && d_position2.SelectedIndex > 0 && string.IsNullOrEmpty(ps))
            {
                Jscript.Alert2(Page, "请选择最少一个区，或选择全省");
                return;
            }

            if (!CheckAppID(appid, appsecret))
            {
                Jscript.Alert2(Page, "经检测公众号信息不正确，请重新填写");
                return;
            }

            if (!CheckOriginalID(t_originalid.Text))
            {
                Jscript.Alert2(Page, "原始ID不合法，请重新填写");
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

                sqlstr = "update tbl_wx_xifenji set tags=@tags,desp=@desp,appid=@appid,appsecret=@appsecret,original_id=@original_id,token=@token,aeskey=@aeskey,title=@title,imgurl=@imgurl,enable=@enable,mp_types=@mp_types,status=1 where id=@id";
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
                MySqlParameter mid = new MySqlParameter("@id", MySqlDbType.Int32);
                mid.Value = int.Parse(id);


                int result = DbHelperMySQL.ExecuteSql(sqlstr, mtags, mdesc, mappid, mappsecret, moriginal_id, mtoken, maeskey, mtitle, mimgurl, menable, mmp_types, mid);
                if (result == 0)
                {
                    Jscript.Alert2(Page, "更新公众号出错！");
                    return;
                }
                else
                {
                    List<string> tl = new List<string>();
                    tl.Add(string.Format("delete from tbl_wx_xifenji_position where xifenji_id={0}", id));
                    if (d_position.SelectedValue == "0")
                    {
                        tl.Add(string.Format("insert into tbl_wx_xifenji_position(xifenji_id,area_id) values({0},0);", id));
                    }
                    else if (d_position2.SelectedIndex == 0)
                    {

                        tl.Add(string.Format("insert into tbl_wx_xifenji_position(xifenji_id,area_id) values({0},{1});", id, d_position2.SelectedValue));
                    }
                    else
                    {
                        string[] cps = ps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string cid in cps)
                        {
                            tl.Add(string.Format("insert into tbl_wx_xifenji_position(xifenji_id,area_id) values({0},{1});", id, cid));
                        }

                    }
                    result = DbHelperMySQL.ExecuteSqlTran(tl);
                    tl.Clear();
                    tl = null;
                }
                //

            }
            else
            {
                sqlstr = "update tbl_wx_xifenji set tags=@tags,desp=@desp,appid=@appid,appsecret=@appsecret,original_id=@original_id,token=@token,aeskey=@aeskey,title=@title,enable=@enable,mp_types=@mp_types,status=1 where id=@id";
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
                MySqlParameter mid = new MySqlParameter("@id", MySqlDbType.Int32);
                mid.Value = int.Parse(id);

                int result = DbHelperMySQL.ExecuteSql(sqlstr, mtags, mdesc, mappid, mappsecret, moriginal_id, mtoken, maeskey, mtitle, menable, mmp_types, mid);
                if (result == 0)
                {
                    Jscript.Alert2(Page, "更新公众号出错！");
                    return;
                }
                else
                {
                    List<string> tl = new List<string>();
                    tl.Add(string.Format("delete from tbl_wx_xifenji_position where xifenji_id={0}", id));
                    if (d_position.SelectedValue == "0")
                    {
                        tl.Add(string.Format("insert into tbl_wx_xifenji_position(xifenji_id,area_id) values({0},0);", id));
                    }
                    else if (d_position2.SelectedIndex == 0)
                    {

                        tl.Add(string.Format("insert into tbl_wx_xifenji_position(xifenji_id,area_id) values({0},{1});", id, d_position2.SelectedValue));
                    }
                    else
                    {
                        string[] cps = ps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string cid in cps)
                        {
                            tl.Add(string.Format("insert into tbl_wx_xifenji_position(xifenji_id,area_id) values({0},{1});", id, cid));
                        }

                    }
                    result = DbHelperMySQL.ExecuteSqlTran(tl);
                    tl.Clear();
                    tl = null;
                }
            }
            p_list.Visible = true;
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

    protected void b_cancel_Click(object sender, EventArgs e)
    {
        p_list.Visible = true;
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
            QRText = "扫码完成后会自动关闭";
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
        int id = int.Parse(xid);
        long ltime = DateTime.Now.Ticks;
        byte[] bid = BitConverter.GetBytes(id);
        byte[] btime = BitConverter.GetBytes(ltime);
        byte[] data = new byte[bid.Length + btime.Length];
        Array.Copy(btime, 0, data, 0, 4);
        Array.Copy(bid, 0, data, 4, 4);
        Array.Copy(btime, 4, data, 8, 4);
        string sid = Convert.ToBase64String(data);
        sid = HttpUtility.UrlEncode(sid);
        Xid = sid;
        sqlstr = string.Format("update tbl_wx_xifenji set status=2 where id={0};", xid);
        int result = DbHelperMySQL.ExecuteSql(sqlstr);
        LoadMPs(false);
    }

    protected void d_position_SelectedIndexChanged(object sender, EventArgs e)
    {
        d_position2.Visible = false;
        string id = d_position.SelectedValue;
        if (id == "0")
            return;
        string sqlstr = string.Format("select * from tbl_position where parent_id={0};", id);
        DataTable dt2 = DbHelperMySQL.Query(sqlstr).Tables[0];
        if (dt2.Rows.Count > 0)
        {
            d_position2.DataSource = dt2;
            d_position2.DataBind();
            d_position2.Items.Insert(0, new ListItem("全省", id.ToString()));
            d_position2.Visible = true;
        }
    }

    private void GetAreaCheckboxs()
    {
        if (!d_position2.Visible || !p_addedit.Visible || d_position.SelectedIndex == 0)
            return;

        string xid = "0";
        if (ViewState["edit_xid"] != null)
        {
            xid = ViewState["edit_xid"].ToString();
        }
        string sqlstr = "";
        DataSet ds = null;
        DataTable dt = null;
        string fullid = "";
        List<long> list = new List<long>();
        if (xid != "0")
        {
            sqlstr = string.Format("select xi.*,p.fullid from tbl_wx_xifenji_position as xi left join tbl_position as p on xi.area_id=p.id where xifenji_id={0};", xid);
            ds = DbHelperMySQL.Query(sqlstr);
            dt = ds.Tables[0];
            long area_id = 0;
            foreach (DataRow row in dt.Rows)
            {
                area_id = long.Parse(row["area_id"].ToString());
                if (!list.Contains(area_id))
                    list.Add(area_id);
                fullid = row["fullid"].ToString();
            }
            
        }

        string  d2 = "";
        if (fullid.Length > 0)
        {
            string[] ss = fullid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length > 1)
                d2 = ss[1];
        }

        string svalue = d_position2.SelectedValue;
        if (d_position2.SelectedIndex > 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sqlstr = string.Format("select * from tbl_position where parent_id={0};", svalue);
            ds = DbHelperMySQL.Query(sqlstr);
            dt = ds.Tables[0];
            string name = "";
            long id = 0;
            bool ischecked = true;
            foreach (DataRow row in dt.Rows)
            {
                name = row["name"].ToString();
                id = long.Parse(row["id"].ToString());
                ischecked = true;
                if (list.Count > 0)
                {
                    if (d_position2.SelectedValue== d2 && !list.Contains(id))
                    {
                        ischecked = false;
                    }
                }
                sb.Append(string.Format("<label><span class=\"mycheckbox\"><input type=\"checkbox\" name=\"c_position\" value=\"{0}\" {1} />{2}</span></label>", id, ischecked ? "checked=\"checked\"" : "", name));
            }
            Areas = sb.ToString();
            sb.Clear();
            sb = null;
        }
        if (dt != null)
            dt.Dispose();
        if (ds != null)
            ds.Dispose();
        
    }

    protected void d_position2_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GetAreaCheckboxs();
    }
}