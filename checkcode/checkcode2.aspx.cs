using System;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class checkcode_checkcode2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
            string checkCode = CreateRandomCode(4);
            Session["CheckCodes"] = checkCode;
            CreateImage(checkCode);

    }
    private string CreateRandomCode(int codeCount)
    {
        Random rand = new Random((int)DateTime.Now.Ticks);
        int abc = rand.Next(1000, 9999);
        string str = abc.ToString();
        return str;
    }
    private void CreateImage(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 12.5);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 18);
            Graphics g = Graphics.FromImage(image);
            Font f = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            Brush b = new System.Drawing.SolidBrush(Color.Green);
            //g.FillRectangle(new System.Drawing.SolidBrush(Color.Blue),0,0,image.Width, image.Height);
            g.Clear(Color.WhiteSmoke);
            g.DrawString(checkCode, f, b, 3, 3);
             Pen blackPen = new Pen(Color.Black, 0);
            Random rand = new Random();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms,System.Drawing.Imaging.ImageFormat.Jpeg);
            Response.ClearContent();
            Response.ContentType = "image/Jpeg";
            Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }


}
