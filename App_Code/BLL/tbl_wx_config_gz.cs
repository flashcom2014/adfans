using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using DBUtility;//Please add references
namespace BLL
{
	/// <summary>
	/// 类tbl_wx_config_gz。
	/// </summary>
	[Serializable]
	public partial class tbl_wx_config_gz
	{
		public tbl_wx_config_gz()
		{}
		#region Model
		private int _id;
		private string _w_appid;
		private string _w_mchid;
		private string _w_key;
		private string _w_appsecret;
		private string _w_sslcert_path;
		private string _w_sslcert_password;
		private string _w_original_id;
		private string _w_access_token;
		private DateTime? _w_expires_in= DateTime.Now;
		private int? _w_qz_gz=0;
		private string _w_ticket;
		private int _payment_id=0;
		private int? _enable;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 绑定支付的APPID（必须配置）
		/// </summary>
		public string W_APPID
		{
			set{ _w_appid=value;}
			get{return _w_appid;}
		}
		/// <summary>
		/// 商户号（必须配置）
		/// </summary>
		public string W_MCHID
		{
			set{ _w_mchid=value;}
			get{return _w_mchid;}
		}
		/// <summary>
		/// 商户支付密钥，参考开户邮件设置（必须配置）
		/// </summary>
		public string W_KEY
		{
			set{ _w_key=value;}
			get{return _w_key;}
		}
		/// <summary>
		/// 公众帐号secert
		/// </summary>
		public string W_APPSECRET
		{
			set{ _w_appsecret=value;}
			get{return _w_appsecret;}
		}
		/// <summary>
		/// 证书路径设置
		/// </summary>
		public string W_SSLCERT_PATH
		{
			set{ _w_sslcert_path=value;}
			get{return _w_sslcert_path;}
		}
		/// <summary>
		/// 证书密码
		/// </summary>
		public string W_SSLCERT_PASSWORD
		{
			set{ _w_sslcert_password=value;}
			get{return _w_sslcert_password;}
		}
		/// <summary>
		/// 原始ID
		/// </summary>
		public string W_ORIGINAL_ID
		{
			set{ _w_original_id=value;}
			get{return _w_original_id;}
		}
		/// <summary>
		/// 全局ACCESS_TOKEN,两小时更新一次
		/// </summary>
		public string W_ACCESS_TOKEN
		{
			set{ _w_access_token=value;}
			get{return _w_access_token;}
		}
		/// <summary>
		/// 全局ACCESS_TOKEN,过期时间
		/// </summary>
		public DateTime? W_EXPIRES_IN
		{
			set{ _w_expires_in=value;}
			get{return _w_expires_in;}
		}
		/// <summary>
		/// 是否强制关注
		/// </summary>
		public int? W_QZ_GZ
		{
			set{ _w_qz_gz=value;}
			get{return _w_qz_gz;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string W_TICKET
		{
			set{ _w_ticket=value;}
			get{return _w_ticket;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int payment_id
		{
			set{ _payment_id=value;}
			get{return _payment_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? enable
		{
			set{ _enable=value;}
			get{return _enable;}
		}
		#endregion Model


		#region  Method

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public tbl_wx_config_gz(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,W_APPID,W_MCHID,W_KEY,W_APPSECRET,W_SSLCERT_PATH,W_SSLCERT_PASSWORD,W_ORIGINAL_ID,W_ACCESS_TOKEN,W_EXPIRES_IN,W_QZ_GZ,W_TICKET,payment_id,enable ");
			strSql.Append(" FROM tbl_wx_config_gz ");
			strSql.Append(" where id=@id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)};
			parameters[0].Value = id;

			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					this.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["W_APPID"]!=null)
				{
					this.W_APPID=ds.Tables[0].Rows[0]["W_APPID"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_MCHID"]!=null)
				{
					this.W_MCHID=ds.Tables[0].Rows[0]["W_MCHID"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_KEY"]!=null)
				{
					this.W_KEY=ds.Tables[0].Rows[0]["W_KEY"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_APPSECRET"]!=null)
				{
					this.W_APPSECRET=ds.Tables[0].Rows[0]["W_APPSECRET"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_SSLCERT_PATH"]!=null)
				{
					this.W_SSLCERT_PATH=ds.Tables[0].Rows[0]["W_SSLCERT_PATH"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_SSLCERT_PASSWORD"]!=null)
				{
					this.W_SSLCERT_PASSWORD=ds.Tables[0].Rows[0]["W_SSLCERT_PASSWORD"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_ORIGINAL_ID"]!=null)
				{
					this.W_ORIGINAL_ID=ds.Tables[0].Rows[0]["W_ORIGINAL_ID"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_ACCESS_TOKEN"]!=null)
				{
					this.W_ACCESS_TOKEN=ds.Tables[0].Rows[0]["W_ACCESS_TOKEN"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_EXPIRES_IN"]!=null && ds.Tables[0].Rows[0]["W_EXPIRES_IN"].ToString()!="")
				{
					this.W_EXPIRES_IN=DateTime.Parse(ds.Tables[0].Rows[0]["W_EXPIRES_IN"].ToString());
				}
				if(ds.Tables[0].Rows[0]["W_QZ_GZ"]!=null && ds.Tables[0].Rows[0]["W_QZ_GZ"].ToString()!="")
				{
					this.W_QZ_GZ=int.Parse(ds.Tables[0].Rows[0]["W_QZ_GZ"].ToString());
				}
				if(ds.Tables[0].Rows[0]["W_TICKET"]!=null)
				{
					this.W_TICKET=ds.Tables[0].Rows[0]["W_TICKET"].ToString();
				}
				if(ds.Tables[0].Rows[0]["payment_id"]!=null && ds.Tables[0].Rows[0]["payment_id"].ToString()!="")
				{
					this.payment_id=int.Parse(ds.Tables[0].Rows[0]["payment_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["enable"]!=null && ds.Tables[0].Rows[0]["enable"].ToString()!="")
				{
					this.enable=int.Parse(ds.Tables[0].Rows[0]["enable"].ToString());
				}
			}
		}

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{

		return DbHelperMySQL.GetMaxID("id", "tbl_wx_config_gz"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tbl_wx_config_gz");
			strSql.Append(" where id=@id ");

			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)};
			parameters[0].Value = id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add()
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tbl_wx_config_gz (");
			strSql.Append("id,W_APPID,W_MCHID,W_KEY,W_APPSECRET,W_SSLCERT_PATH,W_SSLCERT_PASSWORD,W_ORIGINAL_ID,W_ACCESS_TOKEN,W_EXPIRES_IN,W_QZ_GZ,W_TICKET,payment_id,enable)");
			strSql.Append(" values (");
			strSql.Append("@id,@W_APPID,@W_MCHID,@W_KEY,@W_APPSECRET,@W_SSLCERT_PATH,@W_SSLCERT_PASSWORD,@W_ORIGINAL_ID,@W_ACCESS_TOKEN,@W_EXPIRES_IN,@W_QZ_GZ,@W_TICKET,@payment_id,@enable)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32,11),
					new MySqlParameter("@W_APPID", MySqlDbType.VarChar,50),
					new MySqlParameter("@W_MCHID", MySqlDbType.VarChar,50),
					new MySqlParameter("@W_KEY", MySqlDbType.VarChar,255),
					new MySqlParameter("@W_APPSECRET", MySqlDbType.VarChar,255),
					new MySqlParameter("@W_SSLCERT_PATH", MySqlDbType.VarChar,50),
					new MySqlParameter("@W_SSLCERT_PASSWORD", MySqlDbType.VarChar,50),
					new MySqlParameter("@W_ORIGINAL_ID", MySqlDbType.VarChar,50),
					new MySqlParameter("@W_ACCESS_TOKEN", MySqlDbType.VarChar,200),
					new MySqlParameter("@W_EXPIRES_IN", MySqlDbType.Timestamp),
					new MySqlParameter("@W_QZ_GZ", MySqlDbType.Int32,2),
					new MySqlParameter("@W_TICKET", MySqlDbType.VarChar,255),
					new MySqlParameter("@payment_id", MySqlDbType.Int32,100),
					new MySqlParameter("@enable", MySqlDbType.Int32,11)};
			parameters[0].Value = id;
			parameters[1].Value = W_APPID;
			parameters[2].Value = W_MCHID;
			parameters[3].Value = W_KEY;
			parameters[4].Value = W_APPSECRET;
			parameters[5].Value = W_SSLCERT_PATH;
			parameters[6].Value = W_SSLCERT_PASSWORD;
			parameters[7].Value = W_ORIGINAL_ID;
			parameters[8].Value = W_ACCESS_TOKEN;
			parameters[9].Value = W_EXPIRES_IN;
			parameters[10].Value = W_QZ_GZ;
			parameters[11].Value = W_TICKET;
			parameters[12].Value = payment_id;
			parameters[13].Value = enable;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update()
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tbl_wx_config_gz set ");
			strSql.Append("W_APPID=@W_APPID,");
			strSql.Append("W_MCHID=@W_MCHID,");
			strSql.Append("W_KEY=@W_KEY,");
			strSql.Append("W_APPSECRET=@W_APPSECRET,");
			strSql.Append("W_SSLCERT_PATH=@W_SSLCERT_PATH,");
			strSql.Append("W_SSLCERT_PASSWORD=@W_SSLCERT_PASSWORD,");
			strSql.Append("W_ORIGINAL_ID=@W_ORIGINAL_ID,");
			strSql.Append("W_ACCESS_TOKEN=@W_ACCESS_TOKEN,");
			strSql.Append("W_EXPIRES_IN=@W_EXPIRES_IN,");
			strSql.Append("W_QZ_GZ=@W_QZ_GZ,");
			strSql.Append("W_TICKET=@W_TICKET,");
			strSql.Append("payment_id=@payment_id,");
			strSql.Append("enable=@enable");
			strSql.Append(" where id=@id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@W_APPID", MySqlDbType.VarChar,50),
					new MySqlParameter("@W_MCHID", MySqlDbType.VarChar,50),
					new MySqlParameter("@W_KEY", MySqlDbType.VarChar,255),
					new MySqlParameter("@W_APPSECRET", MySqlDbType.VarChar,255),
					new MySqlParameter("@W_SSLCERT_PATH", MySqlDbType.VarChar,50),
					new MySqlParameter("@W_SSLCERT_PASSWORD", MySqlDbType.VarChar,50),
					new MySqlParameter("@W_ORIGINAL_ID", MySqlDbType.VarChar,50),
					new MySqlParameter("@W_ACCESS_TOKEN", MySqlDbType.VarChar,200),
					new MySqlParameter("@W_EXPIRES_IN", MySqlDbType.Timestamp),
					new MySqlParameter("@W_QZ_GZ", MySqlDbType.Int32,2),
					new MySqlParameter("@W_TICKET", MySqlDbType.VarChar,255),
					new MySqlParameter("@payment_id", MySqlDbType.Int32,100),
					new MySqlParameter("@enable", MySqlDbType.Int32,11),
					new MySqlParameter("@id", MySqlDbType.Int32,11)};
			parameters[0].Value = W_APPID;
			parameters[1].Value = W_MCHID;
			parameters[2].Value = W_KEY;
			parameters[3].Value = W_APPSECRET;
			parameters[4].Value = W_SSLCERT_PATH;
			parameters[5].Value = W_SSLCERT_PASSWORD;
			parameters[6].Value = W_ORIGINAL_ID;
			parameters[7].Value = W_ACCESS_TOKEN;
			parameters[8].Value = W_EXPIRES_IN;
			parameters[9].Value = W_QZ_GZ;
			parameters[10].Value = W_TICKET;
			parameters[11].Value = payment_id;
			parameters[12].Value = enable;
			parameters[13].Value = id;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tbl_wx_config_gz ");
			strSql.Append(" where id=@id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)};
			parameters[0].Value = id;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public void GetModel(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,W_APPID,W_MCHID,W_KEY,W_APPSECRET,W_SSLCERT_PATH,W_SSLCERT_PASSWORD,W_ORIGINAL_ID,W_ACCESS_TOKEN,W_EXPIRES_IN,W_QZ_GZ,W_TICKET,payment_id,enable ");
			strSql.Append(" FROM tbl_wx_config_gz ");
			strSql.Append(" where id=@id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)};
			parameters[0].Value = id;

			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					this.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["W_APPID"]!=null )
				{
					this.W_APPID=ds.Tables[0].Rows[0]["W_APPID"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_MCHID"]!=null )
				{
					this.W_MCHID=ds.Tables[0].Rows[0]["W_MCHID"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_KEY"]!=null )
				{
					this.W_KEY=ds.Tables[0].Rows[0]["W_KEY"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_APPSECRET"]!=null )
				{
					this.W_APPSECRET=ds.Tables[0].Rows[0]["W_APPSECRET"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_SSLCERT_PATH"]!=null )
				{
					this.W_SSLCERT_PATH=ds.Tables[0].Rows[0]["W_SSLCERT_PATH"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_SSLCERT_PASSWORD"]!=null )
				{
					this.W_SSLCERT_PASSWORD=ds.Tables[0].Rows[0]["W_SSLCERT_PASSWORD"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_ORIGINAL_ID"]!=null )
				{
					this.W_ORIGINAL_ID=ds.Tables[0].Rows[0]["W_ORIGINAL_ID"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_ACCESS_TOKEN"]!=null )
				{
					this.W_ACCESS_TOKEN=ds.Tables[0].Rows[0]["W_ACCESS_TOKEN"].ToString();
				}
				if(ds.Tables[0].Rows[0]["W_EXPIRES_IN"]!=null && ds.Tables[0].Rows[0]["W_EXPIRES_IN"].ToString()!="")
				{
					this.W_EXPIRES_IN=DateTime.Parse(ds.Tables[0].Rows[0]["W_EXPIRES_IN"].ToString());
				}
				if(ds.Tables[0].Rows[0]["W_QZ_GZ"]!=null && ds.Tables[0].Rows[0]["W_QZ_GZ"].ToString()!="")
				{
					this.W_QZ_GZ=int.Parse(ds.Tables[0].Rows[0]["W_QZ_GZ"].ToString());
				}
				if(ds.Tables[0].Rows[0]["W_TICKET"]!=null )
				{
					this.W_TICKET=ds.Tables[0].Rows[0]["W_TICKET"].ToString();
				}
				if(ds.Tables[0].Rows[0]["payment_id"]!=null && ds.Tables[0].Rows[0]["payment_id"].ToString()!="")
				{
					this.payment_id=int.Parse(ds.Tables[0].Rows[0]["payment_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["enable"]!=null && ds.Tables[0].Rows[0]["enable"].ToString()!="")
				{
					this.enable=int.Parse(ds.Tables[0].Rows[0]["enable"].ToString());
				}
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * ");
			strSql.Append(" FROM tbl_wx_config_gz ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperMySQL.Query(strSql.ToString());
		}

		#endregion  Method
	}
}

