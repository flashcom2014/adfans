using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace pagetablead
{
    public class PageDataTable
    {
        public PageDataTable(int pPageIndex, int pPageSize, int pTotalCount, int pPageCount, DataTable pListData)
        {
            p_pageindex = pPageIndex;
            p_pagesize = pPageSize;
            p_totalcount = pTotalCount;
            p_pagecount = pPageCount;
            p_listData = new ArrayList();
            foreach (DataRow dr in pListData.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in pListData.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dr[dataColumn.ColumnName].ToString());
                }
                p_listData.Add(dictionary); //ArrayList集合中添加键值
            }
        }

        int p_pagecount;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return p_pagecount; }
            set { p_pagecount = value; }
        }

        int p_pageindex;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return p_pageindex; }
            set { p_pageindex = value; }
        }
        int p_pagesize = 20;
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize
        {
            get { return p_pagesize; }
            set { p_pagesize = value; }
        }
        int p_totalcount;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get { return p_totalcount; }
            set { p_totalcount = value; }
        }
        ArrayList p_listData;
        /// <summary>
        /// 数据列表
        /// </summary>
        public ArrayList ListData
        {
            get { return p_listData; }
            set { p_listData = value; }
        }
    }
}
