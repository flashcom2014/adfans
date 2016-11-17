using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.Text;
using MySql.Data.MySqlClient;

public partial class admin_ad_admin_share_code_manage : AdminPageBasead
{
    public string minamount = "1.1";
    public string regurl = "";

    protected string baseurl = UrlHelper.GetNLBPathUrl("/admin_ad/register.aspx?code=");
    private string codelist = "1e3ur486h7pasdw905ty2ioqjxnkcvblzmgf";

    protected void Page_Load(object sender, EventArgs e)
    {
        p_show.Visible = false;

        LoadDatas(true);
        if (!IsPostBack)
        {
            t_subamount.Text = ((float)Settings.GetSettings().ad_charge / 100f).ToString("F2");
            t_scale.Text = "80";
        }
        minamount = ((float)Settings.GetSettings().ad_cost / 100f).ToString();
    }

    private void LoadDatas(bool iscache)
    {
        if (iscache && ViewState["dt"] != null)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            gv.DataSource = dt;
            gv.DataBind();
        }
        else
        {
            string sqlstr = "select * from tbl_ad_share_code as c left join tbl_ad_admin as a on c.ad_id=0-a.admin_id where c.ad_id<0;";
            DataTable dt = DbHelperMySQL.Query(sqlstr).Tables[0];
            gv.DataSource = dt;
            gv.DataBind();
            ViewState["dt"] = dt;
        }
        if (gv.HeaderRow != null)
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected string GetShareLink()
    {
        string code = Eval("share_code").ToString();
        string url = baseurl + code;
        return url;
    }

    protected void b_makecode_Click(object sender, EventArgs e)
    {
        try
        {
            float amount = (float)Settings.GetSettings().ad_charge / 100f;
            if (!float.TryParse(t_subamount.Text, out amount))
            {
                Jscript.Alert2(Page, "参数错误!");
                return;
            }
            amount = amount * 100f;
            float scale = 0;
            if (!float.TryParse(t_scale.Text, out scale))
            {
                Jscript.Alert2(Page, "参数错误");
                return;
            }

            Random ra = new Random(Guid.NewGuid().GetHashCode());
            StringBuilder sb = new StringBuilder();
            string sharecode = "";
            int theid = 0;
            float thescale = 0f;
            float theamount = 0f;
            bool isrepeat = false;
            Dictionary<string, byte> list = new Dictionary<string, byte>();
            string sqlstr = "select * from tbl_ad_share_code";
            DataSet ds = DbHelperMySQL.Query(sqlstr);
            DataTable dt = ds.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                list[row["share_code"].ToString()] = 0;
            }

            dt.Dispose();
            ds.Dispose();

            int len = codelist.Length;
        ag:
            try
            {

                while (true)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        sb.Append(codelist[ra.Next(len)]);
                    }
                    sharecode = sb.ToString();
                    sb.Clear();
                    if (!list.ContainsKey(sharecode))
                        break;

                }
                sqlstr = string.Format("insert into tbl_ad_share_code(share_code,ad_id,scale,amount) values('{0}',{1},{2},{3});", sharecode, 0 - Admin_id, scale, amount);
                int result = DbHelperMySQL.ExecuteSql(sqlstr);
                if (result > 0)
                {
                    l_code.Text = sharecode;
                    l_makescale.Text = t_scale.Text + "%";
                    l_makeamount.Text = (amount / 100f).ToString("F2") + "元/个";
                    regurl = UrlHelper.GetNLBPathUrl("/admin_ad/register.aspx?code=") + sharecode;
                    p_show.Visible = true;
                    LoadDatas(false);
                }
                else
                {
                    Jscript.Alert2(Page, "生成推广码错误，请稍候再试！");
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Message.IndexOf("share_code") > 0)
                {
                    goto ag;
                }
            }
            list.Clear();
            list = null;
        }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("make_share_coe: " + ex.ToString());
            Jscript.Alert2(Page, "系统错误");
        }
    }
}