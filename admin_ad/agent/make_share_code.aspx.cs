using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;
using System.Text;

public partial class admin_ad_expand_make_share_code : AdminPageBasead
{
    public string max = "0";
    public string myscale = "0";
    public bool showresult = false;
    public string regurl = "";
    public string minamount = "1.5";

    private string codelist = "1e3ur486h7pasdw905ty2ioqjxnkcvblzmgf";
    protected void Page_Load(object sender, EventArgs e)
    {
        GetScalePoints();
    }

    private void GetScalePoints()
    {
        if (!IsPostBack)
        {
            if (MyAdMch != null)
            {
                float scale = MyAdMch.scale_points;
                ViewState["scale"] = scale;
                max = (scale * 100).ToString();
                l_scale.Text = myscale = scale.ToString() + "%";
                ViewState["amount"] = t_amount.Value = minamount = (float.Parse(MyAdMch.sub_amount.ToString()) / 100f).ToString("F2");
                l_minamount.Text = string.Format(" (不能低于{0}元/个)", (float.Parse(MyAdMch.sub_amount.ToString()) / 100f).ToString("F2"));
            }
        }
        else
        {
            if (ViewState["scale"] != null && ViewState["amount"] != null)
            {
                float scale = float.Parse(ViewState["scale"].ToString());
                max = (scale * 100).ToString();
                l_scale.Text = myscale = scale.ToString() + "%";
                t_amount.Value = ViewState["amount"].ToString();
            }
        }
    }

    protected void b_makecode_Click(object sender, EventArgs e)
    {
        try
        {
            string tscale = Request.Form["scale"];
            if (string.IsNullOrEmpty(tscale))
            {
                Jscript.Alert2(Page, "参数错误！");
                return;
            }
            string samount = Request.Form["t_amount"];
            if (string.IsNullOrEmpty(samount))
            {
                Jscript.Alert2(Page, "参数错误！");
                return;
            }
            ViewState["amount"] = samount;
            //
            float amount = float.Parse(samount) * 100f;

            if (amount < MyAdMch.sub_amount)
            {
                Jscript.Alert2(Page, "非法操作");
                return;
            }

            float scale_points = float.Parse(tscale);

            if (scale_points > MyAdMch.scale_points)
            {
                Jscript.Alert2(Page, "非法操作");
                return;
            }

            Random ra = new Random(Guid.NewGuid().GetHashCode());
            StringBuilder sb = new StringBuilder();
            string sharecode = "";
            int theid = 0;
            float thescale = 0f;
            float theamount = 0f;
            bool isrepeat = false;
            Dictionary<string, byte> list = new Dictionary<string, byte>();
            string sqlstr = "select * from tbl_ad_share_code";
            DataSet ds = DbHelperMySQL.Query(sqlstr);
            DataTable dt = ds.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                list[row["share_code"].ToString()] = 0;
                theid = int.Parse(row["ad_id"].ToString());
                thescale = float.Parse(row["scale"].ToString());
                theamount = float.Parse(row["amount"].ToString());
                if (theid == ad_id && thescale == scale_points && theamount == amount)
                {
                    isrepeat = true;
                    sharecode = row["share_code"].ToString();
                    break;
                }
            }
            
            dt.Dispose();
            ds.Dispose();

            if (isrepeat)
            {
                Jscript.Alert2(Page, string.Format("您已创建了一个（分成比例为：{0}%,吸粉价格为：{2}元/个）的推广码“{1}”，请不要重复添加！", scale_points, sharecode, samount));
                return;
            }


            int len = codelist.Length;

        ag:
            try
            {

                while (true)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        sb.Append(codelist[ra.Next(len)]);
                    }
                    sharecode = sb.ToString();
                    sb.Clear();
                    if (!list.ContainsKey(sharecode))
                        break;

                }
                sqlstr = string.Format("insert into tbl_ad_share_code(share_code,ad_id,scale,amount) values('{0}',{1},{2},{3});", sharecode, ad_id, scale_points, amount);
                int result = DbHelperMySQL.ExecuteSql(sqlstr);
                if (result > 0)
                {
                    l_code.Text = sharecode;
                    l_makescale.Text = tscale + "%";
                    l_makeamount.Text = (amount / 100f).ToString("F2") + "元/个";
                    regurl = UrlHelper.GetNLBPathUrl("/admin_ad/register.aspx?code=") + sharecode;
                    showresult = true;
                }
                else
                {
                    Jscript.Alert2(Page, "生成推广码错误，请稍候再试！");
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Message.IndexOf("share_code") > 0)
                {
                    goto ag;
                }
            }
            list.Clear();
            list = null;
        }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("make_share_coe: " + ex.ToString());
            Jscript.Alert2(Page, "系统错误");
        }
    }
}