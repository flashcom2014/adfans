using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using LitJson;
using WxPayAPI;

public partial class admin_ad_mp_mp_menu : AdminPageBasead
{
    public string scripts = "";
    public string mptitle = "";

    private string appid = "";
    private string appsecret = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request.QueryString["action"];

        if (string.IsNullOrEmpty(action))
        {
            if (!IsPostBack)
            {
                string sqlstr = string.Format("select * from tbl_wx_xifenji where ad_id={0};", ad_id);
                DataSet ds = DbHelperMySQL.Query(sqlstr);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    scripts = "$('#mpmenu').hide();\r\n";
                    scripts += "$('#no_mp').show();\r\n";
                    scripts += "$('#b_save').hide();\r\n";
                }
                else
                {

                    Session["appid"] = appid = dt.Rows[0]["appid"].ToString();
                    Session["appsecret"] = appsecret = dt.Rows[0]["appsecret"].ToString();
                    ViewState["mptitle"] = mptitle = dt.Rows[0]["tags"].ToString();
                    string name = "", values = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        name = string.Format("{0}[{1}]", row["tags"], row["appid"]);
                        values = row["id"].ToString();
                        d_list.Items.Add(new ListItem(name, values));
                    }
                    //
                    if (string.IsNullOrEmpty(action))
                        GetMenu();
                }
                dt.Dispose();
                ds.Dispose();
            }
            else
            {
                if (Session["appid"] != null && Session["appsecret"] != null)
                {
                    appid = Session["appid"].ToString();
                    appsecret = Session["appsecret"].ToString();
                    mptitle = ViewState["mptitle"].ToString();
                }
            }
        }

        if (!string.IsNullOrEmpty(action) && Request.Form.Count > 0)
        {
            SaveMenu();

        }
    }

    private void SaveMenu()
    {
        try
        {
            if (Session["appid"] == null && Session["appsecret"] == null)
            {
                Response.Write("[{\"result\":\"no\",\"info\":\"生成菜单错误\"}]");
                Response.End();
            }

            appid = Session["appid"].ToString();
            appsecret = Session["appsecret"].ToString();

            string mainname = "", mainurl = "", subname = "", suburl = "";
            mpbuttons mbs = new mpbuttons();
            for (int i = 0; i < 3; i++)
            {
                mainname = Request.Form["mainname" + i];
                mainurl = Request.Form["mainurl" + i];
                if (string.IsNullOrEmpty(mainname))
                    break;
                subname = Request.Form["subname" + i + "0"];
                if (string.IsNullOrEmpty(subname))
                {
                    viewbutton vb = new viewbutton(mainname, mainurl);
                    mbs.AddButtons(vb);
                }
                else
                {
                    subbutton sb = new subbutton(mainname);
                    mbs.AddButtons(sb);
                    for (int j = 0; j < 5; j++)
                    {
                        subname = Request.Form["subname" + i + j];
                        suburl = Request.Form["suburl" + i + j];
                        if (string.IsNullOrEmpty(subname))
                            break;
                        sb.AddButton(new viewbutton(subname, suburl));
                    }
                }
            }
            //
            string json = JsonMapper.ToJson(mbs);
            WX_config wx = AccessToken_Manage.GetWxConfig(appid, appsecret);
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=ACCESS_TOKEN";
            JsonData jsdata = wx.GetJson(url, json);
            if (JsonMapper.IsKey(jsdata, "errcode"))
            {
                if (jsdata["errcode"].ToString() == "0")
                {
                    Response.Write("[{\"result\":\"ok\",\"info\":\"成功生成菜单！\"}]");
                    Response.End();
                }
            }
            Response.Write("[{\"result\":\"no\",\"info\":\"保存菜单失败，请稍候再试！\"}]");
            Response.End();
        }
        catch (Exception ex)
        {

        }
        Response.Write("[{\"result\":\"no\",\"info\":\"生成菜单错误\"}]");
        Response.End();
    }

    private void GetMenu()
    {
        try
        {
            string name = "", url = "";
            int time = 0, num = 0;
            WX_config wx = AccessToken_Manage.GetWxConfig(appid, appsecret);
            string jsonurl = "https://api.weixin.qq.com/cgi-bin/get_current_selfmenu_info?access_token=ACCESS_TOKEN";
            JsonData jsdata = wx.GetJson(jsonurl);
            if (JsonMapper.IsKey(jsdata, "is_menu_open"))
            {
                if (jsdata["is_menu_open"].ToString() == "1")
                {
                    JsonData selfmenu_info = jsdata["selfmenu_info"];
                    if (selfmenu_info != null)
                    {
                        JsonData button = selfmenu_info["button"];
                        foreach (JsonData but in button)
                        {
                            if (JsonMapper.IsKey(but, "type"))
                            {
                                if (but["type"].ToString() == "view")
                                {
                                    name = but["name"].ToString();
                                    url = but["url"].ToString();
                                    scripts += string.Format("addmain('{0}','{1}');\r\n", name, url);
                                    time++;

                                }
                            }
                            else if (JsonMapper.IsKey(but, "sub_button"))
                            {
                                name = but["name"].ToString();
                                scripts += string.Format("addmain('{0}','');\r\n", name);
                                JsonData sub_button = but["sub_button"];
                                JsonData list = sub_button["list"];
                                foreach (JsonData subbtn in list)
                                {
                                    if (JsonMapper.IsKey(subbtn, "type"))
                                    {
                                        if (subbtn["type"].ToString() == "view")
                                        {
                                            name = subbtn["name"].ToString();
                                            url = subbtn["url"].ToString();
                                            num++;
                                            scripts += string.Format("addsubmenu({0},'{1}','{2}');\r\n", time, name, url);
                                        }
                                    }
                                }
                                time++;

                            }
                        }
                    }
                }
                scripts += "selectone();\r\n";
            }

        }
        catch (Exception ex)
        {
            Jscript.Alert2(Page, "菜单数据解释错误，请手动添加菜单！");
        }
    }

    protected void d_list_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = d_list.SelectedValue;
        string sqlstr = string.Format("select * from tbl_wx_xifenji where id={0};", id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            Session["appid"] = appid = dt.Rows[0]["appid"].ToString();
            Session["appsecret"] = appsecret = dt.Rows[0]["appsecret"].ToString();
            ViewState["mptitle"] = mptitle = dt.Rows[0]["tags"].ToString();
            GetMenu();
        }
        else
        {
            scripts += "alert('系统错误，请刷新！');";
        }
    }
}

public class mpbuttons
{
    public List<object> button = new List<object>();

    public void AddButtons(object obj)
    {
        button.Add(obj);
    }
}

public class viewbutton
{
    public string type = "view";
    public string name = "";
    public string url = "";

    public viewbutton(string name, string url)
    {
        this.name = name;
        this.url = url;
    }
}

public class subbutton
{
    public string name = "";
    public List<viewbutton> sub_button = new List<viewbutton>();

    public subbutton(string name)
    {
        this.name = name;
    }

    public void AddButton(viewbutton vb)
    {
        sub_button.Add(vb);
    }
}