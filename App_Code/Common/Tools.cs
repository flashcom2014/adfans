using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Thumbs;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

/// <summary>
/// 加密类
/// </summary>
public class Tools
{
    /// <summary>
    /// 当前程序加密所使用的密钥

    /// </summary>
    public static readonly string myKey = "hbxiong1";
    #region 加密方法
    /// <summary>
    /// 加密方法
    /// </summary>
    /// <param name="pToEncrypt">需要加密字符串</param>
    /// <param name="sKey">密钥</param>
    /// <returns>加密后的字符串</returns>
    public static string Encrypt(string pToEncrypt, string sKey)
    {
        try
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中


            //原来使用的UTF8编码，我改成Unicode编码了，不行
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);

            //建立加密对象的密钥和偏移量


            //使得输入密码必须输入英文文本
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        catch (Exception ex)
        {
            Jscript.Alert("写入配置信息失败，详细信息：" + ex.Message.Replace("\r\n", "").Replace("'", ""));
        }

        return "";
    }
    #endregion

    #region 解密方法
    /// <summary>
    /// 解密方法
    /// </summary>
    /// <param name="pToDecrypt">需要解密的字符串</param>
    /// <param name="sKey">密匙</param>
    /// <returns>解密后的字符串</returns>
    public static string Decrypt(string pToDecrypt, string sKey)
    {
        try
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        catch (Exception ex)
        {
            Jscript.Alert("读取配置信息失败，详细信息：" + ex.Message.Replace("\r\n", "").Replace("'", ""));
        }
        return "";
    }
    #endregion

    public static string MD5(string Password)
    {
        string str = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");
        return str;
    }

    /// <summary>
    /// 限制字符长度
    /// </summary>
    /// <param name="sString">输入字符</param>
    /// <param name="nLeng">限制的字符长度</param>
    /// <returns></returns>
    public static string SubStr(string mText, int byteCount, string nText)
    {
        if (byteCount < 1) return mText;
        if (mText == null) return "";

        if (System.Text.Encoding.Default.GetByteCount(mText) <= byteCount)
        {
            return mText;
        }
        else
        {
            byte[] txtBytes = System.Text.Encoding.Default.GetBytes(mText);
            byte[] newBytes = new byte[byteCount - 4];

            for (int i = 0; i < byteCount - 4; i++)
            {
                newBytes[i] = txtBytes[i];
            }
            return System.Text.Encoding.Default.GetString(newBytes).Replace("?", "") + nText;
        }
    }

    /// <summary>
    /// 检查字符串(防SQL注入)，看有没有非法字符不允许输入，通过 | 分割 
    /// </summary>
    /// <param name="str">要检查的字符串</param>
    public static void CheckSQL(string sqlStr)
    {
        //ClientScriptManager cltScriptMgr = p.ClientScript;
        if (!string.IsNullOrEmpty(sqlStr))
        {
            sqlStr = sqlStr.ToLower();
            string jsStr = "<script language='JavaScript'>alert('你输入的信息含有不合法的字符，请重新输入!');history.back()</script>";
            string illegalStr = "or|and|+|\"|(|)|,|exec|insert|select|delete|update|count|*|%|chr|mid|master|truncate|char|declare|=|_|‘|~|--|";
            string[] newstr = illegalStr.Split('|');
            for (int i = 0; i < newstr.Length - 1; i++)
            {
                if (sqlStr.IndexOf(newstr[i]) != -1)
                {
                    //cltScriptMgr.RegisterStartupScript(p.GetType(), "非法字符信息", jsStr);
                    System.Web.HttpContext.Current.Response.Write(jsStr);
                    System.Web.HttpContext.Current.Response.End();
                }
            }
        }
    }

    #region 内容过滤
    /// 过滤html,js,css代码
    /// <summary>
    /// 过滤html,js,css代码
    /// </summary>
    /// <param name="html">参数传入</param>
    /// <returns></returns>
    public static string CheckStr(string html)
    {
        if (!string.IsNullOrEmpty(html))
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script. *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script. *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe. *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=java script. (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex5.Replace(html, ""); //过滤frameset
            html = regex6.Replace(html, ""); //过滤frameset
            html = regex7.Replace(html, ""); //过滤frameset
            html = regex8.Replace(html, ""); //过滤frameset
            html = regex9.Replace(html, "");
            html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            return html;
        }
        else
            return "";
    }
    #endregion
    #region /// 过滤p /p代码
    /// <summary>
    /// 过滤p /p代码
    /// </summary>
    /// <param name="html">参数传入</param>
    /// <returns></returns>
    public static string InputStr(string html)
    {
        html = html.Replace(@"\<img[^\>]+\>", "");
        html = html.Replace(@"<p>", "");
        html = html.Replace(@"</p>", "");
        return html;
    }
    #endregion

    /// <summary>
    /// 本地路径转换成URL相对路径
    /// </summary>
    /// <param name="imagesurl1">绝对路径</param>
    /// <returns></returns>
    public static string urlconvertor(string imagesurl1)
    {
        try
        {
            string tmpRootDir = HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = imagesurl1.Replace(tmpRootDir, ""); //转换成相对路径
            imagesurl2 = imagesurl2.Replace(@"\", @"/");
            return imagesurl2;
        }
        catch
        {
            return "";
        }
    }
    /// <summary>
    /// 相对路径转换成服务器本地物理路径
    /// </summary>
    /// <param name="imagesurl1">相对路径</param>
    /// <returns></returns>
    public static string urlconvertorlocal(string imagesurl1)
    {
        try
        {
            string tmpRootDir = HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = tmpRootDir + imagesurl1.Replace(@"/", @"\"); //转换成绝对路径
            return imagesurl2;
        }
        catch
        {
            return "";
        }
    }

    public static string GetClientIP()
    {
        string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (null == result || result == String.Empty)
        {
            result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        if (null == result || result == String.Empty)
        {
            result = HttpContext.Current.Request.UserHostAddress;
        }
        return result;
    }

    /// <summary>
    /// 缩略图
    /// </summary>
    /// <param name="inputpath"></param>
    /// <param name="outputpath"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="mode">HW,W,H,Cut,Stuff</param>
    /// <returns></returns>
    public static string Thumbs(string inputpath, string outputpath, int width, int height, string mode)
    {
        ThumbsJpg thumbs = new ThumbsJpg(width, height, mode);
        thumbs.AddThumbs(inputpath, outputpath);
        return thumbs.pic;
    }

    public static string CreateRandomCode(int codeCount)
    {
        string allChar = "0,1,2,3,4,5,6,7,8,9";
        string[] allCharArray = allChar.Split(',');
        string randomCode = "";
        int temp = -1;

        Random rand = new Random();
        for (int i = 0; i < codeCount; i++)
        {
            if (temp != -1)
            {
                rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            }
            int t = rand.Next(9);
            if (temp == t)
            {
                return CreateRandomCode(codeCount);
            }
            temp = t;
            randomCode += allCharArray[t];
        }
        return randomCode;
    }
    ///   <summary>
    ///   去除HTML标记
    ///   </summary>
    ///   <param   name="NoHTML">包括HTML的源码   </param>
    ///   <returns>已经去除后的文字</returns>   
    public static string NoHTML(string Htmlstring)
    {
        //删除脚本   
        Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
        //删除HTML   
        Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
        Htmlstring.Replace("<", "");
        Htmlstring.Replace(">", "");
        Htmlstring.Replace("\r\n", "");
        Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
        return Htmlstring;
    }
    public static string BuildRandomStr(int length)
    {
        Random rand = new Random();

        int num = rand.Next();

        string str = num.ToString();

        if (str.Length > length)
        {
            str = str.Substring(0, length);
        }
        else if (str.Length < length)
        {
            int n = length - str.Length;
            while (n > 0)
            {
                str.Insert(0, "0");
                n--;
            }
        }

        return str;
    }
    /// <summary>    
    /// Creating a Watermarked Photograph with GDI+ for .NET    
    /// </summary>    
    /// <param name="rSrcImgPath">原始图片的物理路径</param>    
    /// <param name="rMarkImgPath">水印图片的物理路径</param>    
    /// <param name="rMarkText">水印文字（不显示水印文字设为空串）</param>    
    /// <param name="rDstImgPath">输出合成后的图片的物理路径</param>    
    public void BuildWatermark(string rSrcImgPath, string rMarkImgPath, string rMarkText, string rDstImgPath)
    {
        //以下（代码）从一个指定文件创建了一个Image 对象，然后为它的 Width 和 Height定义变量。    
        //这些长度待会被用来建立一个以24 bits 每像素的格式作为颜色数据的Bitmap对象。    
        System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(rSrcImgPath);
        int phWidth = imgPhoto.Width;
        int phHeight = imgPhoto.Height;
        Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(72, 72);
        Graphics grPhoto = Graphics.FromImage(bmPhoto);
        //这个代码载入水印图片，水印图片已经被保存为一个BMP文件，以绿色(A=0,R=0,G=255,B=0)作为背景颜色。    
        //再一次，会为它的Width 和Height定义一个变量。    
        System.Drawing.Image imgWatermark = new Bitmap(rMarkImgPath);
        int wmWidth = imgWatermark.Width;
        int wmHeight = imgWatermark.Height;
        //这个代码以100%它的原始大小绘制imgPhoto 到Graphics 对象的（x=0,y=0）位置。    
        //以后所有的绘图都将发生在原来照片的顶部。    
        grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
        grPhoto.DrawImage(
             imgPhoto,
             new Rectangle(0, 0, phWidth, phHeight),
             0,
             0,
             phWidth,
             phHeight,
             GraphicsUnit.Pixel);
        //为了最大化版权信息的大小，我们将测试7种不同的字体大小来决定我们能为我们的照片宽度使用的可能的最大大小。    
        //为了有效地完成这个，我们将定义一个整型数组，接着遍历这些整型值测量不同大小的版权字符串。    
        //一旦我们决定了可能的最大大小，我们就退出循环，绘制文本    
        int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
        Font crFont = null;
        SizeF crSize = new SizeF();
        for (int i = 0; i < 7; i++)
        {
            crFont = new Font("arial", sizes[i],
                  FontStyle.Bold);
            crSize = grPhoto.MeasureString(rMarkText,
                  crFont);
            if ((ushort)crSize.Width < (ushort)phWidth)
                break;
        }
        //因为所有的照片都有各种各样的高度，所以就决定了从图象底部开始的5%的位置开始。    
        //使用rMarkText字符串的高度来决定绘制字符串合适的Y坐标轴。    
        //通过计算图像的中心来决定X轴，然后定义一个StringFormat 对象，设置StringAlignment 为Center。    
        int yPixlesFromBottom = (int)(phHeight * .05);
        float yPosFromBottom = ((phHeight -
             yPixlesFromBottom) - (crSize.Height / 2));
        float xCenterOfImg = (phWidth / 2);
        StringFormat StrFormat = new StringFormat();
        StrFormat.Alignment = StringAlignment.Center;
        //现在我们已经有了所有所需的位置坐标来使用60%黑色的一个Color(alpha值153)创建一个SolidBrush 。    
        //在偏离右边1像素，底部1像素的合适位置绘制版权字符串。    
        //这段偏离将用来创建阴影效果。使用Brush重复这样一个过程，在前一个绘制的文本顶部绘制同样的文本。    
        SolidBrush semiTransBrush2 =
             new SolidBrush(Color.FromArgb(153, 0, 0, 0));
        grPhoto.DrawString(rMarkText,
             crFont,
             semiTransBrush2,
             new PointF(xCenterOfImg + 1, yPosFromBottom + 1),
             StrFormat);
        SolidBrush semiTransBrush = new SolidBrush(
             Color.FromArgb(153, 255, 255, 255));
        grPhoto.DrawString(rMarkText,
             crFont,
             semiTransBrush,
             new PointF(xCenterOfImg, yPosFromBottom),
             StrFormat);
        //根据前面修改后的照片创建一个Bitmap。把这个Bitmap载入到一个新的Graphic对象。    
        Bitmap bmWatermark = new Bitmap(bmPhoto);
        bmWatermark.SetResolution(
             imgPhoto.HorizontalResolution,
             imgPhoto.VerticalResolution);
        Graphics grWatermark =
             Graphics.FromImage(bmWatermark);
        //通过定义一个ImageAttributes 对象并设置它的两个属性，我们就是实现了两个颜色的处理，以达到半透明的水印效果。    
        //处理水印图象的第一步是把背景图案变为透明的(Alpha=0, R=0, G=0, B=0)。我们使用一个Colormap 和定义一个RemapTable来做这个。    
        //就像前面展示的，我的水印被定义为100%绿色背景，我们将搜到这个颜色，然后取代为透明。    
        ImageAttributes imageAttributes =
             new ImageAttributes();
        ColorMap colorMap = new ColorMap();
        colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
        colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
        ColorMap[] remapTable = { colorMap };
        //第二个颜色处理用来改变水印的不透明性。    
        //通过应用包含提供了坐标的RGBA空间的5x5矩阵来做这个。    
        //通过设定第三行、第三列为0.3f我们就达到了一个不透明的水平。结果是水印会轻微地显示在图象底下一些。    
        imageAttributes.SetRemapTable(remapTable,
             ColorAdjustType.Bitmap);
        float[][] colorMatrixElements = {     
                                             new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},    
                                             new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},    
                                             new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},    
                                             new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},    
                                             new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}    
                                        };
        ColorMatrix wmColorMatrix = new
             ColorMatrix(colorMatrixElements);
        imageAttributes.SetColorMatrix(wmColorMatrix,
             ColorMatrixFlag.Default,
             ColorAdjustType.Bitmap);
        //随着两个颜色处理加入到imageAttributes 对象，我们现在就能在照片右手边上绘制水印了。    
        //我们会偏离10像素到底部，10像素到左边。    
        int markWidth;
        int markHeight;
        //mark比原来的图宽    
        if (phWidth <= wmWidth)
        {
            markWidth = phWidth - 10;
            markHeight = (markWidth * wmHeight) / wmWidth;
        }
        else if (phHeight <= wmHeight)
        {
            markHeight = phHeight - 10;
            markWidth = (markHeight * wmWidth) / wmHeight;
        }
        else
        {
            markWidth = wmWidth;
            markHeight = wmHeight;
        }
        int xPosOfWm = ((phWidth - markWidth) - 10);
        int yPosOfWm = 10;
        grWatermark.DrawImage(imgWatermark,
             new Rectangle(xPosOfWm, yPosOfWm, markWidth,
             markHeight),
             0,
             0,
             wmWidth,
             wmHeight,
             GraphicsUnit.Pixel,
             imageAttributes);
        //最后的步骤将是使用新的Bitmap取代原来的Image。 销毁两个Graphic对象，然后把Image 保存到文件系统。    
        imgPhoto = bmWatermark;
        grPhoto.Dispose();
        grWatermark.Dispose();
        imgPhoto.Save(rDstImgPath, ImageFormat.Jpeg);
        imgPhoto.Dispose();
        imgWatermark.Dispose();
    }
    ///   <summary>
    ///   判断是否为微信客户端打开
    ///   </summary>
    ///   <returns>true false</returns>   
    public static bool IsWxApp()
    { 
        string userAgent =HttpContext.Current.Request.UserAgent;
        if (!string.IsNullOrEmpty(userAgent) && userAgent.ToLower().Contains("micromessenger"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static string Merge(string str0, string str1)
    {
        return str0 + (string.IsNullOrEmpty(str1) ? "" : " - " + str1);
    }

    #region 序列化和反序列化

    /// <summary>
    /// 从二进制数组反序列化得到对象
    /// </summary>
    /// <param name="buf">字节数组</param>
    /// <returns>得到的对象</returns>
    public static object DeserializeBinary(byte[] buf)
    {
        try
        {
            System.IO.MemoryStream memStream = new MemoryStream(buf);
            memStream.Position = 0;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter deserializer =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            object newobj = deserializer.Deserialize(memStream);
            memStream.Close();
            return newobj;
        }
        catch(Exception ex)
        {
            Common.Scheduler.SaveExLog("DeserializeBinary: " + ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// 序列化为二进制字节数组
    /// </summary>
    /// <param name="request">要序列化的对象</param>
    /// <returns>字节数组</returns>
    public static byte[] SerializeBinary(object request)
    {
        try
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            serializer.Serialize(memStream, request);
            return memStream.GetBuffer();
        }
        catch(Exception ex)
        {
            Common.Scheduler.SaveExLog("SerializeBinary: " + ex.ToString());
            return null;
        }
    }

    #endregion
}
