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
public partial class person : AdminPageBasead
{
  
    protected string StyleDevicePageTitle = "";
    protected string agent_qr_code_url = "";
    public tbl_ad_mch admch = null;
   
   
    protected void Page_Load(object sender, EventArgs e)
    {







        if (Request.QueryString["action"] != "" && Request.QueryString["action"] == "area")
        {
            string type = Request["type"];

            string parentid = Request["parentid"];
            if (type != "")
            {
                GetAreaall(type, parentid);
            }
        }
        else if (Request.QueryString["action"] != "" && Request.QueryString["action"] == "update")
        {
            Update();
        }
        
          Init();
        
    }
    protected void Init()
    {
        admch = new tbl_ad_mch();
        if (ad_id!=0)
        admch.LoadDatanew(ad_id);
        tbmch_acc.Value = ad_acc;
        tbmobile.InnerText = admch.ad_mobile;
        tbmch_name.Value = admch.ad_name;
        tbtelephone.Value = admch.telephone;
        tbemail.Value = admch.email;
        tbaddress.Value = admch.address;
        tbcomment.Value = admch.comment_;
        tbtjcode.InnerText = admch.share_code;
        if (admch.isset_acc)
        {
            tbmch_acc.Attributes.Add("ReadOnly", "True");
        }
        else
        {
            tbmch_acc.Attributes.Remove("ReadOnly");
        }
        
      
    }

   
     protected void GetAreaall(string type,string id)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("[");
        string sql = "";
   
          if (type == "0")
            {
                sql = "select * from tbl_position where types=0 ";
                DataSet dt = DbHelperMySQL.Query(sql);
                foreach (DataRow dr in dt.Tables[0].Rows)
                  {

                      sb.Append("{");
                      sb.Append("\"id\":\"" + dr["id"].ToString() + "\",\"name\":\"" + dr["name"].ToString() + "\"");
                      sb.Append("},");
                   }
                if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                Response.Write(sb.ToString());
                Response.End();
           
            }
          else if (type == "1")
            {


                sql = "select * from tbl_position where types=1 and parent_id= "+id;
                DataSet dt = DbHelperMySQL.Query(sql);
                foreach (DataRow dr in dt.Tables[0].Rows)
                {

                    sb.Append("{");
                    sb.Append("\"id\":\"" + dr["id"].ToString() + "\",\"name\":\"" + dr["name"].ToString() + "\"");
                    sb.Append("},");
                }
                if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                Response.Write(sb.ToString());
                Response.End();
            }
            else if (type == "2")
            {


                sql = "select * from tbl_position where types=2 and parent_id= " + id;
                DataSet dt = DbHelperMySQL.Query(sql);
                foreach (DataRow dr in dt.Tables[0].Rows)
                {

                    sb.Append("{");
                    sb.Append("\"id\":\"" + dr["id"].ToString() + "\",\"name\":\"" + dr["name"].ToString() + "\"");
                    sb.Append("},");
                }
                if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                Response.Write(sb.ToString());
                Response.End();
            }
            else if (type == "3")
            {


                sql = "select * from tbl_position where types=3 and parent_id= " + id;
                DataSet dt = DbHelperMySQL.Query(sql);
                foreach (DataRow dr in dt.Tables[0].Rows)
                {

                    sb.Append("{");
                    sb.Append("\"id\":\"" + dr["id"].ToString() + "\",\"name\":\"" + dr["name"].ToString() + "\"");
                    sb.Append("},");
                }
                if (sb.Length>1)
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                Response.Write(sb.ToString());
                Response.End();
            }
      
        
       


    } 
    protected void Update()
    {

        string adacc = tbmch_acc.Value;
        string adname = tbmch_name.Value;
        string telephone = tbtelephone.Value;
        string email = tbemail.Value;
        long areaid = 0;
        if (areahidden.Value != "")
        {
            areaid = long.Parse(areahidden.Value);
        }
        string address = tbaddress.Value;
        string comment = tbcomment.Value;
        
        int issetacc =0;
       
        issetacc = (int.Parse(hiddisset.Value) == 1) ? 1 : 0;

     

        string sqlstr="";
        sqlstr = string.Format("select ad_id from tbl_ad_mch where ad_mobile='{0}' or ad_acc='{0}'", adacc);
        DataSet ds = DbHelperMySQL.Query(sqlstr);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count > 0 && int.Parse(dt.Rows[0]["ad_id"].ToString()) != ad_id)
        {
            dt.Dispose();
            ds.Dispose();
            Response.Write("[{\"err\":\"用户名已存在，请更换\"}]");
            Response.End();
            return;
        }
        if (issetacc == 1)
        {
           
            sqlstr = string.Format("update  tbl_ad_mch  set  ad_acc='{0}',ad_name='{1}',telephone='{2}',email='{3}',area_id='{4}',address='{5}',comment_='{6}',isset_acc={8} where ad_id={7} ", adacc, adname, telephone, email, areaid, address, comment, ad_id, issetacc);
        }
        else
            sqlstr = string.Format("update  tbl_ad_mch  set  ad_acc='{0}',ad_name='{1}',telephone='{2}',email='{3}',area_id='{4}',address='{5}',comment_='{6}' where ad_id={7} ", adacc, adname, telephone, email, areaid, address, comment, ad_id);
    
         
            if(DbHelperMySQL.ExecuteSql(sqlstr)>0)
            {
                Session["ad_acc"] = adacc;
                Response.Write("[{\"result\":\"修改成功\"}]");
                Response.End();
                //Jscript.back("修改成功",Request.RawUrl);
                return;
            }
            else
            {
                  //Jscript.back("修改失败",Request.RawUrl);
                Response.Write("[{\"err\":\"修改失败\"}]");
                Response.End();
                return;
               
            }
         
           
       
    }

   
}