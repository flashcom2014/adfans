using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;

public partial class admin_ad_expand_share_code_manage : AdminPageBasead
{
    protected string baseurl = UrlHelper.GetNLBPathUrl("/admin_ad/register.aspx?code=");

    public int index = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDatas();

        }
        else
        {
            if (ViewState["dt"] != null)
            {
                DataTable dt = (DataTable)ViewState["dt"];
                gv_code.DataSource = dt;
                gv_code.DataBind();
                if (gv_code.HeaderRow != null)
                    gv_code.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }

    protected void LoadDatas()
    {
        string sqlstr = string.Format("select * from tbl_ad_share_code where ad_id={0};", ad_id);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];
        gv_code.DataSource = dt;
        gv_code.DataBind();
        if (gv_code.HeaderRow != null)
            gv_code.HeaderRow.TableSection = TableRowSection.TableHeader;
        ViewState["dt"] = dt;
    }

    protected string GetShareLink()
    {
        string code = Eval("share_code").ToString();
        string url = baseurl + code;
        return url;
        /*index++;
        string html = "<span class=\"form-control\" id=\"link" + index + "\" style=\"width:100%; display:inline; \">" + url + "</span>";
        return html;8*/
    }

    protected string GetCopyData()
    {
        return "#link" + index;
    }

    protected void b_delcode_Click(object sender, EventArgs e)
    {
        string sqlstr = string.Format("delete from tbl_ad_share_code where share_code='{0}';", h_code.Value);
        int result = DbHelperMySQL.ExecuteSql(sqlstr);
        LoadDatas();
    }
}