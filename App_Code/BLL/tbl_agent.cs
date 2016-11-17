using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using DBUtility;
using System.Collections;
using System.Collections.Generic;//Please add references
namespace BLL
{
    /// <summary>
    /// 类tbl_agent。
    /// </summary>
    [Serializable]
    public partial class tbl_agent
    {
public tbl_agent()
		{}
		#region Model
		private int _agent_id;
		private string _agent_acc;
		private string _agent_pwd;
		private int? _agent_parent_id;
		private string _fullname;
		private string _telephone;
		private int? _audit;
		private int? _enable;
		private string _mobile;
		private string _email;
		private string _address;
		private string _comment;
		private decimal? _tx_min=0M;
        private string _alias_acc;
        private int? _is_devicebind;
		/// <summary>
		/// 
		/// </summary>
		public int agent_id
		{
			set{ _agent_id=value;}
			get{return _agent_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string agent_acc
		{
			set{ _agent_acc=value;}
			get{return _agent_acc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string agent_pwd
		{
			set{ _agent_pwd=value;}
			get{return _agent_pwd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? agent_parent_id
		{
			set{ _agent_parent_id=value;}
			get{return _agent_parent_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string fullname
		{
			set{ _fullname=value;}
			get{return _fullname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string telephone
		{
			set{ _telephone=value;}
			get{return _telephone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? audit
		{
			set{ _audit=value;}
			get{return _audit;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? enable
		{
			set{ _enable=value;}
			get{return _enable;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string comment
		{
			set{ _comment=value;}
			get{return _comment;}
		}
		/// <summary>
		/// 上级指定的提现最小值
		/// </summary>
		public decimal? tx_min
		{
			set{ _tx_min=value;}
			get{return _tx_min;}
		}

        public int? is_devicebind
        {
            set { _is_devicebind = value; }
            get { return _is_devicebind; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string alias_acc
        {
            set { _alias_acc = value; }
            get { return _alias_acc; }
        }
		#endregion Model


		#region  Method

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public tbl_agent(int agent_id)
		{
            GetModel(agent_id);
            //StringBuilder strSql=new StringBuilder();
            //strSql.Append("select agent_id,agent_acc,agent_pwd,agent_parent_id,fullname,telephone,audit,enable,mobile,email,address,comment,tx_min ");
            //strSql.Append(" FROM tbl_agent ");
            //strSql.Append(" where agent_id=@agent_id ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("@agent_id", MySqlDbType.Int32)};
            //parameters[0].Value = agent_id;

            //DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
            //if(ds.Tables[0].Rows.Count>0)
            //{
            //    if(ds.Tables[0].Rows[0]["agent_id"]!=null && ds.Tables[0].Rows[0]["agent_id"].ToString()!="")
            //    {
            //        this.agent_id=int.Parse(ds.Tables[0].Rows[0]["agent_id"].ToString());
            //    }
            //    if(ds.Tables[0].Rows[0]["agent_acc"]!=null)
            //    {
            //        this.agent_acc=ds.Tables[0].Rows[0]["agent_acc"].ToString();
            //    }
            //    if(ds.Tables[0].Rows[0]["agent_pwd"]!=null)
            //    {
            //        this.agent_pwd=ds.Tables[0].Rows[0]["agent_pwd"].ToString();
            //    }
            //    if(ds.Tables[0].Rows[0]["agent_parent_id"]!=null && ds.Tables[0].Rows[0]["agent_parent_id"].ToString()!="")
            //    {
            //        this.agent_parent_id=int.Parse(ds.Tables[0].Rows[0]["agent_parent_id"].ToString());
            //    }
            //    if(ds.Tables[0].Rows[0]["fullname"]!=null)
            //    {
            //        this.fullname=ds.Tables[0].Rows[0]["fullname"].ToString();
            //    }
            //    if(ds.Tables[0].Rows[0]["telephone"]!=null)
            //    {
            //        this.telephone=ds.Tables[0].Rows[0]["telephone"].ToString();
            //    }
            //    if(ds.Tables[0].Rows[0]["audit"]!=null && ds.Tables[0].Rows[0]["audit"].ToString()!="")
            //    {
            //        this.audit=int.Parse(ds.Tables[0].Rows[0]["audit"].ToString());
            //    }
            //    if(ds.Tables[0].Rows[0]["enable"]!=null && ds.Tables[0].Rows[0]["enable"].ToString()!="")
            //    {
            //        this.enable=int.Parse(ds.Tables[0].Rows[0]["enable"].ToString());
            //    }
            //    if(ds.Tables[0].Rows[0]["mobile"]!=null)
            //    {
            //        this.mobile=ds.Tables[0].Rows[0]["mobile"].ToString();
            //    }
            //    if(ds.Tables[0].Rows[0]["email"]!=null)
            //    {
            //        this.email=ds.Tables[0].Rows[0]["email"].ToString();
            //    }
            //    if(ds.Tables[0].Rows[0]["address"]!=null)
            //    {
            //        this.address=ds.Tables[0].Rows[0]["address"].ToString();
            //    }
            //    if(ds.Tables[0].Rows[0]["comment"]!=null)
            //    {
            //        this.comment=ds.Tables[0].Rows[0]["comment"].ToString();
            //    }
            //    if(ds.Tables[0].Rows[0]["tx_min"]!=null && ds.Tables[0].Rows[0]["tx_min"].ToString()!="")
            //    {
            //        this.tx_min=decimal.Parse(ds.Tables[0].Rows[0]["tx_min"].ToString());
            //    }
            //}
		}
        public tbl_agent(string agent_acc)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select agent_id,agent_acc,agent_pwd,agent_parent_id,fullname,telephone,audit,enable,mobile,email,address,comment,tx_min ");
            strSql.Append(" FROM tbl_agent ");
            strSql.Append(" where agent_acc=@agent_acc ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@agent_acc", MySqlDbType.VarChar)};
            parameters[0].Value = agent_acc;

            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["agent_id"] != null && ds.Tables[0].Rows[0]["agent_id"].ToString() != "")
                {
                    this.agent_id = int.Parse(ds.Tables[0].Rows[0]["agent_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["agent_acc"] != null)
                {
                    this.agent_acc = ds.Tables[0].Rows[0]["agent_acc"].ToString();
                }
                if (ds.Tables[0].Rows[0]["agent_pwd"] != null)
                {
                    this.agent_pwd = ds.Tables[0].Rows[0]["agent_pwd"].ToString();
                }
                if (ds.Tables[0].Rows[0]["agent_parent_id"] != null && ds.Tables[0].Rows[0]["agent_parent_id"].ToString() != "")
                {
                    this.agent_parent_id = int.Parse(ds.Tables[0].Rows[0]["agent_parent_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["fullname"] != null)
                {
                    this.fullname = ds.Tables[0].Rows[0]["fullname"].ToString();
                }
                if (ds.Tables[0].Rows[0]["telephone"] != null)
                {
                    this.telephone = ds.Tables[0].Rows[0]["telephone"].ToString();
                }
                if (ds.Tables[0].Rows[0]["audit"] != null && ds.Tables[0].Rows[0]["audit"].ToString() != "")
                {
                    this.audit = int.Parse(ds.Tables[0].Rows[0]["audit"].ToString());
                }
                if (ds.Tables[0].Rows[0]["enable"] != null && ds.Tables[0].Rows[0]["enable"].ToString() != "")
                {
                    this.enable = int.Parse(ds.Tables[0].Rows[0]["enable"].ToString());
                }
                if (ds.Tables[0].Rows[0]["mobile"] != null)
                {
                    this.mobile = ds.Tables[0].Rows[0]["mobile"].ToString();
                }
                if (ds.Tables[0].Rows[0]["email"] != null)
                {
                    this.email = ds.Tables[0].Rows[0]["email"].ToString();
                }
                if (ds.Tables[0].Rows[0]["address"] != null)
                {
                    this.address = ds.Tables[0].Rows[0]["address"].ToString();
                }
                if (ds.Tables[0].Rows[0]["comment"] != null)
                {
                    this.comment = ds.Tables[0].Rows[0]["comment"].ToString();
                }
                if (ds.Tables[0].Rows[0]["tx_min"] != null && ds.Tables[0].Rows[0]["tx_min"].ToString() != "")
                {
                    this.tx_min = decimal.Parse(ds.Tables[0].Rows[0]["tx_min"].ToString());
                }
            }
        }

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{

		return DbHelperMySQL.GetMaxID("agent_id", "tbl_agent"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int agent_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tbl_agent");
			strSql.Append(" where agent_id=@agent_id ");

			MySqlParameter[] parameters = {
					new MySqlParameter("@agent_id", MySqlDbType.Int32)};
			parameters[0].Value = agent_id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add()
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tbl_agent (");
            strSql.Append("agent_id,agent_acc,agent_pwd,agent_parent_id,fullname,telephone,audit,enable,mobile,email,address,comment,tx_min,alias_acc,is_devicebind)");
			strSql.Append(" values (");
            strSql.Append("@agent_id,@agent_acc,@agent_pwd,@agent_parent_id,@fullname,@telephone,@audit,@enable,@mobile,@email,@address,@comment,@tx_min,@alias_acc,@is_devicebind)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@agent_id", MySqlDbType.Int32,11),
					new MySqlParameter("@agent_acc", MySqlDbType.VarChar,255),
					new MySqlParameter("@agent_pwd", MySqlDbType.VarChar,255),
					new MySqlParameter("@agent_parent_id", MySqlDbType.Int32,11),
					new MySqlParameter("@fullname", MySqlDbType.VarChar,255),
					new MySqlParameter("@telephone", MySqlDbType.VarChar,255),
					new MySqlParameter("@audit", MySqlDbType.Int32,255),
					new MySqlParameter("@enable", MySqlDbType.Int32,11),
					new MySqlParameter("@mobile", MySqlDbType.VarChar,255),
					new MySqlParameter("@email", MySqlDbType.VarChar,255),
					new MySqlParameter("@address", MySqlDbType.VarChar,255),
					new MySqlParameter("@comment", MySqlDbType.Text),
					new MySqlParameter("@tx_min", MySqlDbType.Decimal,11),
                    new MySqlParameter("@alias_acc", MySqlDbType.VarChar,100),
					new MySqlParameter("@is_devicebind", MySqlDbType.Int32,11)};
			parameters[0].Value = agent_id;
			parameters[1].Value = agent_acc;
			parameters[2].Value = agent_pwd;
			parameters[3].Value = agent_parent_id;
			parameters[4].Value = fullname;
			parameters[5].Value = telephone;
			parameters[6].Value = audit;
			parameters[7].Value = enable;
			parameters[8].Value = mobile;
			parameters[9].Value = email;
			parameters[10].Value = address;
			parameters[11].Value = comment;
            parameters[12].Value = tx_min;
            parameters[13].Value = alias_acc;
            parameters[14].Value = is_devicebind;

			return DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update()
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tbl_agent set ");
			strSql.Append("agent_acc=@agent_acc,");
			strSql.Append("agent_pwd=@agent_pwd,");
			strSql.Append("agent_parent_id=@agent_parent_id,");
			strSql.Append("fullname=@fullname,");
			strSql.Append("telephone=@telephone,");
			strSql.Append("audit=@audit,");
			strSql.Append("enable=@enable,");
			strSql.Append("mobile=@mobile,");
			strSql.Append("email=@email,");
			strSql.Append("address=@address,");
			strSql.Append("comment=@comment,");
            strSql.Append("tx_min=@tx_min,is_devicebind=@is_devicebind");
			strSql.Append(" where agent_id=@agent_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@agent_acc", MySqlDbType.VarChar,255),
					new MySqlParameter("@agent_pwd", MySqlDbType.VarChar,255),
					new MySqlParameter("@agent_parent_id", MySqlDbType.Int32,11),
					new MySqlParameter("@fullname", MySqlDbType.VarChar,255),
					new MySqlParameter("@telephone", MySqlDbType.VarChar,255),
					new MySqlParameter("@audit", MySqlDbType.Int32,255),
					new MySqlParameter("@enable", MySqlDbType.Int32,11),
					new MySqlParameter("@mobile", MySqlDbType.VarChar,255),
					new MySqlParameter("@email", MySqlDbType.VarChar,255),
					new MySqlParameter("@address", MySqlDbType.VarChar,255),
					new MySqlParameter("@comment", MySqlDbType.Text),
					new MySqlParameter("@tx_min", MySqlDbType.Decimal,11),
                    new MySqlParameter("@is_devicebind", MySqlDbType.Int32,11),
					new MySqlParameter("@agent_id", MySqlDbType.Int32,11)};
			parameters[0].Value = agent_acc;
			parameters[1].Value = agent_pwd;
			parameters[2].Value = agent_parent_id;
			parameters[3].Value = fullname;
			parameters[4].Value = telephone;
			parameters[5].Value = audit;
			parameters[6].Value = enable;
			parameters[7].Value = mobile;
			parameters[8].Value = email;
			parameters[9].Value = address;
			parameters[10].Value = comment;
            parameters[11].Value = tx_min;
            parameters[12].Value = is_devicebind;
			parameters[13].Value = agent_id;

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
		public bool Delete(int agent_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tbl_agent ");
			strSql.Append(" where agent_id=@agent_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@agent_id", MySqlDbType.Int32)};
			parameters[0].Value = agent_id;

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
		public void GetModel(int agent_id)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select agent_id,agent_acc,agent_pwd,agent_parent_id,fullname,telephone,audit,enable,mobile,email,address,comment,tx_min,is_devicebind ");
			strSql.Append(" FROM tbl_agent ");
			strSql.Append(" where agent_id=@agent_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@agent_id", MySqlDbType.Int32)};
			parameters[0].Value = agent_id;

			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["agent_id"]!=null && ds.Tables[0].Rows[0]["agent_id"].ToString()!="")
				{
					this.agent_id=int.Parse(ds.Tables[0].Rows[0]["agent_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["agent_acc"]!=null )
				{
					this.agent_acc=ds.Tables[0].Rows[0]["agent_acc"].ToString();
				}
				if(ds.Tables[0].Rows[0]["agent_pwd"]!=null )
				{
					this.agent_pwd=ds.Tables[0].Rows[0]["agent_pwd"].ToString();
				}
				if(ds.Tables[0].Rows[0]["agent_parent_id"]!=null && ds.Tables[0].Rows[0]["agent_parent_id"].ToString()!="")
				{
					this.agent_parent_id=int.Parse(ds.Tables[0].Rows[0]["agent_parent_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fullname"]!=null )
				{
					this.fullname=ds.Tables[0].Rows[0]["fullname"].ToString();
				}
				if(ds.Tables[0].Rows[0]["telephone"]!=null )
				{
					this.telephone=ds.Tables[0].Rows[0]["telephone"].ToString();
				}
				if(ds.Tables[0].Rows[0]["audit"]!=null && ds.Tables[0].Rows[0]["audit"].ToString()!="")
				{
					this.audit=int.Parse(ds.Tables[0].Rows[0]["audit"].ToString());
				}
				if(ds.Tables[0].Rows[0]["enable"]!=null && ds.Tables[0].Rows[0]["enable"].ToString()!="")
				{
					this.enable=int.Parse(ds.Tables[0].Rows[0]["enable"].ToString());
				}
				if(ds.Tables[0].Rows[0]["mobile"]!=null )
				{
					this.mobile=ds.Tables[0].Rows[0]["mobile"].ToString();
				}
				if(ds.Tables[0].Rows[0]["email"]!=null )
				{
					this.email=ds.Tables[0].Rows[0]["email"].ToString();
				}
				if(ds.Tables[0].Rows[0]["address"]!=null )
				{
					this.address=ds.Tables[0].Rows[0]["address"].ToString();
				}
				if(ds.Tables[0].Rows[0]["comment"]!=null )
				{
					this.comment=ds.Tables[0].Rows[0]["comment"].ToString();
				}
				if(ds.Tables[0].Rows[0]["tx_min"]!=null && ds.Tables[0].Rows[0]["tx_min"].ToString()!="")
				{
					this.tx_min=decimal.Parse(ds.Tables[0].Rows[0]["tx_min"].ToString());
				}
                if (ds.Tables[0].Rows[0]["is_devicebind"] != null && ds.Tables[0].Rows[0]["is_devicebind"].ToString() != "")
				{
                    this.is_devicebind = int.Parse(ds.Tables[0].Rows[0]["is_devicebind"].ToString());
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
			strSql.Append(" FROM tbl_agent ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperMySQL.Query(strSql.ToString());
		}
        #endregion  Method

        //得到该代理下所有商家
        public static void GetMids(string a_id, ref string m_ids)
        {
            DataTable masall = DbHelperMySQL.Query_("select a_id,m_id,parent_id from alluser").Tables[0];
            DataRow[] mas = masall.Select("parent_id=" + a_id);
            foreach (DataRow ma in mas)
            {
                if (ma["m_id"].ToString().Trim() == "0")
                {
                    GetMids(ma["a_id"].ToString().Trim(), masall, ref m_ids);
                }
                else
                {
                    m_ids += ma["m_id"].ToString() + ",";
                }
            }
            /*DataTable mas = DbHelperMySQL.Query_("select a_id,m_id from alluser where parent_id=" + a_id).Tables[0];
            foreach (DataRow ma in mas.Rows)
            {
                if (ma["m_id"].ToString().Trim() == "0")
                {
                    GetMids(ma["a_id"].ToString().Trim(), ref m_ids);
                }
                else
                    m_ids += ma["m_id"].ToString() + ",";
            }*/
        }
        public static void GetMids(string a_id,DataTable mas,ref string m_ids)
        {
            DataRow[] drs = mas.Select("parent_id=" + a_id);
            foreach (DataRow ma in drs)
            {
                if (ma["m_id"].ToString().Trim() == "0")
                {
                    GetMids(ma["a_id"].ToString().Trim(),mas, ref m_ids);
                }
                else
                    m_ids += ma["m_id"].ToString() + ",";
            }
        }
        //得到该代理下一级商家与代理
        public static void GetMidFs(string a_id, ref ArrayList m_ids, ref ArrayList a_ids)
        {
            DataTable mas = DbHelperMySQL.Query_("select a_id,m_id from alluser where parent_id=" + a_id).Tables[0];
            foreach (DataRow ma in mas.Rows)
            {
                if (ma["m_id"].ToString().Trim() == "0")
                {
                    a_ids.Add(ma["a_id"].ToString());
                }
                else
                    m_ids.Add(ma["m_id"].ToString());
            }
        }
        //得到该代理下所有商家与代理
        public static void GetAMids(string a_id, ref ArrayList m_ids, ref ArrayList a_ids)
        {
            DataTable masall = DbHelperMySQL.Query_("select a_id,m_id,parent_id from alluser").Tables[0];
            DataRow[] mas = masall.Select("parent_id=" + a_id);
            foreach (DataRow ma in mas)
            {
                if (ma["m_id"].ToString().Trim() == "0")
                {
                    a_ids.Add(ma["a_id"].ToString());
                    GetAMids(ma["a_id"].ToString().Trim(), m_ids, a_ids, masall);
                }
                else
                {
                    m_ids.Add(ma["m_id"].ToString());
                }
            }
            /*DataTable mas = DbHelperMySQL.Query_("select a_id,m_id from alluser where parent_id=" + a_id).Tables[0];
            foreach (DataRow ma in mas.Rows)
            {
                if (ma["m_id"].ToString().Trim() == "0")
                {
                    a_ids.Add(ma["a_id"].ToString());
                    GetAMids(ma["a_id"].ToString().Trim(), ref m_ids, ref a_ids);
                }
                else
                    m_ids.Add(ma["m_id"].ToString());
            }*/
        }

        private static void GetAMids(string a_id, ArrayList m_ids, ArrayList a_ids, DataTable masall)
        {
            DataRow[] mas = masall.Select("parent_id=" + a_id);
            foreach (DataRow ma in mas)
            {
                if (ma["m_id"].ToString().Trim() == "0")
                {
                    a_ids.Add(ma["a_id"].ToString());
                    GetAMids(ma["a_id"].ToString().Trim(), m_ids, a_ids, masall);
                }
                else
                {
                    m_ids.Add(ma["m_id"].ToString());
                }
            }
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public static bool Exists(string acc)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from alluser");
            strSql.Append(" where acc=@acc");

            MySqlParameter[] parameters = {
					new MySqlParameter("@acc", MySqlDbType.VarChar)};
            parameters[0].Value = acc;

            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("select count(1) from tbl_child_agent");
            strSql1.Append(" where child_agacc=@child_agacc");

            MySqlParameter[] parameters1 = {
                    new MySqlParameter("@child_agacc", MySqlDbType.VarChar)};
            parameters1[0].Value = acc;
            if (DbHelperMySQL.Exists(strSql.ToString(), parameters) || DbHelperMySQL.Exists(strSql1.ToString(), parameters1))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 得到上级帐号
        /// </summary>
        public static string GetParentAcc(string acc)
        {
            string parent_id=DbHelperMySQL.GetSingle("select parent_id from alluser where acc='" + acc + "'").ToString();
            if (parent_id == "0")
            {
                return DbHelperMySQL.GetSingle("select uid from tbl_admin  where id=1").ToString();
            }
            else
            {
                return DbHelperMySQL.GetSingle("select agent_acc from tbl_agent where agent_id=" + parent_id).ToString();
            }
        }
        //传入商家编号得到支付账号
        public static ArrayList PayAcc(int mch_id, int permission_id)
        {
            bool usenew = false;
            string sqlstr = "select * from tbl_settings where `keys`='payment';";
            DataSet ds = DbHelperMySQL.Query(sqlstr);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["values"].ToString() == "1")
                    usenew = true;
            }
            dt.Dispose();
            ds.Dispose();
            if (usenew)
            {
                sqlstr = string.Format("select * from tbl_mch_payment where mch_id={0} and permission_id={1};", mch_id, permission_id);
                ds = DbHelperMySQL.Query(sqlstr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    int payment_id = (int)dt.Rows[0]["payment_id"];
                    dt.Dispose();
                    ds.Dispose();
                    return GetPayAcc(payment_id, permission_id);
                }
                dt.Dispose();
                ds.Dispose();
            }

            DataTable payment = DBCache.Query("select * from tbl_payment");
            DataTable mpermission = DBCache.Query("select * from tbl_mch_permission");// DbHelperMySQL.Query("select * from tbl_mch_permission").Tables[0];
            
            //第一步判断商家
            DataRow[] drs = payment.Select("payenable=1 and m_id=" + mch_id + " and permission_id=" + permission_id);//是否设置支付账号资料并启用
            bool ism1 = mpermission.Select("enable=1 and mch_id=" + mch_id + " and permission_id=" + permission_id).Length > 0 ? true : false;//权限是否选中
            if (drs.Length > 0 && ism1)
            {
                return GetPayAcc(int.Parse(drs[0]["payment_id"].ToString()), permission_id);
            }
            //if (permission_id == 22000)//支付方式：API 如果商家没有可用账号就返回为空
            //    return new ArrayList();

            DataTable user = DBCache.Query("select * from alluser");
            DataTable apermission = DBCache.Query("select * from tbl_agent_permission");// DbHelperMySQL.Query("select * from tbl_agent_permission").Tables[0];
            //第二步向上循环代理找可用账号
            int a_id = int.Parse(user.Select("m_id=" + mch_id)[0]["parent_id"].ToString());
            if (a_id == 0) return GetPayAcc(0, permission_id);//上级是管理员就直接返回系统默认账号
        ck:
            drs = payment.Select("payenable=1 and a_id=" + a_id + " and permission_id=" + permission_id);//是否设置支付账号资料并启用
            ism1 = apermission.Select("enable=1 and agent_id=" + a_id + " and permission_id=" + permission_id).Length > 0 ? true : false;//权限是否选中
            if (drs.Length > 0 && ism1)
            {
                return GetPayAcc(int.Parse(drs[0]["payment_id"].ToString()), permission_id);
            }
            a_id = int.Parse(user.Select("a_id=" + a_id)[0]["parent_id"].ToString());
            if (a_id == 0) return GetPayAcc(0, permission_id);//上级是管理员就直接返回系统默认账号
            goto ck;
        }
        public static ArrayList PayAcc(int mch_id, int permission_id, ref Dictionary<int,tbl_wx_config_gz> arrlist)
        {
            bool usenew = false;
            string sqlstr = "select * from tbl_settings where `keys`='payment';";
            DataSet ds = DbHelperMySQL.Query(sqlstr);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["values"].ToString() == "1")
                    usenew = true;
            }
            dt.Dispose();
            ds.Dispose();
            if (usenew)
            {
                sqlstr = string.Format("select * from tbl_mch_payment where mch_id={0} and permission_id={1};", mch_id, permission_id);
                ds = DbHelperMySQL.Query(sqlstr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    int payment_id = (int)dt.Rows[0]["payment_id"];

                    if (permission_id == 21000)
                    {
                        DataTable tpayment = DBCache.Query("select * from tbl_payment");
                        DataRow[] gzdrs = tpayment.Select("m_id=" + mch_id + " and permission_id=21000");
                        
                        if (gzdrs.Length > 0)
                        {
                            int payid = int.Parse(gzdrs[0]["payment_id"].ToString());
                            sqlstr = string.Format("select * from tbl_wx_config_gz where enable=1 and payment_id={0};", payid);
                            ds = DbHelperMySQL.Query(sqlstr);
                            dt = ds.Tables[0];
                            tbl_wx_config wx = new tbl_wx_config(payid);
                            if (dt.Rows.Count > 0 && wx.W_QZ_GZ == 1)
                            {
                                IList<tbl_wx_config_gz> list = common.ModelConvertHelper<tbl_wx_config_gz>.ConvertToModel(dt);
                                foreach (tbl_wx_config_gz w in list)
                                {
                                    arrlist.Add(w.id, w);
                                }
                            }
                        }
                    }


                    dt.Dispose();
                    ds.Dispose();
                    return GetPayAcc(payment_id, permission_id);
                }
                dt.Dispose();
                ds.Dispose();
            }



            DataTable payment = DBCache.Query("select * from tbl_payment");
            DataTable mpermission = DBCache.Query("select * from tbl_mch_permission");//DbHelperMySQL.Query("select * from tbl_mch_permission").Tables[0];
            DataTable gz = DbHelperMySQL.Query("select * from tbl_wx_config_gz where enable=1").Tables[0];

            if (permission_id == 21000)
            {
                DataRow[] gzdrs = payment.Select("m_id=" + mch_id + " and permission_id=21000");
                
                if (gzdrs.Length > 0)
                {
                    int payment_id = int.Parse(gzdrs[0]["payment_id"].ToString());
                    DataRow[] qzgzdrs = gz.Select("enable=1 and payment_id=" + payment_id);
                    tbl_wx_config wx = new tbl_wx_config(payment_id);
                    if (qzgzdrs.Length > 0&&wx.W_QZ_GZ==1)
                    {
                        IList<tbl_wx_config_gz> list = common.ModelConvertHelper<tbl_wx_config_gz>.ConvertToModel(qzgzdrs.CopyToDataTable());
                        foreach (tbl_wx_config_gz w in list)
                        {
                            arrlist.Add(w.id, w);
                        }
                    }
                }
            }

            //第一步判断商家
            DataRow[] drs = payment.Select("payenable=1 and m_id=" + mch_id + " and permission_id=" + permission_id);//是否设置支付账号资料并启用
            bool ism1 = mpermission.Select("enable=1 and mch_id=" + mch_id + " and permission_id=" + permission_id).Length > 0 ? true : false;//权限是否选中
            if (drs.Length > 0 && ism1)
            {
                return GetPayAcc(int.Parse(drs[0]["payment_id"].ToString()), permission_id);
            }
            //if (permission_id == 22000)//支付方式：API 如果商家没有可用账号就返回为空
            //    return new ArrayList();

            DataTable user = DBCache.Query("select * from alluser");
            DataTable apermission =DBCache.Query("select * from tbl_agent_permission");//DbHelperMySQL.Query("select * from tbl_agent_permission").Tables[0];
            //第二步向上循环代理找可用账号
            int a_id = int.Parse(user.Select("m_id=" + mch_id)[0]["parent_id"].ToString());
            if (a_id == 0) return GetPayAcc(0, permission_id);//上级是管理员就直接返回系统默认账号
        ck:
            drs = payment.Select("payenable=1 and a_id=" + a_id + " and permission_id=" + permission_id);//是否设置支付账号资料并启用
            ism1 = apermission.Select("enable=1 and agent_id=" + a_id + " and permission_id=" + permission_id).Length > 0 ? true : false;//权限是否选中
            if (drs.Length > 0 && ism1)
            {
                return GetPayAcc(int.Parse(drs[0]["payment_id"].ToString()), permission_id);
            }
            a_id = int.Parse(user.Select("a_id=" + a_id)[0]["parent_id"].ToString());
            if (a_id == 0) return GetPayAcc(0, permission_id);//上级是管理员就直接返回系统默认账号
            goto ck;
        }
        public static ArrayList GetPayAcc(int payment_id, int permission_id)
        {
            ArrayList arr = new ArrayList();
            switch (permission_id)
            {
                //case 20000:
                //    payment_id = payment_id == 0 ? 5 : payment_id;//默认账号自动分配ID
                //    tbl_zfb_config zfb = new tbl_zfb_config(payment_id);
                //    arr.Add(zfb);
                //    break;
                case 21000:
                    payment_id = payment_id == 0 ? 6 : payment_id;//默认账号自动分配ID
                    tbl_wx_config wx = new tbl_wx_config(payment_id);
                    arr.Add(wx);
                    break;
                //case 22000:
                //    payment_id = payment_id == 0 ? 7 : payment_id;//默认账号自动分配ID
                //    api_device0 api = new api_device0(payment_id);
                //    arr.Add(api);
                //    break;
                //case 23000:
                //    payment_id = payment_id == 0 ? 8 : payment_id;//默认账号自动分配ID
                //    tbl_pp_config pp = new tbl_pp_config(payment_id);
                //    arr.Add(pp);
                //    break;
                //case 24000:
                //    payment_id = payment_id == 0 ? 9 : payment_id;//默认账号自动分配ID
                //    tbl_kb_config kb = new tbl_kb_config(payment_id);
                //    arr.Add(kb);
                //    break;
                //case 28000:
                //    payment_id = payment_id == 0 ? 12 : payment_id;//默认账号自动分配ID
                //    tbl_tenpay_config tp = new tbl_tenpay_config(payment_id);
                //    arr.Add(tp);
                //    break;
            }
            return arr;
        }
    }
}

