using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// </summary>
public class Jscript
{
	public Jscript()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 弹出JavaScript小窗口
    /// </summary>
    /// <param name="js">窗口信息</param>
    static public void Alert(string message)
    {
        #region
        string js = @"<Script language='JavaScript'>
                    alert('" + message + "');</Script>";
        HttpContext.Current.Response.Write(js);
        #endregion
    }

    public static void Alert2(System.Web.UI.Page page, string message)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "", "alert('" + message + "');", true);
    }
    public static void script_write(System.Web.UI.Page page, string message)
    {
        string js = @"<Script language='JavaScript'>
                    " + message + "</Script>";
        page.ClientScript.RegisterStartupScript(page.GetType(), "", js);
    }
    public static void Alert(System.Web.UI.Page page, string message)
    {
        string js = @"<Script language='JavaScript'>
                    alert('" + message + "');</Script>";
        page.ClientScript.RegisterStartupScript(page.GetType(), "", js);
    }
    public static void redirect(System.Web.UI.Page page, string url)
    {
        string js = @"<Script language='JavaScript'>
                    location.href='"+url+"'</Script>";
        page.ClientScript.RegisterStartupScript(page.GetType(), "", js);
    }
    public static void windowopen(System.Web.UI.Page page, string message, string windowname, string url)
    {
        string js = @"<script>window.open('" + url + "','" + message + "','width='+screen.width+',height='+screen.height+',top=0,left=0,toolbar=yes,menubar=yes,scrollbars=yes,resizable=yes,location=yes,status=yes');</script>";
        page.ClientScript.RegisterStartupScript(page.GetType(), "", js);
    }

    static public void back(string message, string url)
    {

        string js = @"<Script language='JavaScript'>
                    alert('" + message + "');location.href='" + url + "';</Script>";
        HttpContext.Current.Response.Write(js);

    }
    static public void GoBack(System.Web.UI.Page page, string message,string url)
    {

        string js = @"<Script language='JavaScript'>
                    alert('" + message + "');location.href='"+url+"';</Script>";
        page.ClientScript.RegisterStartupScript(page.GetType(), "", js);

    }
    static public void LocationToHref(System.Web.UI.Page page, string url)
    {

        string js = @"<Script language='JavaScript'>location.href='" + url + "';</Script>";
        page.ClientScript.RegisterStartupScript(page.GetType(), "", js);

    }
    public static void back(System.Web.UI.Page page, string message)
    {
        string js = @"<Script language='JavaScript'>
                    alert('" + message + "');window.close();</Script>";
        page.ClientScript.RegisterStartupScript(page.GetType(), "", js);
    }

    public static void close(System.Web.UI.Page page)
    {
        string js = @"<Script language='JavaScript'>window.close();</Script>";
        page.ClientScript.RegisterStartupScript(page.GetType(), "", js);
    }

     static public void tohref(string url)
    {
        string js = @"<Script language='JavaScript'> location.href='" + url + "';</Script>";
        HttpContext.Current.Response.Write(js);
    }
    static public void windowopen(System.Web.UI.Page page, string url,string name)
    {
        string js = @"<Script language='JavaScript'> window.open('" + url + "','"+name+"');</Script>";
        page.ClientScript.RegisterStartupScript(page.GetType(), "", js);
    }

    static public string CutString(string str,int len)       //截取一段文字内容
    {
        try
        {
            if (str.Length > len)
                return str.Substring(0, len - 3) + "..";
            else
                return str;
        }
        catch (Exception)
        {
            return ("...");
        }
    }
    static public string Cut(string str, int len)       //截取一段文字内容
    {
        try
        {
            if (str.Length > len)
                return str.Substring(0, len - 3);
            else
                return str;
        }
        catch (Exception)
        {
            return ("...");
        }
    }
     static public bool checkform(string[] tb)
     {
        
         foreach (string s in tb)
         {
             if (s.Trim() == "")
             {
                 Alert("请把资料填写完整！");
                 return false;
             }
            
         }
         return true;

     }
    static public bool isNum(string str)
    {
        System.Text.RegularExpressions.Regex Rex=new System.Text.RegularExpressions.Regex(@"^\d+$");
        if (Rex.IsMatch(str))
            return true;
        return false;
    }
    public static string GetTimeSpan(DateTime starttime, DateTime endtime)  //时间差 天数
    {
        TimeSpan ts = endtime - starttime;
        return string.Format("{0}天", ts.Days);
    }

    static public string havehttp(string str)
    {
        if (str.IndexOf("http://") < 0)
            return "http://" + str;
        else
            return str;
    }


    static public string Replacetext(string str)
    {
        string aa=str;
        aa = aa.Replace("'","”");
        aa = aa.Replace(",", "，");
        aa = aa.Replace("\"", "“");
        aa = aa.Replace("select", "");
        aa = aa.Replace("delete", "");
        aa = aa.Replace("update", "");
        return aa;
    }
    /// <summary>
    /// 正则表达式验证
    /// </summary>
    /// <param name="text">要验证的字段</param>
    /// <param name="rex">输入的正则表达式</param>
    static public bool Reg(string text,string rex)
    {
        Regex reg = new Regex(rex);
        if (reg.IsMatch(text)) return true;
        return false;
    }
    /// <summary>
    /// 删除指定的cookie
    /// </summary>
    static public void delcookies(string text)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[text];
        cookie.Expires = DateTime.Today.AddDays(-100);
       HttpContext.Current.Response.Cookies.Add(cookie);
    }
    static public void PrintSEO(string seotitle,string seokey,string seodes)
    {

        string title=@"<title>"+seotitle+"</title>";
        string key = @"<meta name=" + "\"" + "keywords" + "\"" + " content=" + "\"" + "" + seokey + "" + "\"" + "/>";
        string des = @"<meta name=" + "\"" + "description" + "\"" + " content=" + "\"" + "" + seodes + "" + "\"" + "/>";
        HttpContext.Current.Response.Write(title+"\n");
        HttpContext.Current.Response.Write(key + "\n");
        HttpContext.Current.Response.Write(des + "\n");

    }

    static public string mychange(string str)
    {
        string aa=str;
        aa = aa.Replace("\'", "“");
        aa = aa.Replace("\"", "“");
        aa = aa.Replace("select", "");
        aa = aa.Replace("delete", "");
        aa = aa.Replace("update", "");
        aa = aa.Replace("<", "&lt;"); 
        aa = aa.Replace(">", "&gt;"); 
        aa = aa.Replace("   ", "&nbsp;"); 
        aa = aa.Replace("\r\n", "<br>"); 
        return aa;
    }
    static public string change(string str)
    {
        string aa=str;
        aa = aa.Replace("&lt;", "<"); 
        aa = aa.Replace("&gt;", ">"); 
        aa = aa.Replace("&nbsp;", "   "); 
        aa = aa.Replace("<br>", "\r\n");
        return aa;

    }
    /// <summary>
    /// 路径，链接地址，标题，宽，高，替换图片
    /// </summary>
    static public string Printadv(string url,string link,string title,string width,string height,string replace)
    {
        try
        {
            string aa = "";
            if (url.Substring(url.LastIndexOf(".") + 1) == "swf")
            {
                aa += string.Format("<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0' width='{0}' height='{1}'>", width, height);
                aa += string.Format("<param name='movie' value='{0}' />", url);
                aa += string.Format("<param name='quality' value='high' />");
                aa += string.Format("<embed src='{0}' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' type='application/x-shockwave-flash' width='{1}' height='{2}'></embed>", url.Replace("../../", ""), width, height);
                aa += string.Format("</object>");
            }
            else
            {
                aa += string.Format("<a href='{0}' target='_blank'><img src='{0}' alt='{1}' width='{3}' height={4}/></a>", url, title, link, width, height);

            }
            return aa;
        }
        catch { return "<img src='" + replace + "' width='" + width + "' height='" + height + "'/>"; }
     }
    static public string Printadv(string url, string link, string title, string width, string replace)
    {
        try
        {
            string aa = "";
            if (url.Substring(url.LastIndexOf(".") + 1) == "swf")
            {
                aa += string.Format("<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0' width='{0}'>", width);
                aa += string.Format("<param name='movie' value='{0}' />", url);
                aa += string.Format("<param name='quality' value='high' />");
                aa += string.Format("<embed src='{0}' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' type='application/x-shockwave-flash' width='{1}'></embed>", url.Replace("../../", ""), width);
                aa += string.Format("</object>");
            }
            else
            {
                aa += string.Format("<a href='{0}' target='_blank'><img src='{0}' alt='{1}' width='{3}'/></a>", url, title, link, width);

            }
            return aa;
        }
        catch { return "<img src='" + replace + "' width='" + width + "'/>"; }
    }
     /// <summary>
     /// 路径，链接地址，标题
     /// </summary>
     static public string PrintadvWithCSS(string url, string link, string title, string width, string height,string classname)
     {
         try
         {
             string aa = "";
             if (url.Substring(url.LastIndexOf(".") + 1) == "swf")
             {
                 aa += string.Format("<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0' width='{0}' height='{1}'>", width, height);
                 aa += string.Format("<param name='movie' value='{0}' />", url);
                 aa += string.Format("<param name='quality' value='high' />");
                 aa += string.Format("<embed src='{0}' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' type='application/x-shockwave-flash' width='{1}' height='{2}'></embed>", url, width, height);
                 aa += string.Format("</object>");
             }
             else
             {
                 aa += string.Format("<a href='{0}' target='_blank' class='{5}'><img src='{0}' alt='{1}' width='{3}' height={4}/></a>", url, title, link, width, height,classname);

             }
             return aa;
         }
         catch { return "<img src='images/zhaoshang.gif' width='" + width + "' height='" + height + "'/>"; }
     }
    /// <summary>
    /// 获取用户登陆IP
    /// </summary>
    /// <returns>返回用户IP</returns>
    public static string GetIp()
    {
        string user_IP;
        //在负载均衡下失效
        /*if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
        {
            user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else
        {
            user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }*/
        if (IsInNLB())
        {
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (user_IP.IndexOf(",") > 0)
                {
                    user_IP = user_IP.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                }
            }
            else
            {
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        else
        {
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else
            {
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        return user_IP;
    }

    /// <summary>
    /// 判断当前网络是否在阿里负载均衡里面
    /// </summary>
    /// <returns></returns>
    public static bool IsInNLB()
    {
        string localip = System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
        if (!string.IsNullOrEmpty(localip))
        {
            if (GetIPType(localip) == 1)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 判断指定IP是否为广域网IP
    /// </summary>
    /// <param name="ipAddress">要测试的IP地址</param>
    /// <returns>地址分类</returns>
    /// <remarks>0:无效IP,1:局域网IP,2:广域网IP,3:本机IP</remarks>
    public static int GetIPType(string ipAddress)
    {
        //ABC类外的IP地址为广域网IP
        //A类:10.0.0.0~10.255.255.255
        //B类:172.16.0.0~172.31.255.255
        //C类:192.168.0.0~192.168.255.255
        //返回值
        //0:无效IP
        //1:局域网IP
        //2:广域网IP
        //3:本机IP
        if (ipAddress == "127.0.0.1" || ipAddress == "::1")
        {
            return 3;
        }

        string[] ipAddressList = ipAddress.Split('.');
        int ipAddressTemp;

        //检查IP地址是否有效
        if (ipAddressList.Length != 4)
        {
            return 0;
        }
        if (!(int.TryParse(ipAddressList[0], out ipAddressTemp) && int.TryParse(ipAddressList[1], out ipAddressTemp)
            && int.TryParse(ipAddressList[2], out ipAddressTemp) && int.TryParse(ipAddressList[3], out ipAddressTemp)))
        {
            return 0;
        }
        if (!(int.Parse(ipAddressList[0]) >= 0 && int.Parse(ipAddressList[0]) <= 255
                && int.Parse(ipAddressList[1]) >= 0 && int.Parse(ipAddressList[1]) <= 255
                && int.Parse(ipAddressList[2]) >= 0 && int.Parse(ipAddressList[2]) <= 255
                && int.Parse(ipAddressList[3]) >= 0 && int.Parse(ipAddressList[3]) <= 255))
        {
            return 0;
        }

        //局域网IP
        if (int.Parse(ipAddressList[0]) == 10
                || (int.Parse(ipAddressList[0]) == 172 && int.Parse(ipAddressList[1]) >= 16 && int.Parse(ipAddressList[1]) <= 31)
                || (int.Parse(ipAddressList[0]) == 192 && int.Parse(ipAddressList[1]) == 168))
        {
            return 1;
        }

        return 2;
    }

    /// <summary>
    /// 代替父窗口 
    /// </summary>
    /// <returns>返回用户IP</returns>
    static public void RepalceLocation(string messege,string url)
    {
        string js = @"<Script language='JavaScript'>alert('"+messege+"');parent.location.href='" + url + "';</Script>";
        HttpContext.Current.Response.Write(js);
    }
    static public void RepalceLocation(string url)
    {
        string js = @"<Script language='JavaScript'>parent.location.href='" + url + "';</Script>";
        HttpContext.Current.Response.Write(js);
    }
    public static string ClearImg(string html)
    {
      
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        html = regex.Replace(html, ""); 
        return html;
    }

    /// <summary>
    /// 加密
    /// </summary>
    static public string AddCode(string ss)
    {
        return Tools.Encrypt(ss.Trim(), Tools.myKey);
    }
    /// <summary>
    /// 解密
    /// </summary>
    static public string DddCode(string ss)
    {
        return Tools.Decrypt(ss.Trim(), Tools.myKey);
    }
    static public string OverString(object obj)
    {

        try
        {
            return obj.ToString();
        }
        catch
        {
            return "";
        }
    }

    static public void OpenOpener(string editname, string str)
    {
        #region
        string js = @"<script>window.opener.document.getElementById('" + editname + "').value='" + str + "'; window.close();</script>";
        HttpContext.Current.Response.Write(js);
        #endregion
    }
    static public void OpenOpener(string editname, string str, string editname0,string str0)
    {
        #region
        string js = @"<script>window.opener.document.getElementById('" + editname + "').value='" + str + "'; ";
        js += @"window.opener.document.getElementById('" + editname0 + "').value='" + str0 + "'; window.close();</script>";
        HttpContext.Current.Response.Write(js);
        #endregion
    }
    static public void RefreshParentWindows()
    {
        #region
        string js = @"<script>window.opener.location.reload();window.close();</script>";
        HttpContext.Current.Response.Write(js);
        #endregion
    }

    public static void setMeta(string strTitle, string key, string des)
    {
        HtmlMeta keywords = new HtmlMeta(), description = new HtmlMeta();
        keywords.Name = "keywords";
        keywords.Content = key;

        description.Name = "description";
        description.Content = des;

        HtmlTitle title = new HtmlTitle();
        title.Text = strTitle;

        System.Web.UI.Page page = (System.Web.UI.Page)System.Web.HttpContext.Current.Handler;
        page.Header.Controls.Add(keywords);
        page.Header.Controls.Add(description);
        page.Header.Controls.Add(title);
    }
   
}
