using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Common;
using DBUtility;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
public partial class addadmin : AdminPageBasead
{
    public string EncryptCode = "";

    public string QrUrl = "";
    public string BindTips = "子管理员未绑定微信号 随时可绑定";
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {


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

            QrUrl = UrlHelper.GetNLBPathUrl("admin_ad/admin/admin_loginxg_QR.aspx") + "?id=" + EncryptCode + "&action=addamin";

        }


    }
}