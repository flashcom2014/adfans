using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Web;
using MySql.Data.MySqlClient;
namespace DBUtility
{
    public class DB
    {
        private MySqlConnection conn;
        private MySqlCommand comm;
        private MySqlDataAdapter sda;
        private DataTable td;
        private MySqlDataReader dr;
        private DataSet ds;

        public DB()
        {
            string con = ConfigurationManager.AppSettings["connstr"].ToString();
            conn = new MySqlConnection(con);
            conn.Open();
        }
        public void CloseDB()
        {
            if (conn != null)
            {
                if (conn.State.ToString() == "Open")
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public DataTable dtProStore(string sql, MySqlParameter[] par)
        {
            comm = new MySqlCommand(sql, conn);
            comm.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < par.Length; i++)
            {
                comm.Parameters.Add(par[i]);
            }
            sda = new MySqlDataAdapter(comm);
            td = new DataTable();
            sda.Fill(td);
            return td;


        }


    }
}