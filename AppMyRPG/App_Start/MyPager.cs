using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AppMyRPG
{

    public class MyPage
    {
        int total = 0;            //总数
        int pageSize = 10;
        int pageCurr = 1;   //当前页
        int Offset = 5;     //两边偏移几页
        string arg = string.Empty;  //其他的参数（地址栏传送）

        //构造方法接收用户传来的数据
        public MyPage(int total, int pageCurr, int pageSize = 10, int Offset = 5)
        {
            this.total = total;
            this.pageCurr = pageCurr;
            this.pageSize = pageSize;
            this.Offset = Offset;
        }
        public MyPage(int total, int pageCurr, int pageSize = 10, int Offset = 5, string arg = "")
        {
            this.total = total;
            this.pageCurr = pageCurr;
            this.pageSize = pageSize;
            this.Offset = Offset;
            this.arg = arg;
        }

        public string ToHTML()
        {
            if (pageSize < 1)
            {
                pageSize = 10;
            }
            if (total < 0)
            {
                total = 0;
            }
            if (pageCurr < 1)
            {
                pageCurr = 1;
            }
            if (Offset < 1)
            {
                Offset = 5;
            }
            decimal rs = (decimal)total / (decimal)pageSize;
            int pageCount = (int)(Math.Ceiling(rs));
            //校验当前页总数是否正确
            if (pageCurr > pageCount)
            {
                pageCurr = pageCount;
            }
            //生成页码范围
            int start = pageCurr - Offset;
            int end = pageCurr + Offset;
            //校验
            if (start < 1)
            {
                start = 1;
            }
            if (end > pageCount)
            {
                end = pageCount;
            }
            //产生页码范围
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<ul class='pagination'>");
            sb.AppendFormat("<li><a href='?page=1'><i class='fa fa-angle-double-left'></i></a></li>");
            for (int i = start; i <= end; i++)
            {
                if (i == pageCurr)
                {
                    sb.AppendFormat("<li class='active'><a href='?page={0}&{1}'>{0}</a><li>", i, arg);
                }
                else
                {
                    sb.AppendFormat("<li><a href='?page={0}&{1}'>{0}</a></li>", i, arg);
                }
            }
            sb.AppendFormat("<li><a href='?page={0}'><i class='fa fa-angle-double-right'></i></a></li>", pageCount);
            sb.AppendFormat("</ul>");
            return sb.ToString();
        }
    }
}