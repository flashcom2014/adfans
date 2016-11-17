using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DBUtility;
public partial class admin_menu : AdminPageBasead
{
	public int mch_id = 0;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsAdmin)
        {
            uid.Text = ad_acc;
            if (!MyAdMch.IsAgent)
                p_agent.Visible = false;
        }
        else
        {
            admin_uid.Text = Admin_acc;
        }
        
	}

    protected string wtx_html;
    protected void loginout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Jscript.RepalceLocation("login.aspx");
        }
        catch { }
    }

	protected string GetMchComment()
	{
		string comment = "";
		

		return comment;
	}
}
