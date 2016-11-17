using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using DBUtility;
using System.Web;

public class AdminPageBasead : System.Web.UI.Page
{

    string raw_url = string.Empty;
    string old_url = string.Empty;
    protected override void OnInit(EventArgs e)
    {
        raw_url = Request.RawUrl;
        if (Session["ad_id"] == null || Session["ad_acc"] == null)
        {
            Jscript.RepalceLocation(UrlHelper.GetNLBPathUrl("/admin_ad/login.aspx"));
            //string js = @"<Script language='JavaScript'>parent.login();</Script>";
            //HttpContext.Current.Response.Write(js);
            Response.End();
        }
    }

    public void MyAdMch_Referer()
    {
        adm = null;
    }

    public string ad_acc
    {
        get
        {
            if (Session["ad_acc"] == null)
                return string.Empty;
            return Session["ad_acc"].ToString();
        }
    }

    public int ad_id
    {
        get
        {
            if (Session["ad_id"] == null)
                return 0;
            return (int)Session["ad_id"];
        }
    }

    private tbl_ad_mch adm = null;
    public tbl_ad_mch MyAdMch
    {
        get
        {
            if (ad_id == 0)
                return null;
            if (adm == null)
                adm = new tbl_ad_mch(ad_id);
            return adm;
        }
    }

    public bool IsAdmin
    {
        get
        {
            if (Session["ad_id"] == null || Session["ad_acc"] == null)
                return false;
            if (ad_id != 0 || Session["ad_acc"].ToString() != "Admin")
                return false;
            if (Session["Admin_acc"] == null || Session["Admin_id"] == null)
                return false;
            return true;
        }
    }

    public int Admin_id
    {
        get
        {
            if (Session["Admin_id"] == null)
                return 0;
            return (int)Session["Admin_id"];
        }
    }

    public string Admin_acc
    {
        get
        {
            if (Session["Admin_acc"] == null)
                return string.Empty;
            return Session["Admin_acc"].ToString();
        }
    }
}