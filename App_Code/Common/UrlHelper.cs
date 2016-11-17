using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// UrlHelper 的摘要说明
/// </summary>
public class UrlHelper
{
    /// <summary>
    /// 返回相对地址（支持虚拟目录）
    /// </summary>
    /// <param name="pathpage"></param>
    /// <returns></returns>
    public static string GetRelativelyPath(string pathpage)
    {
        if (pathpage.StartsWith("/"))
            pathpage = pathpage.Substring(1);
        string path = HttpContext.Current.Request.ApplicationPath; //虚拟目录
        if (!path.EndsWith("/"))
            path += "/";
        string repath = path + pathpage;
        return repath;
    }

    /// <summary>
    /// 取得负载均衡下对应目录的地址（支持虚拟目录）
    /// </summary>
    /// <param name="pathpage"></param>
    /// <returns></returns>
    public static string GetNLBPathUrl(string pathpage)
    {
        if (pathpage.StartsWith("/"))
            pathpage = pathpage.Substring(1);
        string path = HttpContext.Current.Request.ApplicationPath; //虚拟目录
        if (!path.EndsWith("/"))
            path += "/";
        string reurl = ConfigurationManager.AppSettings["hosts"].ToString() + path + pathpage;

        return reurl;
    }

    /// <summary>
    /// 根据域名前缀取得负载均衡下对应目录的地址（支持虚拟目录）
    /// </summary>
    /// <param name="host">地址前缀</param>
    /// <param name="pathpage">目录位置</param>
    /// <returns></returns>
    public static string GetNLBPathUrl(string host, string pathpage)
    {
        if (pathpage.StartsWith("/"))
            pathpage = pathpage.Substring(1);
        string path = HttpContext.Current.Request.ApplicationPath.Substring(1); //虚拟目录
        if (!path.EndsWith("/"))
            path += "/";
        string reurl = host + path + pathpage;

        return reurl;
    }


    /// <summary>
    /// 取得负载均衡下的相对地址
    /// </summary>
    /// <param name="page"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string GetRelativelyUrl(string page, params string[] args)
    {
        string reurl = GetNLBAbsolutePath();

        int ln = reurl.LastIndexOf("/");
        if (ln > 0)
            reurl = reurl.Remove(ln + 1);

        reurl += page;

        if (args.Length > 0)
            reurl += "?";
        foreach (string s in args)
            reurl += s + "&";
        if (reurl.EndsWith("&"))
            reurl = reurl.Remove(reurl.Length - 1);
        return reurl;
    }


    /// <summary>
    /// 取得负载均衡下当前的地址，不带参数
    /// </summary>
    /// <returns></returns>
    public static string GetNLBAbsolutePath()
    {
        string path = HttpContext.Current.Request.Path;
        //if (path.StartsWith("/"))
        //    path = path.Substring(1);
        string reurl = ConfigurationManager.AppSettings["hosts"].ToString() + path;
        return reurl;
    }

    /// <summary>
    /// 取得负载均衡下当前的地址，带参数
    /// </summary>
    /// <returns></returns>
    public static string GetNLBAbsoluteUri()
    {
        string url = HttpContext.Current.Request.RawUrl;
        //if (url.StartsWith("/"))
        //    url = url.Substring(1);
        string reurl = ConfigurationManager.AppSettings["hosts"].ToString() + url;
        return reurl;
    }
}