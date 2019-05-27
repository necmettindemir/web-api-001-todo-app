using System;
using System.Collections.Generic;
using System.Text;

namespace TodoDB.Helpers
{
    /// <summary>
    /// This object is used for list operations with multi page and multi rows
    /// </summary>
    public class PageParamsForPost
    {
        private int pageRowCount = 10;
        private const int MaxPageRowCount = 50;
        private string ascdesc = "ASC";

        public int PageNumber { get; set; } = 1;


    

       // public int TotalRowCount { get; set; }


        public int PageRowCount
        {
            get { return pageRowCount; }
            set { pageRowCount = (value > MaxPageRowCount) ? MaxPageRowCount : value; }
        }

        

        public string OrderColumnName { get; set; }

        public string AscDesc
        {
            get { return ascdesc;  }
            set { ascdesc = (value.ToUpper() != "ASC" && value.ToUpper() != "DESC") ? "ASC" : value; }
        }

    }
}
