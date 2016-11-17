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

public partial class checkcode_checkcode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
            string checkCode = CreateRandomCode(4);
            Session["code"] = checkCode;
            CreateImage(checkCode);

    }
    private string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9" ;
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for(int i = 0; i < codeCount; i++)
            {
                if(temp != -1)
                {
                    rand = new Random(i*temp*((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(9);
                if(temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
    private void CreateImage(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 12.5);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 20);
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
