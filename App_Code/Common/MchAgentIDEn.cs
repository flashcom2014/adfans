using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// 商家或代理ID加密类
/// </summary>
public static class MchAgentIDEn
{
    private static string KEY = "WeiMaQi"; // 密钥
    private static string StrKey = "0176348295";
    private static string enstr = "10010110110010101100101011001100110110100101";
    private static string APPIDStr = "q03l8wruv5op6eixm1d27afgzhn49sjtykcb";


    /// <summary>
    /// 从APPID获得加密的ID
    /// </summary>
    /// <param name="appid">APPID</param>
    /// <returns></returns>
    public static int GetID(string appid)
    {
        int id = 0;
        appid = appid.ToLower();
        int num = 0;
        foreach (char c in appid)
        {
            num++;
            int n = APPIDStr.IndexOf(c);
            id += n;
            id += num * (n % 8);
        }
        return id;
    }

    /// <summary>
    /// 获取商家的AESKey
    /// </summary>
    /// <param name="mchid"></param>
    /// <returns></returns>
    public static string GetMchAESKey(int mchid)
    {
        return GetAESKey(mchid, 0);
    }

    /// <summary>
    /// 获取代理的AESKey
    /// </summary>
    /// <param name="agentid"></param>
    /// <returns></returns>
    public static string GetAgentAESKey(int agentid)
    {
        return GetAESKey(0, agentid);
    }

    private static string GetAESKey(int mchid, int agetnid)
    {
        string mch_agent = string.Format("{0}|{1}", mchid, agetnid);
        string md5str = Tools.MD5(mch_agent);
        string md51 = Tools.MD5(md5str);
        string md52 = Tools.MD5(md51);
        string aeskey = md51 + md52.Substring(0, 11);
        aeskey = GetEnStr(aeskey);
        return aeskey;
    }

    /// <summary>
    /// 获取商家的Token
    /// </summary>
    /// <param name="mchid"></param>
    /// <returns></returns>
    public static string GetMchToken(int mchid)
    {
        return GetToken(mchid, 0);
    }

    /// <summary>
    /// 获取代理的Token
    /// </summary>
    /// <param name="agentid"></param>
    /// <returns></returns>
    public static string GetAgentToken(int agentid)
    {
        return GetToken(0, agentid);
    }

    private static string GetToken(int mchid, int agentid)
    {
        string mch_agent = string.Format("{0}|{1}", mchid, agentid);
        string md5str = Tools.MD5(mch_agent);
        string token = GetEnStr(md5str);
        return token;
    }

    private static string GetEnStr(string str)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < str.Length; i++)
        {
            if (enstr[i] == '0')
            {
                sb.Append(str[i].ToString().ToLower());
            }
            else
            {
                sb.Append(str[i].ToString().ToUpper());
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// 加密商家ID
    /// </summary>
    /// <param name="mchid">商家ID</param>
    /// <returns></returns>
    public static string EncryptMchID(int mchid)
    {
        string smchid = mchid.ToString();
        int len = smchid.Length;
        StringBuilder sb = new StringBuilder();
        //商家开始为偶数
        int start = len;
        if (!IsEven(len))
            start--;
        sb.Append(start);
        sb.Append(len);
        string midstr = StrKey.Substring(2, StrKey.Length - len - 2);
        sb.Append(midstr);
        sb.Append(smchid);
        string enstr = sb.ToString();
        sb.Clear();
        //
        enstr = Encrypt(enstr);
        enstr = HttpUtility.UrlEncode(enstr);
        return enstr;
    }

    /// <summary>
    /// 加密代理ID
    /// </summary>
    /// <param name="agentid"></param>
    /// <returns></returns>
    public static string EncryptAgentID(int agentid)
    {
        string smchid = agentid.ToString();
        int len = smchid.Length;
        StringBuilder sb = new StringBuilder();
        //代理开始为奇数
        int start = len;
        if (IsEven(len))
            start--;
        sb.Append(start);
        sb.Append(len);
        string midstr = StrKey.Substring(2, StrKey.Length - len - 2);
        sb.Append(midstr);
        sb.Append(smchid);
        string enstr = sb.ToString();
        sb.Clear();
        //
        enstr = Encrypt(enstr);
        enstr = HttpUtility.UrlEncode(enstr);
        return enstr;
    }

    /// <summary>
    /// 解密ID
    /// </summary>
    /// <param name="enstr">待解密的字符串</param>
    /// <param name="ismch">是否为商家</param>
    /// <returns></returns>
    public static int DecryptMchAgent(string enstr, ref bool ismch)
    {
        string enst = HttpUtility.UrlDecode(enstr);
        enst = Decrypt(enst);
        int start = int.Parse(enst[0].ToString());
        if (IsEven(start))
            ismch = true;
        else
            ismch = false;
        int len = int.Parse(enst.Substring(1, 1));
        int id = int.Parse(enst.Substring(enst.Length - len));
        return id;
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name=”txt”></param>
    /// <returns></returns>
    private static string Encrypt(string txt)
    {
        StringBuilder sb = new StringBuilder();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(txt); // 原字符串转换成字节数组
        byte[] keys = System.Text.Encoding.UTF8.GetBytes(KEY); // 密钥转换成字节数组

        // 异或
        for (int i = 0; i < bs.Length; i++)
        {
            bs[i] = (byte)(bs[i] ^ keys[i % keys.Length]);
        }

        return Convert.ToBase64String(bs);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name=”txt”></param>
    /// <returns></returns>
    private static string Decrypt(string txt)
    {
        int len = txt.Length;
        byte[] bs = Convert.FromBase64String(txt);

        byte[] keys = System.Text.Encoding.UTF8.GetBytes(KEY); // 密钥转换成字节数组

        // 异或
        for (int i = 0; i < bs.Length; i++)
        {
            bs[i] = (byte)(bs[i] ^ keys[i % keys.Length]);
        }
        //

        return System.Text.Encoding.UTF8.GetString(bs);
    }

    private static bool IsEven(int num)
    {
        if (num % 2 == 0)
            return true;
        return false;
    }
}