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

public partial class admin_Default : AdminPageBasead
{
    protected string menu_url0 = "";
    protected string menu_url1 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        menu_url0 = "menu.aspx";
        menu_url1 = "main.aspx";
  //      if (Request.Cookies["child_mid"] != null)
  //      {
  //          menu_url0 = "menu_.aspx";
  //          menu_url1 = "main.htm";
  //      }
  //      else if (Request.Cookies["mch_id"] != null)
  //      {
  //          menu_url0 = "menu.aspx";
		//	//menu_url1 = k("15000") ? "baobiao/user.aspx" : "main.htm";
		//	menu_url1 = "main.htm";
		//}
    }
   
}
