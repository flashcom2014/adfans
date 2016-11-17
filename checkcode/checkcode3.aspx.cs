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
using System.Text;
using System.IO;
using System.Drawing.Imaging;

public partial class checkcode3 : System.Web.UI.Page
{
    int j = 0;
    private static object o = new object();
    public string charSet = "2,3,4,5,6,8,9,A,B,C,D,E,F,G,H,J,K,M,N,P,R,S,U,W,X,Y";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SaveEventLog((j++).ToString());
            string validateCode = CreateRandomCode(4);
            //SaveEventLog((j++).ToString());
            Session["ValidateCode"] = validateCode;
            //SaveEventLog((j++).ToString());
            CreateImage(validateCode);
            //SaveEventLog((j++).ToString());
        }
    }
    /// <summary>    
    /// 生成验证码          
    /// <param name="n">位数</param>   
    /// <returns>验证码字符串</returns>   
    private string CreateRandomCode(int n)
    {
        string[] CharArray = charSet.Split(',');
        string randomCode = "";
        int temp = -1;
        Random rand = new Random();
        for (int i = 0; i < n; i++)
        {
            if (temp != -1)
            {
                rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            } int t = rand.Next(CharArray.Length - 1);
            if (temp == t)
            {
                return CreateRandomCode(n);
            } temp = t;
            randomCode += CharArray[t];
        }
        return randomCode;
    }
    private void CreateImage(string checkCode)
    {
        //SaveEventLog((j++).ToString());
        int iwidth = (int)(checkCode.Length * 13);
        //SaveEventLog((j++).ToString());
        System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 23);
       // SaveEventLog((j++).ToString());
        Graphics g = Graphics.FromImage(image);
        //SaveEventLog((j++).ToString());
        Font f = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Bold));        // 前景色        SaveEventLog((j++).ToString());
        Brush b = new System.Drawing.SolidBrush(Color.Black);        // 背景色 
        //SaveEventLog((j++).ToString());
        g.Clear(Color.White);        // 填充文字    
        //SaveEventLog((j++).ToString());
        g.DrawString(checkCode, f, b, 0, 1);        // 随机线条 
        //SaveEventLog((j++).ToString());
        Pen linePen = new Pen(Color.Gray, 0);
        //SaveEventLog((j++).ToString());
        Random rand = new Random();
        //SaveEventLog((j++).ToString());
        for (int i = 0; i < 5; i++)
        {
            int x1 = rand.Next(image.Width);
            int y1 = rand.Next(image.Height);
            int x2 = rand.Next(image.Width);
            int y2 = rand.Next(image.Height);
            g.DrawLine(linePen, x1, y1, x2, y2);
        }        // 随机点  
        //SaveEventLog((j++).ToString());
        for (int i = 0; i < 30; i++)
        {
            int x = rand.Next(image.Width);
            int y = rand.Next(image.Height);
            image.SetPixel(x, y, Color.Gray);
        }        // 边框   
        //SaveEventLog((j++).ToString());
        g.DrawRectangle(new Pen(Color.Gray), 0, 0, image.Width - 1, image.Height - 1);        // 输出图片  
        //SaveEventLog((j++).ToString());
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //SaveEventLog((j++).ToString());
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //SaveEventLog((j++).ToString());
        Response.ClearContent();
        //SaveEventLog((j++).ToString());
        Response.ContentType = "image/Jpeg";
        Response.BinaryWrite(ms.ToArray());
        g.Dispose();
        image.Dispose();
    }

    /// <summary>
    /// 保存出错日志
    /// </summary>
    /// <param name="pLogText"></param>
    public static void SaveEventLog(string pLogText)
    {
        lock (o)
        {
            FileStream pLogFile = null;
            StreamWriter pLogWriter = null;
            string pPath = AppDomain.CurrentDomain.BaseDirectory + "/checkcode/";
            if (!Directory.Exists(pPath))
                Directory.CreateDirectory(pPath);

            pLogFile = new FileStream(pPath + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
            pLogWriter = new StreamWriter(pLogFile, Encoding.GetEncoding("UTF-8"));
            pLogWriter.WriteLine(DateTime.Now.ToString() + ":" + pLogText + "\r\n");
            pLogWriter.Close();
            pLogFile.Close();
        }
    }
}
