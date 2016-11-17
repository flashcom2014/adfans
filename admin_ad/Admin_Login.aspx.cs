using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class admin_ad_Admin_Login : System.Web.UI.Page
{
    public string EncryptCode = "";
    public string QrUrl = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        
        //

        string enid = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(enid))
        {
            EnCode(enid);
            return;
        }

        if (Session["Admin_id"] == null || Session["Admin_acc"] == null)
        {
            Response.Redirect("login.aspx");
            return;
        }

        int admin_id = int.Parse(Session["Admin_id"].ToString());
        byte[] bid = BitConverter.GetBytes(admin_id);
        byte[] btime = BitConverter.GetBytes(DateTime.Now.Ticks);
        byte[] data = new byte[bid.Length + btime.Length];
        for (int i = 0; i < data.Length; i++)
        {
            if (i < 4)
            {
                data[i] = btime[i];
            }
            else if (i < 8)
            {
                data[i] = bid[i - 4];
            }
            else
            {
                data[i] = btime[i - 4];
            }
        }
        string code = Convert.ToBase64String(data);
        code = HttpUtility.UrlEncode(code);
        EncryptCode = code;
        QrUrl =UrlHelper.GetNLBPathUrl("admin_ad/Admin_Login_QR.aspx") + "?id=" + EncryptCode;
        //Session["qrurl"] = QrUrl;
    }

    private void EnCode(string code)
    {
        try
        {
            if (Session["Admin_id"] == null || Session["Admin_acc"] == null)
            {
                Session.Clear();
                Response.Write("[{\"result\":\"no\",\"info\":\"非法操作！\"}]");
                Response.End();
            }
            //code = HttpUtility.UrlDecode(code);
            if (string.IsNullOrEmpty(code))
            {
                Session.Clear();
                Response.Write("[{\"result\":\"no\",\"info\":\"参数不正确！\"}]");
                Response.End();
            }
            else
            {
                byte[] endata = Convert.FromBase64String(code);
                if (endata.Length < 8)
                {
                    Session.Clear();
                    Response.Write("[{\"result\":\"no\",\"info\":\"非法二维码！\"}]");
                    Response.End();
                }
                int admin_id = BitConverter.ToInt32(endata, 4);
                int sessionid = (int)Session["Admin_id"];
                if (admin_id != sessionid)
                {
                    Session.Clear();
                    Response.Write("[{\"result\":\"no\",\"info\":\"非法二维码！\"}]");
                    Response.End();
                }

                string sqlstr = string.Format("select * from tbl_ad_admin where admin_id={0};", admin_id);
                DataSet ds = DbHelperMySQL.Query(sqlstr);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    ds.Dispose();
                    Session.Clear();
                    Response.Write("[{\"result\":\"no\",\"info\":\"非法二维码！\"}]");
                    Response.End();
                }
                string admin_appid = dt.Rows[0]["appid"].ToString();
                string admin_openid = dt.Rows[0]["openid"].ToString();
                dt.Dispose();
                ds.Dispose();


                string rest = "[{\"result\":\"nodata\",\"info\":\"NO Data\"}]";
                string connst = DBUtility.DbHelperMySQL.connectionString;
                using (MySqlConnection conn = new MySqlConnection(connst))
                {
                    DateTime returntime = DateTime.Now.AddSeconds(10);  //10秒没数据直接返回
                    sqlstr = "select * from tbl_ad_admin_qr where encode='" + code + "';";
                    conn.Open();
                    MySqlCommand query = new MySqlCommand(sqlstr, conn);
                    MySqlDataReader reader = null;
                    bool haddata = false;
                    string openid = "", appid = "";
                    int isused = 0;
                    Dictionary<string, string> json = new Dictionary<string, string>();

                    while (DateTime.Now < returntime)
                    {
                        reader = query.ExecuteReader();
                        if (reader.Read())
                        {
                            //
                            appid = reader["appid"].ToString();
                            openid = reader["openid"].ToString();
                            isused = int.Parse(reader["isused"].ToString());

                            haddata = true;
                        }
                        reader.Close();

                        if (haddata)
                        {
                            if (isused != 0)
                            {
                                Session.Clear();
                                rest = "[{\"result\":\"no\",\"info\":\"此二维码已失效！\"}]";
                                break;
                            }
                            //
                            if (admin_appid != appid || admin_openid != openid)
                            {
                                Session.Clear();
                                rest = "[{\"result\":\"no\",\"info\":\"授权失败！\"}]";
                                break;
                            }
                            List<string> tl = new List<string>();
                            tl.Add(string.Format("update tbl_ad_admin_qr set isused=1 where encode='{0}';", code));
                            tl.Add(string.Format("update tbl_ad_admin set login_time=now() where admin_id={0};", admin_id));
                            int result = DbHelperMySQL.ExecuteSqlTran(tl);
                            tl.Clear();
                            tl = null;
                            if (result > 0)
                            {
                                Session["ad_id"] = 0;
                                Session["ad_acc"] = "Admin";
                                rest = "[{\"result\":\"ok\",\"info\":\"OK\"}]";
                            }
                            else
                            {
                                Session.Clear();
                                rest = "[{\"result\":\"no\",\"info\":\"系统错误！\"}]";
                            }
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
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("Admin_Login: " + ex.ToString());
            Response.Write("[{\"result\":\"no\",\"info\":\"参数不正确！\"}]");
            Response.End();
        }
    }
}