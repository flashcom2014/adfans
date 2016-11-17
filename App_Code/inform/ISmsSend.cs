using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace inform
{
    /// <summary>
    /// ISmsSend 的摘要说明
    /// </summary>
    public interface ISmsSend
    {
        bool SendSms(string mobile, string code, string ip, ref string restr);
        bool SendSms(int mchid, int agentid, int channel, float amount, string mobile, string content, ref string restr);
        int GetBalance(ref string restr);
        bool LoadData(DataRow row);
    }

}