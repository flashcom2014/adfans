using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DBUtility;

namespace DBUtility
{

    /// <summary>
    /// 数据库缓存机制类
    /// </summary>
    public static class DBCache
    {
        public static int UpdateSec = 60;
        private static Dictionary<string, CacheDats> cachelist = new Dictionary<string, CacheDats>();

        public static DataTable Query(string sqlstr)
        {
            if (cachelist.ContainsKey(sqlstr))
            {
                CacheDats cds = cachelist[sqlstr];
                if (!cds.IsOverTime)    //没超时
                {
                    return cds.CacheTable;
                }
            }
            return UpdateTable(sqlstr);
        }

        public static void RefreshTable(string sqlstr)
        {
            cachelist.Remove(sqlstr);
        }

        public static DataTable UpdateTable(string sqlstr)
        {
            DataTable dt = DbHelperMySQL.Query_(sqlstr).Tables[0];
            DateTime overtime = DateTime.Now.AddSeconds(UpdateSec);
            CacheDats cds = new CacheDats(overtime, dt);
            cachelist[sqlstr] = cds;
            return dt;
        }
    }


    public class CacheDats
    {
        public DateTime OverTime;
        public DataTable CacheTable = null;

        public CacheDats(DateTime time, DataTable table)
        {
            this.OverTime = time;
            this.CacheTable = table;
        }

        public bool IsOverTime
        {
            get
            {
                if (DateTime.Now >= OverTime)
                    return true;
                return false;
            }
        }
    }
}