using System;
using System.Collections.Generic;
using System.Text;

namespace TodoDB.Models
{
    public class Result
    {
        public int PkId; //primary key
        public int ResultCode;
        public string ResultMessage;

        //public string KEY;

        public int TotalRowCount;
        //public int PageRowcount;

        //public DataSet ds;
        public Object obj;

        public Result()
        {
            ResultCode = -101;
            ResultMessage = "";

            //ds = new DataSet();
            obj = null;
        }
    }
}
