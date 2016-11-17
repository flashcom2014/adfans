using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DBUtility;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Web.Script.Serialization;
using System.Text;

/// <summary>
/// 跨站点数据存取类
/// </summary>
public static class TemporaryStore
{
    private static int indexkey = 0;
    private static Dictionary<int, TemporaryDatas> list = new Dictionary<int, TemporaryDatas>();

    /// <summary>
    /// 把对象存入内存并返回Key
    /// </summary>
    /// <param name="obj">临时对象</param>
    /// <returns></returns>
    public static int SetMenoryDatas(object obj)
    {
        //清除过期内容
        List<int> tl = new List<int>();
        lock (list)
        {
            foreach (TemporaryDatas td in list.Values)
            {
                if (td.IsTimeOut)
                    tl.Add(td.Key);
            }
            foreach (int tkey in tl)
                list.Remove(tkey);
        }
        //
        tl.Clear();
        tl = null;

        //
        int key = Interlocked.Increment(ref indexkey);
        TemporaryDatas tdata = new TemporaryDatas(key, obj);
        list[key] = tdata;
        return key;
    }

    /// <summary>
    /// 把对象按Key存入内存
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="obj">存入内存的临时对象</param>
    public static void SetMenoryDatas(int key, object obj)
    {
        lock (list)
        {
            if (list.ContainsKey(key))
                list[key].UseIt(obj);
        }
    }

    /// <summary>
    /// 返回对应Key的临时对象
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>如果对应Key不存在将返回null</returns>
    public static object GetMenoryDatas(int key)
    {
        lock (list)
        {
            if (list.ContainsKey(key))
                return list[key].GetDatas();
        }
        return null;
    }

    /// <summary>
    /// 返回对应Key的临时对象
    /// </summary>
    /// <typeparam name="T">对应类型</typeparam>
    /// <param name="key">Key</param>
    /// <returns>如果对应Key不存在将返回null</returns>
    public static T GetMenoryDatas<T>(int key)
    {
        return (T)GetMenoryDatas(key);
    }

    /// <summary>
    /// 把对象以JSON形式存在数据库并返回对应Key
    /// </summary>
    /// <param name="obj">临时储存对象</param>
    /// <returns></returns>
    public static int SetJsonDatas(object obj)
    {
        var jser = new JavaScriptSerializer();
        string json = jser.Serialize(obj);
        byte[] data = Encoding.UTF8.GetBytes(json);
        return setdatas(data);
    }

    /// <summary>
    /// 把对象存在数据库并返回对应Key
    /// </summary>
    /// <param name="obj">临时储存对象</param>
    /// <returns></returns>
    public static int SetDatas(object obj)
    {
        byte[] data = Tools.SerializeBinary(obj);
        return setdatas(data);
    }

    private static int setdatas(byte[] data)
    {
        int key = 0;
        string sqlstr = "call AddTempData(@data);";
        MySqlParameter mdata = new MySqlParameter("@data", MySqlDbType.MediumBlob);
        mdata.Value = data;
        MySqlDataReader reader = DbHelperMySQL.ExecuteReader(sqlstr, mdata);
        if (reader.Read())
        {
            key = (int)reader["thekey"];
        }
        reader.Close();
        reader = null;

        return key;
    }

    /// <summary>
    /// 把对象以Json形式序列化后按指定Key存入到数据库中
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="obj"临时储存对象></param>
    /// <returns></returns>
    public static bool SetJsonDatas(int key, object obj)
    {
        var jser = new JavaScriptSerializer();
        string json = jser.Serialize(obj);
        byte[] data = Encoding.UTF8.GetBytes(json);
        return setdatas(key, data);
    }

    /// <summary>
    /// 把对象按指定Key存入到数据库中
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="obj">临时储存对象</param>
    /// <returns></returns>
    public static bool SetDatas(int key, object obj)
    {
        byte[] data = Tools.SerializeBinary(obj);
        return setdatas(key, data);
    }

    private static bool setdatas(int key, byte[] data)
    {
        string sqlstr = "update tbl_temporarystore set datas=@data,usetime=@usetime where thekey=@thekey;";
        MySqlParameter mdata = new MySqlParameter("@data", MySqlDbType.Blob);
        mdata.Value = data;
        MySqlParameter musetime = new MySqlParameter("@usetime", MySqlDbType.DateTime);
        musetime.Value = DateTime.Now;
        MySqlParameter mkey = new MySqlParameter("@thekey", MySqlDbType.Int32);
        mkey.Value = key;
        int result = DbHelperMySQL.ExecuteSql(sqlstr, mdata, musetime, mkey);
        return result > 0;
    }

    /// <summary>
    /// 用Key取回对应对象
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>如果对应Key找不到对象将返回null</returns>
    public static object GetJsonDatas(int key)
    {
        try
        {
            byte[] data = getdatas(key);
            string json = Encoding.UTF8.GetString(data);
            var jser = new JavaScriptSerializer();
            return jser.Deserialize<object>(json);
        }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("TemporaryStore->GetJsonDatas: " + ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// 用Key取回对应对象
    /// </summary>
    /// <typeparam name="T">取回的临时对象类</typeparam>
    /// <param name="key">Key</param>
    /// <returns>如果对应Key找不到对象将返回null</returns>
    public static T GetJsonDatas<T>(int key)
    {
        try
        {
            byte[] data = getdatas(key);
            string json = Encoding.UTF8.GetString(data);
            var jser = new JavaScriptSerializer();
            return jser.Deserialize<T>(json);
        }
        catch (Exception ex)
        {
            Common.Scheduler.SaveExLog("TemporaryStore->GetJsonDatas: " + ex.ToString());
            return default(T);
        }
    }

    /// <summary>
    /// 用Key取回对应对象
    /// </summary>
    /// <typeparam name="T">取回的临时对象类</typeparam>
    /// <param name="key">Key</param>
    /// <returns>如果对应Key找不到对象将返回null</returns>
    public static T GetDatas<T>(int key)
    {
        return (T)GetDatas(key);
    }

    /// <summary>
    /// 用Key取回对应对象
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>如果对应Key找不到对象将返回null</returns>
    public static object GetDatas(int key)
    {
        byte[] data = getdatas(key);
        return Tools.DeserializeBinary(data);
    }

    private static byte[] getdatas(int key)
    {
        string sqlstr = string.Format("call GetTempData({0});", key);
        DataSet ds = DbHelperMySQL.Query_Main(sqlstr);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
        {
            dt.Dispose();
            ds.Dispose();
            return null;
        }
        byte[] data = (byte[])dt.Rows[0]["datas"];
        dt.Dispose();
        ds.Dispose();
        return data;
    }
}

public class TemporaryDatas
{
    private int key = 0;
    private object tempobj = null;
    private DateTime usetime;
    private int timeoutmins = 5;

    public TemporaryDatas(int key, object obj)
    {
        this.key = key;
        this.tempobj = obj;
        usetime = DateTime.Now;
    }

    public object GetDatas()
    {
        usetime = DateTime.Now;
        return tempobj;
    }

    public bool IsTimeOut
    {
        get
        {
            if (usetime.AddMinutes(timeoutmins) < DateTime.Now)
                return true;
            return false;
        }
    }

    public void UseIt(object obj)
    {
        this.usetime = DateTime.Now;
        this.tempobj = obj;
    }

    public int Key
    {
        get
        {
            return this.key;
        }
    }
}